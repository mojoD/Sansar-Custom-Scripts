//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using Sansar;
using Sansar.Simulation;
using Sansar.Script;
using System;
using System.Linq;
using System.Collections.Generic;

class Screen : SceneObjectScript
{
    private Vector ScreenPos = new Vector(0.0f, 0.0f, 0.0f);
    public float XScreenPos = 0.0f;
    public float YScreenPos = 0.0f;
    public float ZScreenPos = 0.0f;
    private Vector ScreenRot = new Vector(0.0f, 0.0f, 0.0f);
    public float XScreenRot = 0.0f;
    public float YScreenRot = 0.0f;
    public float ZScreenRot = 0.0f;
    Quaternion ScreenQuat;

    private RigidBodyComponent RigidBody;

    public class SendScreenPos : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> ScreenPos { get; internal set; }

        public List<string> SetScreenPos
        {
            get
            {
                return ScreenPos;
            }
        }
    }

    private SendScreenPos sendScreenPos = new SendScreenPos();

    private void GetChatCommand(ChatData Data)
    {
        string DataCmd = Data.Message;
        //Log.Write("DataCmd: " + DataCmd);
        if (DataCmd.Contains("/slideshow"))
        {
            sendScreenPos.ScreenPos = new List<string>();
            sendScreenPos.ScreenPos.Clear();
            sendScreenPos.SetScreenPos.Add(XScreenPos.ToString());
            sendScreenPos.SetScreenPos.Add(YScreenPos.ToString());
            sendScreenPos.SetScreenPos.Add(ZScreenPos.ToString());
            sendScreenPos.SetScreenPos.Add(XScreenRot.ToString());
            sendScreenPos.SetScreenPos.Add(YScreenRot.ToString());
            sendScreenPos.SetScreenPos.Add(ZScreenRot.ToString());

            PostScriptEvent(ScriptId.AllScripts, "SendScreenPos", sendScreenPos);
            Log.Write("Sent");
        }
    }

    public override void Init()
    {
        Wait(TimeSpan.FromSeconds(2.0));
        if (RigidBody == null)
        {
            if (!ObjectPrivate.TryGetFirstComponent(out RigidBody))
            {
                // Since object scripts are initialized when the scene loads, no one will actually see this message.
                ScenePrivate.Chat.MessageAllUsers("There is no RigidBodyComponent attached to this object.");
                return;
            }
            {
                //ScreenPos = RigidBody.GetPosition();
                //XScreenPos = ScreenPos[0];
                //YScreenPos = ScreenPos[1];
                //ZScreenPos = ScreenPos[2];
                //ScreenQuat = RigidBody.GetOrientation();
                //ScreenRot = ScreenQuat.GetEulerAngles();
                //XScreenRot = ScreenRot[0];
                //YScreenRot = ScreenRot[1];
                //ZScreenRot = ScreenRot[2];
                //ScreenRot = new Vector(XScreenRot, YScreenRot, ZScreenRot);
                ScenePrivate.Chat.Subscribe(0, GetChatCommand);
            }
        }
    }
}
