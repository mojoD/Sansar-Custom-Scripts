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

public class StandAloneInstrumentV1 : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("start")]
    [DisplayName("Start Event")]
    public string StartEvent;

    [DefaultValue("stop")]
    [DisplayName("Stop Event")]
    public string StopEvent;

    [DefaultValue("c2")]
    [DisplayName("Start Note")]
    public string startNote;

    [DefaultValue("chromatic")]
    [DisplayName("Scale")]
    public string scaleIn;

    [DefaultValue(3)]
    [DisplayName("Octaves")]
    public int octavesIn;

    [DefaultValue(50.0f)]
    [DisplayName("Loudness")]
    public float loudnessIn;

    [DefaultValue(true)]
    [DisplayName("Sustain Note")]
    public bool SustainNote;

    [DefaultValue("")]
    [DisplayName("Instrument")]
    public string StandAloneInstruments;

    [DefaultValue("")]
    [DisplayName("Event Base Name")]
    public string BaseEventNames;

    [DefaultValue(false)]
    [DisplayName("Enable External Events")]
    public bool ExternalControl;

    [DefaultValue(1)]
    [DisplayName("Number of External Notes")]
    public int NumberOfNotes;

    [DefaultValue("SendNote")]
    [DisplayName("Send Base Event")]
    public string SendNote;

    [DefaultValue("SendX")]
    [DisplayName("Base Change Inst Event")]
    public string SendChangeInst;

    [DefaultValue(true)]
    [DisplayName("Change Scale")]
    public bool ChangeScale;

    [DefaultValue(0)]
    [DisplayName("Octave Adjust")]
    public int OctaveAdjust;


    public ISimpleData simpledata;
    private string StandAloneInstrument;
    private List<string> StandAloneInstrumentList = new List<string>();
    private string EventSource = null;
    private AudioComponent audio;
    private Action unsubscribes = null;
    List<IEventSubscription> ButtonSubscriptions = new List<IEventSubscription>();
    private bool SubscribedToKeys = false;
    private bool InstrumentLoaded = false;
    private List<string> InstrumentList = new List<string>();
    private bool Shifted = false;
    private AgentPrivate Hitman;
    private ObjectId HitObject;
    private CollisionData collsionData;
    private string BaseEventName = null;
    string[] BaseEventArray;
    private PlaySettings playSettings;
    //private PlayHandle playHandleSimple;
    private PlayHandle[] playHandleSimple = new PlayHandle[127];
    private List<SoundResource> TrackSamples = new List<SoundResource>();
    private List<int> TrackOffsets = new List<int>();
    private List<int> TrackNotes = new List<int>();
    private string Errormsg = "No Errors";
    private bool strErrors = false;
    private int instrumentcntr = 0;
    private List<string> InstrumentNameArray = new List<string>();
    //public List<string> InstrumentArray = new List<string>(128);
    private List<string>[] InstrumentArray = new List<string>[128];
    private SessionId Jammer = new SessionId();
    AgentPrivate TheUser = null;

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

    public List<SoundResource> SampleLibrary = new List<SoundResource>();
    public List<string> SampleNames = new List<string>();

    public interface SendSamples
    {
        List<object> SendSampleLibrary { get; }
    }

    private void getSamples(ScriptEventData gotSamples)
    {
        //Log.Write("In getSamples");
        if (gotSamples.Data == null)
        {
            Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null event data");
            return;
        }
        SendSamples sendSamples = gotSamples.Data.AsInterface<SendSamples>();
        if (sendSamples == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        SoundResource tempSample;
        string tempSampleName;
        int cntr = 0;
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        if (sendSamples.SendSampleLibrary.Count() > 0)
        {
            do
            {
                //Log.Write("cntr: " + cntr);
                if ((cntr == 1) || (cntr == 3) || (cntr == 5) || (cntr == 7) || (cntr == 9) || (cntr == 11) || (cntr == 13) || (cntr == 15) || (cntr == 17) || (cntr == 19))
                {
                    tempSample = sendSamples.SendSampleLibrary.ElementAt(cntr) as SoundResource;
                    SampleLibrary.Add(tempSample);
                    //Log.Write("sample added: " + tempSample.GetName());
                }
                else
                {
                    tempSampleName = sendSamples.SendSampleLibrary.ElementAt(cntr) as string;
                    SampleNames.Add(tempSampleName);
                    //Log.Write("Sample Name Added: " + tempSampleName);
                }
                //Errors = Errors + ", " + tempSample.GetName();
                //Log.Write("Sample Loaded: " + tempSample.GetName());
                cntr++;
            } while (cntr < sendSamples.SendSampleLibrary.Count());
        }
    }

    public interface SendInstrument
    {
        List<string> SendInstrumentArray { get; }
    }

    private void getInstrument(ScriptEventData gotInstrument)
    {
        if (gotInstrument.Data == null)
        {
            //Log.Write(LogLevel.Warning, Script.ID.ToString(), "Expected non-null event data");
            return;
        }
        SendInstrument sendInstrument = gotInstrument.Data.AsInterface<SendInstrument>();
        if (sendInstrument == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        //Log.Write("######## In getInstrument ######");
        int InstrumentIndex = 0;
        if (sendInstrument.SendInstrumentArray.Count() > 0)
        {
            //InstrumentArray = new List<string>();
            InstrumentIndex = InstrumentNameArray.Count();
            InstrumentArray[InstrumentIndex] = new List<string>();
            int notecntr = 0;
            do
            {
                if (notecntr > 0)
                {
                    InstrumentArray[InstrumentIndex].Add(sendInstrument.SendInstrumentArray[notecntr]);
                }
                else
                {
                    Log.Write("InstrumentIndex: " + InstrumentIndex + "  InstrumentName: " + sendInstrument.SendInstrumentArray[0]);
                    InstrumentNameArray.Add(sendInstrument.SendInstrumentArray[0]);  //first entry of SendInstrumentArray is the name of the instrument
                }
                notecntr++;
            } while (notecntr < sendInstrument.SendInstrumentArray.Count());
            instrumentcntr++;
        }
    }

    private void GetChatCommand(ChatData Data)
    {
        string DataCmd = Data.Message;
        string StartNoteToChange = startNote;
        string ScaleToChange = scaleIn;

        //Log.Write("DataCmd: " + DataCmd);

        if (DataCmd.Contains("/inst("))
        {
            //fix up cmd
            int from = DataCmd.IndexOf("inst(", StringComparison.CurrentCulture);
            string test = DataCmd.Substring(from, DataCmd.Length - from);
            int to = test.IndexOf(")", StringComparison.CurrentCulture);
            string InstrumentToChange = test.Substring(5, to - 5);
            Log.Write("InstrumentToChange: " + InstrumentToChange);
            int newInstrument = FindInstrumentInArray(InstrumentToChange);
            if (newInstrument < 99)
            {
                BuildInstrument(InstrumentToChange);
                StandAloneInstrument = InstrumentToChange;
            }
            else
            {
                Log.Write("New Instrument not found");
            }
        }
        if (ChangeScale)
        {
            if (DataCmd.Contains("/scale("))
            {
                //fix up cmd
                int from = DataCmd.IndexOf("(", StringComparison.CurrentCulture);
                //Log.Write("from: " + from);
                int to = DataCmd.IndexOf(",", StringComparison.CurrentCulture);
                //Log.Write("to: " + to);
                string test = DataCmd.Substring(from + 1, to - from - 1);
                //Log.Write("test: " + test);
                StartNoteToChange = test;

                Log.Write("OctaveAdjust: " + OctaveAdjust);
                if (OctaveAdjust == 0)
                {
                    Log.Write("no Ocatve Adjust");
                }
                else
                {
                    Log.Write("startNoteOctave: " + StartNoteToChange.Substring(1, 1));
                    int StartNoteOctave = Int32.Parse(StartNoteToChange.Substring(1, 1));
                    StartNoteOctave = StartNoteOctave + OctaveAdjust;
                    Log.Write("Octave Adjust: " + OctaveAdjust);
                    Log.Write("new StartNoteOctave: " + StartNoteOctave);
                    StartNoteToChange = StartNoteToChange.Substring(0, 1) + StartNoteOctave;
                }

                Log.Write("New Start Note: " + StartNoteToChange);
                ScaleToChange = DataCmd.Substring(to + 2, DataCmd.Length - to - 3);
                Log.Write("New Scale: " + ScaleToChange);
                scaleIn = ScaleToChange;
                startNote = StartNoteToChange;
                TrackNotes = BuildNotes();  //builds an array of integers that are the valid note numbers for the given scale and octaves
                BuildInstrument(StandAloneInstrument);  //based on the integers that are the valide notes in the scale build an array of sound resources and offsets
            }
        }
    }

    #endregion

    public override void Init()
    {
        //Log.Write("******* ActionInstrument ***************");
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal

        bool testAudio = ObjectPrivate.TryGetFirstComponent(out audio);
        //Log.Write("testAudio: " + testAudio);
        if (!testAudio)
        {
            ScenePrivate.Chat.MessageAllUsers("Musical Instrument requires an Audio Emitter");
            Log.Write("Did not find Audio Component");
        }
        else
        {
            //Log.Write("Found Audio Component");

            ScenePrivate.Chat.Subscribe(0, GetChatCommand);  //anyone can change start key and scale

            BuildMidiNotes();  //builds an array that identifes a number to a note name (i.e. "c3" = 36)
            TrackNotes = BuildNotes();  //builds an array of integers that are the valid note numbers for the given scale and octaves

            StandAloneInstrumentList = StandAloneInstruments.Trim().Split(',').ToList();
            //Log.Write("StandAloneInstrumentList.Count: " + StandAloneInstrumentList.Count());
            int InstrumentCntr = 0;
            string FirstInstrument = null;
            foreach (string StandAloneInstrumentIn in StandAloneInstrumentList)
            {
                //Log.Write("StandAloneInstrumentIn: " + StandAloneInstrumentIn);
                FindInstrument(StandAloneInstrumentIn);
                if (InstrumentCntr == 0)
                {
                    FirstInstrument = StandAloneInstrumentIn;
                }
                Log.Write("Subscribing to Instrument Change: " + SendChangeInst + StandAloneInstrumentIn);
                SubscribeToAll(SendChangeInst + StandAloneInstrumentIn, ChangeInstrument);
                InstrumentCntr++;
            }

            Wait(TimeSpan.FromSeconds(8));
            StandAloneInstrument = FirstInstrument;
            BuildInstrument(FirstInstrument);  //based on the integers that are the valid notes in the scale build an array of sound resources and offsets

            //Subscribe to keynote being pressed or message being sent
            BaseEventArray = BaseEventNames.Trim().Split(',');

            int BaseEventCntr = 0;
            //Log.Write("BaseEventArray.Count: " + BaseEventArray.Count());
            do
            {
                int cntr = 1;
                BaseEventName = BaseEventArray[BaseEventCntr];
                //Log.Write("BaseEventName: " + BaseEventName);
                if (ExternalControl)
                {
                    do
                    {
                        //Log.Write("Subscribing to: " + BaseEventName + cntr);
                        SubscribeToAll(BaseEventName + cntr, PlayInstrument);
                        SubscribeToAll(BaseEventName + cntr + "Off", PlayInstrument);
                        cntr++;
                    } while (cntr < NumberOfNotes + 1);
                }
            BaseEventCntr++;
            } while (BaseEventCntr < BaseEventArray.Count()) ;

            if (SubscribedToKeys == false)
            {
                SubscribeToAll(StartEvent, SubscribeToKeys);
                SubscribeToAll(StopEvent, UnsubscribeToKeys);
            }
        }
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region PCKeys
    private void SubscribeToKeys(ScriptEventData subdata)
    {
        simpledata = subdata.Data?.AsInterface<ISimpleData>();
        AgentPrivate agent = ScenePrivate.FindAgent(simpledata.AgentInfo.SessionId);
        Hitman = agent;
        HitObject = simpledata.AgentInfo.ObjectId;

        SimpleData sd = new SimpleData();
        sd.ObjectId = simpledata.ObjectId;
        sd.AgentInfo = simpledata.AgentInfo;
        if ("stopPan" != StopEvent) SendToAll("stopPan", sd);
        if ("stopPiano" != StopEvent) SendToAll("stopPiano", sd);
        if ("stopOrgan" != StopEvent) SendToAll("stopOrgan", sd);
        if ("stopMarimba" != StopEvent) SendToAll("stopMarimba", sd);
        if ("stopMoog" != StopEvent) SendToAll("stopMoog", sd);
        if ("stopDX" != StopEvent) SendToAll("stopDX", sd);
        if ("stopConga" != StopEvent) SendToAll("stopConga", sd);

        SubscribeKeyPressed(agent, "sub");
        if (InstrumentLoaded)
        {
            Hitman.SendChat("Instrument " + StandAloneInstrument + " loaded");
        }
        else
        {
            Hitman.SendChat("Instrument " + StandAloneInstrument + " not found");
        }
        SubscribedToKeys = true;
    }

    private void UnsubscribeToKeys(ScriptEventData subdata)
    {
        //Log.Write("stopEvent: " + StopEvent);
        //Log.Write("message: " + subdata.Message);
        if (subdata.Message == StopEvent)
        {
            simpledata = subdata.Data?.AsInterface<ISimpleData>();
            AgentPrivate agent = ScenePrivate.FindAgent(simpledata.AgentInfo.SessionId);
            SubscribeKeyPressed(agent, "unsub");
            SubscribedToKeys = true;
        }
    }

    void SubscribeClientToButton(Client client, string Button)
    {
        ButtonSubscriptions.Add(client.SubscribeToCommand(Button, CommandAction.Pressed, CommandReceived, CommandCanceled));
        if (SustainNote)
        {
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

    void CommandReceived(CommandData Button)
    {
        if ((Button.Command == "Modifier") && (Button.Action == CommandAction.Pressed))  //shift acts a toggle
        {
            if (Shifted == true) Shifted = false;
            else Shifted = true;
        }

        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "1");
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "2");
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "3");
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "4");
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "5");
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "6");
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "7");
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "8");
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "9");
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "10");
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "11");
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "12");
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "13");
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "14");
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "15");
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "16");
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "17");
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "18");
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "19");
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (!(Shifted))) SendKey(BaseEventName + "20");
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "21");
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "22");
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "23");
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "24");
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "25");
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "26");
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "27");
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "28");
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "29");
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "30");
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "31");
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "32");
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "33");
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "34");
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "35");
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "36");
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "37");
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "38");
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "39");
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Pressed) && (Shifted)) SendKey(BaseEventName + "40");

       // Log.Write("Relase Key");
        if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[0].Stop();
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[1].Stop();
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[2].Stop();
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[3].Stop();
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[4].Stop();
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[5].Stop();
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[6].Stop();
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[7].Stop();
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[8].Stop();
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[9].Stop();
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[10].Stop();
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[11].Stop();
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[12].Stop();
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[13].Stop();
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[14].Stop();
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[15].Stop();
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[16].Stop();
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[17].Stop();
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[18].Stop();
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (!(Shifted))) playHandleSimple[19].Stop();
        else if ((Button.Command == "Action1") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[20].Stop();
        else if ((Button.Command == "Action2") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[21].Stop();
        else if ((Button.Command == "Action3") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[22].Stop();
        else if ((Button.Command == "Action4") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[23].Stop();
        else if ((Button.Command == "Action5") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[24].Stop();
        else if ((Button.Command == "Action6") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[25].Stop();
        else if ((Button.Command == "Action7") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[26].Stop();
        else if ((Button.Command == "Action8") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[27].Stop();
        else if ((Button.Command == "Action9") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[28].Stop();
        else if ((Button.Command == "Action0") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[29].Stop();
        else if ((Button.Command == "Keypad0") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[30].Stop();
        else if ((Button.Command == "Keypad1") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[31].Stop();
        else if ((Button.Command == "Keypad2") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[32].Stop();
        else if ((Button.Command == "Keypad3") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[33].Stop();
        else if ((Button.Command == "Keypad4") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[34].Stop();
        else if ((Button.Command == "Keypad5") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[35].Stop();
        else if ((Button.Command == "Keypad6") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[36].Stop();
        else if ((Button.Command == "Keypad7") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[37].Stop();
        else if ((Button.Command == "Keypad8") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[38].Stop();
        else if ((Button.Command == "Keypad9") && (Button.Action == CommandAction.Released) && (Shifted)) playHandleSimple[39].Stop();
    }

    void CommandCanceled(CancelData data)
    {
        Log.Write(GetType().Name, "Subscription canceled: " + data.Message);
    }

    void SendKey(string KeyIn)
    {
        string KeyNumber = KeyIn.Replace(BaseEventName, "");
        //Log.Write("KeyNumber: " + KeyNumber);
        int NoteEvent = Int32.Parse(KeyNumber) - 1;
        int intOffset;
        SoundResource UseSample;
        //Log.Write("NoteEvent: " + NoteEvent);
        //Log.Write("TrackSamples.Count(): " + TrackSamples.Count());
        EventSource = "PCKeys";
        if ((NoteEvent*2) < TrackSamples.Count())
        {
            //UseSample = TrackSamples[NoteEvent * 2];  // add string of sample to be used in temp array  
            //intOffset = TrackOffsets[NoteEvent * 2 + 1];
            UseSample = TrackSamples[NoteEvent];  // add string of sample to be used in temp array  
            intOffset = TrackOffsets[NoteEvent];
            PlayNote(EventSource, UseSample, intOffset, NoteEvent + 1);
        }
        else
        {
            Log.Write("note out of range");
        }
    }

    #endregion

        #region Musical Instrument
    private void BuildMidiNotes()
    {
        MidiNote.Add("c-"); MidiNote.Add("db-"); MidiNote.Add("d-"); MidiNote.Add("eb-"); MidiNote.Add("e-"); MidiNote.Add("f-"); MidiNote.Add("gb-"); MidiNote.Add("g-"); MidiNote.Add("ab-"); MidiNote.Add("a-"); MidiNote.Add("bb-"); MidiNote.Add("b-");
        MidiNote.Add("c0"); MidiNote.Add("db0"); MidiNote.Add("d0"); MidiNote.Add("eb0"); MidiNote.Add("e0"); MidiNote.Add("f0"); MidiNote.Add("gb0"); MidiNote.Add("g0"); MidiNote.Add("ab0"); MidiNote.Add("a0"); MidiNote.Add("bb0"); MidiNote.Add("b0");
        MidiNote.Add("c1"); MidiNote.Add("db1"); MidiNote.Add("d1"); MidiNote.Add("eb1"); MidiNote.Add("e1"); MidiNote.Add("f1"); MidiNote.Add("gb1"); MidiNote.Add("g1"); MidiNote.Add("ab1"); MidiNote.Add("a1"); MidiNote.Add("bb1"); MidiNote.Add("b1");
        MidiNote.Add("c2"); MidiNote.Add("db2"); MidiNote.Add("d2"); MidiNote.Add("eb2"); MidiNote.Add("e2"); MidiNote.Add("f2"); MidiNote.Add("gb2"); MidiNote.Add("g2"); MidiNote.Add("ab2"); MidiNote.Add("a2"); MidiNote.Add("bb2"); MidiNote.Add("b2");
        MidiNote.Add("c3"); MidiNote.Add("db3"); MidiNote.Add("d3"); MidiNote.Add("eb3"); MidiNote.Add("e3"); MidiNote.Add("f3"); MidiNote.Add("gb3"); MidiNote.Add("g3"); MidiNote.Add("ab3"); MidiNote.Add("a3"); MidiNote.Add("bb3"); MidiNote.Add("b3");
        MidiNote.Add("c4"); MidiNote.Add("db4"); MidiNote.Add("d4"); MidiNote.Add("eb4"); MidiNote.Add("e4"); MidiNote.Add("f4"); MidiNote.Add("gb4"); MidiNote.Add("g4"); MidiNote.Add("ab4"); MidiNote.Add("a4"); MidiNote.Add("bb4"); MidiNote.Add("b4");
        MidiNote.Add("c5"); MidiNote.Add("db5"); MidiNote.Add("d5"); MidiNote.Add("eb5"); MidiNote.Add("e5"); MidiNote.Add("f5"); MidiNote.Add("gb5"); MidiNote.Add("g5"); MidiNote.Add("ab5"); MidiNote.Add("a5"); MidiNote.Add("bb5"); MidiNote.Add("b5");
        MidiNote.Add("c6"); MidiNote.Add("db6"); MidiNote.Add("d6"); MidiNote.Add("eb6"); MidiNote.Add("e6"); MidiNote.Add("f6"); MidiNote.Add("gb6"); MidiNote.Add("g6"); MidiNote.Add("ab6"); MidiNote.Add("a6"); MidiNote.Add("bb6"); MidiNote.Add("b6");
        MidiNote.Add("c7"); MidiNote.Add("db7"); MidiNote.Add("d7"); MidiNote.Add("eb7"); MidiNote.Add("e7"); MidiNote.Add("f7"); MidiNote.Add("gb7"); MidiNote.Add("g7"); MidiNote.Add("ab7"); MidiNote.Add("a7"); MidiNote.Add("bb7"); MidiNote.Add("b7");
        MidiNote.Add("c8"); MidiNote.Add("db8"); MidiNote.Add("d8"); MidiNote.Add("eb8"); MidiNote.Add("e8"); MidiNote.Add("f8"); MidiNote.Add("gb8"); MidiNote.Add("g8"); MidiNote.Add("ab8"); MidiNote.Add("a8"); MidiNote.Add("bb8"); MidiNote.Add("b8");
        MidiNote.Add("c9"); MidiNote.Add("db8"); MidiNote.Add("d9"); MidiNote.Add("eb9"); MidiNote.Add("e9"); MidiNote.Add("f9"); MidiNote.Add("gb9"); MidiNote.Add("g9");
    }

    private void FindInstrument(string InstrumentName)
    {
        //Log.Write("Find Instrument - InstrumentName: " + InstrumentName);

        int InstrumentNumber = 0;

        switch (InstrumentName)
        {
            case "piano":
                InstrumentNumber = 1;
                break;
            case "saw":
                InstrumentNumber = 9;
                break;
            case "square":
                InstrumentNumber = 10;
                break;
            case "vibes":
                InstrumentNumber = 11;
                break;
            case "bass":
                InstrumentNumber = 12;
                break;
            case "guitar":
                InstrumentNumber = 13;
                break;
            case "b3":
                InstrumentNumber = 14;
                break;
            case "elecpiano":
                InstrumentNumber = 15;
                break;
            case "strings":
                InstrumentNumber = 16;
                break;
            case "pizzicato":
                InstrumentNumber = 17;
                break;
            case "synthstrings":
                InstrumentNumber = 18;
                break;
            case "nylon":
                InstrumentNumber = 19;
                break;
            case "distortion":
                InstrumentNumber = 20;
                break;
            case "overdrive":
                InstrumentNumber = 21;
                break;
            case "churchorgan":
                InstrumentNumber = 22;
                break;
            case "jazzorgan":
                InstrumentNumber = 23;
                break;
            case "drumkit":
                InstrumentNumber = 27;
                break;
            case "steeldrum":
                InstrumentNumber = 32;
                break;
            case "elecguitar":
                InstrumentNumber = 33;
                break;
            case "pickbass":
                InstrumentNumber = 34;
                break;
            case "fingerbass":
                InstrumentNumber = 35;
                break;
            case "elecdrum":
                InstrumentNumber = 37;
                break;
            case "DX1":
                InstrumentNumber = 38;
                break;
            case "DX2":
                InstrumentNumber = 39;
                break;
            case "DX3":
                InstrumentNumber = 40;
                break;
            case "FX1":
                InstrumentNumber = 41;
                break;
            case "FX2":
                InstrumentNumber = 42;
                break;
            case "FX3":
                InstrumentNumber = 43;
                break;
            case "FX4":
                InstrumentNumber = 44;
                break;
            case "conga":
                InstrumentNumber = 45;
                break;
            default:
                Errormsg = "Instrument Not Found";
                Log.Write(Errormsg);
                break;
        }

        //Log.Write("Instrument Number: " + InstrumentNumber);
        if (InstrumentNumber > 0)
        {
            int samplepackcntr = InstrumentNumber;
            string eventString = null;
            eventString = "Samples" + samplepackcntr;
            SubscribeToScriptEvent(eventString, getSamples);
            eventString = "Instrument" + samplepackcntr;
            SubscribeToScriptEvent(eventString, getInstrument);
            InstrumentLoaded = true;
            //Log.Write("Subscribed to: " + InstrumentName);
        }
        else
        {
            InstrumentLoaded = false;
        }

        
    }

    private void BuildInstrument(string InstrumentToUse)
    {
        Log.Write("InstrumentToUse: " + InstrumentToUse);
        int InstrumentNumber = FindInstrumentInArray(InstrumentToUse);
        Log.Write("InstrumentNumber: " + InstrumentNumber);
        int notecntr = 0;
        string strOffset = "";
        int intOffset = 0;
        List<string> TempSamplesIn = new List<string>();
        List<int> TempOffsetsIn = new List<int>();
        TrackSamples.Clear();
        TrackOffsets.Clear();
        Log.Write("TrackNotes.Count: " + TrackNotes.Count());
        Log.Write("InstrumentArray.Count: " + InstrumentArray.Count());
        do
        {
            TrackSamples.Add(FindSample(InstrumentArray[InstrumentNumber][TrackNotes[notecntr] * 2]));

            strOffset = InstrumentArray[InstrumentNumber][TrackNotes[notecntr] * 2 + 1];
            intOffset = Int32.Parse(strOffset);
            TempOffsetsIn.Add(intOffset);  // add int of offset to be used in temp array
            notecntr++;
        } while (notecntr < TrackNotes.Count());
        /*
        Log.Write("TrackSamples.Count: " + TrackSamples.Count());
        int scntr = 0;
        do
        {
            Log.Write(scntr + ": " + TrackSamples[scntr].GetName());
            scntr++;
        } while (scntr < TrackSamples.Count());
        */

        if (TrackSamples.Count() == 0)
        {
            strErrors = true;
            ScenePrivate.Chat.MessageAllUsers("Sample Not Found");
        }
        TrackOffsets = TempOffsetsIn;
    }

    private int FindInstrumentInArray(string InstrumentNameIn)
    {
        int InstrumentFoundInArray = 99;
        int cntr = 0;
        //Log.Write("InstrumentNameArray.Count: " + InstrumentNameArray.Count());
        do
        {
            //Log.Write("InstrumentNameArray[" + cntr + "]: " + InstrumentNameArray[cntr]);
            //Log.Write("InstrumentNameIn: " + InstrumentNameIn);
            if (InstrumentNameArray[cntr] == InstrumentNameIn)
            {
                //og.Write("Instrument Matched: " + cntr);
                InstrumentFoundInArray = cntr;
            }
            cntr++;
        } while (cntr < InstrumentNameArray.Count());

        return InstrumentFoundInArray;
    }

    private SoundResource FindSample(string SampleToFind)
    {
        SoundResource SampleFound = null;
        string strtest;
        bool SampleTest = false;
        int namecntr = 0;
        do
        {
            //Log.Write("namecntr: " + namecntr);
            strtest = SampleNames[namecntr];
            //Log.Write("strtest: " + strtest);
            if (SampleToFind.Contains(strtest))
            {
                SampleTest = true;
                //Log.Write("match");
                //Log.Write("Name of Sample Chosen: " + SampleLibrary[namecntr].GetName());
                SampleFound = SampleLibrary[namecntr];
            }
            namecntr++;
        } while (namecntr < SampleNames.Count);
        if (SampleTest)
        {
            return SampleFound;
        }
        else
        {
            ScenePrivate.Chat.MessageAllUsers("Sample - " + SampleToFind + " - not found");
            return null;
        }
    }

    private void PlayInstrument(ScriptEventData data)
    {
        //Log.Write("data.Message: " + data.Message);

        string test = data.Message;  //Surface1
        int BaseEventCntr = 0;
        do
        {
            BaseEventName = BaseEventArray[BaseEventCntr].Trim(); //Surface
            //Log.Write("BaseEventName: " + BaseEventName);
            //Log.Write("BaseEventName.length: " + BaseEventName.Length);
            //Log.Write("test.length: " + test.Length);
            int BaseNameEventLength = BaseEventName.Length; //7
            string strNoteEvent = null;
            EventSource = "External";
            if (test.Length > BaseNameEventLength)
            {

                strNoteEvent = test.Substring(BaseNameEventLength, test.Length - BaseNameEventLength);

                //Log.Write("strNoteEvent: " + strNoteEvent);
                bool onlyNumbers = true;
                foreach (char c in strNoteEvent)
                {
                    if (c < '0' || c > '9')
                    {
                        onlyNumbers = false;
                    }
                }
                if (onlyNumbers)
                {
                    int NoteEvent = Int32.Parse(strNoteEvent);
                    int intOffset;
                    SoundResource UseSample;
                    UseSample = TrackSamples[NoteEvent * 2];  // add string of sample to be used in temp array  
                    intOffset = TrackOffsets[NoteEvent * 2 + 1];
                    PlayNote(EventSource, UseSample, intOffset, NoteEvent);
                }
            }
            BaseEventCntr++;
        } while (BaseEventCntr < BaseEventArray.Count());

    }

    private void PlayNote(string EventSource, SoundResource PlaySample, float PitchShiftIn, int NoteIn)
    {

        if (EventSource == "External")
        {
            playSettings = PlaySettings.PlayOnce;
        }
        else
        {
            playSettings = SustainNote ? PlaySettings.Looped : PlaySettings.PlayOnce;
        }

        playSettings.Loudness = loudnessIn; // set in Configuration
        playSettings.DontSync = true; // TrackDont_Sync[LoopIn2];
        playSettings.PitchShift = PitchShiftIn;
        //playHandle[LoopIn2] = ScenePrivate.PlaySound(TrackSamples[LoopIn2][PlayIndexIn], playSettings);
        //Log.Write("NoteIn: " + NoteIn);
        Log.Write("NoteIn: " + NoteIn + "  PlaySample: " + PlaySample.GetName() + "  offset: " + PitchShiftIn);
        playHandleSimple[NoteIn-1] = audio.PlaySoundOnComponent(PlaySample, playSettings);
        if (ExternalControl)
        {
            sendSimpleMessage(SendNote + NoteIn);
            //Log.Write("Sending Event: " + SendNote + NoteIn);
        }
    }

    private void sendSimpleMessage(string msg)
    {
        SimpleData sd = new SimpleData();
        sd.AgentInfo = Hitman.AgentInfo; // ScenePrivate.FindAgent(Hitman?.AgentInfo;
        sd.ObjectId = HitObject;
        SendToAll(msg, sd);
    }

    private void ChangeInstrument(ScriptEventData data)
    {
        Log.Write("data.Message - Instrument to Change: " + data.Message);
        Log.Write("SendChangeInst.Length: " + SendChangeInst.Length);
        int from = SendChangeInst.Length;
        string test = data.Message.Substring(from, data.Message.Length - from);
        Log.Write("test: " + test);
        int to = test.IndexOf(")", StringComparison.CurrentCulture);
        string InstrumentToChange = test; // test.Substring(5, to - 5);
        Log.Write("InstrumentToChange: " + InstrumentToChange);
        int newInstrument = FindInstrumentInArray(InstrumentToChange);
        if (newInstrument < 99)
        {
            BuildInstrument(InstrumentToChange);
            StandAloneInstrument = InstrumentToChange;
        }
        else
        {
            Log.Write("New Instrument not found");
        }
    }

    #endregion

    #region NotesScalesChords

    private List<string> BuildScale(string baseNoteIn, string scaleIn)
    {
        int[] TempScaleNotes = null;
        List<string> ReturnNotes = new List<string>();
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
        Log.Write("basenoteIn: " + baseNoteIn);
        Log.Write("basenote: " + basenote);
        Log.Write("TempScaleNotes.Count: " + TempScaleNotes.Count());
        if (!(Errormsg == "Scale or Chord Not Found"))
        {
            //Get the Rest of the notes of the scale
            notecntr = 0;
            do
            {
                Log.Write("TempScaleNotes[notecntr]: " + TempScaleNotes[notecntr]);
                ReturnNotes.Add(MidiNote[basenote + TempScaleNotes[notecntr]]);
                basenote = basenote + TempScaleNotes[notecntr];
                notecntr++;
            } while (notecntr < TempScaleNotes.Count() - 1);
        }

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
        List<string> strTempNotes = new List<string>();
        List<int> intTempNotes = new List<int>();

        strTempNotes = BuildScale(startNote, scaleIn);
        Log.Write("tempNotesCount: " + strTempNotes.Count());
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
