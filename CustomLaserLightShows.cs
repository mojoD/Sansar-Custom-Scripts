//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;


public class CustomLaserLightShows : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue(".")]
    public readonly Interaction ComplexInteraction;

    public bool Debug = false;

    private string ControlSurfaceR0C1 = "OSDrums,45.2085,-32.9175,56.9315,-39.4825,56.9315,-32.9175,45.2085,-32.9175,0,20";
    private string ControlSurfaceR0C2 = "OSSynths,30.8455,-39.4825,42.5685,-39.4825,42.5685,-32.9175,30.8455,-32.9175,0,20";
    private string ControlSurfaceR0C3 = "OSAmbient,15.9285,-39.4825,27.6515,-39.4825,27.6515,-32.9175,15.9285,-32.9175,0,20";
    private string ControlSurfaceR0C4 = "OSMedia,1.5865,-39.4825,13.3095,-39.4825,13.3095,-32.9175,1.5865,-32.9175,0,20";
    private string ControlSurfaceR0C5 = "OSDJ,-13.3095,-39.4825,-1.5865,-39.4825,-1.5865,-32.9175,-13.3095,-32.9175,0,20";
    private string ControlSurfaceR0C6 = "OSEffects,-27.8385,-39.4825,-16.1155,-39.4825,-16.1155,-32.9175,-27.8385,-32.9175,0,20";
    private string ControlSurfaceR0C7 = "OSEffects2,-42.4515,-39.4825,-30.7285,-39.4825,-30.7285,-32.9175,-42.4515,-32.9175,0,20";
    private string ControlSurfaceR0C8 = "OSDrops,-56.9615,-39.4825,-45.2385,-39.4825,-45.2385,-32.9175,-56.9615,-32.9175,0,20";
    private string ControlSurfaceR1C1 = "R1C1,45.2085,-26.2825,56.9315,-26.2825,56.9315,-19.7175,45.2085,-19.7175,0,20";
    private string ControlSurfaceR1C2 = "R1C2,30.8455,-26.2825,42.5685,-26.2825,42.5685,-19.7175,30.8455,-19.7175,0,20";
    private string ControlSurfaceR1C3 = "R1C3,15.9285,-26.2825,27.6515,-26.2825,27.6515,-19.7175,15.9285,-19.7175,0,20";
    private string ControlSurfaceR1C4 = "R1C4,1.5865,-26.2825,13.3095,-26.2825,13.3095,-19.7175,1.5865,-19.7175,0,20";
    private string ControlSurfaceR1C5 = "R1C5,-13.3095,-26.2825,-1.5865,-26.2825,-1.5865,-19.7175,-13.3095,-19.7175,0,20";
    private string ControlSurfaceR1C6 = "R1C6,-27.8385,-26.2825,-16.1155,-26.2825,-16.1155,-19.7175,-27.8385,-19.7175,0,20";
    private string ControlSurfaceR1C7 = "R1C7,-42.4515,-26.2825,-30.7285,-26.2825,-30.7285,-19.7175,-42.4515,-19.7175,0,20";
    private string ControlSurfaceR1C8 = "R1C8,-56.9615,-26.2825,-45.2385,-26.2825,-45.2385,-19.7175,-56.9615,-19.7175,0,20";
    private string ControlSurfaceR2C1 = "R2C1,45.2085,-16.3825,56.9315,-16.3825,56.9315,-9.8175,45.2085,-9.8175,0,20";
    private string ControlSurfaceR2C2 = "R2C2,30.8455,-16.3825,42.5685,-16.3825,42.5685,-9.8175,30.8455,-9.8175,0,20";
    private string ControlSurfaceR2C3 = "R2C3,15.9285,-16.3825,27.6515,-16.3825,27.6515,-9.8175,15.9285,-9.8175,0,20";
    private string ControlSurfaceR2C4 = "R2C4,1.5865,-16.3825,13.3095,-16.3825,13.3095,-9.8175,1.5865,-9.8175,0,20";
    private string ControlSurfaceR2C5 = "R2C5,-13.3095,-16.3825,-1.5865,-16.3825,-1.5865,-9.8175,-13.3095,-9.8175,0,20";
    private string ControlSurfaceR2C6 = "R2C6,-27.8385,-16.3825,-16.1155,-16.3825,-16.1155,-9.8175,-27.8385,-9.8175,0,20";
    private string ControlSurfaceR2C7 = "R2C7,-42.4515,-16.3825,-30.7285,-16.3825,-30.7285,-9.8175,-42.4515,-9.8175,0,20";
    private string ControlSurfaceR2C8 = "R2C8,-56.9615,-16.3825,-45.2385,-16.3825,-45.2385,-9.8175,-56.9615,-9.8175,0,20";
    private string ControlSurfaceR3C1 = "R3C1,45.2085,-6.0825,56.9315,-6.0825,56.9315,0.4825,45.2085,0.4825,0,20";
    private string ControlSurfaceR3C2 = "R3C2,30.8455,-6.0825,42.5685,-6.0825,42.5685,0.4825,30.8455,0.4825,0,20";
    private string ControlSurfaceR3C3 = "R3C3,15.9285,-6.0825,27.6515,-6.0825,27.6515,0.4825,15.9285,0.4825,0,20";
    private string ControlSurfaceR3C4 = "R3C4,1.5865,-6.0825,13.3095,-6.0825,13.3095,0.4825,1.5865,0.4825,0,20";
    private string ControlSurfaceR3C5 = "R3C5,-13.3095,-6.0825,-1.5865,-6.0825,-1.5865,0.4825,-13.3095,0.4825,0,20";
    private string ControlSurfaceR3C6 = "R3C6,-27.8385,-6.0825,-16.1155,-6.0825,-16.1155,0.4825,-27.8385,0.4825,0,20";
    private string ControlSurfaceR3C7 = "R3C7,-42.4515,-6.0825,-30.7285,-6.0825,-30.7285,0.4825,-42.4515,0.4825,0,20";
    private string ControlSurfaceR3C8 = "R3C8,-56.9615,-6.0825,-45.2385,-6.0825,-45.2385,0.4825,-56.9615,0.4825,0,20";
    private string ControlSurfaceR4C1 = "R4C1,45.2085,4.0675,56.9315,4.0675,56.9315,10.6325,45.2085,10.6325,0,20";
    private string ControlSurfaceR4C2 = "R4C2,30.8455,4.0675,42.5685,4.0675,42.5685,10.6325,30.8455,10.6325,0,20";
    private string ControlSurfaceR4C3 = "R4C3,15.9285,4.0675,27.6515,4.0675,27.6515,10.6325,15.9285,10.6325,0,20";
    private string ControlSurfaceR4C4 = "R4C4,1.5865,4.0675,13.3095,4.0675,13.3095,10.6325,1.5865,10.6325,0,20";
    private string ControlSurfaceR4C5 = "R4C5,-13.3095,4.0675,-1.5865,4.0675,-1.5865,10.6325,-13.3095,10.6325,0,20";
    private string ControlSurfaceR4C6 = "R4C6,-27.8385,4.0675,-16.1155,4.0675,-16.1155,10.6325,-27.8385,10.6325,0,20";
    private string ControlSurfaceR4C7 = "R4C7,-42.4515,4.0675,-30.7285,4.0675,-30.7285,10.6325,-42.4515,10.6325,0,20";
    private string ControlSurfaceR4C8 = "R4C8,-56.9615,4.0675,-45.2385,4.0675,-45.2385,10.6325,-56.9615,10.6325,0,20";
    private string ControlSurfaceR5C1 = "R5C1,45.2085,14.8175,56.9315,14.8175,56.9315,21.3825,45.2085,21.3825,0,20";
    private string ControlSurfaceR5C2 = "R5C2,30.8455,14.8175,42.5685,14.8175,42.5685,21.3825,30.8455,21.3825,0,20";
    private string ControlSurfaceR5C3 = "R5C3,15.9285,14.8175,27.6515,14.8175,27.6515,21.3825,15.9285,21.3825,0,20";
    private string ControlSurfaceR5C4 = "R5C4,1.5865,14.8175,13.3095,14.8175,13.3095,21.3825,1.5865,21.3825,0,20";
    private string ControlSurfaceR5C5 = "R5C5,-13.3095,14.8175,-1.5865,14.8175,-1.5865,21.3825,-13.3095,21.3825,0,20";
    private string ControlSurfaceR5C6 = "R5C6,-27.8385,14.8175,-16.1155,14.8175,-16.1155,21.3825,-27.8385,21.3825,0,20";
    private string ControlSurfaceR5C7 = "R5C7,-42.4515,14.8175,-30.7285,14.8175,-30.7285,21.3825,-42.4515,21.3825,0,20";
    private string ControlSurfaceR5C8 = "R5C8,-56.9615,14.8175,-45.2385,14.8175,-45.2385,21.3825,-56.9615,21.3825,0,20";

    private float[] ControlSurfaceAXRelative = new float[48];
    private float[] ControlSurfaceAYRelative = new float[48];
    private float[] ControlSurfaceBXRelative = new float[48];
    private float[] ControlSurfaceBYRelative = new float[48];
    private float[] ControlSurfaceCXRelative = new float[48];
    private float[] ControlSurfaceCYRelative = new float[48];
    private float[] ControlSurfaceDXRelative = new float[48];
    private float[] ControlSurfaceDYRelative = new float[48];
    private float[] ControlSurfaceZMinimum = new float[48];
    private float[] ControlSurfaceZMaximum = new float[48];
    private float[] ControlSurfaceAXRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceAYRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceBXRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceBYRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceCXRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceCYRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceDXRelativeAfterRotation = new float[48];
    private float[] ControlSurfaceDYRelativeAfterRotation = new float[48];

    private string UsersToListenTo = "ALL";
    private List<string> ValidUsers = new List<string>();
    bool validUser = false;
    private string[] ControlSurfaceMessage = new string[48];
    private AgentPrivate Hitman;
    //private RigidBodyComponent RigidBody = null;
    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly Vector WarehouseRot = new Vector(0.0f, 0.0f, 0.0f);
    Quaternion RotQuat = Quaternion.FromEulerAngles(WarehouseRot).Normalized();
    private double ZRotation = new double();
    private int NumOfControlSurfaces = 0;
    private Vector NoVector;
    private int BPM = 0;
    private bool runRandomShow = false;
    private string Change = null;
    private string[] LaserIDSelectionsArray = new string[10] { "ALL", "ALL", "ALL", "ALL", "ALL", "Laser1", "Laser2", "Laser3", "Laser4", "Laser5" };
    private string[] LaserTypeSelectionsArray = new string[4] { "ALL", "RedLaser", "GreenLaser", "BlueLaser" };
    private bool[] RandomBlinkSelectorArray = new bool[4] { true, false, false, false };
    private string[] RandomPanAxisSelectorArray = new string[3] { "X", "Z", "Y" };
    private string[] RandomEffectSelectionArray = new string[9] { "SpinAndChangeLights", "SpinAndChangeLights", "SpinAndChangeLights", "PanAndChangeLights", "PanAndChangeLights", "PanAndChangeLights", "FrontLights", "FrontLights", "TopLights" };

    #endregion

    public override void Init()
    {
        Log.Write("In CustomLaserLightShows");
        ComplexInteraction.SetPrompt(".");
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        ScenePrivate.Chat.Subscribe(0, null, GetChatCommand);
        SubscribeToScriptEvent("SecurityList", getSecurity);
        InitializeComplexInteraction();
    }

    private void InitializeLaserValues()
    {

    }

    private void InitializeComplexInteraction()
    {
        //CurPos = RigidBody.GetPosition();
        //Log.Write("CurPos: " + CurPos);
        //ZRotation = RigidBody.GetOrientation().GetEulerAngles().Z * 57.2958;
        //Log.Write("Zrotation: " + ZRotation);

        CurPos = ObjectPrivate.InitialPosition;
        ZRotation = ObjectPrivate.InitialRotation.GetEulerAngles().Z * 57.2958;

        LoadControlSurfaces(ControlSurfaceR0C1, 0);
        LoadControlSurfaces(ControlSurfaceR0C2, 1);
        LoadControlSurfaces(ControlSurfaceR0C3, 2);
        LoadControlSurfaces(ControlSurfaceR0C4, 3);
        LoadControlSurfaces(ControlSurfaceR0C5, 4);
        LoadControlSurfaces(ControlSurfaceR0C6, 5);
        LoadControlSurfaces(ControlSurfaceR0C7, 6);
        LoadControlSurfaces(ControlSurfaceR0C8, 7);
        LoadControlSurfaces(ControlSurfaceR1C1, 8);
        LoadControlSurfaces(ControlSurfaceR1C2, 9);
        LoadControlSurfaces(ControlSurfaceR1C3, 10);
        LoadControlSurfaces(ControlSurfaceR1C4, 11);
        LoadControlSurfaces(ControlSurfaceR1C5, 12);
        LoadControlSurfaces(ControlSurfaceR1C6, 13);
        LoadControlSurfaces(ControlSurfaceR1C7, 14);
        LoadControlSurfaces(ControlSurfaceR1C8, 15);
        LoadControlSurfaces(ControlSurfaceR2C1, 16);
        LoadControlSurfaces(ControlSurfaceR2C2, 17);
        LoadControlSurfaces(ControlSurfaceR2C3, 18);
        LoadControlSurfaces(ControlSurfaceR2C4, 19);
        LoadControlSurfaces(ControlSurfaceR2C5, 20);
        LoadControlSurfaces(ControlSurfaceR2C6, 21);
        LoadControlSurfaces(ControlSurfaceR2C7, 22);
        LoadControlSurfaces(ControlSurfaceR2C8, 23);
        LoadControlSurfaces(ControlSurfaceR3C1, 24);
        LoadControlSurfaces(ControlSurfaceR3C2, 25);
        LoadControlSurfaces(ControlSurfaceR3C3, 26);
        LoadControlSurfaces(ControlSurfaceR3C4, 27);
        LoadControlSurfaces(ControlSurfaceR3C5, 28);
        LoadControlSurfaces(ControlSurfaceR3C6, 29);
        LoadControlSurfaces(ControlSurfaceR3C7, 30);
        LoadControlSurfaces(ControlSurfaceR3C8, 31);
        LoadControlSurfaces(ControlSurfaceR4C1, 32);
        LoadControlSurfaces(ControlSurfaceR4C2, 33);
        LoadControlSurfaces(ControlSurfaceR4C3, 34);
        LoadControlSurfaces(ControlSurfaceR4C4, 35);
        LoadControlSurfaces(ControlSurfaceR4C5, 36);
        LoadControlSurfaces(ControlSurfaceR4C6, 37);
        LoadControlSurfaces(ControlSurfaceR4C7, 38);
        LoadControlSurfaces(ControlSurfaceR4C8, 39);
        LoadControlSurfaces(ControlSurfaceR5C1, 40);
        LoadControlSurfaces(ControlSurfaceR5C2, 41);
        LoadControlSurfaces(ControlSurfaceR5C3, 42);
        LoadControlSurfaces(ControlSurfaceR5C4, 43);
        LoadControlSurfaces(ControlSurfaceR5C5, 44);
        LoadControlSurfaces(ControlSurfaceR5C6, 45);
        LoadControlSurfaces(ControlSurfaceR5C7, 46);
        LoadControlSurfaces(ControlSurfaceR5C8, 47);
        NumOfControlSurfaces = 48;
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

    public class LightInfo : Reflective
    {
        public string LightEvent { get; set; }
        public string LightIntensity { get; set; }
        public bool LightColorRandom { get; set; }
        public string LightColorRed { get; set; }
        public string LightColorGreen { get; set; }
        public string LightColorBlue { get; set; }
        public string LightColorAlpha { get; set; }
        public string LightAngle { get; set; }
        public string LightFallOff { get; set; }
        public string LightEffect { get; set; }
        public string LightEffectParm1 { get; set; }
        public string LightEffectParm2 { get; set; }
        public string LightEffectParm3 { get; set; }
    }

    private void SendLightInfo(string LightEventIn, string LightIntensityIn, bool LightColorRandomIn, string LightColorRedIn, string LightColorGreenIn, string LightColorBlueIn, string LightColorAlphaIn, string LightAngleIn, string LightFallOffIn, string LightEffectIn, string LightEffectParm1In, string LightEffectParm2In, string LightEffectParm3In)
    {
        //Log.Write("LightEventIn: " + LightEventIn);
        //Log.Write("LightIntensityIn" + LightIntensityIn);
        //Log.Write("LightColorRandomIn: " + LightColorRandomIn);
        //Log.Write("LightColorRedIn: " + LightColorRedIn);
        //Log.Write("LightColorGreenIn: " + LightColorGreenIn);
        //Log.Write("LightColorBlueIn: " + LightColorBlueIn);
        //Log.Write("LightColorAlphaIn: " + LightColorAlphaIn);
        //Log.Write("LightAngleIn: " + LightAngleIn);
        //Log.Write("LightFallOffIn: " + LightFallOffIn);
        //Log.Write("LightEffectIn: " + LightEffectIn);
        //Log.Write("LightEffectParm1In: " + LightEffectParm1In);
        //Log.Write("LightEffectParm2In: " + LightEffectParm2In);
        //Log.Write("LightEffectParm3In: " + LightEffectParm3In);

        LightInfo sendLightInfo = new LightInfo();
        sendLightInfo.LightEvent = LightEventIn;
        sendLightInfo.LightIntensity = LightIntensityIn;
        sendLightInfo.LightColorRandom = LightColorRandomIn;
        sendLightInfo.LightColorRed = LightColorRedIn;
        sendLightInfo.LightColorGreen = LightColorGreenIn;
        sendLightInfo.LightColorBlue = LightColorBlueIn;
        sendLightInfo.LightColorAlpha = LightColorAlphaIn;
        sendLightInfo.LightAngle = LightAngleIn;
        sendLightInfo.LightFallOff = LightFallOffIn;
        sendLightInfo.LightEffect = LightEffectIn;
        sendLightInfo.LightEffectParm1 = LightEffectParm1In;
        sendLightInfo.LightEffectParm2 = LightEffectParm2In;
        sendLightInfo.LightEffectParm3 = LightEffectParm3In;

        PostScriptEvent(ScriptId.AllScripts, "LightInfo", sendLightInfo);
        Log.Write("CustomLaserLightController - Sending LightInfo");
    }

    public class AnimationInfo : Reflective
    {
        public string AnimationEvent { get; set; }
        public string startFrame { get; set; }
        public string endFrame { get; set; }
        public string PlaybackType { get; set; }
        public string AnimationSpeed { get; set; }
        public string BlendDuration { get; set; }
    }

    private void SendAnimationInfo(string AnimationEventIn, string startFrameIn, string endFrameIn, string PlaybackTypeIn, string AnimationSpeedIn, string BlendDurationIn)
    {
        Log.Write("AnimationEventIn: " + AnimationEventIn);
        Log.Write("startFrameIn: " + startFrameIn);
        Log.Write("endFrameIn: " + endFrameIn);
        Log.Write("PlaybackTypeIn: " + PlaybackTypeIn);
        Log.Write("AnimationSpeedIn: " + AnimationSpeedIn);
        Log.Write("BlendDurationIn: " + BlendDurationIn);

        AnimationInfo sendAnimationInfo = new AnimationInfo();
        sendAnimationInfo.AnimationEvent = AnimationEventIn;
        sendAnimationInfo.startFrame = startFrameIn;
        sendAnimationInfo.endFrame = endFrameIn;
        sendAnimationInfo.PlaybackType = PlaybackTypeIn;
        sendAnimationInfo.AnimationSpeed = AnimationSpeedIn;
        sendAnimationInfo.BlendDuration = BlendDurationIn;

        PostScriptEvent(ScriptId.AllScripts, "AnimationInfo", sendAnimationInfo);
        Log.Write("CustomLaserLightController - Sending AnimationInfo");
    }

    public class PanAngleInfo : Reflective
    {
        public string LaserID { get; set; }
        public string PanAxis { get; set; }
        public float PanAngle { get; set; }
    }

    private void SendPanAngle(string inLaser, string inPanAxis, float inPanAngle)
    {
        //Log.Write("inPanAxis: " + inPanAxis + " inPanAngle: " + inPanAngle);
        PanAngleInfo sendPanAngleInfo = new PanAngleInfo();
        sendPanAngleInfo.LaserID = inLaser;
        sendPanAngleInfo.PanAxis = inPanAxis;
        sendPanAngleInfo.PanAngle = inPanAngle;

        PostScriptEvent(ScriptId.AllScripts, "PanAngleMsg", sendPanAngleInfo);
        //Log.Write("CustomLaserLightController - Sending PanAngle");
    }

    public class PanRateInfo : Reflective
    {
        public string LaserID { get; set; }
        public float inPanRate { get; set; }
    }

    private void SendPanRate(string inLaser, float inPanRate2)
    {
        //Log.Write("inPanRate: " + inPanRate2);
        PanRateInfo sendPanRateInfo = new PanRateInfo();
        sendPanRateInfo.LaserID = inLaser;
        sendPanRateInfo.inPanRate = inPanRate2;

        //Log.Write("sendPanRateInfo.inPanRate: " + sendPanRateInfo.inPanRate);
        PostScriptEvent(ScriptId.AllScripts, "PanRateMsg", sendPanRateInfo);
        //Log.Write("CustomLaserLightController - Sending PanRate");
    }

    /*
        public class SpinAngleInfo : Reflective
        {
            public string LaserID { get; set; }
            public float inSpinAngle { get; set; }
        }

        private void SendSpinAngle(string inLaser, float inSpinAngle2)
        {
            //Log.Write("inSpinAngle: " + inSpinAngle2);
            SpinAngleInfo sendSpinAngleInfo = new SpinAngleInfo();
            sendSpinAngleInfo.LaserID = inLaser;
            sendSpinAngleInfo.inSpinAngle = inSpinAngle2;

            //Log.Write("sendSpinAngleInfo.inSpinAngle: " + sendSpinAngleInfo.inSpinAngle);
            PostScriptEvent(ScriptId.AllScripts, "SpinAngleMsg", sendSpinAngleInfo);
            //Log.Write("CustomLaserLightController - Sending SpinAngle");
        }

        public class SpinRateInfo : Reflective
        {
            public string LaserID { get; set; }
            public float inSpinRate { get; set;  }
        }

        private void SendSpinRate(string inLaser, float inSpinRate2)
        {
            //Log.Write("inSpinRates: " + inSpinRate2);
            SpinRateInfo sendSpinRateInfo = new SpinRateInfo();
            sendSpinRateInfo.LaserID = inLaser;
            sendSpinRateInfo.inSpinRate = inSpinRate2;

            //Log.Write("sendSpinRateInfo.inSpinRate: " + sendSpinRateInfo.inSpinRate);
            PostScriptEvent(ScriptId.AllScripts, "SpinRateMsg", sendSpinRateInfo);
            //Log.Write("CustomLaserLightController - Sending SpinRate");
        }
    */

    public class FullSpinInfo : Reflective
    {
        public string LaserID { get; set; }
        public string inFullSpin { get; set; }
    }

    private void SendFullSpin(string inLaser, string inSpinType)
    {
        //Log.Write("inSpinType: " + inSpinType);
        FullSpinInfo sendFullSpinInfo = new FullSpinInfo();
        sendFullSpinInfo.LaserID = inLaser;
        sendFullSpinInfo.inFullSpin = inSpinType;

        //Log.Write("sendFullSpinInfo.inFullSpin: " + sendFullSpinInfo.inFullSpin);
        PostScriptEvent(ScriptId.AllScripts, "SpinTypeMsg", sendFullSpinInfo);
        //Log.Write("CustomLaserLightController - Sending SpinType");
    }

    public class LaserTypeInfo : Reflective
    {
        public string LaserID { get; set; }
        public string inLaserType { get; set; }
    }

    private void SendLaserType(string inLaserID, string LaserTypeSelected)
    {
        LaserTypeInfo sendLaserTypeInfo = new LaserTypeInfo();
        sendLaserTypeInfo.LaserID = inLaserID;
        sendLaserTypeInfo.inLaserType = LaserTypeSelected;

        PostScriptEvent(ScriptId.AllScripts, "LaserType", sendLaserTypeInfo);
        //Log.Write("CustomLaserLightController - Sending LaserType");
    }

    public class BlinkInfo : Reflective
    {
        public string LaserID { get; set; }
        public bool LaserBlink { get; set; }
    }

    private void SendBlinkInfo(string inLaser, bool currentBlink)
    {
        BlinkInfo sendBlinkInfo = new BlinkInfo();
        sendBlinkInfo.LaserID = inLaser;
        sendBlinkInfo.LaserBlink = currentBlink;

        PostScriptEvent(ScriptId.AllScripts, "LaserBlink", sendBlinkInfo);
        //Log.Write("CustomLaserLightController - Sending BlinkInfo");
    }

    public class BlinkRateInfo : Reflective
    {
        public string LaserID { get; set; }
        public float LaserBlinkRate { get; set; }
    }

    private void SendBlinkRateInfo(string inLaser, float currentBlinkRate)
    {
        BlinkRateInfo sendBlinkRateInfo = new BlinkRateInfo();
        sendBlinkRateInfo.LaserID = inLaser;
        sendBlinkRateInfo.LaserBlinkRate = currentBlinkRate;

        PostScriptEvent(ScriptId.AllScripts, "LaserBlinkRate", sendBlinkRateInfo);
        //Log.Write("CustomLaserLightController - Sending BlinkRateInfo");
    }

    public class RelPosInfo : Reflective
    {
        public string LaserID { get; set; }
        public string PositionAxis { get; set; }
        public string Direction { get; set; }
    }

    private void SendRelPosInfo(string inLaser, string inPositionAxis, string inDirection)
    {
        //Log.Write("inPositionAxis: " + inPositionAxis + " inDirection: " + inDirection);
        RelPosInfo sendRelPosInfo = new RelPosInfo();
        sendRelPosInfo.LaserID = inLaser;
        sendRelPosInfo.PositionAxis = inPositionAxis;
        sendRelPosInfo.Direction = inDirection;

        PostScriptEvent(ScriptId.AllScripts, "ChangeRelPos", sendRelPosInfo);
        //Log.Write("CustomLaserLightController - Sending RelPosition");
    }

    public class PosChangeRateInfo : Reflective
    {
        public string LaserID { get; set; }
        public double PositionChangeRate { get; set; }
    }

    private void SendPosChangeRateInfo(string inLaser, double PositionChangeRateIn)
    {
        PosChangeRateInfo sendPosChangeRateInfo = new PosChangeRateInfo();
        sendPosChangeRateInfo.LaserID = inLaser;
        sendPosChangeRateInfo.PositionChangeRate = PositionChangeRateIn;

        PostScriptEvent(ScriptId.AllScripts, "ChangePosChangeRate", sendPosChangeRateInfo);
        //Log.Write("CustomLaserLightController - Sending Position Change Rate");
    }

    public class AngleChangeInfo : Reflective
    {
        public string LaserID { get; set; }
        public float inAngleChangeSpeed { get; set; }
        public int inTargetWorldRotation { get; set; }
    }

    private void SendAngleChangeSpeed(string inLaser, float AngleChangeSpeedIn)
    {
        AngleChangeInfo sendAngleChangeSpeed = new AngleChangeInfo();
        sendAngleChangeSpeed.LaserID = inLaser;
        sendAngleChangeSpeed.inAngleChangeSpeed = AngleChangeSpeedIn;
        sendAngleChangeSpeed.inTargetWorldRotation = 1;

        PostScriptEvent(ScriptId.AllScripts, "ChangeAngleSpeed", sendAngleChangeSpeed);
        //Log.Write("CustomLaserLightController - Sending Change Angle Speed");
    }

    public class WorldPosInfo : Reflective
    {
        public string LaserID { get; set; }
        public int WorldPosition { get; set; }
    }

    private void SendWorldPosInfo(string inLaser, int PositionIndex)
    {
        WorldPosInfo sendWorldPosInfo = new WorldPosInfo();
        sendWorldPosInfo.LaserID = inLaser;
        sendWorldPosInfo.WorldPosition = PositionIndex;

        PostScriptEvent(ScriptId.AllScripts, "ChangeWorldPos", sendWorldPosInfo);
        //Log.Write("CustomLaserLightController - Sending World Position");
    }

    public class RotChangeRateInfo : Reflective
    {
        public string LaserID { get; set; }
        public double RotationChangeRate { get; set; }
    }

    private void SendRotChangeRateInfo(string inLaser, double RotationChangeRateIn)
    {
        RotChangeRateInfo sendRotChangeRateInfo = new RotChangeRateInfo();
        sendRotChangeRateInfo.LaserID = inLaser;
        sendRotChangeRateInfo.RotationChangeRate = RotationChangeRateIn;

        PostScriptEvent(ScriptId.AllScripts, "ChangeRotChangeRate", sendRotChangeRateInfo);
        //Log.Write("CustomLaserLightController - Sending Rotation Change Rate");
    }

    public class WorldRotInfo : Reflective
    {
        public string LaserID { get; set; }
        public int WorldRotation { get; set; }
        public int TargetWorldRotation { get; set; }
        public float PctLoopComplete { get; set; }
    }

    private void SendWorldRotInfo(string inLaser, int PositionIndex, int TargetPositionIndex, float inPctLoopcomplete)
    {
        WorldRotInfo sendWorldRotInfo = new WorldRotInfo();
        sendWorldRotInfo.LaserID = inLaser;
        sendWorldRotInfo.WorldRotation = PositionIndex;
        sendWorldRotInfo.TargetWorldRotation = TargetPositionIndex;
        sendWorldRotInfo.PctLoopComplete = inPctLoopcomplete;

        PostScriptEvent(ScriptId.AllScripts, "ChangeWorldRot", sendWorldRotInfo);
        //Log.Write("CustomLaserLightShow - Sending WorldRotation");
    }

    private void SendStillRotInfo(string inLaser, int PositionIndex)
    {
        WorldRotInfo sendWorldRotInfo = new WorldRotInfo();
        sendWorldRotInfo.LaserID = inLaser;
        sendWorldRotInfo.WorldRotation = PositionIndex;

        PostScriptEvent(ScriptId.AllScripts, "ChangeStillRot", sendWorldRotInfo);
        //Log.Write("CustomLaserLightShow - Sending Still Rotation");
    }

    #endregion

    #region ChatDialog
    private void GetChatCommand(ChatData Data)
    {

        Log.Write(Data.Message);

        if (Data.Message.Contains("bpm"))
        {
            string InString = Data.Message;
            int from = InString.IndexOf("(", StringComparison.CurrentCulture);
            int to = InString.IndexOf(")", StringComparison.CurrentCulture);
            int length = InString.Length;
            int last = to - from;
            //Log.Write("InString: " + InString + " from: " + from + " to: " + to + " length: " + length + " last: " + last);
            string chunk = InString.Substring(from + 1, last - 1);
            BPM = Int32.Parse(chunk);
            //Log.Write("BPM: " + BPM);
        }
    }
    #endregion

    #region Shows

    public void Template(float inTime, bool loopIn)
    {
        //string showLaserID = "ALL";
        //SendFullSpin(showLaserID, "NoSpin");

        //string showLaserType = "RedLaser";
        //SendLaserType(showLaserID, showLaserType);
        //bool showBlink = false;
        //SendBlinkInfo(showLaserID, showBlink);
        //float showBlinkRate = 1.0f;
        //SendBlinkRateInfo(showLaserID, showBlinkRate);

        //SendWorldPosInfo(showLaserID, 1);
        //SendWorldRotInfo(showLaserID, 1);
        //Wait(TimeSpan.FromSeconds(1));
        //SendWorldRotInfo(showLaserID, 2);
        //string showRelAxis = "X";
        //string showRelDirection = "+";
        //SendRelPosInfo(showLaserID, showRelAxis, showRelDirection);


        //float showSpinRate = 1.0f;
        //SendSpinRate(showLaserID, showSpinRate);
        //float showSpinAngle = -40f;
        //SendSpinAngle(showLaserID, showSpinAngle);
        //Wait(TimeSpan.FromSeconds(0.5));
        //string showSpinType = "FullSpin";
        //SendFullSpin(showLaserID, showSpinType);

        //float showPanRate = 1.0f;
        //SendPanRate(showLaserID, showPanRate);
        //string showPanAngleAxis = "X";
        //float showPanAngle = 20f;
        //SendPanAngle(showLaserID, showPanAngleAxis, showPanAngle);

        //Wait(TimeSpan.FromSeconds(inTime));

        //SendFullSpin(showLaserID, "NoSpin");
    }

    //Position over Dance Floor, Spin Lights, No Angle Change
    public void SpinAndChangeLights(float looptimeIn, int loops, string LaserID, string LaserTypeIn, bool blinkIn, float blinkRate, int PositionIn, double PosChangeRateIn, float PanAngleIn, float PanRateIn, string PanAxis, int RotationIn, int TargetRotationIn)
    {
        float looptime = looptimeIn;
        double PosChangeRate = PosChangeRateIn;
        if (BPM > 0)
        {
            looptime = 60 / (float)BPM * looptimeIn;
            PosChangeRate = 60 / (double)BPM * looptimeIn;
        }

        string showLaserID = LaserID;
        SendFullSpin(showLaserID, "NoSpin");
        //Wait(TimeSpan.FromSeconds(0.5));

        showLaserID = "Laser1";
        SendWorldPosInfo(showLaserID, PositionIn);
        showLaserID = "Laser2";
        SendWorldPosInfo(showLaserID, PositionIn);
        showLaserID = "Laser3";
        SendWorldPosInfo(showLaserID, PositionIn);
        showLaserID = "Laser4";
        SendWorldPosInfo(showLaserID, PositionIn);
        showLaserID = "Laser5";
        SendWorldPosInfo(showLaserID, PositionIn);
        //Wait(TimeSpan.FromSeconds(0.5));

        showLaserID = "ALL";
        string showLaserType;
        if (blinkIn)
        {
            bool showBlink = blinkIn;
            SendBlinkInfo(showLaserID, showBlink);
            float showBlinkRate = blinkRate;
            SendBlinkRateInfo(showLaserID, showBlinkRate);
        }

        SendWorldRotInfo(showLaserID, RotationIn, RotationIn, 1.0f);

        float showSpinRate = PanRateIn;
        SendPanRate(showLaserID, showSpinRate);
        //float showSpinAngle = PanAngleIn;
        //SendSpinAngle(showLaserID, showSpinAngle);

        //Wait(TimeSpan.FromSeconds(0.5));
        string showSpinType = "FullSpin";
        SendFullSpin(showLaserID, showSpinType);

        int loopCntr = 0;
        float pctLoopComplete = 0.0f;
        if (LaserTypeIn == "ALL")
        {
            do
            {
                showLaserType = "BlueLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "GreenLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "RedLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                loopCntr++;
                pctLoopComplete = (float)loopCntr / (float)loops;
                SendWorldRotInfo(showLaserID, RotationIn, TargetRotationIn, pctLoopComplete);
            } while (loopCntr < loops);
        }
        else
        {
            showLaserType = LaserTypeIn;
            SendLaserType(showLaserID, showLaserType);

            do
            {
                Wait(TimeSpan.FromSeconds(looptime));
                loopCntr++;
                pctLoopComplete = (float)loopCntr / (float)loops;
                SendWorldRotInfo(showLaserID, RotationIn, TargetRotationIn, pctLoopComplete);
            } while (loopCntr < (loops));
            //Wait(TimeSpan.FromSeconds(inTime * loops));
        }

        SendFullSpin(showLaserID, "NoSpin");
    }

    //Pan Lights from Edges
    public void PanAndChangeLights(float looptimeIn, int loops, string LaserID, string LaserTypeIn, bool blinkIn, float blinkRate, int PositionIn, double PosChangeRateIn, float PanAngleIn, float PanRateIn, string PanAxis, int RotationIn, int TargetRotationIn)
    {
        float looptime = looptimeIn;
        double PosChangeRate = PosChangeRateIn;
        if (BPM > 0)
        {
            looptime = 60 / (float)BPM * looptimeIn;
            PosChangeRate = 60 / (double)BPM * looptimeIn;
        }

        string showLaserID = LaserID;
        SendFullSpin(showLaserID, "NoSpin");
        //Wait(TimeSpan.FromSeconds(0.5));
        SendPosChangeRateInfo(showLaserID, PosChangeRate);
        //Wait(TimeSpan.FromSeconds(0.5));
        SendWorldPosInfo(showLaserID, PositionIn);

        //Wait(TimeSpan.FromSeconds(0.5));

        showLaserID = "ALL";
        string showLaserType;
        if (blinkIn)
        {
            bool showBlink = blinkIn;
            SendBlinkInfo(showLaserID, showBlink);
            float showBlinkRate = blinkRate;
            SendBlinkRateInfo(showLaserID, showBlinkRate);
        }

        SendWorldRotInfo(showLaserID, RotationIn, RotationIn, 0.05f);
        //Wait(TimeSpan.FromSeconds(0.5));

        string showSpinType = "PanSpin";
        SendFullSpin(showLaserID, showSpinType);
        float showPanRate = PanRateIn;
        SendPanRate(showLaserID, showPanRate);
        //Wait(TimeSpan.FromSeconds(0.5));

        string showPanAngleAxis = "X";
        float showPanAngle = PanAngleIn;
        SendPanAngle(showLaserID, showPanAngleAxis, showPanAngle);

        int loopCntr = 0;
        if (LaserTypeIn == "ALL")
        {
            do
            {
                showLaserType = "BlueLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "GreenLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "RedLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                loopCntr++;
                float loopPctReduction = (1 - loopCntr / loops);
                //Log.Write("Sending new Rotation");
                SendWorldRotInfo(showLaserID, RotationIn, TargetRotationIn, loopPctReduction);
            } while (loopCntr < loops);
        }
        else
        {
            showLaserType = LaserTypeIn;
            SendLaserType(showLaserID, showLaserType);
            do
            {
                Wait(TimeSpan.FromSeconds(looptime));
                loopCntr++;
            } while (loopCntr < (loops));
        }

        SendFullSpin(showLaserID, "NoSpin");
    }

    //Lights in Front
    public void FrontLights(float looptimeIn, int loops, string LaserID, string LaserTypeIn, bool blinkIn, float blinkRate, int PositionIn, double PosChangeRateIn, float PanAngleIn, float PanRateIn, string PanAxis, int RotationIn, int TargetRotationIn)
    {
        float looptime = looptimeIn;
        double PosChangeRate = PosChangeRateIn;
        if (BPM > 0)
        {
            looptime = 60 / (float)BPM * looptimeIn;
            PosChangeRate = 60 / (double)BPM * looptimeIn;
        }

        string showLaserID = LaserID;
        SendFullSpin(showLaserID, "NoSpin");
        SendPosChangeRateInfo(showLaserID, PosChangeRate);
        //Wait(TimeSpan.FromSeconds(0.5));
        SendWorldPosInfo(showLaserID, PositionIn);

        SendStillRotInfo(showLaserID, RotationIn);
        //Wait(TimeSpan.FromSeconds(0.5));

        string showLaserType;
        if (blinkIn)
        {
            bool showBlink = blinkIn;
            SendBlinkInfo(showLaserID, showBlink);
            float showBlinkRate = blinkRate;
            SendBlinkRateInfo(showLaserID, showBlinkRate);
        }



        int loopCntr = 0;
        if (LaserTypeIn == "ALL")
        {
            do
            {
                showLaserType = "BlueLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "GreenLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "RedLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                loopCntr++;
            } while (loopCntr < loops);
        }
        else
        {
            showLaserType = LaserTypeIn;
            SendLaserType(showLaserID, showLaserType);
            do
            {
                Wait(TimeSpan.FromSeconds(looptime));
                loopCntr++;
            } while (loopCntr < (loops));
        }

        SendFullSpin(showLaserID, "NoSpin");
    }

    //Lights on top
    public void TopLights(float looptimeIn, int loops, string LaserID, string LaserTypeIn, bool blinkIn, float blinkRate, int PositionIn, double PosChangeRateIn, float PanAngleIn, float PanRateIn, string PanAxis, int RotationIn, int TargetRotationIn)
    {
        float looptime = looptimeIn;
        double PosChangeRate = PosChangeRateIn;
        if (BPM > 0)
        {
            looptime = 60 / (float)BPM * looptimeIn;
            PosChangeRate = 60 / (double)BPM * looptimeIn;
        }

        string showLaserID = LaserID;
        SendFullSpin(showLaserID, "NoSpin");
        SendPosChangeRateInfo(showLaserID, PosChangeRate);
        //Wait(TimeSpan.FromSeconds(0.5));
        SendWorldPosInfo(showLaserID, PositionIn);

        SendStillRotInfo(showLaserID, RotationIn);
        //Wait(TimeSpan.FromSeconds(0.5));

        string showLaserType;
        if (blinkIn)
        {
            bool showBlink = blinkIn;
            SendBlinkInfo(showLaserID, showBlink);
            float showBlinkRate = blinkRate;
            SendBlinkRateInfo(showLaserID, showBlinkRate);
        }

        int loopCntr = 0;
        if (LaserTypeIn == "ALL")
        {
            do
            {
                showLaserType = "BlueLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "GreenLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                showLaserType = "RedLaser";
                SendLaserType(showLaserID, showLaserType);
                Wait(TimeSpan.FromSeconds(looptime / 3));
                loopCntr++;
            } while (loopCntr < loops);
        }
        else
        {
            showLaserType = LaserTypeIn;
            SendLaserType(showLaserID, showLaserType);
            do
            {
                Wait(TimeSpan.FromSeconds(looptime));
                loopCntr++;
            } while (loopCntr < (loops));
        }

        SendFullSpin(showLaserID, "NoSpin");
    }

    //Parameters
    // loopTime, loops, LaserID, LaserType, Blink, BlinkRate, Goto Position, Position Change Speed, PanAngle, PanRate, Pan Axis, Start Rotation, RotationChangeRate

    public void SendLaserSpin1()
    {
        //Wait(TimeSpan.FromSeconds(0.5));
        SpinAndChangeLights(0.5f, 25, "ALL", "ALL", false, 1.0f, 3, 1.0, -45.0f, 0.25f, "X", 12, 12);
        SpinAndChangeLights(0.25f, 25, "ALL", "ALL", false, 1.0f, 3, 1.0, -45.0f, 0.15f, "X", 12, 15);
        SpinAndChangeLights(0.25f, 25, "ALL", "ALL", false, 1.0f, 3, 1.0, -45.0f, 0.05f, "X", 15, 12);
        SpinAndChangeLights(0.5f, 25, "ALL", "ALL", false, 1.0f, 3, 1.0, -45.0f, 0.15f, "X", 12, 12);
        SpinAndChangeLights(0.05f, 25, "ALL", "ALL", false, 1.0f, 3, 1.0, -45.0f, 0.25f, "X", 12, 12);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserPan1()
    {
        //Wait(TimeSpan.FromSeconds(0.5));
        SendLaserType("None", "NoLaser");
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 13, 13);
        PanAndChangeLights(0.33f, 36, "ALL", "RedLaser", false, 0.1f, 1, 1.0, 20.0f, 0.33f, "X", 13, 13);
        PanAndChangeLights(0.03f, 400, "ALL", "GreenLaser", false, 0.1f, 1, 1.0, 40.0f, 0.03f, "X", 13, 13);
        PanAndChangeLights(0.03f, 400, "ALL", "GreenLaser", false, 1.0f, 2, 1.0, 40.0f, 0.03f, "X", 13, 13);
        PanAndChangeLights(0.33f, 36, "ALL", "RedLaser", false, 0.1f, 2, 1.0, 20.0f, 0.33f, "X", 13, 13);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 2, 1.0, 20.0f, 1.0f, "X", 13, 13);
        PanAndChangeLights(0.03f, 400, "ALL", "GreenLaser", false, 1.0f, 7, 1.0, 40.0f, 0.03f, "X", 3, 3);
        PanAndChangeLights(0.33f, 36, "ALL", "RedLaser", false, 0.1f, 7, 1.0, 20.0f, 0.33f, "X", 3, 3);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 7, 1.0, 20.0f, 1.0f, "X", 3, 3);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 6, 1.0, 20.0f, 1.0f, "X", 3, 3);
        PanAndChangeLights(0.33f, 36, "ALL", "RedLaser", false, 0.1f, 6, 1.0, 20.0f, 0.33f, "X", 3, 3);
        PanAndChangeLights(0.03f, 400, "ALL", "GreenLaser", false, 0.1f, 6, 1.0, 40.0f, 0.03f, "X", 3, 3);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserFrontDown1()
    {
        //Wait(TimeSpan.FromSeconds(0.5));
        FrontLights(0.25f, 10, "ALL", "GreenLaser", false, 1.0f, 1, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "RedLaser", false, 1.0f, 4, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "BlueLaser", false, 1.0f, 5, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "BlueLaser", true, 0.2f, 5, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "RedLaser", true, 0.2f, 4, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "GreenLaser", true, 0.2f, 1, 0.5, 20.0f, 0.3f, "X", 1, 1);
        SendLaserType("None", "NoLaser");
        FrontLights(0.025f, 10, "ALL", "GreenLaser", false, 1.0f, 1, 0.05, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.025f, 10, "ALL", "RedLaser", false, 1.0f, 4, 0.05, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.025f, 10, "ALL", "BlueLaser", false, 1.0f, 5, 0.05, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.025f, 10, "ALL", "BlueLaser", true, 0.2f, 5, 0.05, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.025f, 10, "ALL", "RedLaser", true, 0.2f, 4, 0.05, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.025f, 10, "ALL", "GreenLaser", true, 0.2f, 1, 0.05, 20.0f, 0.3f, "X", 1, 1);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserTopOut1()
    {
        TopLights(0.25f, 1, "ALL", "None", false, 1.0f, 2, 0.1, 20.0f, 0.3f, "X", 2, 2);
        TopLights(0.25f, 20, "ALL", "GreenLaser", false, 1.0f, 1, 10.0, 20.0f, 0.3f, "X", 2, 2);
        TopLights(0.25f, 10, "ALL", "RedLaser", false, 1.0f, 1, 0.1, 20.0f, 0.3f, "X", 3, 3);
        TopLights(0.25f, 20, "ALL", "RedLaser", false, 1.0f, 2, 10.0, 20.0f, 0.3f, "X", 3, 3);
        TopLights(0.25f, 20, "ALL", "RedLaser", false, 1.0f, 6, 10.0, 20.0f, 0.3f, "X", 3, 3);
        TopLights(0.25f, 5, "ALL", "RedLaser", false, 1.0f, 6, 5.0, 20.0f, 0.3f, "X", 2, 2);
        TopLights(0.25f, 2, "ALL", "RedLaser", false, 1.0f, 1, 1.0, 20.0f, 0.3f, "X", 1, 1);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserCenterCorners1()
    {
        FrontLights(0.25f, 10, "ALL", "ALL", false, 1.0f, 11, 2.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "ALL", false, 1.0f, 12, 2.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 10, "ALL", "ALL", false, 1.0f, 11, 2.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "RedLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "RedLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "BlueLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "BlueLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "GreenLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "GreenLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "RedLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "RedLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "BlueLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "BlueLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "GreenLaser", false, 1.0f, 12, 0.5, 20.0f, 0.3f, "X", 1, 1);
        FrontLights(0.25f, 2, "ALL", "GreenLaser", false, 1.0f, 11, 0.5, 20.0f, 0.3f, "X", 1, 1);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserPan2()
    {
        TopLights(0.5f, 1, "ALL", "None", false, 1.0f, 1, 0.1, 20.0f, 0.3f, "Z", 2, 2);
        /*
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 1, 1);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 2, 2);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 3, 3);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 4, 4);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 5, 5);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 6, 6);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 7, 7);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 8, 8);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 9, 9);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 10, 10);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 11, 11);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 12, 12);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 13, 13);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 14, 14);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 15, 15);
        PanAndChangeLights(1.0f, 12, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 1.0f, "Z", 16, 16);
        */

        PanAndChangeLights(0.5f, 24, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 0.5f, "Z", 14, 14);
        PanAndChangeLights(0.5f, 24, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 0.5f, "Z", 14, 14);
        PanAndChangeLights(0.5f, 24, "ALL", "GreenLaser", false, 0.1f, 6, 0.1, 20.0f, 0.5f, "Z", 2, 2);
        PanAndChangeLights(0.5f, 24, "ALL", "GreenLaser", false, 0.1f, 6, 0.1, 20.0f, 0.03f, "Z", 2, 2);
        PanAndChangeLights(0.5f, 24, "ALL", "RedLaser", false, 0.1f, 1, 0.1, 20.0f, 0.03f, "Z", 14, 14);
        SendLaserType("None", "NoLaser");
    }

    public void SendLaserShow1()
    {
        //Wait(TimeSpan.FromSeconds(30.0f));
        SendSpot1();
        SendLaserFrontDown1();
        SendLaserTopOut1();
        SendLaserFrontDown1();
        SendLaserPan1();
        SendLaserFrontDown1();
        SendLaserPan2();
        SendLaserFrontDown1();
        SendLaserCenterCorners1();
        SendLaserSpin1();
        SendLaserCenterCorners1();
        SendLaserFrontDown1();
        SendLaserPan2();
        SendSpot1();
    }

    public void RandomShow()
    {
        float RandomloopTime;
        int Randomloops;
        int RandomLaserIDSelector;
        string RandomLaserID;
        int RandomLaserTypeSelector;
        string RandomLaserType;
        int RandomBlinkSelector;
        bool RandomBlink;
        float RandomBlinkRate;
        int RandomGotoPosition;
        double RandomPositionChangeSpeed;
        float RandomPanAngle;
        float RandomPanRate;
        int RandomPanAxisSelector;
        string RandomPanAxis;
        int RandomStartRotation;
        int RandomEndRotation;
        int RandomEffectSelector;
        string RandomEffect;
        int RandomSwitch;

        Random r = new Random();
        RandomloopTime = (float)r.NextDouble();
        Randomloops = r.Next(5, 25);
        RandomLaserIDSelector = r.Next(1, 5);  //only all Lasers
        RandomLaserID = LaserIDSelectionsArray[RandomLaserIDSelector - 1];
        RandomLaserTypeSelector = r.Next(1, 4);
        RandomLaserType = LaserTypeSelectionsArray[RandomLaserTypeSelector - 1];
        RandomBlinkSelector = r.Next(1, 4);
        RandomBlink = RandomBlinkSelectorArray[RandomBlinkSelector - 1];
        RandomBlinkRate = (float)r.NextDouble();
        RandomGotoPosition = r.Next(1, 12);
        RandomPositionChangeSpeed = r.NextDouble();
        RandomPanAngle = (float)(r.NextDouble() * -90.0);
        RandomPanRate = (float)r.NextDouble();
        RandomPanAxisSelector = r.Next(1, 2);
        RandomPanAxis = RandomPanAxisSelectorArray[RandomPanAxisSelector - 1];
        RandomSwitch = r.Next(1, 2);
        if (RandomGotoPosition < 6)
        {
            RandomStartRotation = r.Next(9, 16);
            RandomEndRotation = r.Next(9, 16);
        }
        else if (RandomGotoPosition < 10)
        {
            RandomStartRotation = r.Next(1, 8);
            RandomEndRotation = r.Next(1, 8);
        }
        else
        {
            RandomStartRotation = r.Next(9, 16);
            RandomEndRotation = r.Next(9, 16);
        }
        RandomEffectSelector = r.Next(1, 9);
        RandomEffect = RandomEffectSelectionArray[RandomEffectSelector - 1];

        Log.Write("RandomEffect: " + RandomEffect);
        Log.Write("RandomloopTime: " + RandomloopTime + " Randomloops: " + Randomloops);
        Log.Write("RandomLaserID: " + RandomLaserID + " RandomLaserType: " + RandomLaserType);
        Log.Write("RandomBlink: " + RandomBlink + " RandomBlinkRate: " + RandomBlinkRate);
        Log.Write("RandomGotoPosition: " + RandomGotoPosition + " RandomPositionChangeSpeed: " + RandomPositionChangeSpeed);
        Log.Write("RandomPanAngle: " + RandomPanAngle + " RandomPanRate: " + RandomPanRate + " RandomPanAxis: " + RandomPanAxis);
        Log.Write("RandomStartRotation: " + RandomStartRotation + " RandomEndRotation: " + RandomEndRotation);

        if (RandomEffect == "SpinAndChangeLights")
        {
            SpinAndChangeLights(RandomloopTime, Randomloops, RandomLaserID, RandomLaserType, RandomBlink, RandomBlinkRate, RandomGotoPosition, RandomPositionChangeSpeed, RandomPanAngle, RandomPanRate, RandomPanAxis, RandomStartRotation, RandomEndRotation);
        }
        else if (RandomEffect == "PanAndChangeLights")
        {
            PanAndChangeLights(RandomloopTime, Randomloops, RandomLaserID, RandomLaserType, RandomBlink, RandomBlinkRate, RandomGotoPosition, RandomPositionChangeSpeed, RandomPanAngle, RandomPanRate, RandomPanAxis, RandomStartRotation, RandomEndRotation);
        }
        else if (RandomEffect == "FrontLights")
        {
            FrontLights(RandomloopTime, Randomloops, RandomLaserID, RandomLaserType, RandomBlink, RandomBlinkRate, RandomGotoPosition, RandomPositionChangeSpeed, RandomPanAngle, RandomPanRate, RandomPanAxis, RandomStartRotation, RandomEndRotation);
        }
        else if (RandomEffect == "TopLights")
        {
            TopLights(RandomloopTime, Randomloops, RandomLaserID, RandomLaserType, RandomBlink, RandomBlinkRate, RandomGotoPosition, RandomPositionChangeSpeed, RandomPanAngle, RandomPanRate, RandomPanAxis, RandomStartRotation, RandomEndRotation);
        }
    }

    public void TestRotation()
    {
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 1, 1);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 2, 2);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 3, 3);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 4, 4);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 5, 5);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 6, 6);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 7, 7);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 8, 8);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 9, 9);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 10, 10);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 11, 11);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 12, 12);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 13, 13);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 14, 14);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 15, 15);
        PanAndChangeLights(0.5f, 12, "ALL", "BlueLaser", false, 0.1f, 1, 1.0, 20.0f, 1.0f, "X", 16, 16);
    }

    public void SendRandomShow()
    {
        Random r1 = new Random();
        int rTimes = r1.Next(1, 5);

        double waitTime = r1.NextDouble() * 5.0;
        runRandomShow = true;

        do
        {
            RandomShow();
            //Wait(TimeSpan.FromSeconds(waitTime));
            Log.Write("waitTime: " + waitTime);
        } while (runRandomShow);
        Log.Write("Show Done");
    }

    private void StopRandomShow()
    {
        runRandomShow = false;
        SendLaserType("None", "NoLaser");
    }

    private void SendSpot1()
    {
        //string LightEventOut = "Spot1";
        //string LightIntensityOut = "70";
        //bool LightColorRandomOut = false;
        //string LightColorRedOut = "1";
        //string LightColorGreenOut = "0";
        //string LightColorBlueOut = "0";
        //string LightColorAlphaOut = "1";
        //string LightAngleOut = "90";
        //string LightFallOffOut = "0.5";
        //string LightEffectOut = "spot";
        //string LightEffectParm1Out = "0";  //EffectTime spot - how long between spins, blink how long between blinks
        //string LightEffectParm2Out =" ";  //On Fade and Pulse determines how many steps it will take to go from full intensity to the End Intensity (LightEffectParm3Out) LightEffectParm1Out / LightEffectParm2Out
        //string LightEffectParm3Out = " ";  //On Fade and Pulse the Intensity it Fades or Pulses too
        //SendLightInfo(LightEventOut, LightIntensityOut, LightColorRandomOut, LightColorRedOut, LightColorGreenOut, LightColorBlueOut, LightColorAlphaOut, LightAngleOut, LightFallOffOut, LightEffectOut, LightEffectParm1Out, LightEffectParm2Out, LightEffectParm3Out);
        //string AnimationEventOut = "Motion2";
        //string startFrameOut = "1";
        //string endFrameOut = "40";
        //string PlaybackTypeOut = "loop";
        //string AnimationSpeedOut = "1";
        //string BlendDurationOut = "0";
        //SendAnimationInfo(AnimationEventOut, startFrameOut, endFrameOut, PlaybackTypeOut, AnimationSpeedOut, BlendDurationOut);


        //Get in Initial Positions
        SendWorldRotInfo("Spot1", 12, 12, 0.02f);
        SendPosChangeRateInfo("Spot1", 0.02f);
        SendWorldPosInfo("Spot1", 1);
        SendWorldRotInfo("Spot2", 12, 12, 0.02f);
        SendPosChangeRateInfo("Spot2", 0.02f);
        SendWorldPosInfo("Spot2", 3);
        SendWorldRotInfo("Spot3", 12, 12, 0.02f);
        SendPosChangeRateInfo("Spot3", 0.02f);
        SendWorldPosInfo("Spot3", 7);
        SendWorldRotInfo("Spot4", 12, 12, 0.02f);
        SendPosChangeRateInfo("Spot4", 0.02f);
        SendWorldPosInfo("Spot4", 9);
        SendWorldRotInfo("Spot5", 12, 12, 0.02f);
        SendPosChangeRateInfo("Spot5", 0.02f);
        SendWorldPosInfo("Spot5", 5);
        SendLightInfo("Spot1", "70", false, "1", "0", "0", "1", "50", "0.5", "spot", "0", "0", "0");
        SendAnimationInfo("Spot1", "1", "40", "loop", "1", "0");
        SendLightInfo("Spot2", "70", false, "1", "0", "0", "1", "50", "0.5", "spot", "0", "0", "0");
        SendAnimationInfo("Spot2", "1", "40", "loop", "1", "0");
        SendLightInfo("Spot3", "70", false, "1", "0", "0", "1", "50", "0.5", "spot", "0", "0", "0");
        SendAnimationInfo("Spot3", "1", "40", "loop", "1", "0");
        SendLightInfo("Spot4", "70", false, "1", "0", "0", "1", "50", "0.5", "spot", "0", "0", "0");
        SendAnimationInfo("Spot4", "1", "40", "loop", "1", "0");
        SendLightInfo("Spot5", "70", false, "1", "0", "0", "1", "50", "0.5", "spot", "0", "0", "0");
        SendAnimationInfo("Spot5", "1", "40", "loop", "1", "0");

        //Move To Center
        SendPosChangeRateInfo("Spot1", 3.0f);
        SendWorldPosInfo("Spot1", 5);
        SendPosChangeRateInfo("Spot2", 3.0f);
        SendWorldPosInfo("Spot2", 5);
        SendPosChangeRateInfo("Spot3", 3.0f);
        SendWorldPosInfo("Spot3", 5);
        SendPosChangeRateInfo("Spot4", 3.0f);
        SendWorldPosInfo("Spot4", 5);
        Wait(TimeSpan.FromSeconds(5.0f));

        //Change to Green Light and Move back to Corners
        SendLightInfo("Spot1", "70", false, "0", "1", "0", "1", "50", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot2", "70", false, "0", "1", "0", "1", "50", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot3", "70", false, "0", "1", "0", "1", "50", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot4", "70", false, "0", "1", "0", "1", "50", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot5", "70", false, "0", "1", "0", "1", "50", "0.5", "spot", "1.0", "0", "0");
        SendPosChangeRateInfo("Spot1", 3.0f);
        SendWorldPosInfo("Spot1", 1);
        SendPosChangeRateInfo("Spot2", 3.0f);
        SendWorldPosInfo("Spot2", 3);
        SendPosChangeRateInfo("Spot3", 3.0f);
        SendWorldPosInfo("Spot3", 7);
        SendPosChangeRateInfo("Spot4", 3.0f);
        SendWorldPosInfo("Spot4", 9);
        Wait(TimeSpan.FromSeconds(5.0f));

        //Change to Blue Light in Corner
        SendLightInfo("Spot1", "70", false, "0", "0", "1", "1", "30", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot2", "70", false, "0", "0", "1", "1", "30", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot3", "70", false, "0", "0", "1", "1", "30", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot4", "70", false, "0", "0", "1", "1", "30", "0.5", "spot", "1.0", "0", "0");
        SendLightInfo("Spot5", "70", false, "0", "0", "1", "1", "30", "0.5", "spot", "1.0", "0", "0");
        Wait(TimeSpan.FromSeconds(5.0f));

        //Move to More Center & Random
        SendPosChangeRateInfo("Spot1", 3.0f);
        SendWorldPosInfo("Spot1", 10);
        SendPosChangeRateInfo("Spot2", 3.0f);
        SendWorldPosInfo("Spot2", 11);
        SendPosChangeRateInfo("Spot3", 3.0f);
        SendWorldPosInfo("Spot3", 12);
        SendPosChangeRateInfo("Spot4", 3.0f);
        SendWorldPosInfo("Spot4", 13);
        Wait(TimeSpan.FromSeconds(5.0f));

        SendLightInfo("Spot1", "70", true, "0", "0", "0", "1", "30", "0.25", "spot", "1.0", "0", "0");
        SendLightInfo("Spot2", "70", true, "0", "0", "0", "1", "30", "0.25", "spot", "1.0", "0", "0");
        SendLightInfo("Spot3", "70", true, "0", "0", "0", "1", "30", "0.25", "spot", "1.0", "0", "0");
        SendLightInfo("Spot4", "70", true, "0", "0", "0", "1", "30", "0.25", "spot", "1.0", "0", "0");
        SendLightInfo("Spot5", "70", true, "0", "0", "0", "1", "30", "0.25", "spot", "1.0", "0", "0");
        Wait(TimeSpan.FromSeconds(20.0f));

        SendLightInfo("Spot1", "100", false, "1", "1", "1", "1", "30", "0.25", "blink", "0.01", "0", "0");
        SendLightInfo("Spot2", "100", false, "1", "1", "1", "1", "30", "0.25", "blink", "0.01", "0", "0");
        SendLightInfo("Spot3", "100", false, "1", "1", "1", "1", "30", "0.25", "blink", "0.01", "0", "0");
        SendLightInfo("Spot4", "100", false, "1", "1", "1", "1", "30", "0.25", "blink", "0.01", "0", "0");
        SendLightInfo("Spot5", "100", false, "1", "1", "1", "1", "30", "0.25", "blink", "0.01", "0", "0");
        Wait(TimeSpan.FromSeconds(10.0f));

        //End Show
        SendPosChangeRateInfo("Spot1", 0.02f);
        SendWorldPosInfo("Spot1", 1);
        SendPosChangeRateInfo("Spot2", 0.02f);
        SendWorldPosInfo("Spot2", 3);
        SendPosChangeRateInfo("Spot3", 0.02f);
        SendWorldPosInfo("Spot3", 7);
        SendPosChangeRateInfo("Spot4", 0.02f);
        SendWorldPosInfo("Spot4", 9);
        SendPosChangeRateInfo("Spot5", 0.02f);
        SendWorldPosInfo("Spot5", 5);
        SendLightInfo("Spot1", "0", false, "1", "1", "0", "1", "50", "0.1", "spot", "0", "0", "0");
        SendAnimationInfo("Spot1", "1", "1", "playonce", "1", "0");
        SendLightInfo("Spot2", "0", false, "1", "0", "0", "1", "50", "0.1", "spot", "0", "0", "0");
        SendAnimationInfo("Spot2", "1", "1", "playonce", "1", "0");
        SendLightInfo("Spot3", "0", false, "1", "0", "0", "1", "50", "0.1", "spot", "0", "0", "0");
        SendAnimationInfo("Spot3", "1", "1", "playonce", "1", "0");
        SendLightInfo("Spot4", "0", false, "1", "0", "0", "1", "50", "0.1", "spot", "0", "0", "0");
        SendAnimationInfo("Spot4", "1", "1", "playonce", "1", "0");
        SendLightInfo("Spot5", "0", false, "1", "0", "0", "1", "50", "0.1", "spot", "0", "0", "0");
        SendAnimationInfo("Spot5", "1", "1", "playonce", "1", "0");

    }

    #endregion

    #region Interaction

    private void LoadControlSurfaces(string ControlSurfaceInputString, int cntr)
    {
        //Log.Write("In Load Control Surfaces: " + cntr);
        //Takes Relative Values read in from configuration and converts them to realworld position 
        string[] values = new string[100];
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        //Log.Write("sendNotePositions.SendNotePosition.Count(): " + sendNotePositions.SendNotePosition.Count());

        //Log.Write("ZRotation: " + ZRotation);
        //Log.Write("cntr: " + cntr);
        values = ControlSurfaceInputString.Split(',');
        ControlSurfaceMessage[cntr] = values[0];
        ControlSurfaceAXRelative[cntr] = float.Parse(values[1]);
        ControlSurfaceAYRelative[cntr] = float.Parse(values[2]);
        ControlSurfaceBXRelative[cntr] = float.Parse(values[3]);
        ControlSurfaceBYRelative[cntr] = float.Parse(values[4]);
        ControlSurfaceCXRelative[cntr] = float.Parse(values[5]);
        ControlSurfaceCYRelative[cntr] = float.Parse(values[6]);
        ControlSurfaceDXRelative[cntr] = float.Parse(values[7]);
        ControlSurfaceDYRelative[cntr] = float.Parse(values[8]);
        ControlSurfaceZMinimum[cntr] = float.Parse(values[9]);
        //Log.Write("ControlSurfaceZMinimum[" + cntr + "]: " + ControlSurfaceZMinimum[cntr]);
        ControlSurfaceZMaximum[cntr] = float.Parse(values[10]);
        //Log.Write("ControlSurfaceZMaximum[" + cntr + "]: " + ControlSurfaceZMaximum[cntr]);

        float CosAngle = (float)Math.Cos(ZRotation * 0.0174533);
        float SinAngle = (float)Math.Sin(ZRotation * 0.0174533);

        ControlSurfaceAXRelativeAfterRotation[cntr] = (ControlSurfaceAXRelative[cntr] * CosAngle) - (ControlSurfaceAYRelative[cntr] * SinAngle);
        ControlSurfaceAYRelativeAfterRotation[cntr] = (ControlSurfaceAYRelative[cntr] * CosAngle) + (ControlSurfaceAXRelative[cntr] * SinAngle);
        ControlSurfaceBXRelativeAfterRotation[cntr] = (ControlSurfaceBXRelative[cntr] * CosAngle) - (ControlSurfaceBYRelative[cntr] * SinAngle);
        ControlSurfaceBYRelativeAfterRotation[cntr] = (ControlSurfaceBYRelative[cntr] * CosAngle) + (ControlSurfaceBXRelative[cntr] * SinAngle);
        ControlSurfaceCXRelativeAfterRotation[cntr] = (ControlSurfaceCXRelative[cntr] * CosAngle) - (ControlSurfaceCYRelative[cntr] * SinAngle);
        ControlSurfaceCYRelativeAfterRotation[cntr] = (ControlSurfaceCYRelative[cntr] * CosAngle) + (ControlSurfaceCXRelative[cntr] * SinAngle);
        ControlSurfaceDXRelativeAfterRotation[cntr] = (ControlSurfaceDXRelative[cntr] * CosAngle) - (ControlSurfaceDYRelative[cntr] * SinAngle);
        ControlSurfaceDYRelativeAfterRotation[cntr] = (ControlSurfaceDYRelative[cntr] * CosAngle) + (ControlSurfaceDXRelative[cntr] * SinAngle);

    }

    float Sign(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
    {
        return (p1x - p3x) * (p2y - p3y) - (p2x - p3x) * (p1y - p3y);
    }

    bool IsPointInTri(float ptX, float ptY, float v1X, float v1Y, float v2X, float v2Y, float v3X, float v3Y)
    {
        bool b1, b2, b3;

        b1 = Sign(ptX, ptY, v1X, v1Y, v2X, v2Y) < 0.0f;
        //float b1float = Sign(ptX, ptY, v1X, v1Y, v2X, v2Y);
        //Log.Write("b1float: " + b1float);
        //Log.Write("b1: " + b1);
        b2 = Sign(ptX, ptY, v2X, v2Y, v3X, v3Y) < 0.0f;
        //float b2float = Sign(ptX, ptY, v2X, v2Y, v3X, v3Y);
        //Log.Write("b2float: " + b2float);
        //Log.Write("b2: " + b2);
        b3 = Sign(ptX, ptY, v3X, v3Y, v1X, v1Y) < 0.0f;
        //float b3float = Sign(ptX, ptY, v3X, v3Y, v1X, v1Y);
        //Log.Write("b3float: " + b3float);
        //Log.Write("b3: " + b3);

        return ((b1 == b2) && (b2 == b3));
    }

    bool PointInRectangle(float ptX, float ptY, float AX, float AY, float BX, float BY, float CX, float CY, float DX, float DY)
    {
        //bool test1 = IsPointInTri(ptX, ptY, AX, AY, BX, BY, CX, CY);
        //bool test2 = IsPointInTri(ptX, ptY, AX, AY, CX, CY, DX, DY);
        //Log.Write("Test1: " + test1);
        //Log.Write("Test2: " + test2);
        if (IsPointInTri(ptX, ptY, AX, AY, BX, BY, CX, CY)) return true;   //(X, Y, Z, P)) return true;
        if (IsPointInTri(ptX, ptY, AX, AY, CX, CY, DX, DY)) return true;
        //if (PointInTriangle(X, Z, W, P)) return true;
        return false;
    }

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
               // Log.Write("Valid User: ALL");
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
                //ComplexInteraction.SetPrompt("You Are Not Authorized to Use The Looper");
                //Vector hitPosition = idata.HitPosition;
                //Log.Write("Hit:  " + idata.HitPosition.ToString());
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
        //Log.Write("hitXRelative: " + hitXRelative + " hitYRelative: " + hitYRelative + " hitZRelative: " + hitZRelative);
        int cntr = 0;
        do
        {
            //Log.Write("AX: " + ControlSurfaceAXRelativeAfterRotation[cntr] + " AY: " + ControlSurfaceAYRelativeAfterRotation[cntr] + " BX: " + ControlSurfaceBXRelativeAfterRotation[cntr] + " BY: " + ControlSurfaceBYRelativeAfterRotation[cntr]);
            //Log.Write("CX: " + ControlSurfaceCXRelativeAfterRotation[cntr] + " CY: " + ControlSurfaceCYRelativeAfterRotation[cntr] + " DX: " + ControlSurfaceDXRelativeAfterRotation[cntr] + " DY: " + ControlSurfaceDYRelativeAfterRotation[cntr]);
            bool pointInRectangle = PointInRectangle(hitXRelative, hitYRelative,
                ControlSurfaceAXRelativeAfterRotation[cntr], ControlSurfaceAYRelativeAfterRotation[cntr],
                ControlSurfaceBXRelativeAfterRotation[cntr], ControlSurfaceBYRelativeAfterRotation[cntr],
                ControlSurfaceCXRelativeAfterRotation[cntr], ControlSurfaceCYRelativeAfterRotation[cntr],
                ControlSurfaceDXRelativeAfterRotation[cntr], ControlSurfaceDYRelativeAfterRotation[cntr]);
            if (pointInRectangle)
            {
                //Log.Write("Point in Rectangle");
                if (hitZRelative >= ControlSurfaceZMinimum[cntr] && hitZRelative <= ControlSurfaceZMaximum[cntr])
                {
                    //Simple Message
                    string hitControlSurface = ControlSurfaceMessage[cntr];
                    //Log.Write("Hit Control Surface: " + hitControlSurface);
                    SendLaserCommand(hitControlSurface, idata);
                    break;
                }
            }
            cntr++;
        } while (cntr<NumOfControlSurfaces);
    }

    private void SendLaserCommand(string controlSurface, InteractionData idata)
    {
        string row = controlSurface.Substring(0, 2);
        //Log.Write("row: " + row);
        if (row == "OS")
        {
            Log.Write("Top Row");
            if (controlSurface != null) sendSimpleMessage(controlSurface, idata);
        }
        else
        {
            switch (controlSurface)
            {
                case "R1C1": SendLaserShow1(); break;
                case "R1C2": SendRandomShow(); break;
                //case "R1C2": TestRotation(); break;
                case "R2C2": StopRandomShow(); break;
                case "R1C3": SendLaserSpin1(); break;
                case "R1C4": SendLaserPan1(); break;
                case "R2C4": SendLaserPan2(); break;
                case "R1C5": SendLaserFrontDown1(); break;
                case "R2C5": SendLaserCenterCorners1(); break;
                case "R1C6": SendLaserTopOut1(); break;
                case "R1C7": SendSpot1(); break;
            }
        }
    }

    private void sendSimpleMessage(string msg, InteractionData data)
    {
        SimpleData sd = new SimpleData(this);
        sd.AgentInfo = ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        sd.ObjectId = sd.AgentInfo.ObjectId;
        sd.SourceObjectId = ObjectPrivate.ObjectId;
        Log.Write("Sending msg: " + msg);
        SendToAll(msg, sd);
    }

    #endregion
}
