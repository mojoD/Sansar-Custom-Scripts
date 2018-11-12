using System;
using System.Linq;
using System.Collections.Generic;

using Sansar.Simulation;
using Sansar.Script;

public class CongaSamplePack : SceneObjectScript
{
	public class SendSamples : Reflective
	{
		public ScriptId SourceScriptId { get; internal set; }

		public List<object> SampleLibrary { get; internal set; }

		public List<object> SendSampleLibrary
		{
			get
			{
				return SampleLibrary;
			}
		}
	}

	public class SendInstrument : Reflective
	{
		public ScriptId SourceScriptId { get; internal set; }

		public List<string> InstrumentArray { get; internal set; }

		public List<string> SendInstrumentArray
		{
			get
			{
				return InstrumentArray;
			}
		}
	}
    // PUBLIC MEMBERS ------
    //[DefaultValue("response")]
    //public readonly string responseCommand = "response";	

    public string Sample1Name = null;
    public SoundResource Sample1 = null;
    public string Sample2Name = null;
    public SoundResource Sample2 = null;
    public string Sample3Name = null;
    public SoundResource Sample3 = null;
    public string Sample4Name = null;
    public SoundResource Sample4 = null;
    public string Sample5Name = null;
    public SoundResource Sample5 = null;
    public string Sample6Name = null;
    public SoundResource Sample6 = null;
    public string Sample7Name = null;
    public SoundResource Sample7 = null;
    public string Sample8Name = null;
    public SoundResource Sample8 = null;
    public string Sample9Name = null;
    public SoundResource Sample9 = null;
    public string Sample10Name = null;
    public SoundResource Sample10 = null;

	// PRIVATE MEMBERS ------
	private SendSamples sendSamples = new SendSamples();
	private SendInstrument sendInstrument = new SendInstrument();


	private void BuildSampleLibrary()
	{
		SendSamples sendSamples = new SendSamples();
		SendInstrument sendInstrument = new SendInstrument();
		sendSamples.SampleLibrary = new List<object>();
		sendInstrument.InstrumentArray = new List<string>();
        if (Sample1Name.Length > 0) sendSamples.SampleLibrary.Add(Sample1Name);
        if (Sample1 != null) sendSamples.SampleLibrary.Add(Sample1);
        if (Sample2Name.Length > 0) sendSamples.SampleLibrary.Add(Sample2Name);
        if (Sample2 != null) sendSamples.SampleLibrary.Add(Sample2);
        if (Sample3Name.Length > 0) sendSamples.SampleLibrary.Add(Sample3Name);
        if (Sample3 != null) sendSamples.SampleLibrary.Add(Sample3);
        if (Sample4Name.Length > 0) sendSamples.SampleLibrary.Add(Sample4Name);
        if (Sample4 != null) sendSamples.SampleLibrary.Add(Sample4);
        if (Sample5Name.Length > 0) sendSamples.SampleLibrary.Add(Sample5Name);
        if (Sample5 != null) sendSamples.SampleLibrary.Add(Sample5);
        if (Sample6Name.Length > 0) sendSamples.SampleLibrary.Add(Sample6Name);
        if (Sample6 != null) sendSamples.SampleLibrary.Add(Sample6);
        if (Sample7Name.Length > 0) sendSamples.SampleLibrary.Add(Sample7Name);
        if (Sample7 != null) sendSamples.SampleLibrary.Add(Sample7);
        if (Sample8Name.Length > 0) sendSamples.SampleLibrary.Add(Sample8Name);
        if (Sample8 != null) sendSamples.SampleLibrary.Add(Sample8);
        if (Sample9Name.Length > 0) sendSamples.SampleLibrary.Add(Sample9Name);
        if (Sample9 != null) sendSamples.SampleLibrary.Add(Sample9);
        if (Sample10Name.Length > 0) sendSamples.SampleLibrary.Add(Sample10Name);
        if (Sample10 != null) sendSamples.SampleLibrary.Add(Sample10);

		sendInstrument.InstrumentArray = new List<string>();
		sendInstrument.InstrumentArray.Add("conga");  //name of the instrument is the first entry
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-12");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-11");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-10");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-9");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-8");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-7");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-6");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-5");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-4");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-3");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-2");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("-1");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("63CongaBrLw"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("62CongaBrHi"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("62CongaBrHi"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("61BongoBrLw"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("61BongoBrLw"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("60BongoBrHi"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("60BongoBrHi"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("64FrameOpen"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("64FrameOpen"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiagogo"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiagogo"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("85Castanet1"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("85Castanet1"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Claves"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Claves"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");
        sendInstrument.InstrumentArray.Add("909Hiwoodbl"); sendInstrument.InstrumentArray.Add("0");

        Wait(TimeSpan.FromSeconds(5));
		PostScriptEvent(ScriptId.AllScripts, "Samples45", sendSamples);
        PostScriptEvent(ScriptId.AllScripts, "Instrument45", sendInstrument);
    }

	public override void Init()
	{
        Log.Write("Loading Conga Sample Pack");
        StartCoroutine(BuildSampleLibrary);
	}
}