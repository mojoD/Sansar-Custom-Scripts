//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class ActionSynthModule : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("K1")]
    [DisplayName("Input Channel: ")]
    public string InputChannel;

    //[DefaultValue("false")]
    //[DisplayName("Loop Sustain: ")]
    //public bool LoopSustain = false;

    public List<SoundResource> S0SoundResources = new List<SoundResource>();
    public List<float> S0Offset = new List<float>();
    public List<SoundResource> S1SoundResources = new List<SoundResource>();
    public List<float> S1Offset = new List<float>();
    public List<SoundResource> S2SoundResources = new List<SoundResource>();
    public List<float> S2Offset = new List<float>();
    public List<SoundResource> S3SoundResources = new List<SoundResource>();
    public List<float> S3Offset = new List<float>();
    public List<SoundResource> S4SoundResources = new List<SoundResource>();
    public List<float> S4Offset = new List<float>();
    public List<SoundResource> S5SoundResources = new List<SoundResource>();
    public List<float> S5Offset = new List<float>();
    public List<SoundResource> S6SoundResources = new List<SoundResource>();
    public List<float> S6Offset = new List<float>();
    public List<SoundResource> S7SoundResources = new List<SoundResource>();
    public List<float> S7Offset = new List<float>();

    public string EnableEvent;
    public string DisableEvent;

    [DefaultValue("Panic2")]
    [DisplayName("Panic Event: ")]
    public string PanicEvent = "Panic2";

    private float loudnessIn = 0;
    private bool enabled = false;
    private string KeyIn = null;
    private PlaySettings playSettings;
    private PlayHandle playHandleSimple;
    private PlayHandle[] playHandleArray = new PlayHandle[127];
    private PlaySettings[] playSettingsArray = new PlaySettings[127];

    #endregion

    #region Communication

    #region SimpleHelpers
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
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(',');

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events));
        foreach (string eventName in events)
        {
            PostScriptEvent(eventName.Trim(), data);
        }
    }
    #endregion

    public interface ISendKeyInfo
    {
        string iChannelOut { get; }
        string iSource { get; }
        string iKeySent { get; }
    }

    private void getKeyInfo(ScriptEventData gotKeyInfo)
    {
        if (enabled)
        {
            //Log.Write("ActionSynthModule: In getKeyInfo");
            if (gotKeyInfo.Data == null)
            {
                return;
            }

            ISendKeyInfo sendKeyInfo = gotKeyInfo.Data.AsInterface<ISendKeyInfo>();
            if (sendKeyInfo == null)
            {
                Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
                return;
            }
            if (sendKeyInfo.iChannelOut == InputChannel)
            {
                KeyIn = sendKeyInfo.iKeySent;
                //Log.Write("KeyIn before Trim: " + KeyIn);
                if (KeyIn.Substring(0, 1) == "X")
                {
                    //Log.Write("X Key");
                    KeyIn = KeyIn.Substring(1);
                }
                //Log.Write("KeyIn after Trim: " + KeyIn);
                string SourceIn = sendKeyInfo.iSource;

                if (KeyIn.Contains("U"))
                {

                    switch (KeyIn)
                    {
                        //case "C0": playHandleArray[0].Stop(); break;
                        //case "CS0": playHandleArray[1].Stop(); break;
                        //case "D0": playHandleArray[2].Stop(); break;
                        //case "DS0": playHandleArray[3].Stop(); break;
                        //case "E0": playHandleArray[4].Stop(); break;
                        //case "F0": playHandleArray[5].Stop(); break;
                        //case "FS0": playHandleArray[6].Stop()); break;
                        //case "G0": playHandleArray[7].Stop(); break;
                        //case "GS0": playHandleArray[8].Stop()], S0Offset[8]); break;
                        //case "A0": playHandleArray[9].Stop(); break;
                        //case "AS0": playHandleArray[10].Stop(); break;
                        //case "B0": playHandleArray[11].Stop(); break;
                        case "C1Up": playHandleArray[12].Stop(); break;
                        case "CS1Up": playHandleArray[13].Stop(); break;
                        case "D1Up": playHandleArray[14].Stop(); break;
                        case "DS1Up": playHandleArray[15].Stop(); break;
                        case "E1Up": playHandleArray[16].Stop(); break;
                        case "F1Up": playHandleArray[17].Stop(); break;
                        case "FS1Up": playHandleArray[18].Stop(); break;
                        case "G1Up": playHandleArray[19].Stop(); break;
                        case "GS1Up": playHandleArray[20].Stop(); break;
                        case "A1Up": playHandleArray[21].Stop(); break;
                        case "AS1Up": playHandleArray[22].Stop(); break;
                        case "B1Up": playHandleArray[23].Stop(); break;
                        case "C2Up": playHandleArray[24].Stop(); break;
                        case "CS2Up": playHandleArray[25].Stop(); break;
                        case "D2Up": playHandleArray[26].Stop(); break;
                        case "DS2Up": playHandleArray[27].Stop(); break;
                        case "E2Up": playHandleArray[28].Stop(); break;
                        case "F2Up": playHandleArray[29].Stop(); break;
                        case "FS2Up": playHandleArray[30].Stop(); break;
                        case "G2Up": playHandleArray[31].Stop(); break;
                        case "GS2Up": playHandleArray[32].Stop(); break;
                        case "A2Up": playHandleArray[33].Stop(); break;
                        case "AS2Up": playHandleArray[34].Stop(); break;
                        case "B2Up": playHandleArray[35].Stop(); break;
                        case "C3Up": playHandleArray[36].Stop(); break;
                        case "CS3Up": playHandleArray[37].Stop(); break;
                        case "D3Up": playHandleArray[38].Stop(); break;
                        case "DS3Up": playHandleArray[39].Stop(); break;
                        case "E3Up": playHandleArray[40].Stop(); break;
                        case "F3Up": playHandleArray[41].Stop(); break;
                        case "FS3Up": playHandleArray[42].Stop(); break;
                        case "G3Up": playHandleArray[43].Stop(); break;
                        case "GS3Up": playHandleArray[44].Stop(); break;
                        case "A3Up": playHandleArray[45].Stop(); break;
                        case "AS3Up": playHandleArray[46].Stop(); break;
                        case "B3Up": playHandleArray[47].Stop(); break;
                        case "C4Up": playHandleArray[48].Stop(); break;
                        case "CS4Up": playHandleArray[49].Stop(); break;
                        case "D4Up": playHandleArray[50].Stop(); break;
                        case "DS4Up": playHandleArray[51].Stop(); break;
                        case "E4Up": playHandleArray[52].Stop(); break;
                        case "F4Up": playHandleArray[53].Stop(); break;
                        case "FS4Up": playHandleArray[54].Stop(); break;
                        case "G4Up": playHandleArray[55].Stop(); break;
                        case "GS4Up": playHandleArray[56].Stop(); break;
                        case "A4Up": playHandleArray[57].Stop(); break;
                        case "AS4Up": playHandleArray[58].Stop(); break;
                        case "B4Up": playHandleArray[59].Stop(); break;
                        case "C5Up": playHandleArray[60].Stop(); break;
                        case "CS5Up": playHandleArray[61].Stop(); break;
                        case "D5Up": playHandleArray[62].Stop(); break;
                        case "DS5Up": playHandleArray[63].Stop(); break;
                        case "E5Up": playHandleArray[64].Stop(); break;
                        case "F5Up": playHandleArray[65].Stop(); break;
                        case "FS5Up": playHandleArray[66].Stop(); break;
                        case "G5Up": playHandleArray[67].Stop(); break;
                        case "GS5Up": playHandleArray[68].Stop(); break;
                        case "A5Up": playHandleArray[69].Stop(); break;
                        case "AS5Up": playHandleArray[70].Stop(); break;
                        case "B5Up": playHandleArray[71].Stop(); break;
                        case "C6Up": playHandleArray[72].Stop(); break;
                        case "CS6Up": playHandleArray[73].Stop(); break; ;
                        case "D6Up": playHandleArray[74].Stop(); break;
                        case "DS6Up": playHandleArray[75].Stop(); break;
                        case "E6Up": playHandleArray[76].Stop(); break;
                        case "F6Up": playHandleArray[77].Stop(); break;
                        case "FS6Up": playHandleArray[78].Stop(); break;
                        case "G6Up": playHandleArray[79].Stop(); break;
                        case "GS6Up": playHandleArray[80].Stop(); break;
                        case "A6Up": playHandleArray[81].Stop(); break;
                        case "AS6Up": playHandleArray[82].Stop(); break;
                        case "B6Up": playHandleArray[83].Stop(); break;
                        //case "C7Up": playHandleArray[84].Stop(); break;
                        //case "CS7Up": playHandleArray[85].Stop(); break;
                        //case "D7Up": playHandleArray[86].Stop(); break;
                        //case "DS7Up":playHandleArray[87].Stop(); break;
                        //case "E7Up": playHandleArray[88].Stop(); break;
                        //case "F7Up": playHandleArray[89].Stop(); break;
                        //case "FS7Up": playHandleArray[90].Stop(); break;
                        //case "G7Up": playHandleArray[91].Stop(); break;
                        //case "GS7Up": playHandleArray[92].Stop(); break;
                        //case "A7Up": playHandleArray[93].Stop(); break;
                        //case "AS7Up": playHandleArray[94].Stop(); break;
                        //case "B7Up": playHandleArray[95].Stop(); break;
                    }
                }
                else
                {
                    switch (KeyIn)
                    {
                        //case "C0": PlayNote(SourceIn, 0, S0SoundResources[0], S0Offset[0]); break;
                        //case "CS0": PlayNote(SourceIn, 1, S0SoundResources[1], S0Offset[1]); break;
                        //case "D0": PlayNote(SourceIn, 2, S0SoundResources[2], S0Offset[2]); break;
                        //case "DS0": PlayNote(SourceIn, 3, S0SoundResources[3], S0Offset[3]); break;
                        //case "E0": PlayNote(SourceIn, 4, S0SoundResources[4], S0Offset[4]); break;
                        //case "F0": PlayNote(SourceIn, 5, S0SoundResources[5], S0Offset[5]); break;
                        //case "FS0": PlayNote(SourceIn, 6, S0SoundResources[6], S0Offset[6]); break;
                        //case "G0": PlayNote(SourceIn, 7, S0SoundResources[7], S0Offset[7]); break;
                        //case "GS0": PlayNote(SourceIn, 8, S0SoundResources[8], S0Offset[8]); break;
                        //case "A0": PlayNote(SourceIn, 9, S0SoundResources[9], S0Offset[9]); break;
                        //case "AS0": PlayNote(SourceIn, 10, S0SoundResources[10], S0Offset[10]); break;
                        //case "B0": PlayNote(SourceIn, 11 S0SoundResources[11], S0Offset[11]); break;
                        case "C1": PlayNote(SourceIn, 12, S1SoundResources[0], S1Offset[0]); break;
                        case "CS1": PlayNote(SourceIn, 13, S1SoundResources[1], S1Offset[1]); break;
                        case "D1": PlayNote(SourceIn, 14, S1SoundResources[2], S1Offset[2]); break;
                        case "DS1": PlayNote(SourceIn, 15, S1SoundResources[3], S1Offset[3]); break;
                        case "E1": PlayNote(SourceIn, 16, S1SoundResources[4], S1Offset[4]); break;
                        case "F1": PlayNote(SourceIn, 17, S1SoundResources[5], S1Offset[5]); break;
                        case "FS1": PlayNote(SourceIn, 18, S1SoundResources[6], S1Offset[6]); break;
                        case "G1": PlayNote(SourceIn, 19, S1SoundResources[7], S1Offset[7]); break;
                        case "GS1": PlayNote(SourceIn, 20, S1SoundResources[8], S1Offset[8]); break;
                        case "A1": PlayNote(SourceIn, 21, S1SoundResources[9], S1Offset[9]); break;
                        case "AS1": PlayNote(SourceIn, 22, S1SoundResources[10], S1Offset[10]); break;
                        case "B1": PlayNote(SourceIn, 23, S1SoundResources[11], S1Offset[11]); break;
                        case "C2": PlayNote(SourceIn, 24, S2SoundResources[0], S2Offset[0]); break;
                        case "CS2": PlayNote(SourceIn, 25, S2SoundResources[1], S2Offset[1]); break;
                        case "D2": PlayNote(SourceIn, 26, S2SoundResources[2], S2Offset[2]); break;
                        case "DS2": PlayNote(SourceIn, 27, S2SoundResources[3], S2Offset[3]); break;
                        case "E2": PlayNote(SourceIn, 28, S2SoundResources[4], S2Offset[4]); break;
                        case "F2": PlayNote(SourceIn, 29, S2SoundResources[5], S2Offset[5]); break;
                        case "FS2": PlayNote(SourceIn, 30, S2SoundResources[6], S2Offset[6]); break;
                        case "G2": PlayNote(SourceIn, 31, S2SoundResources[7], S2Offset[7]); break;
                        case "GS2": PlayNote(SourceIn, 32, S2SoundResources[8], S2Offset[8]); break;
                        case "A2": PlayNote(SourceIn, 33, S2SoundResources[9], S2Offset[9]); break;
                        case "AS2": PlayNote(SourceIn, 34, S2SoundResources[10], S2Offset[10]); break;
                        case "B2": PlayNote(SourceIn, 35, S2SoundResources[11], S2Offset[11]); break;
                        case "C3": PlayNote(SourceIn, 36, S3SoundResources[0], S3Offset[0]); break;
                        case "CS3": PlayNote(SourceIn, 37, S3SoundResources[1], S3Offset[1]); break;
                        case "D3": PlayNote(SourceIn, 38, S3SoundResources[2], S3Offset[2]); break;
                        case "DS3": PlayNote(SourceIn, 39, S3SoundResources[3], S3Offset[3]); break;
                        case "E3": PlayNote(SourceIn, 40, S3SoundResources[4], S3Offset[4]); break;
                        case "F3": PlayNote(SourceIn, 41, S3SoundResources[5], S3Offset[5]); break;
                        case "FS3": PlayNote(SourceIn, 42, S3SoundResources[6], S3Offset[6]); break;
                        case "G3": PlayNote(SourceIn, 43, S3SoundResources[7], S3Offset[7]); break;
                        case "GS3": PlayNote(SourceIn, 44, S3SoundResources[8], S3Offset[8]); break;
                        case "A3": PlayNote(SourceIn, 45, S3SoundResources[9], S3Offset[9]); break;
                        case "AS3": PlayNote(SourceIn, 46, S3SoundResources[10], S3Offset[10]); break;
                        case "B3": PlayNote(SourceIn, 47, S3SoundResources[11], S3Offset[11]); break;
                        case "C4": PlayNote(SourceIn, 48, S4SoundResources[0], S4Offset[0]); break;
                        case "CS4": PlayNote(SourceIn, 49, S4SoundResources[1], S4Offset[1]); break;
                        case "D4": PlayNote(SourceIn, 50, S4SoundResources[2], S4Offset[2]); break;
                        case "DS4": PlayNote(SourceIn, 51, S4SoundResources[3], S4Offset[3]); break;
                        case "E4": PlayNote(SourceIn, 52, S4SoundResources[4], S4Offset[4]); break;
                        case "F4": PlayNote(SourceIn, 53, S4SoundResources[5], S4Offset[5]); break;
                        case "FS4": PlayNote(SourceIn, 54, S4SoundResources[6], S4Offset[6]); break;
                        case "G4": PlayNote(SourceIn, 55, S4SoundResources[7], S4Offset[7]); break;
                        case "GS4": PlayNote(SourceIn, 56, S4SoundResources[8], S4Offset[8]); break;
                        case "A4": PlayNote(SourceIn, 57, S4SoundResources[9], S4Offset[9]); break;
                        case "AS4": PlayNote(SourceIn, 58, S4SoundResources[10], S4Offset[10]); break;
                        case "B4": PlayNote(SourceIn, 59, S4SoundResources[11], S4Offset[11]); break;
                        case "C5": PlayNote(SourceIn, 60, S5SoundResources[0], S5Offset[0]); break;
                        case "CS5": PlayNote(SourceIn, 61, S5SoundResources[1], S5Offset[1]); break;
                        case "D5": PlayNote(SourceIn, 62, S5SoundResources[2], S5Offset[2]); break;
                        case "DS5": PlayNote(SourceIn, 63, S5SoundResources[3], S5Offset[3]); break;
                        case "E5": PlayNote(SourceIn, 64, S5SoundResources[4], S5Offset[4]); break;
                        case "F5": PlayNote(SourceIn, 65, S5SoundResources[5], S5Offset[5]); break;
                        case "FS5": PlayNote(SourceIn, 66, S5SoundResources[6], S5Offset[6]); break;
                        case "G5": PlayNote(SourceIn, 67, S5SoundResources[7], S5Offset[7]); break;
                        case "GS5": PlayNote(SourceIn, 68, S5SoundResources[8], S5Offset[8]); break;
                        case "A5": PlayNote(SourceIn, 69, S5SoundResources[9], S5Offset[9]); break;
                        case "AS5": PlayNote(SourceIn, 70, S5SoundResources[10], S5Offset[10]); break;
                        case "B5": PlayNote(SourceIn, 71, S5SoundResources[11], S5Offset[11]); break;
                        case "C6": PlayNote(SourceIn, 72, S6SoundResources[0], S6Offset[0]); break;
                        case "CS6": PlayNote(SourceIn, 73, S6SoundResources[1], S6Offset[1]); break;
                        case "D6": PlayNote(SourceIn, 74, S6SoundResources[2], S6Offset[2]); break;
                        case "DS6": PlayNote(SourceIn, 75, S6SoundResources[3], S6Offset[3]); break;
                        case "E6": PlayNote(SourceIn, 76, S6SoundResources[4], S6Offset[4]); break;
                        case "F6": PlayNote(SourceIn, 77, S6SoundResources[5], S6Offset[5]); break;
                        case "FS6": PlayNote(SourceIn, 78, S6SoundResources[6], S6Offset[6]); break;
                        case "G6": PlayNote(SourceIn, 79, S6SoundResources[7], S6Offset[7]); break;
                        case "GS6": PlayNote(SourceIn, 80, S6SoundResources[8], S6Offset[8]); break;
                        case "A6": PlayNote(SourceIn, 81, S6SoundResources[9], S6Offset[9]); break;
                        case "AS6": PlayNote(SourceIn, 82, S6SoundResources[10], S6Offset[10]); break;
                        case "B6": PlayNote(SourceIn, 83, S6SoundResources[11], S6Offset[11]); break;
                            //case "C7": PlayNote(SourceIn, 84, S7SoundResources[0], S7Offset[0]); break;
                            //case "CS7": PlayNote(SourceIn, 85, S7SoundResources[1], S7Offset[1]); break;
                            //case "D7": PlayNote(SourceIn, 86, S7SoundResources[2], S7Offset[2]); break;
                            //case "DS7": PlayNote(SourceIn, 87, S7SoundResources[3], S7Offset[3]); break;
                            //case "E7": PlayNote(SourceIn, 88, S7SoundResources[4], S7Offset[4]); break;
                            //case "F7": PlayNote(SourceIn, 89, S7SoundResources[5], S7Offset[5]); break;
                            //case "FS7": PlayNote(SourceIn, 90, S7SoundResources[6], S7Offset[6]); break;
                            //case "G7": PlayNote(SourceIn, 91, S7SoundResources[7], S7Offset[7]); break;
                            //case "GS7": PlayNote(SourceIn, 92, S7SoundResources[8], S7Offset[8]); break;
                            //case "A7": PlayNote(SourceIn, 93, S7SoundResources[9], S7Offset[9]); break;
                            //case "AS7": PlayNote(SourceIn, 94, S7SoundResources[10], S7Offset[10]); break;
                            //case "B7": PlayNote(SourceIn, 95, S7SoundResources[11], S7Offset[11]); break;
                    }
                }
 

            }
        }
    }

    private void StopNote(int keyIndexIn)
    {
        if (playHandleArray[keyIndexIn].IsPlaying())
        {
            playHandleArray[keyIndexIn].Stop();
        }
    }

    /*
public interface IPanicInfo
{
    string panic { get; }
}


private void getPanicInfo(ScriptEventData gotPanicInfo)
{
        if (gotPanicInfo.Data == null)
        {
            return;
        }

        IPanicInfo sendPanicInfo = gotPanicInfo.Data.AsInterface<IPanicInfo>();
        if (sendPanicInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        if (sendPanicInfo.panic == "")
        {
        int cntr = 0;
        Log.Write("PANIC!!!!!!");
        do
        {
            if (playHandleArray[cntr].IsPlaying())
            {
                playHandleArray[cntr].Stop();
            }
            cntr++;
        } while (cntr < playHandleArray.Count());
    }
}
*/

    #endregion

    public override void Init()
    {
        //Build Instrument using samples and offsets
        //Listen for Key Event
        SubscribeToScriptEvent("KeySent", getKeyInfo);
        SubscribeToAll(EnableEvent, EnableModule);
        Log.Write("Action Synth Subscribing to: " + EnableEvent);
        SubscribeToAll(DisableEvent, DisableModule);
        SubscribeToAll(PanicEvent, ExecutePanic);
        //int cntr = 0;
        //playSettings = PlaySettings.Looped;
        //do
        //{
        //    playHandleArray[cntr] = ScenePrivate.PlaySound(S1SoundResources[0], playSettings);
        //} while (cntr < playHandleArray.Count());
    }

    private void ExecutePanic(ScriptEventData data)
    {
        int cntr = 0;
        Log.Write("PANIC!!!!!! playHandleArray: " + playHandleArray + " Count: " + playHandleArray.Count() + " EnableModule: " + EnableEvent);
        do
        {
           //Log.Write("Before");
            if (playHandleArray[cntr] != null)
            {
                if (playHandleArray[cntr].IsPlaying())
                {
                    //Log.Write("Found: " + cntr);
                    playHandleArray[cntr].Stop();
                }
            }

            cntr++;
            //Log.Write(cntr.ToString());
        } while (cntr < 127);
    }

    private void EnableModule(ScriptEventData sed)
    {
        enabled = true;
    }

    private void DisableModule(ScriptEventData sed)
    {
        enabled = false;
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region PlayNotes

    private void PlayNote(string Source, int noteIndex, SoundResource PlaySample, float PitchShiftIn)
    {

        //Log.Write("Source: " + Source + " noteIndex: " + noteIndex);
        playSettings = PlaySettings.Looped;
        //playSettings = LoopSustain ? PlaySettings.Looped : PlaySettings.PlayOnce;
        //if (Source == "KeySend")
        //{
        //    playSettings = PlaySettings.PlayOnce;
        //}
        //else
        //{
        //    playSettings = PlaySettings.Looped;
        //}
        playSettings.Loudness = loudnessIn; // set in Configuration
        playSettings.DontSync = true; // TrackDont_Sync[LoopIn2];
        playSettings.PitchShift = PitchShiftIn;
        //playHandle[LoopIn2] = ScenePrivate.PlaySound(TrackSamples[LoopIn2][PlayIndexIn], playSettings);
        playHandleArray[noteIndex] = ScenePrivate.PlaySound(PlaySample, playSettings);
        //if (Source == "KeySend")
        //{
        //    Wait(TimeSpan.FromSeconds(PlaySample.GetDurationInSeconds()));
        //    playHandleArray[noteIndex].Stop();
        //}
    }

    #endregion

}
