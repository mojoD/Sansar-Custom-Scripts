using System;
using System.Linq;
using System.Collections.Generic;

using Sansar.Simulation;
using Sansar.Script;

public class SawWaveInstrumentSamplePack : SceneObjectScript
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
		}	}
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

		sendInstrument.InstrumentArray = new List<string>();
		sendInstrument.InstrumentArray.Add("saw");  //name of the instrument is the first entry
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-69"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-68"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-67"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-66"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-65");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-64"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-63"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-62"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-61"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-60");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-59"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-58"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-57"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-56"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-55");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-54"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-53"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-52"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-51"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-50");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-49"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-48"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-47"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-46"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-45");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-44"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-43"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-42"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-41"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-40");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-39"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-38"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-37"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-36"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-35");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-34"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-33"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-32"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-31"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-30");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-29"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-28"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-27"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-26"); sendInstrument.InstrumentArray.Add("Saw A54"); sendInstrument.InstrumentArray.Add("-25");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-24"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-23"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-22"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-21"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-20");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-19"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-18"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-17"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-16"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-15");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-14"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-13"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-12"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-11"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-10");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-9"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-8"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-7"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-6"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-5");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-4"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-3"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-2"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("-1"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("0");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("1"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("2"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("3"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("4"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("5");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("6"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("7"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("8"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("9"); sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("10");
		sendInstrument.InstrumentArray.Add("Saw A5"); sendInstrument.InstrumentArray.Add("11"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("0"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("1"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("2"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("3");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("4"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("5"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("6"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("7"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("8");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("9"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("10"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("11"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("12"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("13");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("14"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("15"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("16"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("17"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("18");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("19"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("20"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("21"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("22"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("23");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("24"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("25"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("26"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("27"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("28");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("29"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("30"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("31"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("32"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("33");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("34"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("35"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("36"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("37"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("38");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("39"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("40"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("41"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("42"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("43");
		sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("44"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("45"); sendInstrument.InstrumentArray.Add("Saw A6"); sendInstrument.InstrumentArray.Add("46");


		Wait(TimeSpan.FromSeconds(5));
		PostScriptEvent(ScriptId.AllScripts, "Samples9", sendSamples);
        PostScriptEvent(ScriptId.AllScripts, "Instrument9", sendInstrument);	}

	public override void Init()
	{
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

	private void OnCollide(CollisionData Data)
	{
		if (Data.Phase == CollisionEventPhase.TriggerEnter)
		{
			Log.Write("Loading Saw Wave Sample Pack");
            StartCoroutine(BuildSampleLibrary);
		}
		else
		{
		}	}
}