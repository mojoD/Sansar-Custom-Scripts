//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class ActionSynthModuleJammer : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("K2")]
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

    public bool JammerKeyboard = false;

    public string EnableEvent;
    public string DisableEvent;

    [DefaultValue("Panic")]
    [DisplayName("Panic Event: ")]
    public string PanicEvent = "Panic";

    private List<SoundResource> ScaleSoundResources = new List<SoundResource>();
    private List<float> ScaleOffset = new List<float>();

    private float loudnessIn = 0;
    private bool enabled = false;
    private string KeyIn = null;
    private PlaySettings playSettings;
    private PlayHandle playHandleSimple;
    private PlayHandle[] playHandleArray = new PlayHandle[127];
    private PlaySettings[] playSettingsArray = new PlaySettings[127];

    private string startNote;
    private string scaleIn;
    private int octavesIn;

    private const int c0 = 12; const int db0 = 13; const int d0 = 14; const int eb0 = 15; const int e0 = 16; const int f0 = 17; const int gb0 = 18; const int g0 = 19; const int ab0 = 20; const int a0 = 21; const int bb0 = 22; const int b0 = 23;
    private const int c1 = 24; const int db1 = 25; const int d1 = 26; const int eb1 = 27; const int e1 = 28; const int f1 = 29; const int gb1 = 30; const int g1 = 31; const int ab1 = 32; const int a1 = 33; const int bb1 = 34; const int b1 = 35;
    private const int c2 = 36; const int db2 = 37; const int d2 = 38; const int eb2 = 39; const int e2 = 40; const int f2 = 41; const int gb2 = 52; const int g2 = 43; const int ab2 = 44; const int a2 = 45; const int bb2 = 46; const int b2 = 47;
    private const int c3 = 48; const int db3 = 49; const int d3 = 50; const int eb3 = 51; const int e3 = 52; const int f3 = 53; const int gb3 = 54; const int g3 = 55; const int ab3 = 56; const int a3 = 57; const int bb3 = 58; const int b3 = 59;
    private const int c4 = 60; const int db4 = 61; const int d4 = 62; const int eb4 = 63; const int e4 = 64; const int f4 = 65; const int gb4 = 66; const int g4 = 67; const int ab4 = 68; const int a4 = 69; const int bb4 = 70; const int b4 = 71;
    private const int c5 = 72; const int db5 = 73; const int d5 = 74; const int eb5 = 75; const int e5 = 76; const int f5 = 77; const int gb5 = 78; const int g5 = 79; const int ab5 = 80; const int a5 = 81; const int bb5 = 82; const int b5 = 83;
    private const int c6 = 84; const int db6 = 85; const int d6 = 86; const int eb6 = 87; const int e6 = 88; const int f6 = 89; const int gb6 = 90; const int g6 = 91; const int ab6 = 92; const int a6 = 93; const int bb6 = 94; const int b6 = 95;
    private const int c7 = 96; const int db7 = 97; const int d7 = 98; const int eb7 = 99; const int e7 = 100; const int f7 = 101; const int gb7 = 102; const int g7 = 103; const int ab7 = 104; const int a7 = 105; const int bb7 = 106; const int b7 = 107;
    private const int c8 = 108; const int db8 = 109; const int d8 = 110; const int eb8 = 111; const int e8 = 112; const int f8 = 113; const int gb8 = 114; const int g8 = 115; const int ab8 = 116; const int a8 = 117; const int bb8 = 118; const int b8 = 119;
    private const int c9 = 120; const int db9 = 121; const int d9 = 122; const int eb9 = 123; const int e9 = 124; const int f9 = 125; const int gb9 = 126; const int g9 = 127;

    private List<string> MidiNote = new List<string>();
    private List<int> TrackNotes = new List<int>();

    //scales
    private static int[] major = { 2, 2, 1, 2, 2, 2, 1 };
    private static int[] dorian = { 1, 2, 2, 1, 2, 2, 2 };
    private static int[] phrygian = { 2, 1, 2, 2, 1, 2, 2 };
    private static int[] lydian = { 2, 2, 1, 2, 2, 1, 2 };
    private static int[] mixolydian = { 2, 2, 2, 1, 2, 2, 1 };
    private static int[] aelian = { 1, 2, 2, 2, 1, 2, 2 };
    private static int[] minor = { 2, 1, 2, 2, 2, 1, 2 };
    private static int[] minor_pentatonic = { 3, 2, 2, 3, 2 };
    private static int[] major_pentatonic = { 2, 3, 2, 2, 3 };
    private static int[] egyptian = { 3, 2, 3, 2, 2 };
    private static int[] jiao = { 2, 3, 2, 3, 2 };
    private static int[] zhi = { 2, 2, 3, 2, 3 };
    private static int[] whole_tone = { 2, 2, 2, 2, 2, 2 };
    private static int[] whole = { 2, 2, 2, 2, 2, 2 };
    private static int[] chromatic = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private static int[] harmonic_minor = { 2, 1, 2, 2, 1, 3, 1 };
    private static int[] melodic_minor_asc = { 2, 1, 2, 2, 2, 2, 1 };
    private static int[] hungarian_minor = { 2, 1, 3, 1, 1, 3, 1 };
    private static int[] octatonic = { 2, 1, 2, 1, 2, 1, 2, 1 };
    private static int[] messiaen1 = { 2, 2, 2, 2, 2, 2 };
    private static int[] messiaen2 = { 1, 2, 1, 2, 1, 2, 1, 2 };
    private static int[] messiaen3 = { 2, 1, 1, 2, 1, 1, 2, 1, 1 };
    private static int[] messiaen4 = { 1, 1, 3, 1, 1, 1, 3, 1 };
    private static int[] messiaen5 = { 1, 4, 1, 1, 4, 1 };
    private static int[] messiaen6 = { 2, 2, 1, 1, 2, 2, 1, 1 };
    private static int[] messiaen7 = { 1, 1, 1, 2, 1, 1, 1, 1, 2, 1 };
    private static int[] super_locrian = { 1, 2, 1, 2, 2, 2, 2 };
    private static int[] hirajoshi = { 2, 1, 4, 1, 4 };
    private static int[] kumoi = { 2, 1, 4, 2, 3 };
    private static int[] neapolitan_major = { 1, 2, 2, 2, 2, 2, 1 };
    private static int[] bartok = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] bhairav = { 1, 3, 1, 2, 1, 3, 1 };
    private static int[] locrian_major = { 2, 2, 1, 1, 2, 2, 2 };
    private static int[] ahirbhairav = { 1, 3, 1, 2, 2, 1, 2 };
    private static int[] enigmatic = { 1, 3, 2, 2, 2, 1, 1 };
    private static int[] neapolitan_minor = { 1, 2, 2, 2, 1, 3, 1 };
    private static int[] pelog = { 1, 2, 4, 1, 4 };
    private static int[] augmented2 = { 1, 3, 1, 3, 1, 3 };
    private static int[] scriabin = { 1, 3, 3, 2, 3 };
    private static int[] harmonic_major = { 2, 2, 1, 2, 1, 3, 1 };
    private static int[] melodic_minor_desc = { 2, 1, 2, 2, 1, 2, 2 };
    private static int[] romanian_minor = { 2, 1, 3, 1, 2, 1, 2 };
    private static int[] hindu = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] iwato = { 1, 4, 1, 4, 2 };
    private static int[] melodic_minor = { 2, 1, 2, 2, 2, 2, 1 };
    private static int[] diminished2 = { 2, 1, 2, 1, 2, 1, 2, 1 };
    private static int[] marva = { 1, 3, 2, 1, 2, 2, 1 };
    private static int[] melodic_major = { 2, 2, 1, 2, 1, 2, 2 };
    private static int[] indian = { 4, 1, 2, 3, 2 };
    private static int[] spanish = { 1, 3, 1, 2, 1, 2, 2 };
    private static int[] prometheus = { 2, 2, 2, 5, 1 };
    private static int[] diminished = { 1, 2, 1, 2, 1, 2, 1, 2 };
    private static int[] todi = { 1, 2, 3, 1, 1, 3, 1 };
    private static int[] leading_whole = { 2, 2, 2, 2, 2, 1, 1 };
    private static int[] augmented = { 3, 1, 3, 1, 3, 1 };
    private static int[] purvi = { 1, 3, 2, 1, 1, 3, 1 };
    private static int[] chinese = { 4, 2, 1, 4, 1 };
    //chords
    private static int[] chdmajor = { 0, 4, 7 };
    private static int[] chdminor = { 0, 3, 7 };
    private static int[] chdmajor7 = { 0, 4, 7, 11 };
    private static int[] chddom7 = { 0, 4, 7, 10 };
    private static int[] chdminor7 = { 0, 3, 7, 10 };
    private static int[] chdaug = { 0, 4, 8 };
    private static int[] chddim = { 0, 3, 6 };
    private static int[] chddim7 = { 0, 3, 6, 9 };
    private static int[] chdhalfdim = { 0, 3, 6, 10 };
    private static int[] chd1 = { 0 };
    private static int[] chd5 = { 0, 7 };
    private static int[] chdmaug5 = { 0, 3, 8 };
    private static int[] chdsus2 = { 0, 2, 7 };
    private static int[] chdsus4 = { 0, 5, 7 };
    private static int[] chd6 = { 0, 4, 7, 9 };
    private static int[] chdm6 = { 0, 3, 7, 9 };
    private static int[] chd7sus2 = { 0, 2, 7, 10 };
    private static int[] chd7sus4 = { 0, 5, 7, 10 };
    private static int[] chd7dim5 = { 0, 4, 6, 10 };
    private static int[] chd7aug5 = { 0, 4, 8, 10 };
    private static int[] chdm7aug5 = { 0, 3, 8, 10 };
    private static int[] chd9 = { 0, 4, 7, 10, 14 };
    private static int[] chdm9 = { 0, 3, 7, 10, 14 };
    private static int[] chdm7aug9 = { 0, 3, 7, 10, 14 };
    private static int[] chdmaj9 = { 0, 4, 7, 11, 14 };
    private static int[] chd9sus4 = { 0, 5, 7, 10, 14 };
    private static int[] chd6sus9 = { 0, 4, 7, 9, 14 };
    private static int[] chdm6sus9 = { 0, 3, 9, 7, 14 };
    private static int[] chd7dim9 = { 0, 4, 7, 10, 13 };
    private static int[] chdm7dim9 = { 0, 3, 7, 10, 13 };
    private static int[] chd7dim10 = { 0, 4, 7, 10, 15 };
    private static int[] chd7dim11 = { 0, 4, 7, 10, 16 };
    private static int[] chd7dim13 = { 0, 4, 7, 10, 20 };
    private static int[] chd9dim5 = { 0, 10, 13 };
    private static int[] chdm9dim5 = { 0, 10, 14 };
    private static int[] chd7aug5dim9 = { 0, 4, 8, 10, 13 };
    private static int[] chdm7aug5dim9 = { 0, 3, 8, 10, 13 };
    private static int[] chd11 = { 0, 4, 7, 10, 14, 17 };
    private static int[] chdm11 = { 0, 3, 7, 10, 14, 17 };
    private static int[] chdmaj11 = { 0, 4, 7, 11, 14, 17 };
    private static int[] chd11aug = { 0, 4, 7, 10, 14, 18 };
    private static int[] chdm11aug = { 0, 3, 7, 10, 14, 18 };
    private static int[] chd13 = { 0, 4, 7, 10, 14, 17, 21 };
    private static int[] chdm13 = { 0, 3, 7, 10, 14, 17, 21 };
    private static int[] chdadd2 = { 0, 2, 4, 7 };
    private static int[] chdadd4 = { 0, 4, 5, 7 };
    private static int[] chdadd9 = { 0, 4, 7, 14 };
    private static int[] chdadd11 = { 0, 4, 7, 17 };
    private static int[] add13 = { 0, 4, 7, 21 };
    private static int[] madd2 = { 0, 2, 3, 7 };
    private static int[] madd4 = { 0, 3, 5, 7 };
    private static int[] madd9 = { 0, 3, 7, 14 };
    private static int[] madd11 = { 0, 3, 7, 17 };
    private static int[] madd13 = { 0, 3, 7, 21 };

    private List<string> strTempNotes = new List<string>();
    private List<int> intTempNotes = new List<int>();
    private List<string> ReturnNotes = new List<string>();

    private static string[] validNotes = { "c0", "db0", "d0", "eb0", "e0", "f0", "gb0", "g0", "ab0", "a0", "bb0", "b0", "c1", "db1", "d1", "eb1", "e1", "f1", "gb1", "g1", "ab1", "a1", "bb1", "b1", "c2", "db2", "d2", "eb2", "e2", "f2", "gb2", "g2", "ab2", "a2", "bb2", "b2", "c3", "db3", "d3", "eb3", "e3", "f3", "gb3", "g3", "ab3", "a3", "bb3", "b3", "c4", "db4", "d4", "eb4", "e4", "f4", "gb4", "g4", "ab4", "a4", "bb4", "b4", "c5", "db5", "d5", "eb5", "e5", "f5", "gb5", "g5", "ab5", "a5", "bb5", "b5", "c6", "db6", "d6", "eb6", "e6", "f6", "gb6", "g6", "ab6", "a6", "bb6", "b6", "c7", "db7", "d7", "eb7", "e7", "f7", "gb7", "g7", "ab7", "a7", "bb7", "b7", "c8", "db8", "d8", "eb8", "e8", "f8", "gb8", "g8", "ab8", "a8", "bb8", "b8", "c9", "db9", "d9", "eb9", "e9", "f9", "gb9", "g9" };
    private string Errors = "Errors: ";
    private string Errormsg = "No Errors";
    private bool strErrors = false;
    private int NumberOfNotes = 0;

    private int instrumentcntr = 0;
    private List<string> InstrumentName = new List<string>();

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

    private void getKeyInfo(ScriptEventData gotKeyInfo)
    {
        if (enabled)
        {
            if (JammerKeyboard)
            {
                //Log.Write("ActionSynthModule: In getKeyInfo");
                //Log.Write("ScaleSoundResources: " + ScaleSoundResources.Count());
                //Log.Write("ScaleOffset: " + ScaleOffset.Count());
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
                            case "Key1Up": if (playHandleArray[12].IsPlaying()) { playHandleArray[12].Stop(); } break; 
                            case "Key2Up": if (playHandleArray[13].IsPlaying()) { playHandleArray[13].Stop(); } break;
                            case "Key3Up": if (playHandleArray[14].IsPlaying()) { playHandleArray[14].Stop(); } break;
                            case "Key4Up": if (playHandleArray[15].IsPlaying()) { playHandleArray[15].Stop(); } break;
                            case "Key5Up": if (playHandleArray[16].IsPlaying()) { playHandleArray[16].Stop(); } break;
                            case "Key6Up": if (playHandleArray[17].IsPlaying()) { playHandleArray[17].Stop(); } break;
                            case "Key7Up": if (playHandleArray[18].IsPlaying()) { playHandleArray[18].Stop(); } break;
                            case "Key8Up": if (playHandleArray[19].IsPlaying()) { playHandleArray[19].Stop(); } break;
                            case "Key9Up": if (playHandleArray[20].IsPlaying()) { playHandleArray[20].Stop(); } break;
                            case "Key10Up": if (playHandleArray[21].IsPlaying()) { playHandleArray[21].Stop(); } break;
                            case "Key11Up": if (playHandleArray[22].IsPlaying()) { playHandleArray[22].Stop(); } break;
                            case "Key12Up": if (playHandleArray[23].IsPlaying()) { playHandleArray[23].Stop(); } break;
                            case "Key13Up": if (playHandleArray[24].IsPlaying()) { playHandleArray[24].Stop(); } break;
                            case "Key14Up": if (playHandleArray[25].IsPlaying()) { playHandleArray[25].Stop(); } break;
                            case "Key15Up": if (playHandleArray[26].IsPlaying()) { playHandleArray[26].Stop(); } break;
                            case "Key16Up": if (playHandleArray[27].IsPlaying()) { playHandleArray[27].Stop(); } break;
                            case "Key17Up": if (playHandleArray[28].IsPlaying()) { playHandleArray[28].Stop(); } break;
                            case "Key18Up": if (playHandleArray[29].IsPlaying()) { playHandleArray[29].Stop(); } break;
                            case "Key19Up": if (playHandleArray[30].IsPlaying()) { playHandleArray[30].Stop(); } break;
                            case "Key20Up": if (playHandleArray[31].IsPlaying()) { playHandleArray[31].Stop(); } break;
                            case "Key21Up": if (playHandleArray[32].IsPlaying()) { playHandleArray[32].Stop(); } break;
                            case "Key22Up": if (playHandleArray[33].IsPlaying()) { playHandleArray[33].Stop(); } break;
                            case "Key23Up": if (playHandleArray[34].IsPlaying()) { playHandleArray[34].Stop(); } break;
                            case "Key24Up": if (playHandleArray[35].IsPlaying()) { playHandleArray[35].Stop(); } break;
                            case "Key25Up": if (playHandleArray[36].IsPlaying()) { playHandleArray[36].Stop(); } break;
                            case "Key26Up": if (playHandleArray[37].IsPlaying()) { playHandleArray[37].Stop(); } break;
                            case "Key27Up": if (playHandleArray[38].IsPlaying()) { playHandleArray[38].Stop(); } break;
                            case "Key28Up": if (playHandleArray[39].IsPlaying()) { playHandleArray[39].Stop(); } break;
                            case "Key29Up": if (playHandleArray[40].IsPlaying()) { playHandleArray[40].Stop(); } break;
                            case "Key30Up": if (playHandleArray[41].IsPlaying()) { playHandleArray[41].Stop(); } break;
                            case "Key31Up": if (playHandleArray[42].IsPlaying()) { playHandleArray[42].Stop(); } break;
                            case "Key32Up": if (playHandleArray[43].IsPlaying()) { playHandleArray[43].Stop(); } break; 
                            case "Key33Up": if (playHandleArray[44].IsPlaying()) { playHandleArray[44].Stop(); } break;
                            case "Key34Up": if (playHandleArray[45].IsPlaying()) { playHandleArray[45].Stop(); } break;
                            case "Key35Up": if (playHandleArray[46].IsPlaying()) { playHandleArray[46].Stop(); } break;
                            case "Key36Up": if (playHandleArray[47].IsPlaying()) { playHandleArray[47].Stop(); } break;
                            case "Key37Up": if (playHandleArray[48].IsPlaying()) { playHandleArray[48].Stop(); } break;
                            case "Key38Up": if (playHandleArray[49].IsPlaying()) { playHandleArray[49].Stop(); } break;
                            case "Key39Up": if (playHandleArray[50].IsPlaying()) { playHandleArray[50].Stop(); } break;
                            case "Key40Up": if (playHandleArray[51].IsPlaying()) { playHandleArray[51].Stop(); } break;
                        }
                    }
                    else
                    {
                        switch (KeyIn)
                        {
                            case "Key1": PlayNote(SourceIn, 12, ScaleSoundResources[0], ScaleOffset[0]); break;
                            case "Key2": PlayNote(SourceIn, 13, ScaleSoundResources[1], ScaleOffset[1]); break;
                            case "Key3": PlayNote(SourceIn, 14, ScaleSoundResources[2], ScaleOffset[2]); break;
                            case "Key4": PlayNote(SourceIn, 15, ScaleSoundResources[3], ScaleOffset[3]); break;
                            case "Key5": PlayNote(SourceIn, 16, ScaleSoundResources[4], ScaleOffset[4]); break;
                            case "Key6": PlayNote(SourceIn, 17, ScaleSoundResources[5], ScaleOffset[5]); break;
                            case "Key7": PlayNote(SourceIn, 18, ScaleSoundResources[6], ScaleOffset[6]); break;
                            case "Key8": PlayNote(SourceIn, 19, ScaleSoundResources[7], ScaleOffset[7]); break;
                            case "Key9": PlayNote(SourceIn, 20, ScaleSoundResources[8], ScaleOffset[8]); break;
                            case "Key10": PlayNote(SourceIn, 21, ScaleSoundResources[9], ScaleOffset[9]); break;
                            case "Key11": PlayNote(SourceIn, 22, ScaleSoundResources[10], ScaleOffset[10]); break;
                            case "Key12": PlayNote(SourceIn, 23, ScaleSoundResources[11], ScaleOffset[11]); break;
                            case "Key13": PlayNote(SourceIn, 24, ScaleSoundResources[12], ScaleOffset[12]); break;
                            case "Key14": PlayNote(SourceIn, 25, ScaleSoundResources[13], ScaleOffset[13]); break;
                            case "Key15": PlayNote(SourceIn, 26, ScaleSoundResources[14], ScaleOffset[14]); break;
                            case "Key16": PlayNote(SourceIn, 27, ScaleSoundResources[15], ScaleOffset[15]); break;
                            case "Key17": PlayNote(SourceIn, 28, ScaleSoundResources[16], ScaleOffset[16]); break;
                            case "Key18": PlayNote(SourceIn, 29, ScaleSoundResources[17], ScaleOffset[17]); break;
                            case "Key19": PlayNote(SourceIn, 30, ScaleSoundResources[18], ScaleOffset[18]); break;
                            case "Key20": PlayNote(SourceIn, 31, ScaleSoundResources[19], ScaleOffset[19]); break;
                            case "Key21": PlayNote(SourceIn, 32, ScaleSoundResources[20], ScaleOffset[20]); break;
                            case "Key22": PlayNote(SourceIn, 33, ScaleSoundResources[21], ScaleOffset[21]); break;
                            case "Key23": PlayNote(SourceIn, 34, ScaleSoundResources[22], ScaleOffset[22]); break;
                            case "Key24": PlayNote(SourceIn, 35, ScaleSoundResources[23], ScaleOffset[23]); break;
                            case "Key25": PlayNote(SourceIn, 36, ScaleSoundResources[24], ScaleOffset[24]); break;
                            case "Key26": PlayNote(SourceIn, 37, ScaleSoundResources[25], ScaleOffset[25]); break;
                            case "Key27": PlayNote(SourceIn, 38, ScaleSoundResources[26], ScaleOffset[26]); break;
                            case "Key28": PlayNote(SourceIn, 39, ScaleSoundResources[27], ScaleOffset[27]); break;
                            case "Key29": PlayNote(SourceIn, 40, ScaleSoundResources[28], ScaleOffset[28]); break;
                            case "Key30": PlayNote(SourceIn, 41, ScaleSoundResources[29], ScaleOffset[29]); break;
                            case "Key31": PlayNote(SourceIn, 42, ScaleSoundResources[30], ScaleOffset[30]); break;
                            case "Key32": PlayNote(SourceIn, 43, ScaleSoundResources[31], ScaleOffset[31]); break;
                            case "Key33": PlayNote(SourceIn, 44, ScaleSoundResources[32], ScaleOffset[32]); break;
                            case "Key34": PlayNote(SourceIn, 45, ScaleSoundResources[33], ScaleOffset[33]); break;
                            case "Key35": PlayNote(SourceIn, 46, ScaleSoundResources[34], ScaleOffset[34]); break;
                            case "Key36": PlayNote(SourceIn, 47, ScaleSoundResources[35], ScaleOffset[35]); break;
                            case "Key37": PlayNote(SourceIn, 48, ScaleSoundResources[36], ScaleOffset[36]); break;
                            case "Key38": PlayNote(SourceIn, 49, ScaleSoundResources[37], ScaleOffset[37]); break;
                            case "Key39": PlayNote(SourceIn, 50, ScaleSoundResources[38], ScaleOffset[38]); break;
                            case "Key40": PlayNote(SourceIn, 51, ScaleSoundResources[39], ScaleOffset[39]); break;
                        }
                    }
                }
            }
            else
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
    }

    public interface ICurrentScaleInfo
    {
        AgentPrivate HitmanOut { get; }
        string ChannelOut { get; }
        string CurrentScale { get; }
    }

    private void getScaleInfo(ScriptEventData gotScaleInfo)
    {
        if (JammerKeyboard)
        {
            Log.Write("ActionSynthModule: In getScaleInfo");
            if (gotScaleInfo.Data == null)
            {
                return;
            }

            ICurrentScaleInfo sendScaleInfo = gotScaleInfo.Data.AsInterface<ICurrentScaleInfo>();

            if (sendScaleInfo == null)
            {
                Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
                return;
            }

            Log.Write("ChannelOut: " + sendScaleInfo.ChannelOut + " CurrentScale: " + sendScaleInfo.CurrentScale);
            if (sendScaleInfo.ChannelOut == InputChannel)
            {
                startNote = "c1";
                octavesIn = 7;
                scaleIn = sendScaleInfo.CurrentScale;
                SetUpScale();
            }
        }
    }

    private void SetUpScale()
    {
        TrackNotes.Clear();
        ScaleSoundResources.Clear();
        ScaleOffset.Clear();
        TrackNotes = BuildNotes();  //builds an array of integers that are the valid note numbers for the given scale and octaves
        //Log.Write("In Init after BuildNotes");
        int cntr = 0;
        //Log.Write("TrackNote Count: " + TrackNotes.Count());
        do
        {
            //Log.Write("TrackNote[" + cntr + "]: " + TrackNotes[cntr]);
            BuildScaleSounds(cntr, TrackNotes[cntr]);
            cntr++;
        } while (cntr < TrackNotes.Count());
        
        //cntr = 0;
        //do
        //{
        //    Log.Write("ScaleSoundResources: " + ScaleSoundResources[cntr].GetName() + " ScaleOffset: " + ScaleOffset[cntr]);
        //    cntr++;
        //} while (cntr < ScaleSoundResources.Count());
    }

/*
    private void StopNote(int keyIndexIn)
    {
        if (playHandleArray[keyIndexIn].IsPlaying())
        {
            playHandleArray[keyIndexIn].Stop();
        }
    }
*/

    #endregion

    public override void Init()
    {
        //Build Instrument using samples and offsets
        ScaleSoundResources.Clear();
        ScaleOffset.Clear();
        int i = 2;
        i++;
        Log.Write("In ActionSynthModuleJammer");
        if (JammerKeyboard)
        {
            startNote = "c1";
            scaleIn = "major";
            octavesIn = 7;
            BuildMidiNotes();  //builds an array that identifes a number to a note name (i.e. "c3" = 36)
            SetUpScale();
            SubscribeToScriptEvent("CurrentScale", getScaleInfo);

            //cntr = 0;
            //do
            //{
            //    Log.Write("Index: " + cntr + " SoundResource: " + ScaleSoundResources[cntr].GetName() + " Offset: " + ScaleOffset[cntr]);
            //    cntr++;
            //} while (cntr < ScaleOffset.Count());
            //Log.Write("Im Init After Writing out messages");
        }

        //Listen for Key Event
        SubscribeToScriptEvent("KeySent", getKeyInfo);
        SubscribeToScriptEvent(PanicEvent, ExecutePanic);

        SubscribeToAll(EnableEvent, EnableModule);
        //Log.Write("Action Synth Subscribing to: " + EnableEvent);
        SubscribeToAll(DisableEvent, DisableModule);
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

        //Log.Write("Source: " + Source + " noteIndex: " + noteIndex + " NumberOfNotes: " + NumberOfNotes + " PlaySample: " + PlaySample.GetName() + " PitchShift: " + PitchShiftIn);
        if (PlaySample.IsValid)
        {
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
    }

    #endregion

    #region NotesScalesChords

    private void BuildScaleSounds(int KeyIndex, int ScaleIndex)
    {
        //Log.Write("In BuildScaleSounds KeyIndex: " + KeyIndex + " ScaleIndex: " + ScaleIndex);

        if (ScaleIndex < 12)
        {
            if (S0SoundResources.Count() > 0)
            {
                ScaleSoundResources.Add(S0SoundResources[ScaleIndex]);
                ScaleOffset.Add(S0Offset[ScaleIndex]);
            }
        }
        else if (ScaleIndex < 24)
        {
            ScaleSoundResources.Add(S1SoundResources[ScaleIndex - 12]);
            ScaleOffset.Add(S1Offset[ScaleIndex - 12]);
        }
        else if (ScaleIndex < 36)
        {
            ScaleSoundResources.Add(S2SoundResources[ScaleIndex - 24]);
            ScaleOffset.Add(S2Offset[ScaleIndex - 24]);
        }
        else if (ScaleIndex < 48)
        {
            ScaleSoundResources.Add(S3SoundResources[ScaleIndex - 36]);
            ScaleOffset.Add(S3Offset[ScaleIndex - 36]);
        }
        else if (ScaleIndex < 60)
        {
            ScaleSoundResources.Add(S4SoundResources[ScaleIndex - 48]);
            ScaleOffset.Add(S4Offset[ScaleIndex - 48]);
        }
        else if (ScaleIndex < 72)
        {
            ScaleSoundResources.Add(S5SoundResources[ScaleIndex - 60]);
            ScaleOffset.Add(S5Offset[ScaleIndex - 60]);
        }
        else if (ScaleIndex < 84)
        {
            ScaleSoundResources.Add(S6SoundResources[ScaleIndex - 72]);
            ScaleOffset.Add(S6Offset[ScaleIndex - 72]);
        }
        else if (ScaleIndex < 96)
        {
            Log.Write("S7SoundResources.Count(): " + S7SoundResources.Count());
            //Log.Write("S7SoundResources[0]: " + S7SoundResources[0]);
            if (S7SoundResources.Count() > 0)
            {
                ScaleSoundResources.Add(S7SoundResources[ScaleIndex - 84]);
                ScaleOffset.Add(S7Offset[ScaleIndex - 84]);
            }
        }
        //Log.Write("Done Loading Sound Resources");
    }

    private void BuildMidiNotes()
    {
        MidiNote.Add("c0"); MidiNote.Add("db0"); MidiNote.Add("d0"); MidiNote.Add("eb0"); MidiNote.Add("e0"); MidiNote.Add("f0"); MidiNote.Add("gb0"); MidiNote.Add("g0"); MidiNote.Add("ab0"); MidiNote.Add("a0"); MidiNote.Add("bb0"); MidiNote.Add("b0");
        MidiNote.Add("c1"); MidiNote.Add("db1"); MidiNote.Add("d1"); MidiNote.Add("eb1"); MidiNote.Add("e1"); MidiNote.Add("f1"); MidiNote.Add("gb1"); MidiNote.Add("g1"); MidiNote.Add("ab1"); MidiNote.Add("a1"); MidiNote.Add("bb1"); MidiNote.Add("b1");
        MidiNote.Add("c2"); MidiNote.Add("db2"); MidiNote.Add("d2"); MidiNote.Add("eb2"); MidiNote.Add("e2"); MidiNote.Add("f2"); MidiNote.Add("gb2"); MidiNote.Add("g2"); MidiNote.Add("ab2"); MidiNote.Add("a2"); MidiNote.Add("bb2"); MidiNote.Add("b2");
        MidiNote.Add("c3"); MidiNote.Add("db3"); MidiNote.Add("d3"); MidiNote.Add("eb3"); MidiNote.Add("e3"); MidiNote.Add("f3"); MidiNote.Add("gb3"); MidiNote.Add("g3"); MidiNote.Add("ab3"); MidiNote.Add("a3"); MidiNote.Add("bb3"); MidiNote.Add("b3");
        MidiNote.Add("c4"); MidiNote.Add("db4"); MidiNote.Add("d4"); MidiNote.Add("eb4"); MidiNote.Add("e4"); MidiNote.Add("f4"); MidiNote.Add("gb4"); MidiNote.Add("g4"); MidiNote.Add("ab4"); MidiNote.Add("a4"); MidiNote.Add("bb4"); MidiNote.Add("b4");
        MidiNote.Add("c5"); MidiNote.Add("db5"); MidiNote.Add("d5"); MidiNote.Add("eb5"); MidiNote.Add("e5"); MidiNote.Add("f5"); MidiNote.Add("gb5"); MidiNote.Add("g5"); MidiNote.Add("ab5"); MidiNote.Add("a5"); MidiNote.Add("bb5"); MidiNote.Add("b5");
        MidiNote.Add("c6"); MidiNote.Add("db6"); MidiNote.Add("d6"); MidiNote.Add("eb6"); MidiNote.Add("e6"); MidiNote.Add("f6"); MidiNote.Add("gb6"); MidiNote.Add("g6"); MidiNote.Add("ab6"); MidiNote.Add("a6"); MidiNote.Add("bb6"); MidiNote.Add("b6");
        MidiNote.Add("c7"); MidiNote.Add("db7"); MidiNote.Add("d7"); MidiNote.Add("eb7"); MidiNote.Add("e7"); MidiNote.Add("f7"); MidiNote.Add("gb7"); MidiNote.Add("g7"); MidiNote.Add("ab7"); MidiNote.Add("a7"); MidiNote.Add("bb7"); MidiNote.Add("b7");
    }

    private List<string> BuildScale(string baseNoteIn, string scaleIn)
    {
        int[] TempScaleNotes = null;
        ReturnNotes.Clear();

        int notecntr = 0;
        int basenote = 0;
        //find index of base note in MidiNoteArray
        do
        {
            if (MidiNote[notecntr] == baseNoteIn)
            {
                basenote = notecntr;
                break;
            }
            notecntr++;
        } while (notecntr < MidiNote.Count());
        //ScaleIn[1] = ScaleIn[1].Substring(1, ScaleIn[1].Length - 1);

        switch (scaleIn.Trim())
        {
            case "major":
                TempScaleNotes = major;
                break;
            case "dorian":
                TempScaleNotes = dorian;
                break;
            case "phrygian":
                TempScaleNotes = phrygian;
                break;
            case "lydian":
                TempScaleNotes = lydian;
                break;
            case "mixolydian":
                TempScaleNotes = mixolydian;
                break;
            case "aelian":
                TempScaleNotes = aelian;
                break;
            case "minor":
                TempScaleNotes = minor;
                break;
            case "minor_pentatonic":
                TempScaleNotes = minor_pentatonic;
                break;
            case "major_pentatonic":
                TempScaleNotes = major_pentatonic;
                break;
            case "egyptian":
                TempScaleNotes = egyptian;
                break;
            case "jiao":
                TempScaleNotes = jiao;
                break;
            case "zhi":
                TempScaleNotes = zhi;
                break;
            case "whole_tone":
                TempScaleNotes = whole_tone;
                break;
            case "whole":
                TempScaleNotes = whole;
                break;
            case "chromatic":
                TempScaleNotes = chromatic;
                break;
            case "harmonic_minor":
                TempScaleNotes = harmonic_minor;
                break;
            case "melodic_minor_asc":
                TempScaleNotes = melodic_minor_asc;
                break;
            case "hungarian_minor":
                TempScaleNotes = hungarian_minor;
                break;
            case "octatonic":
                TempScaleNotes = octatonic;
                break;
            case "messiaen1":
                TempScaleNotes = messiaen1;
                break;
            case "messiaen2":
                TempScaleNotes = messiaen2;
                break;
            case "messiaen3":
                TempScaleNotes = messiaen3;
                break;
            case "messiaen4":
                TempScaleNotes = messiaen4;
                break;
            case "messiaen5":
                TempScaleNotes = messiaen5;
                break;
            case "messiaen6":
                TempScaleNotes = messiaen6;
                break;
            case "messiaen7":
                TempScaleNotes = messiaen7;
                break;
            case "super_locrian":
                TempScaleNotes = super_locrian;
                break;
            case "hirajoshi":
                TempScaleNotes = hirajoshi;
                break;
            case "kumoi":
                TempScaleNotes = kumoi;
                break;
            case "neapolitan_major":
                TempScaleNotes = neapolitan_major;
                break;
            case "bartok":
                TempScaleNotes = bartok;
                break;
            case "bhairav":
                TempScaleNotes = bhairav;
                break;
            case "locrian_major":
                TempScaleNotes = locrian_major;
                break;
            case "ahirbhairav":
                TempScaleNotes = ahirbhairav;
                break;
            case "enigmatic":
                TempScaleNotes = enigmatic;
                break;
            case "neapolitan_minor":
                TempScaleNotes = neapolitan_minor;
                break;
            case "pelog":
                TempScaleNotes = pelog;
                break;
            case "augmented2":
                TempScaleNotes = augmented2;
                break;
            case "scriabin":
                TempScaleNotes = scriabin;
                break;
            case "harmonic_major":
                TempScaleNotes = harmonic_major;
                break;
            case "melodic_minor_desc":
                TempScaleNotes = melodic_minor_desc;
                break;
            case "romanian_minor":
                TempScaleNotes = romanian_minor;
                break;
            case "hindu":
                TempScaleNotes = hindu;
                break;
            case "iwato":
                TempScaleNotes = iwato;
                break;
            case "melodic_minor":
                TempScaleNotes = melodic_minor;
                break;
            case "diminished":
                TempScaleNotes = diminished;
                break;
            case "marva":
                TempScaleNotes = marva;
                break;
            case "melodic_major":
                TempScaleNotes = melodic_major;
                break;
            case "indian":
                TempScaleNotes = indian;
                break;
            case "spanish":
                TempScaleNotes = spanish;
                break;
            case "prometheus":
                TempScaleNotes = prometheus;
                break;
            case "diminished2":
                TempScaleNotes = diminished2;
                break;
            case "todi":
                TempScaleNotes = todi;
                break;
            case "leading_whole":
                TempScaleNotes = leading_whole;
                break;
            case "augmented":
                TempScaleNotes = augmented;
                break;
            case "purvi":
                TempScaleNotes = purvi;
                break;
            case "chinese":
                TempScaleNotes = chinese;
                break;
            case "chdmajor":
                TempScaleNotes = chdmajor;
                break;
            case "chdminor":
                TempScaleNotes = chdminor;
                break;
            case "chdmajor7":
                TempScaleNotes = chdmajor7;
                break;
            case "chddom7":
                TempScaleNotes = chddom7;
                break;
            case "chdminor7":
                TempScaleNotes = chdminor7;
                break;
            case "chdaug":
                TempScaleNotes = chdaug;
                break;
            case "chddim":
                TempScaleNotes = chddim;
                break;
            case "chddim7":
                TempScaleNotes = chddim7;
                break;
            case "chdhalfdim":
                TempScaleNotes = chdhalfdim;
                break;
            case "chd1":
                TempScaleNotes = chd1;
                break;
            case "chd5":
                TempScaleNotes = chd5;
                break;
            case "chdmaug5":
                TempScaleNotes = chdmaug5;
                break;
            case "chdsus2":
                TempScaleNotes = chdsus2;
                break;
            case "chdsus4":
                TempScaleNotes = chdsus4;
                break;
            case "chd6":
                TempScaleNotes = chd6;
                break;
            case "chdm6":
                TempScaleNotes = chdm6;
                break;
            case "chd7sus2":
                TempScaleNotes = chd7sus2;
                break;
            case "chd7sus4":
                TempScaleNotes = chd7sus4;
                break;
            case "chd7dim5":
                TempScaleNotes = chd7dim5;
                break;
            case "chd7aug5":
                TempScaleNotes = chd7aug5;
                break;
            case "chdm7aug5":
                TempScaleNotes = chdm7aug5;
                break;
            case "chd9":
                TempScaleNotes = chd9;
                break;
            case "chdm9":
                TempScaleNotes = chdm9;
                break;
            case "chdm7aug9":
                TempScaleNotes = chdm7aug9;
                break;
            case "chdmaj9":
                TempScaleNotes = chdmaj9;
                break;
            case "chd9sus4":
                TempScaleNotes = chd9sus4;
                break;
            case "chd6sus9":
                TempScaleNotes = chd6sus9;
                break;
            case "chdm6sus9":
                TempScaleNotes = chdm6sus9;
                break;
            case "chd7dim9":
                TempScaleNotes = chd7dim9;
                break;
            case "chdm7dim9":
                TempScaleNotes = chdm7dim9;
                break;
            case "chd7dim10":
                TempScaleNotes = chd7dim10;
                break;
            case "chd7dim11":
                TempScaleNotes = chd7dim11;
                break;
            case "chd7dim13":
                TempScaleNotes = chd7dim13;
                break;
            case "chd9dim5":
                TempScaleNotes = chd9dim5;
                break;
            case "chdm9dim5":
                TempScaleNotes = chdm9dim5;
                break;
            case "chd7aug5dim9":
                TempScaleNotes = chd7aug5dim9;
                break;
            case "chdm7aug5dim9":
                TempScaleNotes = chdm7aug5dim9;
                break;
            case "chd11":
                TempScaleNotes = chd11;
                break;
            case "chdm11":
                TempScaleNotes = chdm11;
                break;
            case "chdmaj11":
                TempScaleNotes = chdmaj11;
                break;
            case "chd11aug":
                TempScaleNotes = chd11aug;
                break;
            case "chdm11aug":
                TempScaleNotes = chdm11aug;
                break;
            case "chd13":
                TempScaleNotes = chd13;
                break;
            case "chdm13":
                TempScaleNotes = chdm13;
                break;
            case "chdadd2":
                TempScaleNotes = chdadd2;
                break;
            case "chdadd4":
                TempScaleNotes = chdadd4;
                break;
            case "chdadd9":
                TempScaleNotes = chdadd9;
                break;
            case "chdadd11":
                TempScaleNotes = chdadd11;
                break;
            case "add13":
                TempScaleNotes = add13;
                break;
            case "madd2":
                TempScaleNotes = madd2;
                break;
            case "madd4":
                TempScaleNotes = madd4;
                break;
            case "madd9":
                TempScaleNotes = madd9;
                break;
            case "madd11":
                TempScaleNotes = madd11;
                break;
            case "madd13":
                TempScaleNotes = madd13;
                break;
            default:
                Errormsg = "Scale or Chord Not Found";
                break;
        }

        ReturnNotes.Add(baseNoteIn); //first note is the base note
        //Log.Write("scaleIn: " + scaleIn);
        //Log.Write("basenoteIn: " + baseNoteIn);
        //Log.Write("basenote: " + basenote);
        //Log.Write("TempScaleNotes.Count: " + TempScaleNotes.Count());
        if (!(Errormsg == "Scale or Chord Not Found"))
        {
            //Get the Rest of the notes of the scale
            notecntr = 0;
            do
            {
                //Log.Write("TempScaleNotes[" + notecntr +"]: " + TempScaleNotes[notecntr]);
                ReturnNotes.Add(MidiNote[basenote + TempScaleNotes[notecntr]]);
                basenote = basenote + TempScaleNotes[notecntr];
                notecntr++;
            } while (notecntr < TempScaleNotes.Count() - 1);
        }

        //Log.Write("TempScaleNotes: " + TempScaleNotes);

        return ReturnNotes;
    }

    private int FindMidiNote(string MidiNoteIn)
    {
        int x = 0;
        do
        {
            if (MidiNote[x] == MidiNoteIn) break;
            x++;
        } while (x < MidiNote.Count());
        return x;
    }

    private List<int> BuildNotes()
    {
        //int octaves = 0;
        int notecntr = 0;
        //List<string> strTempNotes = new List<string>();
        //List<int> intTempNotes = new List<int>();

        strTempNotes.Clear();
        intTempNotes.Clear();

        strTempNotes = BuildScale(startNote, scaleIn);
        //Log.Write("tempNotesCount: " + strTempNotes.Count());
        if (strTempNotes.Count() > 0)
        {
            notecntr = 0;
            do
            {
                intTempNotes.Add(FindMidiNote(strTempNotes[notecntr]));
                notecntr++;
            } while (notecntr < strTempNotes.Count());
        }

        if (octavesIn > 0)
        {
            int octcntr = 0;
            int arraylength = intTempNotes.Count();
            do
            {
                notecntr = 0;
                do
                {
                    intTempNotes.Add(intTempNotes[notecntr] + (12 * (octcntr + 1)));
                    notecntr++;
                } while (notecntr < arraylength);
                octcntr++;
            } while (octcntr < octavesIn - 1);
        }

        return intTempNotes;
    }

    #endregion
}
