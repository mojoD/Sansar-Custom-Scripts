/* This content is licensed under the terms of the Creative Commons Attribution 4.0 International License.
 * When using this content, you must:
 * •    Acknowledge that the content is from the Sansar Knowledge Base.
 * •    Include our copyright notice: "© 2017 Linden Research, Inc."
 * •    Indicate that the content is licensed under the Creative Commons Attribution-Share Alike 4.0 International License.
 * •    Include the URL for, or link to, the license summary at https://creativecommons.org/licenses/by-sa/4.0/deed.hi (and, if possible, to the complete license terms at https://creativecommons.org/licenses/by-sa/4.0/legalcode.
 * For example:
 * "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."
 */

// throw this script into the system camera object to gain basic control

using Sansar.Script;
using Sansar.Simulation;
using Sansar;
using System;
using System.Linq;
using System.Collections.Generic;

public class CameraControls : SceneObjectScript
{
    #region EditorProperties
    [Tooltip("Activate remote camera")]
    [DefaultValue("CameraOn")]
    [DisplayName("-> Make camera active")]
    public readonly string activateCameraComponent;

    [Tooltip("Reset remote camera, return to default")]
    [DefaultValue("CameraOff")]
    [DisplayName("-> Back to main cam")]
    public readonly string resetCameraComponent;

    [Tooltip("Kill Camera Movement to Break Looping")]
    [DefaultValue("KillCamera")]
    [DisplayName("-> Kill Camera Movement")]
    public readonly string killCameraMovementEvent;

    [Tooltip("Name of the Guest for CameraAngles")]
    [DefaultValue("")]
    [DisplayName("Guest Name")]
    public readonly string GuestNameIn;

    [Tooltip("Activates Free Camera")]
    [DefaultValue("FreeCamera")]
    [DisplayName("Free Camera Event: ")]
    public readonly string FreeCameraEvent;

    // Start playing on these events. Can be a comma separated list of event names.
    public List<string> CameraMovement1 = new List<string>();
    public List<string> CameraMovement2 = new List<string>();

    #endregion

    #region ConstantsVariables
    //Event,EventDone,DoneEventDelay,SpecialCameraType,Global/LocalPosition,PosX,PosY,PosZ,RotX,RotY,RotZ,TimeDurationOfMove,Target/Host/Guest,GuestConfigOverride
    private bool killCameraMovement = false;
    private int NumberOfCameraAngles = 0;
    private List<string> CameraMovementEvent = new List<string>();
    private List<string> CameraMovementDoneEvent = new List<string>();
    private List<string> CameraDoneEventDelay = new List<string>();
    private List<string> CameraType = new List<string>();  //Dolly, Orbit
    private List<string> CameraTargetType = new List<string>();  //Global, Host, Guest
    private List<string> CameraMovementPosX = new List<string>();
    private List<string> CameraMovementPosY = new List<string>();
    private List<string> CameraMovementPosZ = new List<string>();
    private List<string> CameraMovementRotX = new List<string>();
    private List<string> CameraMovementRotY = new List<string>();
    private List<string> CameraMovementRotZ = new List<string>();
    private List<string> CameraMovementTimeDuration = new List<string>();
    private List<string> CameraOrbitRadius = new List<string>();
    private List<string> CameraMovementArray = new List<string>();

    private bool rotateCamera = true;
    private bool moveCamera = true;

    private AgentPrivate agent;
    private AgentPrivate targetAgent;
    private Vector targetPosition;
    private Quaternion targetRotation;
    private Vector targetForwardVector;
    private Vector targetRightVector;
    private Vector currentPosition;
    private string GuestName;

    private Vector startPosition;
    private Quaternion startRotation;
    private Vector currentRotation;

    private Vector CameraPosition;
    private Vector CameraRotation;
    private float Orientation;

    private CameraComponent CamComponent;
    //private RigidBodyComponent CameraMesh;
    private MeshComponent CameraMesh;

    //private SessionId Jammer = new SessionId();
    AgentPrivate TheUser = null;
    List<IEventSubscription> ButtonSubscriptions = new List<IEventSubscription>();
    private bool Shifted = false;

    private bool freeCamera;
    private int MoveFreeCamera;
    private double FreeDelay = 10; // 10 seconds 
    private float positionOffset = 50.0f; //move a 50 meters
    private float rotationSpeed = 1.0f; //rotate a radian 1/360

    Vector forwardPosition;
    Vector forwardRotation;

    #endregion

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

    private void GetChatCommand(ChatData Data)
    {
        Log.Write("Chat From: " + Data.SourceId);
        Log.Write("Chat person: " + ScenePrivate.FindAgent(Data.SourceId).AgentInfo.Name);

        if (Data.Message.Contains("/Focus"))
        {
            string ChatIn = Data.Message;
            Log.Write("ChatIn: " + ChatIn);
            GuestName = ChatIn.Substring(7, ChatIn.Length - 7);
            Log.Write("Guest Name from Chat: " + GuestName);
        }
        /*
        if (Data.Message.Contains("/"))
        {
            if (Data.Message.Contains("/Guest"))
            {
                string ChatIn = Data.Message;
                Log.Write("ChatIn: " + ChatIn);
                GuestName = ChatIn.Substring(1, ChatIn.Length-1);
                Log.Write("Guest Name from Chat: " + GuestName);
            }
        }
        */
    }

    void SubscribeClientToButton(Client client, string Button)
    {
        if (freeCamera)
        {
            //Log.Write("Assigning free Camera Buttons");
            ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Pressed, FreeCommandReceived, CommandCanceled));
            ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Released, FreeCommandReceived, CommandCanceled));
        }
        else
        {
            ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Pressed, CommandReceived, CommandCanceled));
            ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Released, CommandReceived, CommandCanceled));
        }
    }

    void UnsubscribeAllButtons()
    {
        foreach (IEventSubscription sub in ButtonSubscriptions) sub.Unsubscribe();
        ButtonSubscriptions.Clear();
    }

    void SubscribeKeyPressed(AgentPrivate agent, string Message)
    {
        //Log.Write("SubscribeKeyPressed agent: " + agent);
        if (agent == null) return;

        if (Message == "sub" && TheUser == null)
        {
            TheUser = agent;
            Client client = agent.Client;
            SubscribeClientToButton(client, "Keypad0");
            SubscribeClientToButton(client, "Keypad1");
            SubscribeClientToButton(client, "Keypad2");
            SubscribeClientToButton(client, "Keypad3");
            SubscribeClientToButton(client, "Keypad4");
            SubscribeClientToButton(client, "Keypad5");
            SubscribeClientToButton(client, "Keypad6");
            SubscribeClientToButton(client, "Keypad7");
            SubscribeClientToButton(client, "Keypad8");
            SubscribeClientToButton(client, "Keypad9");
            SubscribeClientToButton(client, "Action1");
            SubscribeClientToButton(client, "Action2");
            SubscribeClientToButton(client, "Action3");
            SubscribeClientToButton(client, "Action4");
            SubscribeClientToButton(client, "Action5");
            SubscribeClientToButton(client, "Action6");
            SubscribeClientToButton(client, "Action7");
            SubscribeClientToButton(client, "Action8");
            SubscribeClientToButton(client, "Action9");
            SubscribeClientToButton(client, "Action0");
            SubscribeClientToButton(client, "Modifier");
        }

        if (Message == "unsub")
        {
            UnsubscribeAllButtons();
            TheUser = null;
            return;
        }
    }

    void FreeCommandReceived(CommandData Button)
    {
        if ((Button.Command == "Modifier") && (Button.Action == CommandAction.Pressed))  //shift acts a toggle
        {
            if (Shifted == true) Shifted = false;
            else Shifted = true;
        }

        //Log.Write("FreeCommandReceived Button: " + Button);
        if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) MoveFreeCamera = 0;
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Tilt Up
        {
            forwardRotation.X = -(float)Math.PI;
            forwardRotation.Y = 0.0f * 0.0174533f;
            forwardRotation.Z = 0.0f * 0.0174533f;
            //Log.Write("CameraRotation: " + forwardRotation + " Forward Vector: " + ObjectPrivate.ForwardVector + " Right Vector: " + ObjectPrivate.RightVector);
            Quaternion movedRotation = Quaternion.FromEulerAngles(forwardRotation);
            ObjectPrivate.Mover.AddRotateOffset(movedRotation, 4 / rotationSpeed, MoveMode.Smoothstep);
        }
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Go Backward
        {
            forwardPosition.X = 0.0f;
            forwardPosition.Y = -positionOffset;
            forwardPosition.Z = 0.0f;
            //Log.Write("Forward Command forwardPosition: " + forwardPosition + " FreeDelay: " + FreeDelay);
            ObjectPrivate.Mover.AddTranslateOffset(forwardPosition, FreeDelay, MoveMode.Smoothstep);
        }
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Down
        {
            forwardPosition.X = 0.0f;
            forwardPosition.Y = 0.0f;
            forwardPosition.Z = -positionOffset;
            ObjectPrivate.Mover.AddTranslateOffset(forwardPosition, FreeDelay, MoveMode.Smoothstep);
            //Log.Write("Forward Command forwardPosition: " + forwardPosition + " FreeDelay: " + FreeDelay);
        }
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Rotate Left
        {
            forwardRotation.X = 0.0f * 0.0174533f; 
            forwardRotation.Y = 0.0f * 0.0174533f;  
            forwardRotation.Z = (float)Math.PI;
            //Log.Write("CameraRotation: " + forwardRotation + " Forward Vector: " + ObjectPrivate.ForwardVector + " Right Vector: " + ObjectPrivate.RightVector);
            //if (ObjectPrivate.ForwardVector.Y < 0) forwardRotation.Z = -(float)Math.PI;
            //else forwardRotation.Z = (float)Math.PI;
            Quaternion movedRotation = Quaternion.FromEulerAngles(forwardRotation);
            ObjectPrivate.Mover.AddRotateOffset(movedRotation, 4 / rotationSpeed, MoveMode.Smoothstep);
        }
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) MoveFreeCamera = 5;
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Rotate Right
        {
            forwardRotation.X = 0.0f * 0.0174533f; //float.Parse(CameraMovementRotX[CameraMovementNumberIn]) * 0.0174533f;
            forwardRotation.Y = 0.0f * 0.0174533f;  // float.Parse(CameraMovementRotY[CameraMovementNumberIn]) * 0.0174533f;
            forwardRotation.Z = -(float)Math.PI;
            //Log.Write("CameraRotation: " + CameraRotation + " TargetRotation: " + targetRotation);
            Quaternion movedRotation = Quaternion.FromEulerAngles(forwardRotation);
            ObjectPrivate.Mover.AddRotateOffset(movedRotation, 4 / rotationSpeed, MoveMode.Smoothstep);
        }
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (!(Shifted)))
        {
            forwardRotation.X = (float)Math.PI;
            forwardRotation.Y = 0.0f * 0.0174533f;
            forwardRotation.Z = 0.0f * 0.0174533f;
            //Log.Write("CameraRotation: " + forwardRotation + " Forward Vector: " + ObjectPrivate.ForwardVector + " Right Vector: " + ObjectPrivate.RightVector);
            Quaternion movedRotation = Quaternion.FromEulerAngles(forwardRotation);
            ObjectPrivate.Mover.AddRotateOffset(movedRotation, 4 / rotationSpeed, MoveMode.Smoothstep);
        }
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Go Forward
        {
            forwardPosition.X = 0.0f;
            forwardPosition.Y = positionOffset;
            forwardPosition.Z = 0.0f;
            ObjectPrivate.Mover.AddTranslateOffset(forwardPosition, FreeDelay, MoveMode.Smoothstep);
            //Log.Write("Forward Command forwardPosition: " + forwardPosition + " FreeDelay: " + FreeDelay);
        }
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) //Up
        {
            forwardPosition.X = 0.0f;
            forwardPosition.Y = 0.0f;
            forwardPosition.Z = positionOffset;
            ObjectPrivate.Mover.AddTranslateOffset(forwardPosition, FreeDelay, MoveMode.Smoothstep);
            //Log.Write("Forward Command forwardPosition: " + forwardPosition + " FreeDelay: " + FreeDelay);
        }

        if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (!(Shifted))) 
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (!(Shifted))) 
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (!(Shifted))) 
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (!(Shifted))) 
        {
            ObjectPrivate.Mover.StopAndClear();
        }
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (!(Shifted)))
        {
            ObjectPrivate.Mover.StopAndClear();
        }

        //ObjectPrivate.Mover.AddMove(currentPosition, movedRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]) * rotateFactor, MoveMode.Smoothstep);
    }

    void CommandReceived(CommandData Button)
    {
        if ((Button.Command == "Modifier") && (Button.Action == CommandAction.Pressed))  //shift acts a toggle
        {
            if (Shifted == true) Shifted = false;
            else Shifted = true;
        }

        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[0]);
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[1]);
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[2]);
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[3]);
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[4]);
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[5]);
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[6]);
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[7]);
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[8]);
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[9]);
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[10]);
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[11]);
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[12]);
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[13]);
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[14]);
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[15]);
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[16]);
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[17]);
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[18]);
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(CameraMovementEvent[19]);
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[20]);
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[21]);
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[22]);
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[23]);
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[24]);
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[25]);
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[26]);
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[27]);
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[28]);
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[29]);
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[30]);
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[31]);
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[32]);
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[33]);
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[34]);
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[35]);
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[36]);
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[37]);
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[38]);
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(CameraMovementEvent[39]);
        /*
        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "1Off");
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "2Off");
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "3Off");
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "4Off");
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "5Off");
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "6Off");
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "7Off");
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "8Off");
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "9Off");
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "10Off");
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "11Off");
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "12Off");
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "13Off");
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "14Off");
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "15Off");
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "16Off");
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "17Off");
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "18Off");
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "19Off");
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (!(Shifted))) SendKey(BaseEventName + "2Off");
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "21Off");
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "22Off");
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "23Off");
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "24Off");
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "25Off");
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "26Off");
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "27Off");
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "28Off");
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "29Off");
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "30Off");
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "31Off");
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "32Off");
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "33Off");
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "34Off");
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "35Off");
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "36Off");
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "37Off");
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "38Off");
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "39Off");
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (Shifted)) SendKey(BaseEventName + "40Off");
        */
    }

    void CommandCanceled(CancelData data)
    {
        //Log.Write(GetType().Name, "Subscription canceled: " + data.Message);
    }

    void SendKey(string KeyIn)
    {
        sendSimpleMessage(KeyIn);
    }

    private void sendSimpleMessage(string msg)
    {
        //Log.Write("Event from Command Data Interface: " + msg);
        SimpleData sd = new SimpleData(this);
        sd.AgentInfo = agent.AgentInfo;
        sd.ObjectId = agent.AgentInfo.ObjectId;
        sd.ObjectId = agent.AgentInfo.ObjectId;
        sd.SourceObjectId = ObjectPrivate.ObjectId;
        SendToAll(msg, sd);
    }

    #endregion

    public override void Init()
    {
        ScenePrivate.Chat.Subscribe(0, GetChatCommand);
        if (!string.IsNullOrWhiteSpace(GuestNameIn))
        {
            GuestName = GuestNameIn;
        }
    
        if (!ObjectPrivate.TryGetFirstComponent<CameraComponent>(out CamComponent))
        {
            Log.Write("No camera component found!");
            return;
        }
        Log.Write("Found camera component");

        if (!ObjectPrivate.TryGetFirstComponent<MeshComponent>(out CameraMesh))
        {
            Log.Write("No camera Mesh component found!");
            return;
        }
        //Log.Write("Found camera mesh");

        if (!string.IsNullOrWhiteSpace(activateCameraComponent))
        {
            SubscribeToAll(activateCameraComponent, (data) =>
            {
                ISimpleData idata = data.Data.AsInterface<ISimpleData>();
                agent = ScenePrivate.FindAgent(idata.ObjectId);

                SubscribeKeyPressed(agent, "sub");

                if (agent != null)
                {
                    agent.Client.SetActiveCamera(CamComponent, ScriptCameraControlMode.NoControl);
                }
            });
        }

        forwardPosition = ObjectPrivate.Position;
        forwardPosition.X = 0.0f;
        forwardPosition.Y = 0.0f;
        forwardPosition.Z = 0.0f;

        forwardRotation = ObjectPrivate.Position;
        forwardRotation.X = 0.0f;
        forwardRotation.Y = 0.0f;
        forwardRotation.Z = 0.0f;

        //Log.Write("FreeCameraEvent: " + FreeCameraEvent);
        SubscribeToAll(FreeCameraEvent, StartFreeCamera);
        SubscribeToAll(FreeCameraEvent + "Off", StopFreeCamera);

        if (!string.IsNullOrWhiteSpace(resetCameraComponent))
        {
            SubscribeToAll(resetCameraComponent, (data) =>
            {
                ISimpleData idata = data.Data.AsInterface<ISimpleData>();
                AgentPrivate agent = ScenePrivate.FindAgent(idata.ObjectId);

                SubscribeKeyPressed(agent, "unsub");

                if (agent != null)
                    agent.Client.ResetCamera();
            });
        }
        SubscribeToAll(killCameraMovementEvent, KillCameraMovement);
        LoadCameraAngles();

    }

    private void LoadCameraAngles()

    {
        int loopCntr = 0;
        //Log.Write("CameraMovement1To20.Count: " + CameraMovement1.Count());
        if (CameraMovement1.Count() > 0)
        {
            do
            {
                ParseCameraMovement(CameraMovement1[loopCntr]);
                loopCntr++;
            } while (loopCntr < CameraMovement1.Count());
        }
        //Log.Write("CameraMovement21To40.Count: " + CameraMovement2.Count());
        if (CameraMovement2.Count() > 0)
        {
            loopCntr = 21;
            do
            {
                ParseCameraMovement(CameraMovement1[loopCntr]);
                loopCntr++;
            } while (loopCntr < CameraMovement1.Count());
        }
        NumberOfCameraAngles = loopCntr+1;
    }

    private void ParseCameraMovement(string CameraMovementIn)
    {
        //Event,EventDone,DoneEventSendTiming,SpecialCameraType,Global/LocalPosition,PosX,PosY,PosZ,RotX,RotY,RotZ,TimeDurationOfMove
        //Log.Write("In ParseCameraMovement CameraMovementIn: " + CameraMovementIn);
        CameraMovementArray.Clear();
        CameraMovementArray = CameraMovementIn.Split(',').ToList();
        if (CameraMovementArray.Count != 13) ScenePrivate.Chat.MessageAllUsers("Bad Camera Control String Wrong Number of Parameters: " + CameraMovementIn);
        CameraMovementEvent.Add(CameraMovementArray[0]);
        CameraMovementDoneEvent.Add(CameraMovementArray[1]);
        CameraDoneEventDelay.Add(CameraMovementArray[2]);
        CameraType.Add(CameraMovementArray[3]);
        CameraTargetType.Add(CameraMovementArray[4]);
        CameraMovementPosX.Add(CameraMovementArray[5]);
        CameraMovementPosY.Add(CameraMovementArray[6]);
        CameraMovementPosZ.Add(CameraMovementArray[7]);
        CameraMovementRotX.Add(CameraMovementArray[8]);
        CameraMovementRotY.Add(CameraMovementArray[9]);
        CameraMovementRotZ.Add(CameraMovementArray[10]);
        CameraMovementTimeDuration.Add(CameraMovementArray[11]);
        CameraOrbitRadius.Add(CameraMovementArray[12]);

        SubscribeToAll(CameraMovementArray[0], ExecuteCameraMovement);
        //Log.Write("Subscribed to: " + CameraMovementArray[0]);
    }

    private void KillCameraMovement(ScriptEventData data)
    {
        //Log.Write("In KillCameraMovement");
        killCameraMovement = true;
    }

    private void ExecuteCameraMovement(ScriptEventData data)
    {
        //Log.Write("In Execute Camera Movement data message: " + data.Message);
        //Log.Write("Camera Movement Event Count: " + CameraMovementEvent.Count());
        //Log.Write("Camera Movement Event 0: " + CameraMovementEvent[0]);
        if (data.Message == CameraMovementEvent[0]) PlayCameraMovement(0, data);
        else if (data.Message == CameraMovementEvent[1]) PlayCameraMovement(1, data);
        else if (data.Message == CameraMovementEvent[2]) PlayCameraMovement(2, data);
        else if (data.Message == CameraMovementEvent[3]) PlayCameraMovement(3, data);
        else if (data.Message == CameraMovementEvent[4]) PlayCameraMovement(4, data);
        else if (data.Message == CameraMovementEvent[5]) PlayCameraMovement(5, data);
        else if (data.Message == CameraMovementEvent[6]) PlayCameraMovement(6, data);
        else if (data.Message == CameraMovementEvent[7]) PlayCameraMovement(7, data);
        else if (data.Message == CameraMovementEvent[8]) PlayCameraMovement(8, data);
        else if (data.Message == CameraMovementEvent[9]) PlayCameraMovement(9, data);
        else if (data.Message == CameraMovementEvent[10]) PlayCameraMovement(10, data);
        else if (data.Message == CameraMovementEvent[11]) PlayCameraMovement(11, data);
        else if (data.Message == CameraMovementEvent[12]) PlayCameraMovement(12, data);
        else if (data.Message == CameraMovementEvent[13]) PlayCameraMovement(13, data);
        else if (data.Message == CameraMovementEvent[14]) PlayCameraMovement(14, data);
        else if (data.Message == CameraMovementEvent[15]) PlayCameraMovement(15, data);
        else if (data.Message == CameraMovementEvent[16]) PlayCameraMovement(16, data);
        else if (data.Message == CameraMovementEvent[17]) PlayCameraMovement(17, data);
        else if (data.Message == CameraMovementEvent[18]) PlayCameraMovement(18, data);
        else if (data.Message == CameraMovementEvent[19]) PlayCameraMovement(19, data);
        else if (data.Message == CameraMovementEvent[20]) PlayCameraMovement(20, data);
        else if (data.Message == CameraMovementEvent[21]) PlayCameraMovement(21, data);
        else if (data.Message == CameraMovementEvent[22]) PlayCameraMovement(22, data);
        else if (data.Message == CameraMovementEvent[23]) PlayCameraMovement(23, data);
        else if (data.Message == CameraMovementEvent[24]) PlayCameraMovement(24, data);
        else if (data.Message == CameraMovementEvent[25]) PlayCameraMovement(25, data);
        else if (data.Message == CameraMovementEvent[26]) PlayCameraMovement(26, data);
        else if (data.Message == CameraMovementEvent[27]) PlayCameraMovement(27, data);
        else if (data.Message == CameraMovementEvent[28]) PlayCameraMovement(28, data);
        else if (data.Message == CameraMovementEvent[29]) PlayCameraMovement(29, data);
        else if (data.Message == CameraMovementEvent[30]) PlayCameraMovement(30, data);
        else if (data.Message == CameraMovementEvent[31]) PlayCameraMovement(31, data);
        else if (data.Message == CameraMovementEvent[32]) PlayCameraMovement(32, data);
        else if (data.Message == CameraMovementEvent[33]) PlayCameraMovement(33, data);
        else if (data.Message == CameraMovementEvent[34]) PlayCameraMovement(34, data);
        else if (data.Message == CameraMovementEvent[35]) PlayCameraMovement(35, data);
        else if (data.Message == CameraMovementEvent[36]) PlayCameraMovement(36, data);
        else if (data.Message == CameraMovementEvent[37]) PlayCameraMovement(37, data);
        else if (data.Message == CameraMovementEvent[38]) PlayCameraMovement(38, data);
        else if (data.Message == CameraMovementEvent[39]) PlayCameraMovement(39, data);
    }

    private void PlayCameraMovement(int CameraMovementNumberIn, ScriptEventData data)
    {
        //Log.Write("In PlayCameraMovement CameraMovementNumber: " + CameraMovementNumberIn);

        //Log.Write("Global/Local: " + CameraMovementGlobalLocalPos[CameraMovementNumberIn]);
        //Log.Write("RigidBody Pos: " + rigidBody.GetPosition() + " Rot: " + rigidBody.GetOrientation());

        startPosition = ObjectPrivate.Position;
        startRotation = ObjectPrivate.Rotation;

        //Log.Write("CameraTargetType[CameraMovementNumberIn]: " + CameraTargetType[CameraMovementNumberIn]);
        // this Code Sets Target Position
        if (CameraTargetType[CameraMovementNumberIn] == "Host")
        {
            //Log.Write("Found Host");
            targetAgent = agent;
            targetPosition = ScenePrivate.FindObject(agent.AgentInfo.ObjectId).Position;
            targetRotation = ScenePrivate.FindObject(agent.AgentInfo.ObjectId).Rotation;
            targetForwardVector = ScenePrivate.FindObject(agent.AgentInfo.ObjectId).ForwardVector;
            targetRightVector = ScenePrivate.FindObject(agent.AgentInfo.ObjectId).RightVector;
        }
        else if (CameraTargetType[CameraMovementNumberIn] == "Guest")
        {
            //Log.Write("GuestName: " + GuestName);
            bool targetAgentFound = false;
            foreach (AgentPrivate agents in ScenePrivate.GetAgents())
            {
                //Log.Write("agents.AgentInfo.Name: " + agents.AgentInfo.Name);
                if (agents.AgentInfo.Name == GuestName)
                {
                    //Log.Write("Found Guest");
                    targetAgent = agents;
                    //Log.Write("targetAgent: " + targetAgent);
                    targetPosition = ScenePrivate.FindObject(agents.AgentInfo.ObjectId).Position;
                    targetRotation = ScenePrivate.FindObject(agents.AgentInfo.ObjectId).Rotation;
                    targetForwardVector = ScenePrivate.FindObject(agents.AgentInfo.ObjectId).ForwardVector;
                    targetRightVector = ScenePrivate.FindObject(agents.AgentInfo.ObjectId).RightVector;
                    //Log.Write("TargetPosition: " + targetPosition);
                }
            }
            if (!targetAgentFound) agent.SendChat("No Guest Found");             
        }
        else if (CameraTargetType[CameraMovementNumberIn] == "Global")
        {
            targetPosition.X = 0.0f;
            targetPosition.Y = 0.0f;
            targetPosition.Z = 0.0f;
            targetRotation.X = 0.0f;
        }

        //Log.Write("CameraType[CameraMovementNumberIn]: " + CameraType[CameraMovementNumberIn]);

        if ((CameraType[CameraMovementNumberIn] == "LookAt") || (CameraType[CameraMovementNumberIn] == "Dolly"))
        {
            //Log.Write("In LookAt");
            //Log.Write("rotateFactor: " + rotateFactor);

            CameraRotation.X = float.Parse(CameraMovementRotX[CameraMovementNumberIn]) * 0.0174533f;
            CameraRotation.Y = float.Parse(CameraMovementRotY[CameraMovementNumberIn]) * 0.0174533f;
            CameraRotation.Z = float.Parse(CameraMovementRotZ[CameraMovementNumberIn]) * 0.0174533f;
            CameraPosition.X = float.Parse(CameraMovementPosX[CameraMovementNumberIn]);
            CameraPosition.Y = float.Parse(CameraMovementPosY[CameraMovementNumberIn]);
            CameraPosition.Z = float.Parse(CameraMovementPosZ[CameraMovementNumberIn]);

            //Log.Write("Camera Rotation: " + CameraRotation + "Target Rotation: " + targetRotation);
            //Log.Write("targetRightVector: " + targetRightVector + " targetForwardVector: " + targetForwardVector + " targetPosition: " + targetPosition + " CameraPosition: " + CameraPosition);
            currentPosition = CameraPosition;
            float XOffset = (targetForwardVector.X * CameraPosition.Y) + (targetRightVector.X * CameraPosition.X);
            currentPosition.X = XOffset + targetPosition.X;
            float YOffset = (targetForwardVector.Y * CameraPosition.Y) + (targetRightVector.Y * CameraPosition.X);
            //Log.Write("XOffset: " + XOffset + " YOffset: " + YOffset);
            currentPosition.Y = YOffset + targetPosition.Y;

            if (CameraType[CameraMovementNumberIn] == "LookAt")
            {
                double Opposite = CameraPosition.X;
                double Adjacent = CameraPosition.Y;
                double angle = Math.Atan((Opposite / Adjacent));
                //Log.Write("Opposite: " + Opposite + " Adjacent: " + Adjacent + " angle: " + angle);
                Vector calcRot = CameraRotation;
                calcRot.X = 0;
                calcRot.Y = 0;
                if (CameraPosition.Y > 0)
                {
                    calcRot.Z = -(float)angle;
                }
                else
                {
                    calcRot.Z = -(float)angle - (180.0f * 0.0174533f);
                }
                //Log.Write("calcRot: " + calcRot);
                //Log.Write("New CameraPosition: " + currentPosition);

                WaitFor(ObjectPrivate.Mover.AddMove, currentPosition, targetRotation * Quaternion.FromEulerAngles(CameraRotation) * Quaternion.FromEulerAngles(calcRot), double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
            }
            if (CameraType[CameraMovementNumberIn] == "Dolly")
            {
                //Log.Write(" in Dolly targetPosition: " + targetPosition);
                //Log.Write(" X: " + CameraMovementPosX[CameraMovementNumberIn] + " Y: " + CameraMovementPosY[CameraMovementNumberIn] + " Z: " + CameraMovementPosZ[CameraMovementNumberIn]);

                if ((currentPosition.X == 0.0f) && (currentPosition.Y == 0.0f) && (currentPosition.Z == 0.0f))  //Don't Change Position
                {
                    moveCamera = false;
                }

                CameraRotation.X = float.Parse(CameraMovementRotX[CameraMovementNumberIn]) * 0.0174533f;
                CameraRotation.Y = float.Parse(CameraMovementRotY[CameraMovementNumberIn]) * 0.0174533f;
                CameraRotation.Z = float.Parse(CameraMovementRotZ[CameraMovementNumberIn]) * 0.0174533f;
                //Log.Write("CameraRotation: " + CameraRotation + " TargetRotation: " + targetRotation);
                Quaternion movedRotation = Quaternion.FromEulerAngles(CameraRotation);
                if ((CameraRotation.X == 0.0f) && (CameraRotation.Y == 0.0f) && (CameraRotation.Z == 0.0f))  //Don't Change Rotation
                {
                    rotateCamera = false;
                }

                if ((moveCamera) && (rotateCamera))
                {
                    if (CameraTargetType[CameraMovementNumberIn] == "Global")
                    {
                        WaitFor(ObjectPrivate.Mover.AddMove, CameraPosition, movedRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                    }
                    else
                    {
                        WaitFor(ObjectPrivate.Mover.AddMove, currentPosition, movedRotation * targetRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                    }
                       
                }
                else if ((!moveCamera) && (rotateCamera)) //just Rotate/Pan Camera
                {
                    if (CameraTargetType[CameraMovementNumberIn] == "Global")
                    {
                        WaitFor(ObjectPrivate.Mover.AddRotate, movedRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                        moveCamera = true;
                    }
                    else
                    {
                        WaitFor(ObjectPrivate.Mover.AddRotate, movedRotation * targetRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                        moveCamera = true;
                    }
                }
                else if ((moveCamera) && (!rotateCamera)) //just Move Camera
                {
                    if (CameraTargetType[CameraMovementNumberIn] == "Global")
                    {
                        WaitFor(ObjectPrivate.Mover.AddTranslate, CameraPosition, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                        rotateCamera = true;
                    }
                    else
                    {
                        WaitFor(ObjectPrivate.Mover.AddTranslate, currentPosition, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]), MoveMode.Smoothstep);
                        rotateCamera = true;
                    }
                }
            }
        }
        else if (CameraType[CameraMovementNumberIn] == "Orbit")
        {
            float rotateFactor = 1.0f / 360.0f;

            //Log.Write("rotateFactor: " + rotateFactor);

            CameraRotation.X = 0.0f;
            CameraRotation.Y = 0.0f;
            CameraRotation.Z = 0.0f;
            CameraPosition.X = targetPosition.X + float.Parse(CameraMovementPosX[CameraMovementNumberIn]);
            CameraPosition.Y = targetPosition.Y + float.Parse(CameraMovementPosY[CameraMovementNumberIn]);
            CameraPosition.Z = targetPosition.Z + float.Parse(CameraMovementPosZ[CameraMovementNumberIn]);

            float radius = float.Parse(CameraOrbitRadius[CameraMovementNumberIn]);
            //Log.Write("Camera Rotation: " + CameraRotation + " Camera Position: " + CameraPosition + " targetPosition" + targetPosition);

            Quaternion movedRotation;
            float xChange = 0;
            float yChange = 0;
            float angle = 0;
            currentPosition = CameraPosition;
            float LookAtRadian = 1.570797f; 
            for (int i = 1; i < 361; i++)
            {
                angle = (float)i * 0.0174533f;
                xChange = radius * (float)Math.Cos(angle) + CameraPosition.X;
                yChange = radius * (float)Math.Sin(angle) + CameraPosition.Y;
                currentPosition.X = xChange;
                currentPosition.Y = yChange;
                //CameraRotation.Z = angle;   //this will follow the circle
                //CameraRotation.Z = angle + LookAtRadian;   //this will look away from the axis
                CameraRotation.Z = angle + LookAtRadian;   //this will look at the axis

                movedRotation = Quaternion.FromEulerAngles(CameraRotation);  //this will follow the circle
                WaitFor(ObjectPrivate.Mover.AddMove, currentPosition, movedRotation, double.Parse(CameraMovementTimeDuration[CameraMovementNumberIn]) * rotateFactor, MoveMode.Smoothstep);
            }
        }

        if (killCameraMovement)
        {
            //Log.Write("Killing Camera");
            //agent.Client.ResetCamera();
            killCameraMovement = false;
        }
        else
        {
            string DoneEvent = CameraMovementDoneEvent[CameraMovementNumberIn];
            Wait(TimeSpan.FromSeconds(double.Parse(CameraDoneEventDelay[CameraMovementNumberIn])));
            SendToAll(DoneEvent, data.Data);
            //killCameraMovement = false;
            //Log.Write("Sent Done Event: " + DoneEvent);
        }
    }
    
    private void StartFreeCamera(ScriptEventData freedata)
    {
        //Log.Write("In StartFreeCamera");
        freeCamera = true;
        if (agent == null)
        {
            ISimpleData idata = freedata.Data.AsInterface<ISimpleData>();
            agent = ScenePrivate.FindAgent(idata.ObjectId);
        }
        SubscribeKeyPressed(agent, "unsub");
        Wait(TimeSpan.FromSeconds(2));
        SubscribeKeyPressed(agent, "sub");
    }

    private void StopFreeCamera(ScriptEventData freedata)
    {
        freeCamera = false;
    }

}


