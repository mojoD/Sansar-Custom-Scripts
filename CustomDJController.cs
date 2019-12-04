//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

// This script is attached to a 3d model.  It has configuration paramters to identify control surfaces on the 3d model.  
// A control surface is a portion of the model that when left mouse clicked on in desktop mode of touched with your hand and 
// trigger in VR mode by the user it will send a simple message and a Reflex Bang that sends the message associated with the 
// control surface.  A good example of this is that the 3d model is a drum set.  Each drum and cymbal you define a Control Surface
// for.  Control Surfaces are Circles.  The Control Surfaces are configured using the followinng structure"
// EventName, XcenterofControlSurface, YcenterofConntrolSurface, RadiusOfControlSurface, Zminimum, Zmaximum
// SnareDrumHit, -12, 35, 25, 0, 200
// The units above are in centimeters and they are all relative from the center of the model.  So, this means that the control surface
// defined says that if the user hits on the model in an area that is within a circle located with an origin of X=-12cm, Y=35cm with a
// radius of 25cm and anywhere from 0 to 200 cm on the Z axis a Simple Message Event named SnareDrumHit will be sent.
//
// This means that any Simple Script Effector could be listening for the Event SnareDrumHit and then execute things like turning on
// a light, moving an object, generating a sound, etc.


public class CustomDJController : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue(".")]
    public readonly Interaction ComplexInteraction;
    //public Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    //public double ZRotationIn = 0.0;
    public float OffTimer = 0;
    public bool Debug = false;

    private string ControlSurface1 = "Station1,54.247,-12.672,43.247,-12.672,43.247,-23.672,54.247,-23.672,80,140";
    private string ControlSurface2 = "Station2,54.247,-0.429,43.247,-0.429,43.247,-11.429,54.247,-11.429,80,140";
    private string ControlSurface3 = "Station3,54.247,13.158,43.247,13.158,43.247,2.158,54.247,2.158,80,140";
    private string ControlSurface4 = "Media,54.247,26.597,43.247,26.597,43.247,15.597,54.247,15.597,80,140";
    private string ControlSurface5 = "Motion1,30.452,-16.24,19.452,-16.24,19.452,-23.24,30.452,-23.24,80,140";
    private string ControlSurface6 = "Motion2,30.452,-7.374,19.452,-7.374,19.452,-14.374,30.452,-14.374,80,140";
    private string ControlSurface7 = "Motion3,30.452,1.521,19.452,1.521,19.452,-5.479,30.452,-5.479,80,140";
    private string ControlSurface8 = "Motion4,30.452,10.206,19.452,10.206,19.452,3.206,30.452,3.206,80,140";
    private string ControlSurface9 = "Motion5,30.452,18.947,19.452,18.947,19.452,11.947,30.452,11.947,80,140";
    private string ControlSurface10 = "Motion6,30.452,27.686,19.452,27.686,19.452,20.686,30.452,20.686,80,140";
    private string ControlSurface11 = "Lights1,6.048,-16.24,-4.952,-16.24,-4.952,-23.24,6.048,-23.24,80,140";
    private string ControlSurface12 = "Lights2,6.048,-7.374,-4.952,-7.374,-4.952,-14.374,6.048,-14.374,80,140";
    private string ControlSurface13 = "Lights3,6.048,1.521,-4.952,1.521,-4.952,-5.479,6.048,-5.479,80,140";
    private string ControlSurface14 = "Lights4,6.048,10.206,-4.952,10.206,-4.952,3.206,6.048,3.206,80,140";
    private string ControlSurface15 = "Lights5,6.048,18.947,-4.952,18.947,-4.952,11.947,6.048,11.947,80,140";
    private string ControlSurface16 = "Lights6,6.048,27.686,-4.952,27.686,-4.952,20.686,6.048,20.686,80,140";
    private string ControlSurface17 = "Effects1,-18.165,-16.24,-29.165,-16.24,-29.165,-23.24,-18.165,-23.24,80,140";
    private string ControlSurface18 = "Effects2,-18.165,-7.374,-29.165,-7.374,-29.165,-14.374,-18.165,-14.374,80,140";
    private string ControlSurface19 = "Effects3,-18.165,1.521,-29.165,1.521,-29.165,-5.479,-18.165,-5.479,80,140";
    private string ControlSurface20 = "Effects4,-18.165,10.206,-29.165,10.206,-29.165,3.206,-18.165,3.206,80,140";
    private string ControlSurface21 = "Effects5,-18.165,18.947,-29.165,18.947,-29.165,11.947,-18.165,11.947,80,140";
    private string ControlSurface22 = "Effects6,-18.165,27.686,-29.165,27.686,-29.165,20.686,-18.165,20.686,80,140";
    private string ControlSurface23 = "Special1,42.053,28.24,53.053,28.24,53.053,35.24,42.053,35.24,80,140";
    private string ControlSurface24 = "Special2,-42.053,-7.374,-53.053,-7.374,-53.053,-14.374,-42.053,-14.374,80,140";
    private string ControlSurface25 = "Special3,-42.053,1.521,-53.053,1.521,-53.053,-5.479,-42.053,-5.479,80,140";
    private string ControlSurface26 = "Special4,-42.053,10.206,-53.053,10.206,-53.053,3.206,-42.053,3.206,80,140";
    private string ControlSurface27 = "Special5,-42.053,18.947,-53.053,18.947,-53.053,11.947,-42.053,11.947,80,140";
    private string ControlSurface28 = "Special6,-42.053,27.686,-53.053,27.686,-53.053,20.686,-42.053,20.686,80,140";

    private float[] ControlSurfaceAXRelative = new float[30];
    private float[] ControlSurfaceAYRelative = new float[30];
    private float[] ControlSurfaceBXRelative = new float[30];
    private float[] ControlSurfaceBYRelative = new float[30];
    private float[] ControlSurfaceCXRelative = new float[30];
    private float[] ControlSurfaceCYRelative = new float[30];
    private float[] ControlSurfaceDXRelative = new float[30];
    private float[] ControlSurfaceDYRelative = new float[30];
    private float[] ControlSurfaceZMinimum = new float[30];
    private float[] ControlSurfaceZMaximum = new float[30];
    private float[] ControlSurfaceAXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceAYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceBXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceBYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceCXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceCYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceDXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceDYRelativeAfterRotation = new float[30];

    private string UsersToListenTo = "ALL";
    private List<string> ValidUsers = new List<string>();
    bool validUser = false;
    private string[] ControlSurfaceMessage = new string[30];
    private AgentPrivate Hitman;
    private RigidBodyComponent RigidBody = null;
    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly Vector WarehouseRot = new Vector(0.0f, 0.0f, 0.0f);
    Quaternion RotQuat = Quaternion.FromEulerAngles(WarehouseRot).Normalized();
    private double ZRotation = new double();
    private int NumOfControlSurfaces = 0;
    private string Change = null;

    #endregion

    public override void Init()
    {
        Log.Write("In CustomDJController");
        ComplexInteraction.SetPrompt(".");
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        Change = "Stuff";
        if (RigidBody == null)
        {
            if (!ObjectPrivate.TryGetFirstComponent(out RigidBody))
            {
                // Since object scripts are initialized when the scene loads, no one will actually see this message.
                ScenePrivate.Chat.MessageAllUsers("There is no RigidBodyComponent attached to this object.");
                return;
            }
        }
        SubscribeToScriptEvent("SecurityList", getSecurity);
        InitializeComplexInteraction();
    }

    private void InitializeComplexInteraction()
    {
        CurPos = RigidBody.GetPosition();
        //Log.Write("CurPos: " + CurPos);
        ZRotation = RigidBody.GetOrientation().GetEulerAngles().Z * 57.2958;
        //Log.Write("Zrotation: " + ZRotation);

        LoadControlSurfaces(ControlSurface1, 0);
        LoadControlSurfaces(ControlSurface2, 1);
        LoadControlSurfaces(ControlSurface3, 2);
        LoadControlSurfaces(ControlSurface4, 3);
        LoadControlSurfaces(ControlSurface5, 4);
        LoadControlSurfaces(ControlSurface6, 5);
        LoadControlSurfaces(ControlSurface7, 6);
        LoadControlSurfaces(ControlSurface8, 7);
        LoadControlSurfaces(ControlSurface9, 8);
        LoadControlSurfaces(ControlSurface10, 9);
        LoadControlSurfaces(ControlSurface11, 10);
        LoadControlSurfaces(ControlSurface12, 11);
        LoadControlSurfaces(ControlSurface13, 12);
        LoadControlSurfaces(ControlSurface14, 13);
        LoadControlSurfaces(ControlSurface15, 14);
        LoadControlSurfaces(ControlSurface16, 15);
        LoadControlSurfaces(ControlSurface17, 16);
        LoadControlSurfaces(ControlSurface18, 17);
        LoadControlSurfaces(ControlSurface19, 18);
        LoadControlSurfaces(ControlSurface20, 19);
        LoadControlSurfaces(ControlSurface21, 20);
        LoadControlSurfaces(ControlSurface22, 21);
        LoadControlSurfaces(ControlSurface23, 22);
        LoadControlSurfaces(ControlSurface24, 23);
        LoadControlSurfaces(ControlSurface25, 24);
        LoadControlSurfaces(ControlSurface26, 25);
        LoadControlSurfaces(ControlSurface27, 26);
        LoadControlSurfaces(ControlSurface28, 27);
        NumOfControlSurfaces = 28;
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
        Log.Write("ComplexLooperController: In getSecurity");
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
        Log.Write("CustomControllerLooper UsersToListenTo After getSecurity: " + UsersToListenTo);
    }

    #endregion

    #region Interaction

    private void LoadControlSurfaces(string ControlSurfaceInputString, int cntr)
    {
        Log.Write("In Load Control Surfaces: " + cntr);
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

            Log.Write("Interacting person: " + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
            ValidUsers.Clear();
            ValidUsers = UsersToListenTo.Split(',').ToList();
            if (UsersToListenTo.Contains("ALL"))
            {
                Log.Write("Valid User: ALL");
                validUser = true;
            }
            else
            {
                foreach (string ValidUser in ValidUsers)
                {
                    Log.Write("ValidUser: " + ValidUser);
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
        Log.Write("hitXRelative: " + hitXRelative + " hitYRelative: " + hitYRelative + " hitZRelative: " + hitZRelative);
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
                    Log.Write("Hit Control Surface: " + hitControlSurface);
                    sendSimpleMessage(ControlSurfaceMessage[cntr], idata);
                    break;
                }
            }
            cntr++;
        } while (cntr<NumOfControlSurfaces);
    }

    private void sendSimpleMessage(string msg, InteractionData data)
    {
        SimpleData sd = new SimpleData(this);
        sd.AgentInfo = ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        sd.ObjectId = sd.AgentInfo.ObjectId;
        sd.SourceObjectId = ObjectPrivate.ObjectId;
        SendToAll(msg, sd);

        if (OffTimer > 0.0)
        {
            Wait(TimeSpan.FromMilliseconds((int)OffTimer * 1000));
            SendToAll(msg + "Off", sd);
        }
    }

    #endregion
}
