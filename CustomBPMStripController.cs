//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

// My Documentation


public class CustomBPMStripController : SceneObjectScript

{
    #region ConstantsVariables
    public readonly Interaction ComplexInteraction;
    //public int VolumeStripNumer = 0;

    public bool Debug = false;

    private string UsersToListenTo = "ALL";
    private List<string> ValidUsers = new List<string>();
    bool validUser = false;

    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);

    private double ZRotation = new double();


    #endregion

    public override void Init()
    {
        Log.Write("In CustomBPMStripController");
        ComplexInteraction.SetPrompt(".");
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        //ObjectPrivate.TryGetComponent<AudioComponent>(0, out audio);
        
        SubscribeToScriptEvent("SecurityList", getSecurity);
        InitializeComplexInteraction();

    }

    private void InitializeComplexInteraction()
    {

        CurPos = ObjectPrivate.InitialPosition;
        //ZRotation = ObjectPrivate.InitialRotation.GetEulerAngles().Z * 57.2958;
        //if (ZRotation < 0) ZRotation = 360 - ZRotation;

        ComplexInteractionHandler();
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

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

        public Reflective ExtraData { get; set;  }
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

    public interface ISendSecurityInfo
    {
        string SecurityString { get; }
    }

    private void getSecurity(ScriptEventData gotSecurity)
    {
        //Log.Write("ComplexLooperController: In getSecurity");
        if (gotSecurity.Data == null)
        {
            return;
        }

        ISendSecurityInfo sendSecurityInfo = gotSecurity.Data.AsInterface<ISendSecurityInfo>();
        if (sendSecurityInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        UsersToListenTo = sendSecurityInfo.SecurityString;
        //Log.Write("CustomControllerLooper UsersToListenTo After getSecurity: " + UsersToListenTo);
    }

    public class BPMStripInfo : Reflective
    {
        public int BPMStripPosition { get; set; }
    }

    #endregion

    #region Interaction

    private void ComplexInteractionHandler()
    {
        //Log.Write("In ComplexInteractionHandler");
        ComplexInteraction.Subscribe((InteractionData idata) =>
        {
            if (Debug)
            {
                ComplexInteraction.SetPrompt("Debug: "
                    + "\nHit:" + idata.HitPosition.ToString()
                    + "\nBy:" + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
                //Vector hitPosition = idata.HitPosition;
                //Log.Write("Hit:  " + idata.HitPosition.ToString());
            }

            //Log.Write("Interacting person: " + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
            ValidUsers.Clear();
            ValidUsers = UsersToListenTo.Split(',').ToList();
            if (UsersToListenTo.Contains("ALL"))
            {
                //Log.Write("Valid User: ALL");
                validUser = true;
            }
            else
            {
                foreach (string ValidUser in ValidUsers)
                {
                    //Log.Write("ValidUser: " + ValidUser);
                    if (ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name == ValidUser.Trim())
                    {
                        validUser = true;
                    }
                }
            }
            if (!validUser)
            {
            }
            else
            {

                ExecuteInteraction(idata);
            }

        });
    }

    private void ExecuteInteraction(InteractionData idata)
    {
        //loopNote = false;
        float hitXRelative = 0;
        float hitYRelative = 0;
        float hitZRelative = 0;
        Vector hitPosition = idata.HitPosition;
        //Log.Write("CurPosX: " + CurPos.X);
        //Log.Write("CurPosY: " + CurPos.Y);
        //Log.Write("hitPosition.X: " + hitPosition.X);
        //Log.Write("hitPosition.Y: " + hitPosition.Y);
        //normalize to origin 0,0

        if (hitPosition.X > CurPos.X)
        {
            hitXRelative = (hitPosition.X - CurPos.X) * 100;
        }
        else
        {
            hitXRelative = (hitPosition.X - CurPos.X) * 100;
        }

        if (hitPosition.Y > CurPos.Y)
        {
            hitYRelative = (hitPosition.Y - CurPos.Y) * 100;
        }
        else
        {
            hitYRelative = (hitPosition.Y - CurPos.Y) * 100;
        }
        if (hitPosition.Z > CurPos.Z)
        {
            hitZRelative = (hitPosition.Z - CurPos.Z) * 100;
        }
        else
        {
            hitZRelative = (hitPosition.Z - CurPos.Z) * 100;
        }
        //Log.Write("CurPos: " + CurPos);
        //Log.Write("ZSet: " + ObjectPrivate.InitialRotation.Z + " ZRotation: " + ZRotation + " ZConfig: " + ZConfig);
        //Log.Write("hitXRelative: " + hitXRelative + " hitYRelative: " + hitYRelative + " hitZRelative: " + hitZRelative);
        //float X2 = hitXRelative * hitXRelative;
        //float Y2 = hitYRelative * hitYRelative;
        //float length = (float) Math.Sqrt(X2 + Y2);
        //Log.Write("length: " + length);
        //Log.Write("length/23.5: " + length / 23.5f);
        //float XMin = 23.65f;
        //float Xmax = 23.35f;
        //float pct = 0;

        //if (hitZRelative > 0.9)
        //{
            //pct = (length + XMin) / (XMin + Xmax) + 0.5f;
        //    pct = (length / 47.0f) + 0.5f;
        //}
        //else
        //{
        //    pct = 0.5f - (length / 47.0f);
        //}

        //Log.Write("Pct: " + pct);

        float X2 = hitXRelative * hitXRelative;
        float Y2 = hitYRelative * hitYRelative;
        float length = (float)Math.Sqrt(X2 + Y2);
        //Log.Write("length: " + length);
        //Log.Write("length/23.5: " + length / 23.5f);
        float pct = length / 47.0f;

        //Log.Write("Pct: " + pct);        

        int BPMPosition = (int)Math.Round(pct * 80, 0);
        //Log.Write("volumePosition: " + volumePosition);
        sendBPMStripPosition(BPMPosition, idata);

    }

    private void sendBPMStripPosition(int BPMPositionIn, InteractionData idata)
    {
        BPMStripInfo sendBPMPosition = new BPMStripInfo();
        sendBPMPosition.BPMStripPosition = BPMPositionIn;
        //Log.Write("TrackStripNumber: " + volumeStripNumberIn + " volumePosition: " + volumePositionIn);

        PostScriptEvent(ScriptId.AllScripts, "BPM", sendBPMPosition);
    }

    #endregion
}
