//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

#define SansarBuild
#define InstrumentBuild

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

//Version used using normal collisions with an object and not 

public class CustomKeyPressedJammer : SceneObjectScript

{

    #region ConstantsVariables

    public string ChannelOut = "K1";
    private SessionId Jammer = new SessionId();
    private SessionId LastJammer = new SessionId();
    AgentPrivate TheUser = null;
    List<IEventSubscription> ButtonSubscriptions = new List<IEventSubscription>();
    private int ActiveOctave = 1;

    private AgentPrivate Hitman;
    private RigidBodyComponent rigidBody;
    private CollisionData collsionData;
    private readonly string[] KeysToSend = { "Key1", "Key2", "Key3", "Key4", "Key5", "Key6", "Key7", "Key8", "Key9", "Key10", "Key11", "Key12", "Key13", "Key14", "Key15", "Key16", "Key17", "Key18", "Key19", "Key20", "Key21", "Key22", "Key23", "Key24", "Key25", "Key26", "Key27", "Key28", "Key29", "Key30", "Key31", "Key32", "Key33", "Key34", "Key35", "Key36", "Key37", "Key38", "Key39", "Key40" };
    private readonly string[] KeysToRelease = { "Key1Up", "Key2Up", "Key3Up", "Key4Up", "Key5Up", "Key6Up", "Key7Up", "Key8Up", "Key9Up", "Key10Up", "Key11Up", "Key12Up", "Key13Up", "Key14Up", "Key15Up", "Key16Up", "Key17Up", "Key18Up", "Key19Up", "Key20Up", "Key21Up", "Key22Up", "Key23Up", "Key24Up", "Key25Up", "Key26Up", "Key27Up", "Key28Up", "Key29Up", "Key30Up", "Key31Up", "Key32Up", "Key33Up", "Key34Up", "Key35Up", "Key36Up", "Key37Up", "Key38Up", "Key39Up", "Key40Up" };
    private bool[] KeyDown = new bool[127];

    #endregion

    #region Communication

    public class SendKeyInfo : Reflective
    {
        public string iChannelOut { get; set; }
        public string iSource { get; set; }
        public string iKeySent { get; set; }
    }

    void SubscribeClientToButton(Client client, string Button)
    {
        ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Pressed, CommandPressed, CommandCanceled));
        ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Released, CommandReleased, CommandCanceled));
    }

    void UnsubscribeAllButtons()
    {
        foreach (IEventSubscription sub in ButtonSubscriptions) sub.Unsubscribe();
        ButtonSubscriptions.Clear();
    }

    void SubscribeKeyPressed(AgentPrivate agent, string Message)
    {
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

    void CommandPressed(CommandData Button)
    {
        string Chk0 = Button.Command.Substring(0, 1);
        int Chk6 = int.Parse(Button.Command.Substring(6, 1));
        string KeyOut = null;
        int KeyIndex = 0;
        //Log.Write("Pressed Action: " + Button.Action);
        if (Chk0 == "K") 
        {
            if (Chk6 < 7) // it is a octave change
            {
                ActiveOctave = Chk6;
            }
            else if (Chk6 < 9)
            {
                KeyIndex = ActiveOctave * 12 + Chk6 + 3;
                if (!KeyDown[KeyIndex])
                {
                    KeyOut = KeysToSend[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = true;
                }
            }
        }
        else if (Chk0 == "A")
        {
            if (Chk6 == 0)
            {
                KeyIndex = ActiveOctave * 12 + 9;
                if (!KeyDown[KeyIndex])
                {
                    KeyOut = KeysToSend[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = true;
                }
            }
            else
            {

                KeyIndex = ActiveOctave * 12 + Chk6 - 1;
                if (!KeyDown[KeyIndex])
                {
                    KeyOut = KeysToSend[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = true;
                }
            }
        }
        //Log.Write("Pressed KeyIndex: " + KeyIndex + " KeyDown: " + KeyDown[KeyIndex]);
        //Log.Write("Button.Command: " + Button.Command + " Button.Action: " + Button.Action + " Octave: " + ActiveOctave + " KeyOut: " + KeyOut);
    }

    void CommandReleased(CommandData Button)
    {
        string Chk0 = Button.Command.Substring(0, 1);
        int Chk6 = int.Parse(Button.Command.Substring(6, 1));
        string KeyOut = null;
        int KeyIndex = 0;
        //Log.Write("Released Action: " + Button.Action);
        if (Chk0 == "K")
        {
            if ((Chk6 == 7) || (Chk6 == 8))
            {
                KeyIndex = ActiveOctave * 12 + Chk6 + 3;

                if (KeyDown[KeyIndex])
                {
                    KeyOut = KeysToRelease[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = false; 
                }
            }
        }
        else if (Chk0 == "A")
        {
            if (Chk6 == 0)
            {
                KeyIndex = ActiveOctave * 12 + 9;
                if (KeyDown[KeyIndex])
                {
                    KeyOut = KeysToRelease[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = false;
                }
            }
            else
            {
                KeyIndex = ActiveOctave * 12 + Chk6 - 1;
                if (KeyDown[KeyIndex])
                {
                    KeyOut = KeysToRelease[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = false;
                }
            }
        }
        //Log.Write("Release KeyIndex: " + KeyIndex + " KeyDown: " + KeyDown[KeyIndex]);
        //Log.Write("Button.Command: " + Button.Command + " Button.Action: " + Button.Action + " Octave: " + ActiveOctave + " KeyOut: " + KeyOut);
    }

    void CommandCanceled(CancelData data)
    {
        Log.Write(GetType().Name, "Subscription canceled: " + data.Message);
    }

    #endregion

    public override void Init()
    {
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        //Log.Write("In Custom PC Key Pressed");
        if (ObjectPrivate.TryGetFirstComponent(out rigidBody))
        {
            Log.Write("Calling OnCollide");
            rigidBody.Subscribe(CollisionEventType.AllCollisions, OnCollide);
        }
        else
        {
        }
        int i = 0;
        do
        {
            KeyDown[i] = false;
            i++;
        }
        while (i < 88);
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    private void OnCollide(CollisionData Data)
    {

        //Log.Write("In OnCollide");
        collsionData = Data;
        Hitman = ScenePrivate.FindAgent(Data.HitComponentId.ObjectId);
        //SceneInfo info = ScenePrivate.SceneInfo;
        Jammer = Hitman.AgentInfo.SessionId;
        if (Jammer != LastJammer)
        {
            SubscribeKeyPressed(Hitman, "unsub");
            Wait(TimeSpan.FromSeconds(2));  //wait a couple of seconds for the keys to unsubscribe
            SubscribeKeyPressed(Hitman, "sub");
            //Log.Write("Keys subscribed for Player: " + Hitman.AgentInfo.Name);
            LastJammer = Jammer;
        }
    }

    void SendKey(string KeyIn)
    {
        //sendSimpleMessage(KeyIn, collsionData);
        //Log.Write("CustomKeyPressed: " + KeyIn);
        //SendKeyInteraction(KeyIn);
        SendKeyInfo sendKeyInfo = new SendKeyInfo();
        sendKeyInfo.iChannelOut = ChannelOut;
        sendKeyInfo.iSource = "KeyPress";
        sendKeyInfo.iKeySent = KeyIn;
        PostScriptEvent(ScriptId.AllScripts, "KeySent", sendKeyInfo);
        PostScriptEvent(ScriptId.AllScripts, KeyIn, sendKeyInfo);
    }

}
