//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

#define SansarBuild

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class YouTubeViewer : SceneObjectScript
{
    #region ConstantsVariables

    [DefaultValue("Welcome to GranddadGotMojos Media Screen")]
    [DisplayName("Welcome Message:")]
    public string WelcomeMessage = "Welcome to GranddadGotMojos Media Screen";

    [DefaultValue("ALL")]
    [DisplayName("Valid User List:")]
    public string UsersToListenTo = "ALL";

    [DefaultValue(1080)]
    [DisplayName("Screen Height: ")]
    public int ScreenHeight = 1080;

    [DefaultValue(1920)]
    [DisplayName("Screen Width:")]
    public int ScreenWidth = 1920;

    public string Play1 = null;
    public string Play2 = null;
    public string Play3 = null;
    public string Play4 = null;
    public string Play5 = null;
    public string Play6 = null;
    public string Play7 = null;
    public string Play8 = null;
    public string Play9 = null;
    public string Play10 = null;
    public string Play11 = null;
    public string Play12 = null;
    public string Play13 = null;
    public string Play14 = null;
    public string Play15 = null;

    private AudioComponent audio;
    private PlayHandle currentPlayHandle;

    private List<string> ValidUsers = new List<string>();
    private string video = null;
    private string EmbedVideoID = null;
    DateTime VideoStartTime = DateTime.Now;
    private double VideoCurrentTime = 0;
    private int intVideoCurrentTime = 0;
    private double NewDb = 0;
    private double LastDb = 0;
    private double volume = 0;
    private double LastVolume = 50;  //assume that starting volume is halfway between max and min
    private PlayHandle myVolume;
    private bool IsWatchFormat = false;

    private string Errormsg = "No Errors";
    private bool strErrors = false;
    private SessionId Jammer = new SessionId();

    #endregion

    public override void Init()
    {

        //myVolume = ScenePrivate.PlayStream(StreamChannel.MediaChannel, 0);
        ObjectPrivate.TryGetFirstComponent(out audio);
        if (audio != null)
        {
            ScenePrivate.Chat.MessageAllUsers("Media Screen for Youtube Viewer requires an Audio Emitter");
        }
        else
        {
            Log.Write("Found Audio Component");
            currentPlayHandle = audio.PlayStreamOnComponent(StreamChannel.MediaChannel, 50);
        }


        Log.Write("Script Started");
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal
        ScenePrivate.Chat.MessageAllUsers(WelcomeMessage);
        ScenePrivate.Chat.Subscribe(0, GetChatCommand);
    }

    private void UnhandledException(object Sender, Exception Ex)
    {

        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region Communication
    #endregion

    private void GetChatCommand(ChatData Data)
    {
        Log.Write("Chat From: " + Data.SourceId);
        Log.Write("Chat person: " + ScenePrivate.FindAgent(Data.SourceId).AgentInfo.Name);
        AgentPrivate agent = ScenePrivate.FindAgent(Data.SourceId);
        ValidUsers.Clear();
        ValidUsers  = UsersToListenTo.Split(',').ToList();
        if (UsersToListenTo.Contains("ALL"))
        {
            string DataCmd = Data.Message;
            ParseCommands(DataCmd, agent);
        }
        else
        {
            foreach (string ValidUser in ValidUsers)
            {
                Log.Write("ValidUser: " + ValidUser);
                if (ScenePrivate.FindAgent(Data.SourceId).AgentInfo.Name == ValidUser.Trim())
                {
                    string DataCmd = Data.Message;
                    ParseCommands(DataCmd, agent);
                }
            }
    }
}

    private void ParseCommands(string DataCmdIn, AgentPrivate agent)
    {
        Errormsg = "No Errors";
        Log.Write("DataCmdIn: " + DataCmdIn);
        if (DataCmdIn.Contains("/"))
        {
            strErrors = false;
            if (DataCmdIn.Contains("/forcevideo"))
            {
                //play video
                IsWatchFormat = false;
                video = DataCmdIn.Substring(12, DataCmdIn.Length - 12);
                Log.Write("video: " + video);
                VideoStartTime = DateTime.Now;
                VideoCurrentTime = 0;
                ScenePrivate.OverrideMediaSource(video, ScreenWidth, ScreenHeight);
            }
            if ((DataCmdIn.Contains("/video")) || (DataCmdIn.Contains("/stream")))
            {
                //play video
                IsWatchFormat = false;
                video = DataCmdIn.Substring(7, DataCmdIn.Length - 7);
                Log.Write("video: " + video);
                if (DataCmdIn.Contains("/watch?v="))
                {
                    IsWatchFormat = true;
                    video = URLToEmbedFormat(DataCmdIn);
                    Log.Write("New Video: " + video);
                }
                if (DataCmdIn.Contains("Youtu.be"))
                {
                    IsWatchFormat = true;
                    video = ShortenedURLToEmbedFormat(DataCmdIn);
                    Log.Write("New Video: " + video);
                }
                VideoStartTime = DateTime.Now;
                VideoCurrentTime = 0;
                ScenePrivate.OverrideMediaSource(video, ScreenWidth, ScreenHeight);
            }
            if (DataCmdIn.Contains("/play"))
            {
                //play video
                string VideoToPlay = null;
                Log.Write("DataCmdIn: " + DataCmdIn.Trim());
                switch (DataCmdIn.Trim())
                {
                    case "/play1":
                        if (Play1.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play1;
                        }
                        break;
                    case "/play2":
                        if (Play2.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play2;
                        }
                        break;
                    case "/play3":
                        if (Play3.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play3;
                        }
                        break;
                    case "/play4":
                        if (Play4.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play4;
                        }
                        break;
                    case "/play5":
                        if (Play5.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play5;
                        }
                        break;
                    case "/play6":
                        if (Play6.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play6;
                        }
                        break;
                    case "/play7":
                        if (Play7.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play7;
                        }
                        break;
                    case "/play8":
                        if (Play8.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play8;
                        }
                        break;
                    case "/play9":
                        if (Play9.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play9;
                        }
                        break;
                    case "/play10":
                        if (Play10.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play10;
                        }
                        break;
                    case "/play11":
                        if (Play11.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play11;
                        }
                        break;
                    case "/play12":
                        if (Play12.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play12;
                        }
                        break;
                    case "/play13":
                        if (Play13.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play13;
                        }
                        break;
                    case "/play14":
                        if (Play14.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play14;
                        }
                        break;
                    case "/play15":
                        if (Play15.Length > 0)
                        {
                            IsWatchFormat = false;
                            VideoToPlay = Play15;
                        }
                        break;
                    default:
                        Errormsg = "Must be Play1 thru Play15";
                        break;
                }

                Log.Write("video: " + VideoToPlay);
                if (VideoToPlay.Contains("/watch?v="))
                {
                    IsWatchFormat = true;
                    VideoToPlay = URLToEmbedFormat(VideoToPlay);
                }
                VideoStartTime = DateTime.Now;
                VideoCurrentTime = 0;
                ScenePrivate.OverrideMediaSource(VideoToPlay, ScreenWidth, ScreenHeight);
            }
            if (DataCmdIn.Contains("/pause") && IsWatchFormat)
            {
                intVideoCurrentTime = (int)(DateTime.Now - VideoStartTime).TotalSeconds;
                video = "https://www.youtube.com/embed/" + EmbedVideoID + "?rel=0&end=1&controls=0&showinfo=0&autoplay=1&allowfullscreen";
                Log.Write("Video on pause: " + video);
                ScenePrivate.OverrideMediaSource(video, ScreenWidth, ScreenHeight);
            }
            if (DataCmdIn.Contains("/resume") && IsWatchFormat)
            {
                intVideoCurrentTime = (int)(DateTime.Now - VideoStartTime).TotalSeconds;
                video = "https://www.youtube.com/embed/" + EmbedVideoID + "?rel=0&start=" + intVideoCurrentTime.ToString() + "&controls=0&showinfo=0&autoplay=1&allowfullscreen";
                Log.Write("Video on resume: " + video);
                ScenePrivate.OverrideMediaSource(video, ScreenWidth, ScreenHeight);
            }
            if (DataCmdIn.Contains("/stop") && IsWatchFormat)
            {
                intVideoCurrentTime = 0;
                VideoStartTime = DateTime.Now;
                EmbedVideoID = "4wTPTh6-sSo";
                video = "https://www.youtube.com/embed/" + EmbedVideoID + "  ?rel=0&controls=0&showinfo=0&autoplay=1&allowfullscreen";
                Log.Write("Video on pause: " + video);
                ScenePrivate.OverrideMediaSource(video, ScreenWidth, ScreenHeight);
            }
            if (DataCmdIn.Contains("/volume") && IsWatchFormat)
            {
                string strvolume = DataCmdIn.Substring(7, DataCmdIn.Length - 7);
                volume = double.Parse(strvolume.Trim());
                //Log.Write("GetLoudness Before Volume: " + myVolume.GetLoudness());

                //Calculate Db if the Volume had started at 50

                Log.Write("volume: " + volume);
                if (volume == 0.0) volume = 0.1;
                if (volume < 0.0) volume = 0.1;
                if (volume > 100) volume = 100;
                NewDb = 33.24 * Math.Log10(volume/LastVolume);
                if (volume < LastVolume)
                {

                }
                else
                {
                    NewDb = NewDb * .1;
                }

                LastVolume = volume;  
                Log.Write("NewDb: " + NewDb);
                // 33.24 * LOG(A8 / A$8, 10)
                currentPlayHandle.SetLoudness((float) NewDb);
                //CurrentDb = NewDb;
                Wait(TimeSpan.FromSeconds(2));
                //Log.Write("GetLoundess After Volume: " + myVolume.GetLoudness());
            }
            if (DataCmdIn.Contains("/commands"))
            {
                DisplayHelp(agent);

            }
        }
    }

    private string URLToEmbedFormat(string URLInWatchFormat)
    {
        string URLInEmbedFormat = null;

        int VideoIDStart = URLInWatchFormat.IndexOf("?") + 3;
        Log.Write("VideoIDStart: " + VideoIDStart);
        Log.Write("URLInWatchFormat.Length: " + URLInWatchFormat.Length);
        EmbedVideoID = URLInWatchFormat.Substring(VideoIDStart, URLInWatchFormat.Length - VideoIDStart);
        Log.Write("EmbedVideoID: " + EmbedVideoID);
        URLInEmbedFormat = "https://www.youtube.com/embed/" + EmbedVideoID + "?rel=0&controls=0&showinfo=0&autoplay=1&allowfullscreen";

        return URLInEmbedFormat;
    }

    private string ShortenedURLToEmbedFormat(string URLInWatchFormat)
    {
        string URLInEmbedFormat = null;

        int VideoIDStart = URLInWatchFormat.IndexOf(".be/") + 5;
        Log.Write("VideoIDStart: " + VideoIDStart);
        Log.Write("URLInWatchFormat.Length: " + URLInWatchFormat.Length);
        EmbedVideoID = URLInWatchFormat.Substring(VideoIDStart, URLInWatchFormat.Length - VideoIDStart);
        Log.Write("EmbedVideoID: " + EmbedVideoID);
        URLInEmbedFormat = "https://www.youtube.com/embed/" + EmbedVideoID + "?rel=0&controls=0&showinfo=0&autoplay=1&allowfullscreen";

        return URLInEmbedFormat;
    }

    private void DisplayHelp(AgentPrivate agent)
    {
        agent.SendChat("Command Summary");
        agent.SendChat("/video YouTubeURL - starts Youtube Video");
        agent.SendChat("/pause - pauses video");
        agent.SendChat("/resume - resumes paused video");
        agent.SendChat("/stop - stops video playing");
        agent.SendChat("/play1 thru /play15 - plays preconfigured videos");
        agent.SendChat("/volume 30 - sets volume to 30. This is a volume slider and volume starts at a default of 50.  You can enter from 0 to 100 as a volume.");
        agent.SendChat("/commands - displays this help menu");
        agent.SendChat("To switch Videos just type in a new /video or /playX commmand");
        agent.SendChat("");
        agent.SendChat("contact GranddadGotMojo if any questions or issues");
    }

}
