﻿//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

// My Documentation


public class CustomKeySend : SceneObjectScript

{
    #region ConstantsVariables
    public readonly Interaction ComplexInteraction;

    public string ChannelOut;
    public string KeyToSend;
    //public float delayOff = 0.15f;
    public bool Debug = false;

    private string UsersToListenTo = "ALL";
    private List<string> ValidUsers = new List<string>();
    bool validUser = false;

    private AgentPrivate Hitman;
    private AgentInfo Jammer;
    private RigidBodyComponent RigidBody = null;
    private Client client;
    private string currentKey = null;

    public class CurrentInstrumentInfo : Reflective
    {
        public AgentPrivate HitmanOut { get; set; }
        public string ChannelOut { get; set; }
        public string CurrentInstrument { get; set; }
    }

    public interface ICurrentInstrumentInfo
    {
        AgentPrivate HitmanOut { get; }
        string ChannelOut { get; }
        string CurrentInstrument { get; }
    }
    #endregion

    public override void Init()
    {
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        SubscribeToScriptEvent("SecurityList", getSecurity);
        SubscribeToScriptEvent("CurrentInstrument", getCurrentInstrument);
        ComplexInteractionHandler();
    }

    private void getCurrentInstrument(ScriptEventData gotCurrentInstrument)
    {
        //Log.Write("In getCurrentInstrument");
        if (gotCurrentInstrument.Data == null)
        {
            return;
        }

        ICurrentInstrumentInfo sendCurrentInstrument = gotCurrentInstrument.Data.AsInterface<ICurrentInstrumentInfo>();

        if (sendCurrentInstrument == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        Hitman = sendCurrentInstrument.HitmanOut;
        //Log.Write("Hitman in getCurrentInsturment: " + Hitman);
        client = Hitman.Client;
        //Log.Write("In ComplexInteractionHandler");
        //Hitman = ScenePrivate.FindAgent(idata.AgentId);
        //Log.Write("Hitman in InteractionHandler: " + Hitman.AgentInfo.Name);
        //Client client = Hitman.Client;
        //Log.Write("A");
        //client.SubscribeToCommand("Trigger", CommandAction.Pressed, SendKeyInteraction, CommandCanceled);
        //Log.Write("B");
        client.SubscribeToCommand("Trigger", CommandAction.Released, SendKeyRelease, CommandCanceled);
        //Log.Write("C");
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

    public class SendKeyInfo : Reflective
    {
        public string iChannelOut { get; set; }
        public string iSource { get; set; }
        public string iKeySent { get; set; }
    }



    private void ComplexInteractionHandler()
    {


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
            SendKeyInteraction();
        });
    }

    private void CheckSecurity()
    {

    }

    #endregion

    #region Interaction

    private void sendSimpleMessage(string msg, InteractionData data)
    {
        SimpleData sd = new SimpleData(this);
        //SimpleDataExt thisObjectDataExt = new SimpleDataExt(this);
        sd.AgentInfo = ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        sd.ObjectId = sd.AgentInfo.ObjectId;
        sd.SourceObjectId = ObjectPrivate.ObjectId;
        // assign our data to reflective data field
        SendToAll(msg, sd);
        //Log.Write("Key Message Sent: " + msg);
    }

    private void SendKeyInteraction()
    {
        //Log.Write("In Key Pressed");

        SendKeyInfo sendKeyInfo = new SendKeyInfo();
        sendKeyInfo.iChannelOut = ChannelOut;
        sendKeyInfo.iSource = "KeySend";
        sendKeyInfo.iKeySent = KeyToSend;
        currentKey = KeyToSend;
        //Log.Write("Key Sent: " + KeyToSend);
        PostScriptEvent(ScriptId.AllScripts, "KeySent", sendKeyInfo);  //used by synth
        PostScriptEvent(ScriptId.AllScripts, KeyToSend, sendKeyInfo);  //used by animation
        //Wait(TimeSpan.FromSeconds(delayOff));
        //sendKeyInfo.iKeySent = KeyToSend + "Up";
        //PostScriptEvent(ScriptId.AllScripts, "KeySent", sendKeyInfo);  //used by synth
        //Log.Write("Key Interaction Sent");
    }

    private void SendKeyRelease(CommandData Button)
    {
        //Log.Write("In Key Release");
        if (currentKey == KeyToSend)
        {
            SendKeyInfo sendKeyInfo = new SendKeyInfo();
            sendKeyInfo.iChannelOut = ChannelOut;
            sendKeyInfo.iSource = "KeySend";
            sendKeyInfo.iKeySent = KeyToSend + "Up";
            //Log.Write("Key Released: " + sendKeyInfo.iKeySent);
            PostScriptEvent(ScriptId.AllScripts, "KeySent", sendKeyInfo);  //used by synth
            PostScriptEvent(ScriptId.AllScripts, KeyToSend, sendKeyInfo);  //used by animation
            currentKey = null;
            //Wait(TimeSpan.FromSeconds(delayOff));
            //sendKeyInfo.iKeySent = KeyToSend + "Up";
            //PostScriptEvent(ScriptId.AllScripts, "KeySent", sendKeyInfo);  //used by synth
            //Log.Write("Key Interaction Sent");
        }
    }

    void CommandCanceled(CancelData data)
    {
        Log.Write(GetType().Name, "Subscription canceled: " + data.Message);
    }

    #endregion
}
