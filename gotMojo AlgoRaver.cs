//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;


public class Beats : SceneObjectScript
{

	#region ConstantsVariables
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

	//scales
	private static int[] major = {2, 2, 1, 2, 2, 2, 1};
	private static int[] dorian = {1, 2, 2, 1, 2, 2, 2};
	private static int[] phrygian = {2, 1, 2, 2, 1, 2, 2};
	private static int[] lydian = {2, 2, 1, 2, 2, 1, 2};
	private static int[] mixolydian = {2, 2, 2, 1, 2, 2, 1};
	private static int[] aelian = {1, 2, 2, 2, 1, 2, 2};
	private static int[] minor = {2, 1, 2, 2, 2, 1, 2};
	private static int[] minor_pentatonic = { 3, 2, 2, 3, 2 };
	private static int[] major_pentatonic = { 2, 3, 2, 2, 3 };
	private static int[] egyptian = { 3, 2, 3, 2, 2 };
	private static int[] jiao = { 2, 3, 2, 3, 2 };
	private static int[] zhi = { 2, 2, 3, 2, 3 };	
	private static int[] whole_tone = {2, 2, 2, 2, 2, 2};
	private static int[] whole = {2, 2, 2, 2, 2, 2};
	private static int[] chromatic = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
	private static int[] harmonic_minor = {2, 1, 2, 2, 1, 3, 1};
	private static int[] melodic_minor_asc = {2, 1, 2, 2, 2, 2, 1};
	private static int[] hungarian_minor = {2, 1, 3, 1, 1, 3, 1};
	private static int[] octatonic = {2, 1, 2, 1, 2, 1, 2, 1};
	private static int[] messiaen1 = {2, 2, 2, 2, 2, 2};
	private static int[] messiaen2 = {1, 2, 1, 2, 1, 2, 1, 2};
	private static int[] messiaen3 = {2, 1, 1, 2, 1, 1, 2, 1, 1};
	private static int[] messiaen4 = {1, 1, 3, 1, 1, 1, 3, 1};
	private static int[] messiaen5 = {1, 4, 1, 1, 4, 1};
	private static int[] messiaen6 = {2, 2, 1, 1, 2, 2, 1, 1};
	private static int[] messiaen7 = {1, 1, 1, 2, 1, 1, 1, 1, 2, 1};
	private static int[] super_locrian = {1, 2, 1, 2, 2, 2, 2};
	private static int[] hirajoshi = {2, 1, 4, 1, 4};
	private static int[] kumoi = {2, 1, 4, 2, 3};
	private static int[] neapolitan_major = {1, 2, 2, 2, 2, 2, 1};
	private static int[] bartok = {2, 2, 1, 2, 1, 2, 2};
	private static int[] bhairav = {1, 3, 1, 2, 1, 3, 1};
	private static int[] locrian_major = {2, 2, 1, 1, 2, 2, 2};
	private static int[] ahirbhairav = {1, 3, 1, 2, 2, 1, 2};
	private static int[] enigmatic = {1, 3, 2, 2, 2, 1, 1};
	private static int[] neapolitan_minor = {1, 2, 2, 2, 1, 3, 1};
	private static int[] pelog = {1, 2, 4, 1, 4};
	private static int[] augmented2 = {1, 3, 1, 3, 1, 3};
	private static int[] scriabin = {1, 3, 3, 2, 3};
	private static int[] harmonic_major = {2, 2, 1, 2, 1, 3, 1};
	private static int[] melodic_minor_desc = {2, 1, 2, 2, 1, 2, 2};
	private static int[] romanian_minor = {2, 1, 3, 1, 2, 1, 2};
	private static int[] hindu = {2, 2, 1, 2, 1, 2, 2};
	private static int[] iwato = {1, 4, 1, 4, 2};
	private static int[] melodic_minor = {2, 1, 2, 2, 2, 2, 1};
	private static int[] diminished2 = {2, 1, 2, 1, 2, 1, 2, 1};
	private static int[] marva = {1, 3, 2, 1, 2, 2, 1};
	private static int[] melodic_major = {2, 2, 1, 2, 1, 2, 2};
	private static int[] indian = {4, 1, 2, 3, 2};
	private static int[] spanish = {1, 3, 1, 2, 1, 2, 2};
	private static int[] prometheus = {2, 2, 2, 5, 1};
	private static int[] diminished = {1, 2, 1, 2, 1, 2, 1, 2};
	private static int[] todi = {1, 2, 3, 1, 1, 3, 1};
	private static int[] leading_whole = {2, 2, 2, 2, 2, 1, 1};
	private static int[] augmented = {3, 1, 3, 1, 3, 1};
	private static int[] purvi = {1, 3, 2, 1, 1, 3, 1};
	private static int[] chinese = {4, 2, 1, 4, 1};
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
	private static int[] chdaug5 = {0, 4, 8};
	private static int[] chdmaug5 = {0, 3, 8};
	private static int[] chdsus2 = { 0, 2, 7 };
	private static int[] chdsus4 = { 0, 5, 7 };
	private static int[] chd6 = { 0, 4, 7, 9 };
	private static int[] chdm6 = { 0, 3, 7, 9 };
	private static int[] chd7sus2 = { 0, 2, 7, 10 };
	private static int[] chd7sus4 = { 0, 5, 7, 10 };
	private static int[] chd7dim5 = {0, 4, 6, 10};
	private static int[] chd7aug5 = {0, 4, 8, 10};
	private static int[] chdm7aug5 = {0, 3, 8, 10};
	private static int[] chd9 = { 0, 4, 7, 10, 14 };
	private static int[] chdm9 = { 0, 3, 7, 10, 14 };
	private static int[] chdm7aug9 = {0, 3, 7, 10, 14};
	private static int[] chdmaj9 = { 0, 4, 7, 11, 14 };
	private static int[] chd9sus4 = { 0, 5, 7, 10, 14 };
	private static int[] chd6sus9 = {0, 4, 7, 9, 14};
	private static int[] chdm6sus9 = {0, 3, 9, 7, 14};
	private static int[] chd7dim9 = {0, 4, 7, 10, 13};
	private static int[] chdm7dim9 = {0, 3, 7, 10, 13};
	private static int[] chd7dim10 = {0, 4, 7, 10, 15};
	private static int[] chd7dim11 = {0, 4, 7, 10, 16};
	private static int[] chd7dim13 = {0, 4, 7, 10, 20};
	private static int[] chd9dim5 = {0, 10, 13};
	private static int[] chdm9dim5 = {0, 10, 14};
	private static int[] chd7aug5dim9 = {0, 4, 8, 10, 13};
	private static int[] chdm7aug5dim9 = {0, 3, 8, 10, 13};
	private static int[] chd11 = { 0, 4, 7, 10, 14, 17 };
	private static int[] chdm11 = { 0, 3, 7, 10, 14, 17 };
	private static int[] chdmaj11 = { 0, 4, 7, 11, 14, 17 };
	private static int[] chd11aug = {0, 4, 7, 10, 14, 18};
	private static int[] chdm11aug = {0, 3, 7, 10, 14, 18};
	private static int[] chd13 = { 0, 4, 7, 10, 14, 17, 21 };
	private static int[] chdm13 = { 0, 3, 7, 10, 14, 17, 21 };
	private static int[] chdadd2 = { 0, 2, 4, 7 };
	private static int[] chdadd4 = { 0, 4, 5, 7 };
	private static int[] chdadd9 = { 0, 4, 7, 14 };
	private static int[] chdadd11 = { 0, 4, 7, 17 };
	private static int[] add13 = {0, 4, 7, 21};
	private static int[] madd2 = {0, 2, 3, 7};
	private static int[] madd4 = {0, 3, 5, 7};
	private static int[] madd9 = {0, 3, 7, 14};
	private static int[] madd11 = {0, 3, 7, 17};
	private static int[] madd13 = {0, 3, 7, 21};

	private List<string> MidiNote = new List<string>();

	private List<string> Errorlog = new List<string>();
	private string Errors = "Errors: ";

	private int instrumentcntr = 0;
	private List<string> InstrumentName = new List<string>();
	private int bpm = 60;
	private const char s = 's';  //sample
	private const char b = 'b';  //beat
	private const char m = 'm';  //multibeat
	private const char i = 'i';  //instrument

	private int loopNum = 0;
	private const int numTracks = 8;
	private bool[] CueActive = new bool[numTracks];
	private List<SoundResource>[] TrackSamples = new List<SoundResource>[numTracks];
	private List<int>[] TrackOffsets = new List<int>[numTracks];
	private List<float>[] TrackMilliSeconds = new List<float>[numTracks];
	private float[] TrackTotalMilliseconds = new float[numTracks];
	private List<char>[] TrackSequence = new List<char>[numTracks];
	private List<int>[] TrackNotes = new List<int>[numTracks];
	private string[] TrackArrayAccess = new string[numTracks]; //"ring";
	private bool[] TrackRunning = new bool[numTracks];
	private bool[] TrackStop = new bool[numTracks];
	private float[] TrackVolume = new float[numTracks];  //0f;
	private float[] TrackPitchShift = new float[numTracks];  //0f;
	private bool[] TrackPlay_Once = new bool[numTracks];  //true;
	private bool[] TrackDont_Sync = new bool[numTracks];  //true;

	private bool[] TrackBlock = { false, false, false, false, false, false, false, false };
	private int[] SyncTrack = { 0, 0, 0, 0, 0, 0, 0, 0 };

	#endregion

	#region BuildSampleLibraries

	public List<SoundResource> SampleLibrary = new List<SoundResource>();

	public interface SendSamples
	{
		List<SoundResource> SendSampleLibrary { get; }	}

	private void getSamples(ScriptEventData gotSamples)
	{
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
		int cntr = 0;
		do
		{
			tempSample = sendSamples.SendSampleLibrary.ElementAt(cntr);
			SampleLibrary.Add(tempSample);
			Errors = Errors + ", " + tempSample.GetName();
			cntr++;
		} while (cntr < sendSamples.SendSampleLibrary.Count());	}

	public List<string>[] InstrumentArray = new List<string>[99];

	public interface SendInstrument
	{
		List<string> SendInstrumentArray { get; }	}

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
		if (sendInstrument.SendInstrumentArray.Count() > 0)
		{
			InstrumentArray[instrumentcntr] = new List<string>();
			int notecntr = 0;
			do
			{
				if (notecntr > 0)
				{
					InstrumentArray[instrumentcntr].Add(sendInstrument.SendInstrumentArray[notecntr]);
				}
				else
				{
					InstrumentName.Add(sendInstrument.SendInstrumentArray[0]);  //first entry of SendInstrumentArray is the name of the instrument
				}
				notecntr++;
			} while (notecntr < sendInstrument.SendInstrumentArray.Count());
			instrumentcntr++;
		}	}


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

	#endregion

	#region InitializeBuildSamples
	private void Initalize()
	{
		int cntr = 0;
		do
		{
			TrackArrayAccess[cntr] = "ring";
			TrackVolume[cntr] = 0f;
			TrackPitchShift[cntr] = 0f;
			TrackPlay_Once[cntr] = true;
			TrackDont_Sync[cntr] = true;
			cntr++;
		} while (cntr < numTracks);
	}

	private List<string> ParseIt(string InString)
	{
		List<string> cmds = new List<string>();
		int strlen = 0;
		char semi = ')';
		string test = InString;
		string cmd;
		int beg = 0;
		int endcmd = 0;

		do
		{
			endcmd = test.IndexOf(semi);
			cmd = test.Substring(beg, endcmd);
			cmd = cmd.Replace('(', ' ');
			cmds.Add(cmd);
			test = test.Remove(beg, endcmd + 1);
			strlen = test.Length;
		} while (test.Length > 1);
		return cmds;
	}

	private List<SoundResource> BuildSamples(List<string> Tempcmds)
	{
		string strtest;
		string cmdline;
		int cntr = 0;
		bool SampleFound = false;
		List<SoundResource> TempSamples = new List<SoundResource>();
		do
		{
			cmdline = Tempcmds[cntr];
			if (cmdline.Contains("sample")  || cmdline.Contains("inst"))
			{
				foreach (SoundResource Sample in SampleLibrary)
				{
					strtest = Sample.GetName();
					if (cmdline.Contains(strtest))
					{
						SampleFound = true;
						TempSamples.Add(Sample);
						break;
					}
				}
				if (!SampleFound) Log.Write("Sample - " + cmdline + " - not found");
			}
			cntr++;
		} while (cntr < Tempcmds.Count);

		return TempSamples;	}

	private float BuildVolume(string InString)
	{
		string substr = "vol(";
		int from = InString.IndexOf(substr, StringComparison.CurrentCulture);
		int length = InString.LastIndexOf(")", StringComparison.CurrentCulture);
		int last = length - from;
		string chunk = InString.Substring(from, last + 1);
		int next = chunk.IndexOf(")", StringComparison.CurrentCulture);
		string strVolume = chunk.Substring(4, next - 4);
		float fltVolume = float.Parse(strVolume);
		return fltVolume;
	}

	private float BuildPitchShift(string InString)
	{
		string substr = "pitch(";
		int from = InString.IndexOf(substr, StringComparison.CurrentCulture);
		int length = InString.Length;
		int last = length - from;
		string chunk = InString.Substring(from, last);
		int next = chunk.IndexOf(")", StringComparison.CurrentCulture);
		string strPitch = chunk.Substring(6, next - 6);
		float fltPitch = float.Parse(strPitch);
		return fltPitch;		}

#endregion

	#region NotesScalesChords

	private List<string> BuildScale(string[] ScaleIn)
	{
		int[] TempScaleNotes = null;
		List<string> ReturnNotes = new List<string>();
		int notecntr = 0;
		int basenote = 0;
		//find index of base note in MidiNoteArray
		do
		{
			if (MidiNote[notecntr] == ScaleIn[0])
			{
				basenote = notecntr;
				break;
			}
			notecntr++;
		} while (notecntr < MidiNote.Count());
		ScaleIn[1] = ScaleIn[1].Substring(1, ScaleIn[1].Length - 1);
		switch (ScaleIn[1])
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
		}

		ReturnNotes.Add(ScaleIn[0]); //first note is the base note

		//Get the Rest of the notes of the scale
		notecntr = 0;
		do
		{
			ReturnNotes.Add(MidiNote[basenote + TempScaleNotes[notecntr]]);
			basenote = basenote + TempScaleNotes[notecntr];
			notecntr++;
		} while (notecntr < TempScaleNotes.Count()-1);

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

	private List<int> BuildNotes(List<string> Tempcmds)
	{
		string cmdline;
		string strNotes;
		char comma = ',';
		int octaves = 0;
		int cntr = 0;
		int notecntr = 0;
		List<string> strTempNotes = new List<string>();
		List<int> intTempNotes= new List<int>();
		do
		{
			cmdline = Tempcmds[cntr];
			if (cmdline.Contains("notes"))
			{
				//parse note arrays
				// notes[e3,g3,a3]
				if (cmdline.Contains("["))
				{
					string[] NoteArray = cmdline.Split(comma);
					int from = NoteArray[0].IndexOf("[", StringComparison.CurrentCulture);
					strNotes = NoteArray[0].Substring(from+1, 2);
					NoteArray[0] = strNotes;
					//fix up last entry 
					int lastentry = NoteArray.Count();
					strNotes = NoteArray[lastentry - 1];
					strNotes = NoteArray[lastentry - 1].Substring(0, 2);
					NoteArray[lastentry - 1] = strNotes;
					notecntr = 0;
					do
					{
						strTempNotes.Add(NoteArray[notecntr]);
						notecntr++;
					} while (notecntr < NoteArray.Count());
				}

				//parse single note
				else 
				{
					strTempNotes.Add(cmdline.Substring(cmdline.Length - 2, 2));
				}

			}
			//parse scales
			if (cmdline.Contains("scale") || cmdline.Contains("chord"))
			{
				string[] NoteArray = cmdline.Split(comma);
				NoteArray[0] = NoteArray[0].Substring(7, NoteArray[0].Length-7);
				string scale = NoteArray[1];
				strTempNotes = BuildScale(NoteArray);
			}

			//octaves processing
			if (cmdline.Contains("octaves"))
			{
				string strOctaves = cmdline.Substring(9, 1);
				octaves = Int32.Parse(cmdline.Substring(9, 1));
			}

			cntr++;
		} while (cntr < Tempcmds.Count);

		notecntr = 0;
		do
		{
			intTempNotes.Add(FindMidiNote(strTempNotes[notecntr]));
			notecntr++;
		} while (notecntr < strTempNotes.Count());

		if (octaves > 0)
		{
			int octcntr = 0;
			int arraylength = intTempNotes.Count();
			do
			{
				notecntr = 0;
				do
				{
					intTempNotes.Add(intTempNotes[notecntr]+(12*(octcntr+1)));
					notecntr++;
				} while (notecntr < arraylength);
				octcntr++;
			} while (octcntr < octaves-1);
		}

		notecntr = 0;
		do
		{
			notecntr++;
		} while (notecntr < intTempNotes.Count());

		return intTempNotes;	}

	private double RateToPitch(double rateIn)
	{
		double semitone = 1.059463094359;
		double exponent = 1.0;
		double testval = 0;
		double pitchval = 0.0;
		double adder = 1.0;
		int iterator = 1;
		int precision = 1;

		if (rateIn > 1.0)
		{
			while (precision < 11)
			{
				iterator = 1;
				while (iterator < 12)
				{
					testval = Math.Pow(semitone, exponent);  //2
					if (testval < rateIn)
					{
						iterator++;
						exponent = exponent + adder;  //1.1
					}
					else
					{
						pitchval = exponent - adder;  //1.1-.1
						adder = adder * .1;  //.01
						exponent = pitchval;  //1.01
						precision++;
						iterator = 12;
					}
				}
			}
		}
		else if (rateIn < 1.0)
		{
			exponent = -1;
			adder = -1;
			while (precision < 11)
			{
				iterator = 1;
				while (iterator < 12)
				{
					testval = Math.Pow(semitone, exponent);  //2
					if (testval > rateIn)
					{
						iterator++;
						exponent = exponent + adder;  //-2
					}
					else
					{
						pitchval = exponent - adder;  //1.1-.1
						adder = adder * .1;  //.01
						exponent = pitchval;  //1.01
						precision++;
						iterator = 12;
					}
				}
			}
		}
		return pitchval;
	}

#endregion

	#region BuildTimingandSequence
	private List<float> BuildTiming(List<string> Tempcmds)
	{
		string cmdline;
		int cntr = 0;
		float sleepbeats = 0;
		float sleeptime = 0;
		int start = 0;
		int timecntr = 0;
		char comma = ',';
		string strTime = "";
		string secs = "";
		List<float> TempMilliSeconds = new List<float>();

		do
		{
			cmdline = Tempcmds[cntr];
			if (cmdline.Contains("beats"))
			{
				if (cmdline.Contains("["))
				{
					string[] TimeArray = cmdline.Split(comma);
					do
					{
						timecntr++;
					} while (timecntr < TimeArray.Count());
					//fix up first entry
					int from = TimeArray[0].IndexOf("[", StringComparison.CurrentCulture);
					strTime = TimeArray[0].Substring(from + 1, TimeArray[0].Length - from - 1);
					TimeArray[0] = strTime;
					//fix up last entry 
					int lastentry = TimeArray.Count();
					strTime = TimeArray[lastentry - 1];
					strTime = TimeArray[lastentry - 1].Substring(0, TimeArray[lastentry-1].Length-1);
					TimeArray[lastentry - 1] = strTime;
					timecntr = 0;
					do
					{
						secs = TimeArray[timecntr];
						sleepbeats = float.Parse(secs);
						sleeptime = sleepbeats / bpm* 60 * 1000;
						TempMilliSeconds.Add(sleeptime - 10);  //10 milliseconds less 
						timecntr++;
					} while (timecntr < TimeArray.Count());
				}
				else
				{
					start = cmdline.LastIndexOf("beats ", StringComparison.CurrentCulture);
					cmdline = cmdline.Substring(start, cmdline.Length - start);
					secs = cmdline.Substring(6, cmdline.Length - 6);
					sleepbeats = float.Parse(secs);
					secs = "";
					sleeptime = sleepbeats / bpm * 60 * 1000;
					TempMilliSeconds.Add(sleeptime - 10);  //10 milliseconds less 
				}
			}
			cntr++;
		} while (cntr < Tempcmds.Count);
		return TempMilliSeconds;	}

	private float BuildTotalTime(List<float> TimingArray)
	{
		float totalLoopTime = 0;
		int cntr = 0;

		do
		{
			totalLoopTime = totalLoopTime + TimingArray[cntr] + 10;  //Add back in the 10 milliseconds
			cntr++;
		} while (cntr < TimingArray.Count);
		return totalLoopTime;	}

	private List<char> BuildSequence(List<string> Tempcmds)
	{
		string cmdline;
		int cntr = 0;
		List<char> TempSequence = new List<char>();
		do
		{
			cmdline = Tempcmds[cntr];
			if (cmdline.Contains("sample")) TempSequence.Add(s);
			else if  (cmdline.Contains("inst")) TempSequence.Add(i);
			else if  (cmdline.Contains("beats [")) TempSequence.Add(m);
			else if  (cmdline.Contains("beats")) TempSequence.Add(b);
			cntr++;
		} while (cntr < Tempcmds.Count);
		return TempSequence;	}

	#endregion



	#region dump
	private void dump()
	{
		Log.Write("In dump");
		if (!(TrackSequence[loopNum] == null))
		{
			int cntr = 0;
			do
			{
				Log.Write("TrackSequence " + loopNum + ": " + TrackSequence[loopNum][cntr]);
				cntr++;
			} while (cntr < TrackSequence[loopNum].Count());
		}
		else Log.Write("Track Sequence is null");

		Log.Write("TrackSamplesCount: " + TrackSamples[loopNum].Count());
		//if (!(TrackSamples[loopNum] == null))
		//{
			int xcntr = 0;
			do
			{
				Log.Write("TrackSample" + loopNum + ": " + TrackSamples[loopNum][xcntr].GetName());
				xcntr++;
			} while (xcntr < TrackSamples[loopNum].Count());
		//}
		//else Log.Write("Track Samples is null");

		if (!(TrackOffsets[loopNum] == null))
		{
			int cntr = 0;
			do
			{
				Log.Write("TrackOffset" + loopNum + ": " + TrackOffsets[loopNum][cntr]);
				cntr++;
			} while (cntr < TrackOffsets[loopNum].Count());
		}
		else Log.Write("TrackOffset is null");

		Log.Write("TrackMilliSecondsCount: " + TrackMilliSeconds[loopNum].Count());
		if (!(TrackMilliSeconds[loopNum] == null))
		{
			int cntr = 0;
			do
			{
				Log.Write("TrackBeats: " + TrackMilliSeconds[loopNum][cntr]);
				cntr++;
			} while (cntr < TrackMilliSeconds[loopNum].Count());
		}
		else Log.Write("Track MilliSeconds is null");
	}
#endregion

	#region Tracks
	private void StopTrack()
	{
		int loopIn = loopNum;
		TrackStop[loopIn] = true;
		while (TrackRunning[loopIn])
		{
			Wait(TimeSpan.FromMilliseconds(10));
		}
		TrackStop[loopIn] = false;
	}

	private void PlayTrack()
	{
		int loopIn = loopNum;
		int seqcntr = 0;
		int samplecntr = 0;
		int playindex = 0;
		int beatscntr = 0;
		int beatscntr2 = 0;
		DateTime CurrentTime;
		DateTime LastTime = new DateTime(2017, 10, 1);
		TimeSpan timerdif;
		float tickadjust;
		double sleepadjusted;
		Random r = new Random();

		while (!TrackStop[loopIn]) //live loop
		{
			CurrentTime = DateTime.Now;
			timerdif = CurrentTime.Subtract(LastTime);

			if (CueActive[loopIn])
			{
				while (TrackBlock[SyncTrack[loopIn]]) Wait(TimeSpan.FromMilliseconds(10));
				CueActive[loopIn] = false;
			}
			TrackRunning[loopIn] = true;
			if (TrackSequence[loopIn].Count > 0)
			{
				TrackBlock[loopIn] = true;
				do
				{
					if (TrackSequence[loopIn][seqcntr] == s)
					{
						PlaySettings playSettings = TrackPlay_Once[loopIn] ? PlaySettings.PlayOnce : PlaySettings.Looped;
						playSettings.Loudness = TrackVolume[loopIn];
						playSettings.DontSync = TrackDont_Sync[loopIn];
						playSettings.PitchShift = TrackPitchShift[loopIn];
						PlayHandle playHandle;
						playHandle = ScenePrivate.PlaySoundAtPosition(TrackSamples[loopIn][samplecntr], this.ObjectPrivate.Position, playSettings);

						samplecntr++;
					}
					else if (TrackSequence[loopIn][seqcntr] == i)
					{
						PlaySettings playSettings = TrackPlay_Once[loopIn] ? PlaySettings.PlayOnce : PlaySettings.Looped;
						playSettings.Loudness = TrackVolume[loopIn];
						playSettings.DontSync = TrackDont_Sync[loopIn];
						if (TrackArrayAccess[loopIn] == "random")
						{
							playindex = r.Next(0, TrackSamples[loopIn].Count());
						}
						else if (TrackArrayAccess[loopIn] == "shuffle")
						{
						}
						else if (TrackArrayAccess[loopIn] == "invert")
						{
						}
						else playindex = samplecntr;
						playSettings.PitchShift = TrackOffsets[loopIn][playindex];
						PlayHandle playHandle;
						playHandle = ScenePrivate.PlaySoundAtPosition(TrackSamples[loopIn][playindex], this.ObjectPrivate.Position, playSettings);
						samplecntr++;
					}
					else if (TrackSequence[loopIn][seqcntr] == b)
					{
						Log.Write("beatscntr: " + beatscntr);;
						if (beatscntr == TrackMilliSeconds[loopIn].Count - 1)  //last wait in loop 
						{
							//Adjust the wait time based on how much time we have lost during the loop playing
							tickadjust = (timerdif.Ticks - (TrackTotalMilliseconds[loopIn] * 10000) - 100000);  //calculate using ticks
							sleepadjusted = TrackMilliSeconds[loopIn][beatscntr] - (tickadjust / 10000);  //apply using milliseconds
							if (tickadjust < 1000000)
							{
								Wait(TimeSpan.FromMilliseconds(sleepadjusted));  //skips the first time the loop is executed
							}
							else
							{
								Wait(TimeSpan.FromMilliseconds(TrackMilliSeconds[loopIn][beatscntr]));
							}
						}
						else
						{
							//not the last wait in the loop, so, do not adjust the wait time and use the beats statement
							Wait(TimeSpan.FromMilliseconds(TrackMilliSeconds[loopIn][beatscntr]));
						}
						TrackBlock[loopIn] = false;
						Wait(TimeSpan.FromMilliseconds(10));
						beatscntr++;
					}
					else if (TrackSequence[loopIn][seqcntr] == m)
					{
						if (beatscntr2 == TrackMilliSeconds[loopIn].Count - 1)  //last wait in loop 
						{
							//Adjust the wait time based on how much time we have lost during the loop playing
							tickadjust = (timerdif.Ticks - (TrackTotalMilliseconds[loopIn] * 10000) - 100000);  //calculate using ticks
							sleepadjusted = TrackMilliSeconds[loopIn][beatscntr2] - (tickadjust / 10000);  //apply using milliseconds
							if (tickadjust < 1000000)
							{
								Wait(TimeSpan.FromMilliseconds(sleepadjusted));  //skips the first time the loop is executed
							}
							else
							{
								Wait(TimeSpan.FromMilliseconds(TrackMilliSeconds[loopIn][beatscntr2]));
							}
						}
						else
						{
							//not the last wait in the loop, so, do not adjust the wait time and use the beats statement
							Wait(TimeSpan.FromMilliseconds(TrackMilliSeconds[loopIn][beatscntr2]));
						}
						TrackBlock[loopIn] = false;
						Wait(TimeSpan.FromMilliseconds(10));
						beatscntr2++;
						if (beatscntr2 == TrackMilliSeconds[loopIn].Count())
						{
							beatscntr2 = 0;  //make it a ring
						}

					}
					seqcntr++;
				} while (seqcntr < TrackSequence[loopIn].Count);
				seqcntr = 0;
				samplecntr = 0;
				beatscntr = 0;
			}
			LastTime = CurrentTime;
		}
		TrackRunning[loopIn] = false;
	}

	private void StopAll()
	{
		int loopNum = 0;
		int loopcntr = 0;
		do
		{
			if (TrackRunning[loopNum])
			{
				loopNum = loopcntr;
				StartCoroutine(StopTrack);
				loopcntr++;
			}
		} while (loopcntr < TrackRunning.Count());
	}
	#endregion

	#region Init
	public override void Init()
	{
		Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
 

		RigidBodyComponent rigidBody;
		if (ObjectPrivate.TryGetFirstComponent(out rigidBody)
			&& rigidBody.IsTriggerVolume())
		{
			rigidBody.Subscribe(CollisionEventType.Trigger, OnCollide);
		}
		else
		{
		}
	}


	private void UnhandledException(object Sender, Exception Ex)
	{
		Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
		return;
	}//UnhandledException

	private void OnCollide(CollisionData Data)
	{
		if (Data.Phase == CollisionEventPhase.TriggerEnter)
		{
			//Log.Write("has entered my volume!");
			string eventString = null;
			int samplepackcntr = 1;
			do
			{
				eventString = "Samples" + samplepackcntr;
				//Log.Write("samplepacknumber: " + samplepackcntr);
				SubscribeToScriptEvent(eventString, getSamples);
				eventString = "Instrument" + samplepackcntr;
				SubscribeToScriptEvent(eventString, getInstrument);
				samplepackcntr++;
			} while (samplepackcntr < 32);
			ScenePrivate.Chat.Subscribe(0, null, GetChatCommand);
			BuildMidiNotes();
			Initalize();
		}
		else
		{
			//Log.Write("has left my volume!");
			StopAll();
		}
	}
	#endregion

	#region ChatDialog
	private void GetChatCommand(ChatData Data)
	{
		loopNum = 0;
		Log.Write(Data.Message);
		if (Data.Message.Contains("/"))
		{
			int loopStop = 0;
			if (Data.Message.Contains("/loop"))
			{
				if (Data.Message.Contains("/loop0")) loopNum = 0;
				else if (Data.Message.Contains("/loop1")) loopNum = 1;
				else if (Data.Message.Contains("/loop2")) loopNum = 2;
				else if (Data.Message.Contains("/loop3")) loopNum = 3;
				else if (Data.Message.Contains("/loop4")) loopNum = 4;
				else if (Data.Message.Contains("/loop5")) loopNum = 5;
				else if (Data.Message.Contains("/loop6")) loopNum = 6;
				else if (Data.Message.Contains("/loop7")) loopNum = 7;

				if (Data.Message.Contains("stop"))
				{
					if (TrackRunning[loopNum])
					{
						loopStop = 0;
						StartCoroutine(StopTrack);
					}
				}
				else if (Data.Message.Contains("sample("))
				{
					List<string> TempCmdsIn;
					TempCmdsIn = ParseIt(Data.Message);
					if (TrackRunning[loopNum]) StartCoroutine(StopTrack);
					if (Data.Message.Contains("sync("))
					{
						CueActive[loopNum] = true;
						int from = Data.Message.IndexOf("sync(", StringComparison.CurrentCulture);
						string test = Data.Message.Substring(from, Data.Message.Length - from);
						int to = test.IndexOf(")", StringComparison.CurrentCulture);
						SyncTrack[loopNum] = Int32.Parse(test.Substring(9, to-9));  //SyncTrack[2] = 1
					}
					if (Data.Message.Contains("vol(")) TrackVolume[loopNum] = BuildVolume(Data.Message);
					if (Data.Message.Contains("pitch(")) TrackPitchShift[loopNum] = BuildPitchShift(Data.Message);
					if (Data.Message.Contains("rate(")) 
					{
						int from = Data.Message.IndexOf("rate(", StringComparison.CurrentCulture);
						string test = Data.Message.Substring(from, Data.Message.Length - from);
						int to = test.IndexOf(")", StringComparison.CurrentCulture);
						//proccessing to handle 100/80 for easy sample timing matching
						double tempRate = 0.0;
						if (test.Substring(5, to - 5).Contains("/"))
						{
							char slash = '/';
							string[] PitchRatio = test.Substring(5, to - 5).Split(slash);
							tempRate = double.Parse(PitchRatio[0]) / double.Parse(PitchRatio[1]);
						}
						else tempRate = Double.Parse(test.Substring(5, to-5));
						string tempPitch = RateToPitch(tempRate).ToString("G");
						string tempPitch2 = "pitch(" + tempPitch + ")";
						TrackPitchShift[loopNum] = BuildPitchShift(tempPitch2);
					}
					if (!(TrackSamples[loopNum] == null)) TrackSamples[loopNum].Clear();
					if (!(TrackMilliSeconds[loopNum] == null)) TrackMilliSeconds[loopNum].Clear();
					TrackSamples[loopNum] = BuildSamples(TempCmdsIn);
					TrackMilliSeconds[loopNum] = BuildTiming(TempCmdsIn);
					TrackTotalMilliseconds[loopNum] = BuildTotalTime(TrackMilliSeconds[loopNum]);
					TrackSequence[loopNum] = BuildSequence(TempCmdsIn);

					Log.Write("Starting Track" + loopNum);
                 	//dump();
					StartCoroutine(PlayTrack);
				}
				else if (Data.Message.Contains("inst("))
				{
					List<string> TempCmdsIn;
					List<string> TempSamplesIn = new List<string>();
					List<int> TempOffsetsIn = new List<int>();
					List<float> TempTimingIn = new List<float>();
					List<float> TempTimingIn2 = new List<float>();
					List<char> TempSequenceIn = new List<char>();
					TempCmdsIn = ParseIt(Data.Message);
					if (TrackRunning[loopNum]) StartCoroutine(StopTrack);
					if (Data.Message.Contains("sync(")) CueActive[loopNum] = true;
					if (Data.Message.Contains("vol(")) TrackVolume[loopNum] = BuildVolume(Data.Message);
					if (Data.Message.Contains("pitch(")) TrackPitchShift[loopNum] = BuildPitchShift(Data.Message);

					if (Data.Message.Contains("random")) TrackArrayAccess[loopNum] = "random";
					else if (Data.Message.Contains("shuffle")) TrackArrayAccess[loopNum] = "shuffle";
					else if (Data.Message.Contains("invert")) TrackArrayAccess[loopNum] = "invert";
					else TrackArrayAccess[loopNum] = "ring";

					TrackNotes[loopNum] = BuildNotes(TempCmdsIn);

					if (!(TrackSequence[loopNum] == null)) TrackSequence[loopNum].Clear();
					if (!(TrackSamples[loopNum] == null)) TrackSamples[loopNum].Clear();
					if (!(TrackMilliSeconds[loopNum] == null)) TrackMilliSeconds[loopNum].Clear();
					if (!(TrackOffsets[loopNum] == null)) TrackOffsets[loopNum].Clear();
					int notecntr = 0;
					//int timecntr = 0;
					string strOffset = "";
					int intOffset = 0;
					do
					{
						//Build Instrument
						// match instrument named in chat commad to instrument loaded into instrumentname array
						int from = Data.Message.IndexOf("inst(", StringComparison.CurrentCulture);
						string test = Data.Message.Substring(from, Data.Message.Length - from);
						int to = test.IndexOf(")", StringComparison.CurrentCulture);
						string InstrumentToFind = test.Substring(5, to - 5);
						int instcntr = 0;
						int instindex = 99;
						do  //Find Instrument in Instrument Array
						{
							if (InstrumentToFind == InstrumentName[instcntr]) instindex = instcntr;
							instcntr++;
						} while (instcntr < InstrumentName.Count());

						TempSamplesIn.Add("sample " + InstrumentArray[instindex][TrackNotes[loopNum][notecntr] * 2]);  // add string of sample to be used in temp array

						strOffset = InstrumentArray[instindex][TrackNotes[loopNum][notecntr] * 2 + 1];
						intOffset = Int32.Parse(strOffset);
						TempOffsetsIn.Add(intOffset);  // add int of offset to be used in temp array

						TempTimingIn = BuildTiming(TempCmdsIn);
						int timecntr = 0;
						do
						{
							TempTimingIn2.Add(TempTimingIn[timecntr]);
							timecntr++;
						} while (timecntr < TempTimingIn.Count());

						//Build Sequence
						TempSequenceIn.Add(i);
						if (!Data.Message.Contains("chord"))
						{
							if (Data.Message.Contains("beats([")) TempSequenceIn.Add(m);
							else TempSequenceIn.Add(b);  //for set or scale note, beats, note, beats
						}
						notecntr++;
					} while (notecntr<TrackNotes[loopNum].Count());

					//Build the SoundResources Samples List by passing a list of strings that have the names of the samples to be built by BuildSamples
					TrackSamples[loopNum] = BuildSamples(TempSamplesIn);
					TrackOffsets[loopNum] = TempOffsetsIn;
					TrackMilliSeconds[loopNum] = TempTimingIn2;
					if (Data.Message.Contains("chord"))  // If it is a chord you only want to play it onnce per beat
					{
						if (Data.Message.Contains("beats([")) TempSequenceIn.Add(m);
						else TempSequenceIn.Add(b);  //for set or scale note, beats, note, beats
					}

					TrackSequence[loopNum] = TempSequenceIn;

					TrackTotalMilliseconds[loopNum] = BuildTotalTime(TrackMilliSeconds[loopNum]);

					//dump();

					StartCoroutine(PlayTrack);
				}
			}

		}

		if (Data.Message.Contains("stopall"))
		{
			StopAll();
		}

		if (Data.Message.Contains("bpm"))
		{
			string InString = Data.Message;
			int from = InString.IndexOf("(", StringComparison.CurrentCulture);
			int to = InString.IndexOf(")", StringComparison.CurrentCulture);
			int length = InString.Length;
			int last = to - from;
			string chunk = InString.Substring(from + 1, last - 1);
			bpm = Int32.Parse(chunk);
		}
	}
#endregion
}