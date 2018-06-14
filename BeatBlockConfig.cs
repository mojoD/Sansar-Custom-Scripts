//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using Sansar;
using Sansar.Simulation;
using Sansar.Script;
using System;
using System.Linq;
using System.Collections.Generic;

public class BeatBlockConfig : SceneObjectScript
{
    // Components can be set in the editor if the correct component types are added to the object
    public string BeatBlockName;
    public SoundResource Sample1 = null;
    public string beats;
    public string BlockGenre;

    public BeatBlockConfig()
    {
    }

    #region Communications

    public class SendSamplescfg : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public ObjectId myObjectID { get; internal set; }

        public List<SoundResource> SampleLibrarycfg { get; internal set; }

        public List<SoundResource> SendSampleLibrarycfg
        {
            get
            {
                return SampleLibrarycfg;
            }
        }
    }

    public class SendBlockNamescfg : Reflective
    {
        public ScriptId SourceScriptId { get; internal set; }

        public List<string> BlockNameArraycfg { get; internal set; }

        public List<string> SendBlockArraycfg
        {
            get
            {
                return BlockNameArraycfg;
            }
        }
    }
    #endregion

    public override void Init()
    {

        string myObject;
        myObject = ObjectPrivate.ObjectId.ToString();
        Wait(TimeSpan.FromSeconds(5.0));

        SendBlockNamescfg sendBlockscfg = new SendBlockNamescfg();
        sendBlockscfg.BlockNameArraycfg = new List<string>();
        //BeatBlockName = Sample1.GetName();
        sendBlockscfg.BlockNameArraycfg.Add(BeatBlockName);
        sendBlockscfg.BlockNameArraycfg.Add(beats);
        sendBlockscfg.BlockNameArraycfg.Add(BlockGenre);
        string BeatBlockConfigEvent = "BeatBlockConfig" + myObject;
        PostScriptEvent(ScriptId.AllScripts, BeatBlockConfigEvent, sendBlockscfg);

        SendSamplescfg sendSamplescfg = new SendSamplescfg();
        sendSamplescfg.SampleLibrarycfg = new List<SoundResource>();
        if (Sample1 != null) sendSamplescfg.SampleLibrarycfg.Add(Sample1);
        string BeatBlockSampleConfigEvent = "BeatBlockSampleConfig" + myObject;
        PostScriptEvent(ScriptId.AllScripts, BeatBlockSampleConfigEvent, sendSamplescfg);
    }
}
