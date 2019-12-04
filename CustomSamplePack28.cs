using System;
using System.Linq;
using System.Collections.Generic;

using Sansar.Simulation;
using Sansar.Script;

public class CustomSamplePack28 : SceneObjectScript
{
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

    public int ScriptOrder = 1;
    public SoundResource Sample1 = null;
	public SoundResource Sample2 = null;
	public SoundResource Sample3 = null;
	public SoundResource Sample4 = null;
	public SoundResource Sample5 = null;
	public SoundResource Sample6 = null;
	public SoundResource Sample7 = null;
	public SoundResource Sample8 = null;
	public SoundResource Sample9 = null;

	// PRIVATE MEMBERS ------
	private SendSamples sendSamples = new SendSamples();
	private SendInstrument sendInstrument = new SendInstrument();


	private void BuildSampleLibrary()
	{
		SendSamples sendSamples = new SendSamples();
		SendInstrument sendInstrument = new SendInstrument();
		sendSamples.SampleLibrary = new List<SoundResource>();
		sendInstrument.InstrumentArray = new List<string>();
		if (Sample1 != null) sendSamples.SampleLibrary.Add(Sample1);
		if (Sample2 != null) sendSamples.SampleLibrary.Add(Sample2);
		if (Sample3 != null) sendSamples.SampleLibrary.Add(Sample3);
		if (Sample4 != null) sendSamples.SampleLibrary.Add(Sample4);
		if (Sample5 != null) sendSamples.SampleLibrary.Add(Sample5);
		if (Sample6 != null) sendSamples.SampleLibrary.Add(Sample6);
		if (Sample7 != null) sendSamples.SampleLibrary.Add(Sample7);
		if (Sample8 != null) sendSamples.SampleLibrary.Add(Sample8);
		if (Sample9 != null) sendSamples.SampleLibrary.Add(Sample9);

		sendInstrument.InstrumentArray = new List<string>();

		Wait(TimeSpan.FromSeconds(3) + TimeSpan.FromSeconds(ScriptOrder*2));
		PostScriptEvent(ScriptId.AllScripts, "Samples28", sendSamples);
		PostScriptEvent(ScriptId.AllScripts, "Instrument2", sendInstrument);
	}

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
			Log.Write("Loading Custom Sample Pack28");
            Log.Write("scriptID: " + Script.ID.ToString());
            StartCoroutine(BuildSampleLibrary);
		}
		else
		{
		}
	}
}