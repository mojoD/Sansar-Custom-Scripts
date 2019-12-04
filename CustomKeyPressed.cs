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

public class CustomKeyPressed : SceneObjectScript

{

    #region ConstantsVariables

    public string ChannelOut = "K1";
    public bool MidiController = false;

    private SessionId Jammer = new SessionId();
    private SessionId LastJammer = new SessionId();
    AgentPrivate TheUser = null;
    List<IEventSubscription> ButtonSubscriptions = new List<IEventSubscription>();
    private int ActiveOctave = 1;

    private AgentPrivate Hitman;
    private RigidBodyComponent rigidBody;
    private CollisionData collsionData;
    private readonly string[] KeysToSend = {"C1","CS1","D1","DS1","E1","F1","FS1","G1","GS1","A1","AS1","B1","C2","CS2","D2","DS2","E2","F2","FS2","G2","GS2","A2","AS2","B2","C3","CS3","D3","DS3","E3","F3","FS3","G3","GS3","A3","AS3","B3","C4","CS4","D4","DS4","E4","F4","FS4","G4","GS4","A4","AS4","B4","C5","CS5","D5","DS5","E5","F5","FS5","G5","GS5","A5","AS5","B5","C6","CS6","D6","DS6","E6","F6","FS6","G6","GS6","A6","AS6","B6","C7","CS7","D7","DS7","E7","F7","FS7","G7","GS7","A7","AS7","B7"};
    private readonly string[] KeysToRelease = { "C1Up", "CS1Up", "D1Up", "DS1Up", "E1Up", "F1Up", "FS1Up", "G1Up", "GS1Up", "A1Up", "AS1Up", "B1Up", "C2Up", "CS2Up", "D2Up", "DS2Up", "E2Up", "F2Up", "FS2Up", "G2Up", "GS2Up", "A2Up", "AS2Up", "B2Up", "C3Up", "CS3Up", "D3Up", "DS3Up", "E3Up", "F3Up", "FS3Up", "G3Up", "GS3Up", "A3Up", "AS3Up", "B3Up", "C4Up", "CS4Up", "D4Up", "DS4Up", "E4Up", "F4Up", "FS4Up", "G4Up", "GS4Up", "A4Up", "AS4Up", "B4Up", "C5Up", "CS5Up", "D5Up", "DS5Up", "E5Up", "F5Up", "FS5Up", "G5Up", "GS5Up", "A5Up", "AS5Up", "B5Up", "C6Up", "CS6Up", "D6Up", "DS6Up", "E6Up", "F6Up", "FS6Up", "G6Up", "GS6Up", "A6Up", "AS6Up", "B6Up", "C7Up", "CS7Up", "D7Up", "DS7Up", "E7Up", "F7Up", "FS7Up", "G7Up", "GS7Up", "A7Up", "AS7Up", "B7Up" };
    private bool[] KeyDown = new bool[127];

    private bool Register1 = false;
    private bool Register2 = false;
    private bool Register3 = false;
    private bool Register4 = false;
    private int KeyOffset = 0;

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

        Log.Write("No Midicontroller: " + MidiController);
        //Log.Write("Command: " + Button.Command);

        if (!MidiController)
        {
            Log.Write("In Command Pressed");
            string Chk0 = Button.Command.Substring(0, 1);
            int Chk6 = int.Parse(Button.Command.Substring(6, 1));
            string KeyOut = null;
            int KeyIndex = 0;
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
            //Log.Write("Pressed KeyIndex: " + KeyIndex + " KeyDown: " + KeyDown[KeyIndex] + " Command: " + Button.Command + " Action: " + Button.Action + " Octave: " + ActiveOctave + " KeyOut: " + KeyOut);
        }
        else
        {
            Log.Write("Midicontroller: " + MidiController);
            string Chk0 = Button.Command.Substring(0, 1);
            string KeyOut = null;
            int KeyIndex = 0;

            if (Chk0 == "A")
            {
                int Chk6 = int.Parse(Button.Command.Substring(6, 1));
                KeyIndex = KeyOffset + Chk6;
                if (KeyDown[KeyIndex])
                {
                    KeyOut = KeysToRelease[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = false;
                }
                else
                {
                    KeyOut = KeysToSend[KeyIndex];
                    SendKey(KeyOut);
                    KeyDown[KeyIndex] = true;
                }
                KeyOffset = 0;
            }
            else if (Chk0 == "K")
            {
                int Chk6 = int.Parse(Button.Command.Substring(6, 1));
                if (Chk6 < 7)
                {
                    KeyIndex = 10 + KeyOffset + Chk6;
                    if (KeyDown[KeyIndex])
                    {
                        KeyOut = KeysToRelease[KeyIndex];
                        SendKey(KeyOut);
                        KeyDown[KeyIndex] = false;
                    }
                    else
                    {
                        KeyOut = KeysToSend[KeyIndex];
                        SendKey(KeyOut);
                        KeyDown[KeyIndex] = true;
                    }
                    KeyOffset = 0;
                }
                else
                {
                    if (Button.Command == "Keypad7")
                    {
                        Register1 = true;
                        KeyOffset = 17;
                    }
                    else if (Button.Command == "Keypad8")
                    {
                        Register2 = true;
                        KeyOffset = 34;
                    }
                    else if (Button.Command == "Keypad9")
                    {
                        Register3 = true;
                        KeyOffset = 51;
                    }
                }
            }
            else
            {
                if (Button.Command == "Modifier")
                {
                    Register4 = true;
                    KeyOffset = 68;
                }
            }

            Log.Write("Button: " + Button.Command + " Action: " + Button.Action + " Reg1: " + Register1 + " Reg2: " + Register2 + " Reg3: " + Register3 + " Reg4: " + Register4 + " Offset: " + KeyOffset + " Index: " + KeyIndex + " Down: " + KeyDown[KeyIndex] + " KeyOut: " + KeyOut);
            Register1 = false;
            Register2 = false;
            Register3 = false;
            Register4 = false;
        }
    }

    void CommandReleased(CommandData Button)
    {

        if (!MidiController)
        {
            if (Button.Command != "Modifier")
            {
                string Chk0 = Button.Command.Substring(0, 1);
                int Chk6 = int.Parse(Button.Command.Substring(6, 1));
                Log.Write("In CommandReleased");
                if (Chk0 == "K")
                {
                    if ((Chk6 == 7) || (Chk6 == 8))
                    {
                        string KeyOut = null;
                        int KeyIndex = 0;
                        KeyIndex = ActiveOctave * 12 + Chk6 + 3;

                        if (KeyDown[KeyIndex])
                        {
                            KeyOut = KeysToRelease[KeyIndex];
                            SendKey(KeyOut);
                            KeyDown[KeyIndex] = false;
                            Log.Write("Released Action: " + Button.Action + " Command: " + Button.Command);
                        }
                    }
                }
                else if (Chk0 == "A")
                {
                    string KeyOut = null;
                    int KeyIndex = 0;
                    if (Chk6 == 0)
                    {
                        KeyIndex = ActiveOctave * 12 + 9;
                        if (KeyDown[KeyIndex])
                        {
                            KeyOut = KeysToRelease[KeyIndex];
                            SendKey(KeyOut);
                            KeyDown[KeyIndex] = false;
                            Log.Write("Released Action: " + Button.Action + " Command: " + Button.Command);
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
                            Log.Write("Released Action: " + Button.Action + " Command: " + Button.Command);
                        }
                    }
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
