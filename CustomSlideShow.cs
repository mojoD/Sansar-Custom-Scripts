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
using System;
using System.Linq;
using System.Collections.Generic;

//Plays Slide Show with individual Timers on Each Slide

public class CustomSlideShow: SceneObjectScript
{
    #region EditorProperties

    [DefaultValue("NextSlide")]
    [DisplayName("Next Slide Event")]
    public readonly string NextEvent;

    [DefaultValue("PrevSlide")]
    [DisplayName("Previous Slide Event")]
    public readonly string PreviousEvent;

    [DefaultValue("GoTo")]
    [DisplayName("Go To Slide Event")]
    public readonly string GoToEvent;

    // Start playing on these events. Can be a comma separated list of event names.
    [DefaultValue("on")]
    [DisplayName("Play Event")]
    public readonly string PlayEvent;

    // Pause playing on these events. Can be a comma separated list of event names.
    [DefaultValue("off")]
    [DisplayName("Stop Event")]
    public readonly string StopEvent;

    // Slided to Play.
    [DisplayName("Slides To Play")]
    public readonly string SlidesToPlay;

    // Slide Timing.
    [DisplayName("Slide Timing")]
    public readonly string SlideTiming;

    // Loop
    [DefaultValue(false)]
    [DisplayName("Loop")]
    public readonly bool Loop;

    #endregion



    private Animation animation = null;
    private AnimationParameters initialAnimationParameters;
    private bool keepPlaying = true;
    private List<string> SequenceList = new List<string>();
    private List<string> strTimingList = new List<string>();
    private List<int> TimingList = new List<int>();
    private int currentSlideNumber = 0;

    #region Communication
    #region SimpleHelpers
    public interface ISimpleData
    {
        AgentInfo AgentInfo { get; }
        ObjectId ObjectId { get; }
    }

    public class SimpleData : Reflective, ISimpleData
    {
        public AgentInfo AgentInfo { get; set; }
        public ObjectId ObjectId { get; set; }
    }

    public interface IDebugger { bool DebugSimple { get; } }
    private bool __debugInitialized = false;
    private bool __SimpleDebugging = false;
    private string __SimpleTag = "";
    private void SetupSimple()
    {
        __debugInitialized = true;
        __SimpleTag = GetType().Name + " [S:" + Script.ID.ToString() + " O:" + ObjectPrivate.ObjectId.ToString() + "]";
        Wait(TimeSpan.FromSeconds(1));
        IDebugger debugger = ScenePrivate.FindReflective<IDebugger>("SimpleDebugger").FirstOrDefault();
        if (debugger != null) __SimpleDebugging = debugger.DebugSimple;
    }

    private Action SubscribeToAll(string csv, Action<ScriptEventData> callback)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return null;
        Action unsubscribes = null;
        string[] events = csv.Trim().Split(',');
        if (__SimpleDebugging)
        {
            Log.Write(LogLevel.Info, __SimpleTag, "Subscribing to " + events.Length + " events: " + string.Join(", ", events));
        }
        foreach (string eventName in events)
        {
            if (__SimpleDebugging)
            {
                var sub = SubscribeToScriptEvent(eventName.Trim(), (ScriptEventData data) =>
                {
                    Log.Write(LogLevel.Info, __SimpleTag, "Received event " + eventName);
                    callback(data);
                });
                unsubscribes += sub.Unsubscribe;
            }
            else
            {
                var sub = SubscribeToScriptEvent(eventName.Trim(), callback);
                unsubscribes += sub.Unsubscribe;
            }

        }
        return unsubscribes;
    }

    private void SendToAll(string csv, Reflective data)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(',');

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events));
        foreach (string eventName in events)
        {
            PostScriptEvent(eventName.Trim(), data);
        }
    }
    #endregion

    public class ItemChosen : Reflective
    {
        public int ItemChosenIn { get; set; }
    }

    private void SendItemChosen(int ItemChosenToSend)
    {
        ItemChosen sendItemChosen = new ItemChosen();
        sendItemChosen.ItemChosenIn = ItemChosenToSend;

        PostScriptEvent(ScriptId.AllScripts, "ItemChosen", sendItemChosen);
    }

    #endregion

    public override void Init()
    {
        AnimationComponent animComponent;
        if (!ObjectPrivate.TryGetFirstComponent<AnimationComponent>(out animComponent))
        {
            Log.Write(LogLevel.Error, "SimpleAnimation.Init", "Object must have an animation added at edit time for SimpleAnimation to work");
            return;
        }

        animation = animComponent.DefaultAnimation;
        initialAnimationParameters = animation.GetParameters();
        Log.Write("Subscribing To: " + PlayEvent);

        SubscribeToScriptEvent(PlayEvent, PlayTimedAnimation);
        SubscribeToScriptEvent(StopEvent, StopTimedAnimation);
        SubscribeToScriptEvent(NextEvent, NextSlide);
        SubscribeToScriptEvent(PreviousEvent, PreviousSlide);
        SubscribeToScriptEvent(GoToEvent + "1", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "2", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "3", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "4", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "5", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "6", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "7", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "8", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "9", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "10", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "11", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "12", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "13", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "14", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "15", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "16", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "17", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "18", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "19", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "20", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "21", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "22", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "23", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "24", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "25", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "26", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "27", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "28", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "29", GotoSlide);
        SubscribeToScriptEvent(GoToEvent + "30", GotoSlide);
    }

    private void NextSlide(ScriptEventData data)
    {
        if (currentSlideNumber < 30)
        {
            currentSlideNumber++;
            //Send Message on Slide Number Shown
            SendItemChosen(currentSlideNumber);

            initialAnimationParameters.RangeStartFrame = currentSlideNumber;
            initialAnimationParameters.RangeEndFrame = currentSlideNumber;
            initialAnimationParameters.ClampToRange = true;
            animation.Play(initialAnimationParameters);
        }
    }

    private void PreviousSlide(ScriptEventData data)
    {
        Log.Write("currentSlideNumber: " + currentSlideNumber);
        if (currentSlideNumber > 0)
        {
            currentSlideNumber--;
            //Send Message on Slide Number Shown
            SendItemChosen(currentSlideNumber);

            //Log.Write("Playing Slide: " + currentSlideNumber);
            initialAnimationParameters.RangeStartFrame = currentSlideNumber;
            initialAnimationParameters.RangeEndFrame = currentSlideNumber;
            initialAnimationParameters.ClampToRange = true;
            animation.Play(initialAnimationParameters);
        }
    }

    private void GotoSlide(ScriptEventData inMessage)
    {
        if (inMessage.Data == null)
        {
            return;
        }
        ISimpleData simpleData = inMessage.Data?.AsInterface<ISimpleData>();

        string strGoToSlide = inMessage.Message;
        Log.Write("inMessage: " + strGoToSlide);
        strGoToSlide = strGoToSlide.Substring(GoToEvent.Length, strGoToSlide.Length-GoToEvent.Length);
        Log.Write("inMessage2: " + strGoToSlide);
        int GoToSlide = Int32.Parse(strGoToSlide);
        Log.Write("GoToSlide: " + GoToSlide);
        if ((GoToSlide > 0) && (GoToSlide < 31))
        {
            Log.Write("Going to Slide" + GoToSlide);
            currentSlideNumber = GoToSlide-1;
            //Send Message on Slide Number Shown
            SendItemChosen(currentSlideNumber);

            initialAnimationParameters.RangeStartFrame = currentSlideNumber;
            initialAnimationParameters.RangeEndFrame = currentSlideNumber;
            initialAnimationParameters.ClampToRange = true;
            animation.Play(initialAnimationParameters);
        }
    }

    private void PlayTimedAnimation(ScriptEventData data)
    {
        keepPlaying = true;
        Log.Write("In PlayAnimation");
        initialAnimationParameters.PlaybackSpeed = 1;
        SequenceList.Clear();
        SequenceList = SlidesToPlay.Split(',').ToList();
        TimingList.Clear();
        strTimingList.Clear();
        strTimingList = SlideTiming.Split(',').ToList();
        foreach (string TimeElement in strTimingList)
        {
            TimingList.Add(Int32.Parse(TimeElement));
        }
        int Scntr = 0;
        do
        {
            foreach (string SequenceElement in SequenceList)
            {
                Log.Write("Slide: " + SequenceElement);
                initialAnimationParameters.RangeStartFrame = Int32.Parse(SequenceElement)-1;
                initialAnimationParameters.RangeEndFrame = Int32.Parse(SequenceElement)-1;
                initialAnimationParameters.ClampToRange = true;
                animation.Play(initialAnimationParameters);
                Log.Write("Delay: " + TimingList[Scntr]);
                Wait(TimeSpan.FromSeconds(TimingList[Scntr]));
                Scntr++;
                Log.Write("keepPlaying: " + keepPlaying);
                if (keepPlaying == false)
                {
                    break;
                }
            }
            if (Loop == false)
            {
                keepPlaying = false;
            }
        } while (keepPlaying);
    }

    private void StopTimedAnimation(ScriptEventData data)
    {
        Log.Write("Stop Animation");
        initialAnimationParameters.RangeStartFrame = 0;
        initialAnimationParameters.RangeEndFrame = 0;
        initialAnimationParameters.ClampToRange = true;
        animation.Play(initialAnimationParameters);
        keepPlaying = false;
        currentSlideNumber = 0;
    }
}
