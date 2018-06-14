//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using Sansar;
using Sansar.Simulation;
using Sansar.Script;
using System;
using System.Linq;
using System.Collections.Generic;

public class BPMBlock : SceneObjectScript
{
    // Components can be set in the editor if the correct component types are added to the object
    private RigidBodyComponent RigidBody = null;
    public string bpm;
    private float XWarehousePos;
    private float YWarehousePos;
    private float ZWarehousePos;
    private string ypos;
    private bool goodhit;
    private bool hitDetected = false;
    private Vector WarehousePos = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly Vector WarehouseRot = new Vector(0.0f, 0.0f, 0.0f);
    Quaternion RotQuat = Quaternion.FromEulerAngles(WarehouseRot).Normalized();
    private Vector OffsetPos = new Vector(0.0f, 0.0f, -1000.0f);
    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    private float BeatBlockMass;
    public string BeatBlockName;
    public SoundResource Sample1;
    public string pos;
    public string beats;
    private string newpos;
    private bool subscribe = false;

    public BPMBlock()
    {
    }

    #region Communications
    public class SendBPM : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> BPMArray { get; internal set; }

        public List<string> SendBPMArray
        {
            get
            {
                return BPMArray;
            }
        }
    }

    public List<string> ActiveBin = new List<string>();

    public interface SendActiveBins
    {
        List<string> SendActiveBin { get; }
    }

    private void getBin(ScriptEventData gotBin)
    {
        //Log.Write("A");
        if (gotBin.Data == null)
        {
            return;
        }
        //Log.Write("B");
        SendActiveBins sendBin = gotBin.Data.AsInterface<SendActiveBins>();
        //Log.Write("C");
        if (sendBin == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        string binToReturn;
        binToReturn = sendBin.SendActiveBin.ElementAt(0);
        //Log.Write("BPMBlock BinToReturn: " + binToReturn);
        string VolumeFlag = sendBin.SendActiveBin.ElementAt(1);
        //Log.Write("binToReturn: " + binToReturn);
        //og.Write("VolumeFlag: " + VolumeFlag);
        // Log.Write("pos: " + pos);
        if (!(VolumeFlag == "volume"))
        {
            if ((binToReturn == pos) || (binToReturn == "all"))
            {
                if (subscribe == true)
                {
                    ReturnBPMBlock();
                }
            }
        }
    }

    public class SendSamples : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<SoundResource> SampleLibrary { get; internal set; }

        public List<SoundResource> SendSampleLibrary
        {
            get
            {
                return SampleLibrary;
            }
        }
    }

    public class SendBlockNames : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> BlockNameArray { get; internal set; }

        public List<string> SendBlockArray
        {
            get
            {
                return BlockNameArray;
            }
        }
    }

    #endregion

    // Override Init to set up event handlers and start coroutines.
    public override void Init()
    {
        Script.UnhandledException += UnhandledException;
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
        XWarehousePos = CurPos[0];
        YWarehousePos = CurPos[1];
        ZWarehousePos = CurPos[2];

        // Convert the supplied bools to the correct CollisionEventType to track
        CollisionEventType trackedEvents = 0;
        trackedEvents |= CollisionEventType.RigidBodyContact;

        StartCoroutine(CheckForCollisions, trackedEvents);
    }

    private void CheckForCollisions(CollisionEventType trackedEvents)
    {
        while (true)
        {
            // This will block the coroutine until a collision happens
            CollisionData data = (CollisionData)WaitFor(RigidBody.Subscribe, trackedEvents, Sansar.Script.ComponentId.Invalid);
            if (data.EventType == CollisionEventType.CharacterContact)
            {
                //Log.Write("I hit an avatar");
            }
            else
            {
                //Log.Write("I hit an object");
                //Log.Write("CollisionEventType: " + data.EventType);
                goodhit = false;
                if (!hitDetected)
                {
                    //Log.Write("BPMBlock");
                    //Log.Write("BPMBlock: Position: " + RigidBody.GetPosition());
                    pos = GetXPosition(RigidBody.GetPosition().ToString4());
                    ypos = GetYPosition(RigidBody.GetPosition().ToString4());
                    if ((ypos == "-0") || (ypos == "0."))
                    {
                        goodhit = true;
                    }   
   
                    SendBPM sendBPM = new SendBPM();
                    sendBPM.BPMArray  = new List<string>();
                    sendBPM.BPMArray.Add("bpm");
                    sendBPM.BPMArray.Add(pos);
                    sendBPM.BPMArray.Add(bpm);
                    SendBlockNames sendBlocks = new SendBlockNames();
                    sendBlocks.BlockNameArray = new List<string>();
                    sendBlocks.BlockNameArray.Add(BeatBlockName);
                    sendBlocks.BlockNameArray.Add("0");
                    sendBlocks.BlockNameArray.Add(beats);
                    SendSamples sendSamples = new SendSamples();
                    sendSamples.SampleLibrary = new List<SoundResource>();
                    if (Sample1 != null)
                    {
                        //Log.Write("BPMBlock: sample being sent from BPM: " + Sample1.GetName());
                        sendSamples.SampleLibrary.Add(Sample1);
                    }

                    if (goodhit)
                    {
                        //Log.Write("BPMBlock: goodhit");
                        PostScriptEvent(ScriptId.AllScripts, "BPMBlock", sendBPM);
                        PostScriptEvent(ScriptId.AllScripts, "BeatBlock", sendBlocks);
                        PostScriptEvent(ScriptId.AllScripts, "BeatBlockSample", sendSamples);
                        Wait(TimeSpan.FromSeconds(1.5));
                        DisplayBPMBlock();
                        Wait(TimeSpan.FromSeconds(1.0));
                        SubscribeToScriptEvent("ReturnBeatBlock", getBin);
                        subscribe = true;
                        hitDetected = true;
                    }
                    else
                    {
                        //ScenePrivate.Chat.MessageAllUsers("Drop the BPM Block in a Loop Bin");
                        ReturnBPMBlock();
                    }
                }
             }
         }
    }

    private string GetXPosition(string strVectorIn)
    {
        int from = strVectorIn.IndexOf("<", StringComparison.CurrentCulture);
        string chunk = strVectorIn.Substring(from + 1, 2);
        //Log.Write("XPos: " + chunk);

        switch (chunk)
        {
            case "0.":
                newpos = "0";
                break;
            case "1.":
                newpos = "0";
                break;
            case "2.":
                newpos = "1";
                break;
            case "3.":
                newpos = "1";
                break;
            case "4.":
                newpos = "2";
                break;
            case "5.":
                newpos = "2";
                break;
            case "6.":
                newpos = "3";
                break;
            case "7.":
                newpos = "3";
                break;
            case "8.":
                newpos = "4";
                break;
            case "9.":
                newpos = "4";
                break;
            case "10":
                newpos = "5";
                break;
            case "11":
                newpos = "5";
                break;
            case "12":
                newpos = "6";
                break;
            case "13":
                newpos = "6";
                break;
            case "14":
                newpos = "7";
                break;
            case "15":
                newpos = "7";
                break;
            case "16":
                newpos = "8";
                break;
            case "17":
                newpos = "8";
                break;
            case "18":
                newpos = "9";
                break;
            case "19":
                newpos = "9";
                break;
        }
        //Log.Write("newpos: " + newpos);
        return newpos;
    }


    private string GetYPosition(string strVectorIn)
    {
        int from = strVectorIn.IndexOf(",", StringComparison.CurrentCulture);
        string chunk = strVectorIn.Substring(from + 1, 2);
        return chunk;
    }

    private void DisplayBPMBlock() //Take Block from Bin and Place it Above The Bin to show what is currently playing
    {
        BeatBlockMass = RigidBody.GetMass();
        CurPos = RigidBody.GetPosition();
        RigidBody.SetMass(0.0f);
        float XDisplayPos = 1.0f;
        float YDisplayPos = 0.7f;
        float ZDisplayPos = 2.2f;
        Vector DisplayPos = new Vector(XDisplayPos, YDisplayPos, ZDisplayPos);
        RigidBody.SetPosition(DisplayPos);
        RigidBody.SetOrientation(RotQuat);
        RigidBody.SetMass(BeatBlockMass);
    }


    private void ReturnBPMBlock()
    {
        //Take Block from Bin and Return it to Beat Box Staging Area
        BeatBlockMass = RigidBody.GetMass();
        CurPos = RigidBody.GetPosition();
        RigidBody.SetMass(0.0f);
        RigidBody.SetPosition(CurPos - OffsetPos);
        //Take Block from Bin and Return it to Beat Box Staging Area
        CurPos = RigidBody.GetPosition();
        Vector WarehousePos = new Vector(XWarehousePos, YWarehousePos, ZWarehousePos);
        RigidBody.SetPosition(WarehousePos);
        RigidBody.SetOrientation(RotQuat);
        RigidBody.SetMass(BeatBlockMass);
        hitDetected = false;
        subscribe = false;
    }

    private void UnhandledException(object sender, Exception e)
    {
        if (!Script.UnhandledExceptionRecoverable)
        {
            ScenePrivate.Chat.MessageAllUsers("Unrecoverable exception happened, the script will now be removed.");
        }
        else
        {
            ScenePrivate.Chat.MessageAllUsers("This script will be allowed to continue.");
        }
    }


}
