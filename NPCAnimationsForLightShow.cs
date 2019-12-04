/* This content is licensed under the terms of the Creative Commons Attribution 4.0 International License.
 * When using this content, you must:
 * •    Acknowledge that the content is from the Sansar Knowledge Base.
 * •    Include our copyright notice: "© 2017 Linden Research, Inc."
 * •    Indicate that the content is licensed under the Creative Commons Attribution-Share Alike 4.0 International License.
 * •    Include the URL for, or link to, the license summary at https://creativecommons.org/licenses/by-sa/4.0/deed.hi (and, if possible, to the complete license terms at https://creativecommons.org/licenses/by-sa/4.0/legalcode.
 * For example:
 * "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."
 */

using Sansar.Script;
using Sansar.Simulation;
using Sansar;
using System;
using System.Linq;
using System.Collections.Generic;

public class NPCAnimationForLightShow : SceneObjectScript
{
    #region EditorProperties
    // Start playing on these events. Can be a comma separated list of event names.
    public string AnimationName = null;

    #endregion

    private Animation animation = null;
    private AnimationParameters initialAnimationParameters;

    private bool killAnimation = false;
    private string[] AnimationEvent = new string[18];
    private string[] AnimationDoneEvent = new string[18];
    private string[] startFrame = new string[18];
    private string[] endFrame = new string[18];
    private string[] PlaybackType = new string[18];
    private string[] AnimationSpeed = new string[18];
    private string[] BlendDuration = new string[18];
    private AnimationComponent animComponent;

    #region Communication

    #region SimpleHelpers v2
    // Update the region tag above by incrementing the version when updating anything in the region.

    // If a Group is set, will only respond and send to other SimpleScripts with the same Group tag set.
    // Does NOT accept CSV lists of groups.
    // To send or receive events to/from a specific group from outside that group prepend the group name with a > to the event name
    // my_group>on
    [DefaultValue("")]
    [DisplayName("Group")]
    public string Group = "";

    public interface ISimpleData
    {
        AgentInfo AgentInfo { get; }
        ObjectId ObjectId { get; }
        ObjectId SourceObjectId { get; }

        // Extra data
        Reflective ExtraData { get; }
    }

    public class SimpleData : Reflective, ISimpleData
    {
        public SimpleData(ScriptBase script) { ExtraData = script; }
        public AgentInfo AgentInfo { get; set; }
        public ObjectId ObjectId { get; set; }
        public ObjectId SourceObjectId { get; set; }

        public Reflective ExtraData { get; }
    }

    public interface IDebugger { bool DebugSimple { get; } }
    private bool __debugInitialized = false;
    private bool __SimpleDebugging = false;
    private string __SimpleTag = "";

    private string GenerateEventName(string eventName)
    {
        eventName = eventName.Trim();
        if (eventName.EndsWith("@"))
        {
            // Special case on@ to send the event globally (the null group) by sending w/o the @.
            return eventName.Substring(0, eventName.Length - 1);
        }
        else if (Group == "" || eventName.Contains("@"))
        {
            // No group was set or already targeting a specific group as is.
            return eventName;
        }
        else
        {
            // Append the group
            return $"{eventName}@{Group}";
        }
    }

    private void SetupSimple()
    {
        __debugInitialized = true;
        __SimpleTag = GetType().Name + " [S:" + Script.ID.ToString() + " O:" + ObjectPrivate.ObjectId.ToString() + "]";
        Wait(TimeSpan.FromSeconds(1));
        IDebugger debugger = ScenePrivate.FindReflective<IDebugger>("SimpleDebugger").FirstOrDefault();
        if (debugger != null) __SimpleDebugging = debugger.DebugSimple;
    }

    System.Collections.Generic.Dictionary<string, Func<string, Action<ScriptEventData>, Action>> __subscribeActions = new System.Collections.Generic.Dictionary<string, Func<string, Action<ScriptEventData>, Action>>();
    private Action SubscribeToAll(string csv, Action<ScriptEventData> callback)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return null;

        Func<string, Action<ScriptEventData>, Action> subscribeAction;
        if (__subscribeActions.TryGetValue(csv, out subscribeAction))
        {
            return subscribeAction(csv, callback);
        }

        // Simple case.
        if (!csv.Contains(">>"))
        {
            __subscribeActions[csv] = SubscribeToAllInternal;
            return SubscribeToAllInternal(csv, callback);
        }

        // Chaining
        __subscribeActions[csv] = (_csv, _callback) =>
        {
            System.Collections.Generic.List<string> chainedCommands = new System.Collections.Generic.List<string>(csv.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries));

            string initial = chainedCommands[0];
            chainedCommands.RemoveAt(0);
            chainedCommands.Add(initial);

            Action unsub = null;
            Action<ScriptEventData> wrappedCallback = null;
            wrappedCallback = (data) =>
            {
                string first = chainedCommands[0];
                chainedCommands.RemoveAt(0);
                chainedCommands.Add(first);
                if (unsub != null) unsub();
                unsub = SubscribeToAllInternal(first, wrappedCallback);
                Log.Write(LogLevel.Info, "CHAIN Subscribing to " + first);
                _callback(data);
            };

            unsub = SubscribeToAllInternal(initial, wrappedCallback);
            return unsub;
        };

        return __subscribeActions[csv](csv, callback);
    }

    private Action SubscribeToAllInternal(string csv, Action<ScriptEventData> callback)
    {
        Action unsubscribes = null;
        string[] events = csv.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (__SimpleDebugging)
        {
            Log.Write(LogLevel.Info, __SimpleTag, "Subscribing to " + events.Length + " events: " + string.Join(", ", events));
        }
        Action<ScriptEventData> wrappedCallback = callback;

        foreach (string eventName in events)
        {
            if (__SimpleDebugging)
            {
                var sub = SubscribeToScriptEvent(GenerateEventName(eventName), (ScriptEventData data) =>
                {
                    Log.Write(LogLevel.Info, __SimpleTag, "Received event " + GenerateEventName(eventName));
                    wrappedCallback(data);
                });
                unsubscribes += sub.Unsubscribe;
            }
            else
            {
                var sub = SubscribeToScriptEvent(GenerateEventName(eventName), wrappedCallback);
                unsubscribes += sub.Unsubscribe;
            }
        }
        return unsubscribes;
    }

    System.Collections.Generic.Dictionary<string, Action<string, Reflective>> __sendActions = new System.Collections.Generic.Dictionary<string, Action<string, Reflective>>();
    private void SendToAll(string csv, Reflective data)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;

        Action<string, Reflective> sendAction;
        if (__sendActions.TryGetValue(csv, out sendAction))
        {
            sendAction(csv, data);
            return;
        }

        // Simple case.
        if (!csv.Contains(">>"))
        {
            __sendActions[csv] = SendToAllInternal;
            SendToAllInternal(csv, data);
            return;
        }

        // Chaining
        System.Collections.Generic.List<string> chainedCommands = new System.Collections.Generic.List<string>(csv.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries));
        __sendActions[csv] = (_csv, _data) =>
        {
            string first = chainedCommands[0];
            chainedCommands.RemoveAt(0);
            chainedCommands.Add(first);

            Log.Write(LogLevel.Info, "CHAIN Sending to " + first);
            SendToAllInternal(first, _data);
        };
        __sendActions[csv](csv, data);
    }

    private void SendToAllInternal(string csv, Reflective data)
    {
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events) + (Group != "" ? (" to group " + Group) : ""));
        foreach (string eventName in events)
        {
            PostScriptEvent(GenerateEventName(eventName), data);
        }
    }
    #endregion

    public interface IAnimationInfo
    {
        string AnimationEvent { get; }
        string startFrame { get; }
        string endFrame { get; }
        string PlaybackType { get; }
        string AnimationSpeed { get; }
        string BlendDuration { get; }
    }

    private void getAnimationInfo(ScriptEventData gotAnimationInfo)
    {

        //Log.Write("In getAnimationInfo gotAnimationInfo.Data: " + gotAnimationInfo.Data);
        if (gotAnimationInfo.Data == null)
        {
            return;
        }

        IAnimationInfo sendAnimationInfo = gotAnimationInfo.Data.AsInterface<IAnimationInfo>();
        if (sendAnimationInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        //Log.Write("sendAnimationInfo.AnimationEvent: " + sendAnimationInfo.AnimationEvent);
        //Log.Write("sendAnimationInfo.startFrame: " + sendAnimationInfo.startFrame);
        //Log.Write("sendAnimationInfo.endFrame: " + sendAnimationInfo.endFrame);
        //Log.Write("sendAnimationInfo.PlaybackType: " + sendAnimationInfo.PlaybackType);
        //Log.Write("sendAnimationInfo.AnimationSpeed: " + sendAnimationInfo.AnimationSpeed);
        //Log.Write("sendAnimationInfo.BlendDuration: " + sendAnimationInfo.BlendDuration);

        PlayAnimationEvent(sendAnimationInfo.AnimationEvent, sendAnimationInfo.startFrame, sendAnimationInfo.endFrame, sendAnimationInfo.PlaybackType, sendAnimationInfo.AnimationSpeed, sendAnimationInfo.BlendDuration);

    }

    #endregion

    public override void Init()
    {

        if (!ObjectPrivate.TryGetFirstComponent<AnimationComponent>(out animComponent))
        {
            Log.Write(LogLevel.Error, "NPCAnimation.Init", "Object must have an animation added at edit time for NPCAnimation to work");
            return;
        }

        animation = animComponent.DefaultAnimation;
        animation.JumpToFrame(0);
        initialAnimationParameters = animation.GetParameters();
        SubscribeToAll("Kill", KillAnimation);
        SubscribeToScriptEvent("AnimationInfo", getAnimationInfo);

        /*
        if (EnableEvent != "")
        {
            Log.Write("Enable Event was not null: " + EnableEvent);
            SubscribeToAll(EnableEvent, Subscribe);
        }
        else
        {
            Subscribe(null);  //executes it by passing null data
        }

        if (DisableEvent != "")
        {
            SubscribeToAll(DisableEvent, Unsubscribe);
        }
        */

    }

    private void KillAnimation(ScriptEventData data)
    {
        killAnimation = true;
    }

    private void PlayAnimationEvent(string AnimationEventIn, string startFrameIn, string endFrameIn, string PlaybackTypeIn, string AnimationSpeedIn, string BlendDurationIn)
    {
        if ((AnimationEventIn == "ALL") || (AnimationEventIn == AnimationName))
        {
            //Log.Write("Playing Animation Number: " + AnimationNumber + "  Animation: " + AnimationEvent[AnimationNumber]);
            int firstFrame = Int32.Parse(startFrameIn);
            //Log.Write("firstFrame: " + firstFrame);
            int lastFrame = Int32.Parse(endFrameIn);
            //Log.Write("lastFrame: " + lastFrame);
            float NumberOfFrames = lastFrame - firstFrame;
            //Log.Write("NumberOfFrames: " + NumberOfFrames);
            float AnimationTimeToComplete = NumberOfFrames / 30.0f;
            //Log.Write("AnimationTimeToComplete: " + AnimationTimeToComplete);

            AnimationParameters animationParameters = initialAnimationParameters;

            if (PlaybackTypeIn.Contains("oop")) animationParameters.PlaybackMode = AnimationPlaybackMode.Loop;
            if (PlaybackTypeIn.Contains("ong")) animationParameters.PlaybackMode = AnimationPlaybackMode.PingPong;
            if (PlaybackTypeIn.Contains("nce")) animationParameters.PlaybackMode = AnimationPlaybackMode.PlayOnce;

            float fltPlaybackSpeed = float.Parse(AnimationSpeedIn);
            animationParameters.PlaybackSpeed = Math.Abs(fltPlaybackSpeed) * Math.Sign(lastFrame - firstFrame);

            int intBlendDuration = Int32.Parse(BlendDurationIn);
            if (intBlendDuration > 0) animationParameters.BlendDuration = Int32.Parse(BlendDurationIn);

            //Log.Write("PlaybackSpeed: " + animationParameters.PlaybackSpeed);
            if (animationParameters.PlaybackSpeed > 0.0f)
            {
                animationParameters.RangeStartFrame = firstFrame;
                animationParameters.RangeEndFrame = lastFrame;
            }
            else
            {
                // Backwards playback uses negative playback speed but start frame still less than end frame
                animationParameters.RangeStartFrame = lastFrame;
                animationParameters.RangeEndFrame = firstFrame;
            }

            animationParameters.ClampToRange = true;
            float TimeAdjust = 1.0f / fltPlaybackSpeed;
            //Log.Write("Type Length: " + PlaybackType[AnimationNumber].Length);
            //Log.Write("Number: " + PlaybackType[AnimationNumber].Substring(4, PlaybackType[AnimationNumber].Length - 4));
            if (PlaybackTypeIn.Contains("oop"))
            {
                //you can say loop5 and it will loop 5 times
                if (PlaybackTypeIn.Length > 4)
                {
                    int loopNum = Int32.Parse(PlaybackTypeIn.Substring(4, PlaybackTypeIn.Length - 4));
                    int i = 0;
                    do
                    {
                        animation.Play(animationParameters);
                        //Log.Write("TimeAdjust: " + TimeAdjust);
                        Wait(TimeSpan.FromMilliseconds(AnimationTimeToComplete * 1000 * TimeAdjust));
                        i++;
                    } while (i < loopNum);
                }
                else
                {
                    //loop indefinitely
                    animationParameters.PlaybackMode = AnimationPlaybackMode.Loop;
                    animation.Play(animationParameters);
                }
            }
            else
            {
                //PlayOnce
                animation.Play(animationParameters);
                //Log.Write("TimeAdjust: " + TimeAdjust);
                Wait(TimeSpan.FromMilliseconds(AnimationTimeToComplete * 1000 * TimeAdjust));
            }

            if (killAnimation)
            {
                animation.Reset();
                killAnimation = false;
            }
            //else
            //{
            //    string DoneEvent = AnimationDoneEvent[AnimationNumber];
            //    SendToAll(DoneEvent, data.Data);
            //Log.Write("Sent Done Event: " + DoneEvent);
            //}
        }
    }
}


