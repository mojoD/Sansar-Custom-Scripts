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


public class TriggerComplexInteraction : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("Click Me!")]
    public Interaction ComplexInteraction;

    private float[] ControlSurfaceXRelative = new float[20];
    private float[] ControlSurfaceYRelative = new float[20];
    private float[] ControlSurfaceRadiusArray = new float[20];
    private float[] ControlSurfaceZMinimum = new float[20];
    private float[] ControlSurfaceZMaximum = new float[20];
    private float[] ControlSurfaceXRelativeAfterRotation = new float[20];
    private float[] ControlSurfaceYRelativeAfterRotation = new float[20];

    private string[] ControlSurfaceMessage = new string[20];
    private AgentPrivate Hitman;
    private RigidBodyComponent RigidBody = null;
    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly Vector WarehouseRot = new Vector(0.0f, 0.0f, 0.0f);
    Quaternion RotQuat = Quaternion.FromEulerAngles(WarehouseRot).Normalized();
    private double ZRotation = new double();
    private int NumOfControlSurfaces = 0;

    //public Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    //public double ZRotation = new double();
    public float OffTimer = 0;
    public bool Debug = false;
    public string ControlSurface1 = null;
    public string ControlSurface2 = null;
    public string ControlSurface3 = null;
    public string ControlSurface4 = null;
    public string ControlSurface5 = null;
    public string ControlSurface6 = null;
    public string ControlSurface7 = null;
    public string ControlSurface8 = null;
    public string ControlSurface9 = null;
    public string ControlSurface10 = null;

    #endregion

    public override void Init()
    {
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal

        //SubscribeToScriptEvent("CollisionData", getCollisionData);

        if (RigidBody == null)
        {
            if (!ObjectPrivate.TryGetFirstComponent(out RigidBody))
            {
                // Since object scripts are initialized when the scene loads, no one will actually see this message.
                ScenePrivate.Chat.MessageAllUsers("There is no RigidBodyComponent attached to this object.");
                return;
            }
        }

        CurPos = RigidBody.GetPosition();
        Log.Write("CurPos: " + CurPos);
        ZRotation = RigidBody.GetOrientation().GetEulerAngles().Z * 57.2958;
        Log.Write("Zrotation: " + ZRotation);

        int cntr = 0;

        if (ControlSurface1.Length > 0)
        {
            LoadControlSurfaces(ControlSurface1, cntr);
            cntr++;
        }
        if (ControlSurface2.Length > 0)
        {
            LoadControlSurfaces(ControlSurface2, cntr);
            cntr++;
        }
        if (ControlSurface3.Length > 0)
        {
            LoadControlSurfaces(ControlSurface3, cntr);
            cntr++;
        }
        if (ControlSurface4.Length > 0)
        {
            LoadControlSurfaces(ControlSurface4, cntr);
            cntr++;
        }
        if (ControlSurface5.Length > 0)
        {
            LoadControlSurfaces(ControlSurface5, cntr);
            cntr++;
        }
        if (ControlSurface6.Length > 0)
        {
            LoadControlSurfaces(ControlSurface6, cntr);
            cntr++;
        }
        if (ControlSurface7.Length > 0)
        {
            LoadControlSurfaces(ControlSurface7, cntr);
            cntr++;
        }
        if (ControlSurface8.Length > 0)
        {
            LoadControlSurfaces(ControlSurface8, cntr);
            cntr++;
        }
        if (ControlSurface9.Length > 0)
        {
            LoadControlSurfaces(ControlSurface9, cntr);
            cntr++;
        }
        if (ControlSurface10.Length > 0)
        {
            LoadControlSurfaces(ControlSurface10, cntr);
            cntr++;
        }
        NumOfControlSurfaces = cntr;
        ComplexInteractionHandler();
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region Communication

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
        //Log.Write("In SendToAll");
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(',');

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events));
        foreach (string eventName in events)
        {
            //Log.Write("EventName: " + eventName);
            PostScriptEvent(eventName.Trim(), data);
        }
    }

    #endregion

    #region Interaction

    private void LoadControlSurfaces(string ControlSurfaceInputString, int cntr)
    {
        //Takes Relative Values read in from configuration and converts them to realworld position 
        string[] values = new string[100];
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        //Log.Write("sendNotePositions.SendNotePosition.Count(): " + sendNotePositions.SendNotePosition.Count());

        //Log.Write("ZRotation: " + ZRotation);
        //Log.Write("cntr: " + cntr);
        values = ControlSurfaceInputString.Split(',');
        ControlSurfaceMessage[cntr] = values[0];
        ControlSurfaceXRelative[cntr] = float.Parse(values[1]);
        //Log.Write("ControlSurfaceXRelative[" + cntr + "]: " + ControlSurfaceXRelative[cntr]);
        ControlSurfaceYRelative[cntr] = float.Parse(values[2]);
        //Log.Write("ControlSurfaceYRelative[" + cntr + "]: " + ControlSurfaceYRelative[cntr]);
        ControlSurfaceRadiusArray[cntr] = float.Parse(values[3]);
        //Log.Write("ControlSurfaceRadiusArray[" + cntr + "]: " + ControlSurfaceRadiusArray[cntr]);
        ControlSurfaceZMinimum[cntr] = float.Parse(values[4]);
        //Log.Write("ControlSurfaceZMinimum[" + cntr + "]: " + ControlSurfaceZMinimum[cntr]);
        ControlSurfaceZMaximum[cntr] = float.Parse(values[5]);
        //Log.Write("ControlSurfaceZMaximum[" + cntr + "]: " + ControlSurfaceZMaximum[cntr]);
        float CosAngle = (float)Math.Cos(ZRotation * 0.0174533);
        float SinAngle = (float)Math.Sin(ZRotation * 0.0174533);

        ControlSurfaceXRelativeAfterRotation[cntr] = (ControlSurfaceXRelative[cntr] * CosAngle) - (ControlSurfaceYRelative[cntr] * SinAngle);
        //Log.Write("ControlSurfaceXRelativeAfterRotation[" + cntr + "]: " + ControlSurfaceXRelativeAfterRotation[cntr]);
        ControlSurfaceYRelativeAfterRotation[cntr] = (ControlSurfaceYRelative[cntr] * CosAngle) + (ControlSurfaceXRelative[cntr] * SinAngle);
        //Log.Write("ControlSurfaceYRelativeAfterRotation[" + cntr + "]: " + ControlSurfaceYRelativeAfterRotation[cntr]);
    }

    private void ComplexInteractionHandler()
    {
        ComplexInteraction.Subscribe((InteractionData idata) =>
        {
            if (Debug)
            {
                ComplexInteraction.SetPrompt("Debug: "
                    + "\nHit:" + idata.HitPosition.ToString()
                    + "\nFrom:" + idata.Origin.ToString()
                    + "\nNormal:" + idata.HitNormal.ToString()
                    + "\nBy:" + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
                //Vector hitPosition = idata.HitPosition;
            }
            ExecuteInteraction(idata);
            //Log.Write("idata.HitPosition.ToString()" + idata.HitPosition.ToString());
        });
    }

    private void ExecuteInteraction(InteractionData idata)
    {
        //loopNote = false;
        float hitXRelative = 0;
        float hitYRelative = 0;
        float hitZRelative = 0;
        float hitRadius = 0;
        Vector hitPosition = idata.HitPosition;
        //Log.Write("CurPosX: " + CurPos.X);
        //Log.Write("CurPosY: " + CurPos.Y);
        //Log.Write("hitPosition.X: " + hitPosition.X);
        //Log.Write("hitPosition.Y: " + hitPosition.Y);
        //normalize to origin 0,0

        if ((float) hitPosition.X > (float) CurPos.X)
        {
            hitXRelative = ((float) hitPosition.X - (float) CurPos.X) * 100;
        }
        else
        {
            hitXRelative = ((float) hitPosition.X - (float) CurPos.X) * 100;
        }

        if ((float) hitPosition.Y > (float) CurPos.Y)
        {
            hitYRelative = ((float) hitPosition.Y - (float) CurPos.Y) * 100;
        }
        else
        {
            hitYRelative = ((float) hitPosition.Y - (float) CurPos.Y) * 100;
        }
        if ((float) hitPosition.Z > (float) CurPos.Z)
        {
            hitZRelative = ((float) hitPosition.Z - (float) CurPos.Z) * 100;
        }
        else
        {
            hitZRelative = ((float) hitPosition.Z - (float)CurPos.Z) * 100;
        }
        //Log.Write("hitXRelative: " + hitXRelative);
        //Log.Write("hitYRelative: " + hitYRelative);
        //Log.Write("hitZRelative: " + hitZRelative);

        //Check to See if the Relative Hit Radius falls within a Control Surface
        int cntr = 0;
        //int hitControlSurface;
        float XminTest = 0;
        float XmaxTest = 0;
        float YminTest = 0;
        float YmaxTest = 0;
        //Log.Write("Doing Some Math");
        do
        {
            XminTest = ControlSurfaceXRelativeAfterRotation[cntr] - ControlSurfaceRadiusArray[cntr];
            //Log.Write("XminTest: " + XminTest);
            XmaxTest = ControlSurfaceXRelativeAfterRotation[cntr] + ControlSurfaceRadiusArray[cntr];
            //Log.Write("XmaxTest: " + XmaxTest);
            YminTest = ControlSurfaceYRelativeAfterRotation[cntr] - ControlSurfaceRadiusArray[cntr];
            //Log.Write("YminTest: " + YminTest);
            YmaxTest = ControlSurfaceYRelativeAfterRotation[cntr] + ControlSurfaceRadiusArray[cntr];
            //Log.Write("YmaxTest: " + YmaxTest);

            if ((hitXRelative >= XminTest) && (hitXRelative <= XmaxTest))
            {
                //Log.Write("Within X Range");
                //Log.Write("hitYRelative: " + hitYRelative);
                //Log.Write("YminTest: " + YminTest);
                //Log.Write("YmaxTest: " + YmaxTest);
                //if (hitYRelative >= YminTest) Log.Write("Y > Min Y");
                //if (hitYRelative <= YmaxTest) Log.Write("Y < Max Y");
                if ((hitYRelative >= YminTest) && (hitYRelative <= YmaxTest))
                {
                    //Log.Write("Within Y Range");
                    hitRadius = (float)Math.Sqrt(((hitXRelative - ControlSurfaceXRelativeAfterRotation[cntr]) * (hitXRelative - ControlSurfaceXRelativeAfterRotation[cntr])) + ((hitYRelative - ControlSurfaceYRelativeAfterRotation[cntr]) * (hitYRelative - ControlSurfaceYRelativeAfterRotation[cntr])));
                    //Log.Write("hitRadius: " + hitRadius);
                    //Log.Write("ControlSurfaceRadiusArray[" + cntr + "]: " + ControlSurfaceRadiusArray[cntr]);
                    if (hitRadius <= ControlSurfaceRadiusArray[cntr])
                    {
                        //Log.Write("Within Radius Range");
                        if (hitZRelative >= ControlSurfaceZMinimum[cntr] && hitZRelative <= ControlSurfaceZMaximum[cntr])
                        {
                            //Simple Message
                            string hitControlSurface = ControlSurfaceMessage[cntr];
                            //Log.Write("Hit Control Surface: " + hitControlSurface);
                            sendSimpleMessage(ControlSurfaceMessage[cntr], idata);
                        }
                    }
                }
            }
            cntr++;
        } while (cntr < NumOfControlSurfaces);

        //Call Reflect Communication that Sends Message



        //Reflex Message
    }

    private void sendSimpleMessage(string msg, InteractionData data)
    {
        SimpleData sd = new SimpleData();
        sd.AgentInfo = ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        sd.ObjectId = sd.AgentInfo.ObjectId;
        SendToAll(msg, sd);

        Wait(TimeSpan.FromMilliseconds((int) OffTimer*1000));
        SendToAll(msg + "Off", sd);
    }

    #endregion
}
