//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class RandomContestantAndPrizeSelector : SceneObjectScript

{
    #region ConstantsAndVariables

    //public string PrizeCmd;
    public List<string> Prizes1To20 = new List<string>();
    public List<string> Prizes21To30 = new List<string>();

    //[DefaultValue("Prize")]
    //[DisplayName("Prize Event Name")]
    //public string PrizeEventName = "Prize";

    // How close to get before detected
    [DefaultValue(100.0)]
    [DisplayName("Detection Range")]
    public float DetectionRange = 100.0f;

    [DefaultValue("Winner")]
    [DisplayName("Random Prize Event")]
    public string RandomPrizeEvent = "Winner";

    //[DefaultValue("PreviousGame")]
    //[DisplayName("Prev Game Event")]
    //public string PrevGameEvent = "PreviousGame";

    //[DefaultValue("NextGame")]
    //[DisplayName("Next Game Event")]
    //public string NextGameEvent = "NextGame";

    //[DefaultValue("Winner")]
    //[DisplayName("Winner Event")]
    //public string WinnerEvent = "Winner";

    private int NumberofPrizes = 0;
    private int NumberofContestants = 0;
    //private int ItemChosen = 0;
    private int PrizeNumber;
    private AgentPrivate Hitman;

    bool AgentInDetectionRange = false;
    int PeopleInRange = 0;

    //private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    //ObjectId hitter;
    Guid ProductGuid;

    private List<AgentPrivate> Contestants = new List<AgentPrivate>();
    private List<string> AllPrizes = new List<string>();
    private AgentPrivate ChosenContestant;
    private Random r = new Random();

    //private int BoardColumns = 25;
    //public class SendChar : Reflective
    //{
    //    public int CharIndex { get; set; }
    //    public string CharToSend { get; set; }
    //}

    #endregion

    #region Communication

    /*
    public interface IItemChosen
    {
        int ItemChosenIn { get; }
    }

    public void getItemChosen(ScriptEventData gotItemChosen)
    {

        //Log.Write("In getItemChosen");
        if (gotItemChosen.Data == null)
        {
            return;
        }

        IItemChosen sendItemChosen = gotItemChosen.Data.AsInterface<IItemChosen>();
        if (sendItemChosen == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        Log.Write("sendItemChosen.ItemChosenIn: " + sendItemChosen.ItemChosenIn);
        if (sendItemChosen.ItemChosenIn > 0)
        {
            ItemChosen = sendItemChosen.ItemChosenIn;

        }
    }

    public interface IBuyInfo
    {
        AgentPrivate HitmanIn { get; }
    }

    public void getBuyInfo(ScriptEventData gotBuyInfo)
    {

        Log.Write("In getBuyInfo");
        if (gotBuyInfo.Data == null)
        {
            return;
        }

        IBuyInfo sendBuyInfo = gotBuyInfo.Data.AsInterface<IBuyInfo>();
        if (sendBuyInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        Log.Write("sendBuyInfo.HitmanIn: " + sendBuyInfo.HitmanIn + " ItemChosen: " + ItemChosen + " ProductID: " + AllPrizes[ItemChosen]);
        Hitman = sendBuyInfo.HitmanIn;
        string ProductId = AllPrizes[ItemChosen];
        if (!Guid.TryParse(ProductId, out ProductGuid))
        {
            bool foundId = false;
            // Find the ID from the store listing url. Very generic, will just find the first url segment or query arg it can convert to a UUID.
            // https://store.sansar.com/listings/9eb72eb2-38c1-4cd3-a9eb-360e2f19e403/female-pirate-hat-r3d
            string[] segments = ProductId.Split(new string[] { "/", "?", "&", "=" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string segment in segments)
            {
                if (segment.Length >= 32
                    && Guid.TryParse(segment, out ProductGuid))
                {
                    foundId = true;
                    break;
                }
            }

            if (!foundId)
            {
                Log.Write("Not Found in Store");
            }
            else
            {
            }
        }

        Log.Write("ProductID: " + ProductId);
        Log.Write("Product GUID: " + ProductGuid);
        Hitman.Client.OpenStoreListing(ProductGuid);
    }
    */

    #region SimpleHelpers v2
    // Update the region tag above by incrementing the version when updating anything in the region.

    // If a Group is set, will only respond and send to other SimpleScripts with the same Group tag set.
    // Does NOT accept CSV lists of groups.
    // To send or receive events to/from a specific group from outside that group prepend the group name with a > to the event name
    // my_group>on
    [DefaultValue("")]
    [DisplayName("Group")]
    public string Group = "";

    public interface ISimpleData
    {
        AgentInfo AgentInfo { get; }
        ObjectId ObjectId { get; }
        ObjectId SourceObjectId { get; }

        // Extra data
        Reflective ExtraData { get; }
    }

    public class SimpleData : Reflective, ISimpleData
    {
        public SimpleData(ScriptBase script) { ExtraData = script; }
        public AgentInfo AgentInfo { get; set; }
        public ObjectId ObjectId { get; set; }
        public ObjectId SourceObjectId { get; set; }

        public Reflective ExtraData { get; set; }
    }

    public interface IDebugger { bool DebugSimple { get; } }
    private bool __debugInitialized = false;
    private bool __SimpleDebugging = false;
    private string __SimpleTag = "";

    private string GenerateEventName(string eventName)
    {
        eventName = eventName.Trim();
        if (eventName.EndsWith("@"))
        {
            // Special case on@ to send the event globally (the null group) by sending w/o the @.
            return eventName.Substring(0, eventName.Length - 1);
        }
        else if (Group == "" || eventName.Contains("@"))
        {
            // No group was set or already targeting a specific group as is.
            return eventName;
        }
        else
        {
            // Append the group
            return $"{eventName}@{Group}";
        }
    }

    private void SetupSimple()
    {
        __debugInitialized = true;
        __SimpleTag = GetType().Name + " [S:" + Script.ID.ToString() + " O:" + ObjectPrivate.ObjectId.ToString() + "]";
        Wait(TimeSpan.FromSeconds(1));
        IDebugger debugger = ScenePrivate.FindReflective<IDebugger>("SimpleDebugger").FirstOrDefault();
        if (debugger != null) __SimpleDebugging = debugger.DebugSimple;
    }

    System.Collections.Generic.Dictionary<string, Func<string, Action<ScriptEventData>, Action>> __subscribeActions = new System.Collections.Generic.Dictionary<string, Func<string, Action<ScriptEventData>, Action>>();
    private Action SubscribeToAll(string csv, Action<ScriptEventData> callback)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return null;

        Func<string, Action<ScriptEventData>, Action> subscribeAction;
        if (__subscribeActions.TryGetValue(csv, out subscribeAction))
        {
            return subscribeAction(csv, callback);
        }

        // Simple case.
        if (!csv.Contains(">>"))
        {
            __subscribeActions[csv] = SubscribeToAllInternal;
            return SubscribeToAllInternal(csv, callback);
        }

        // Chaining
        __subscribeActions[csv] = (_csv, _callback) =>
        {
            System.Collections.Generic.List<string> chainedCommands = new System.Collections.Generic.List<string>(csv.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries));

            string initial = chainedCommands[0];
            chainedCommands.RemoveAt(0);
            chainedCommands.Add(initial);

            Action unsub = null;
            Action<ScriptEventData> wrappedCallback = null;
            wrappedCallback = (data) =>
            {
                string first = chainedCommands[0];
                chainedCommands.RemoveAt(0);
                chainedCommands.Add(first);
                if (unsub != null) unsub();
                unsub = SubscribeToAllInternal(first, wrappedCallback);
                Log.Write(LogLevel.Info, "CHAIN Subscribing to " + first);
                _callback(data);
            };

            unsub = SubscribeToAllInternal(initial, wrappedCallback);
            return unsub;
        };

        return __subscribeActions[csv](csv, callback);
    }

    private Action SubscribeToAllInternal(string csv, Action<ScriptEventData> callback)
    {
        Action unsubscribes = null;
        string[] events = csv.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (__SimpleDebugging)
        {
            Log.Write(LogLevel.Info, __SimpleTag, "Subscribing to " + events.Length + " events: " + string.Join(", ", events));
        }
        Action<ScriptEventData> wrappedCallback = callback;

        foreach (string eventName in events)
        {
            if (__SimpleDebugging)
            {
                var sub = SubscribeToScriptEvent(GenerateEventName(eventName), (ScriptEventData data) =>
                {
                    Log.Write(LogLevel.Info, __SimpleTag, "Received event " + GenerateEventName(eventName));
                    wrappedCallback(data);
                });
                unsubscribes += sub.Unsubscribe;
            }
            else
            {
                var sub = SubscribeToScriptEvent(GenerateEventName(eventName), wrappedCallback);
                unsubscribes += sub.Unsubscribe;
            }
        }
        return unsubscribes;
    }

    System.Collections.Generic.Dictionary<string, Action<string, Reflective>> __sendActions = new System.Collections.Generic.Dictionary<string, Action<string, Reflective>>();
    private void SendToAll(string csv, Reflective data)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;

        Action<string, Reflective> sendAction;
        if (__sendActions.TryGetValue(csv, out sendAction))
        {
            sendAction(csv, data);
            return;
        }

        // Simple case.
        if (!csv.Contains(">>"))
        {
            __sendActions[csv] = SendToAllInternal;
            SendToAllInternal(csv, data);
            return;
        }

        // Chaining
        System.Collections.Generic.List<string> chainedCommands = new System.Collections.Generic.List<string>(csv.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries));
        __sendActions[csv] = (_csv, _data) =>
        {
            string first = chainedCommands[0];
            chainedCommands.RemoveAt(0);
            chainedCommands.Add(first);

            Log.Write(LogLevel.Info, "CHAIN Sending to " + first);
            SendToAllInternal(first, _data);
        };
        __sendActions[csv](csv, data);
    }

    private void SendToAllInternal(string csv, Reflective data)
    {
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events) + (Group != "" ? (" to group " + Group) : ""));
        foreach (string eventName in events)
        {
            PostScriptEvent(GenerateEventName(eventName), data);
        }
    }
    #endregion

    #endregion

    public override void Init()
    {
        Log.Write("Random Prize  Giver Script Started");
        Script.UnhandledException += UnhandledException;
        LoadPrizes();

        SubscribeToScriptEvent(RandomPrizeEvent, GiveRandomPrize);
        //SubscribeToScriptEvent(NextGameEvent, DisplayRandomPrize);
        //SubscribeToScriptEvent(PrevGameEvent, DisplayRandomPrize);
        //SubscribeToScriptEvent(WinnerEvent, GivePrize);
    }

    public void GiveRandomPrize(ScriptEventData data)
    {
        try
        {
            Log.Write("In GiveRandomPrize");
            PeopleInRange = 0;
            NumberofContestants = 0;
            AgentInDetectionRange = false;
            Contestants.Clear();
            foreach (AgentPrivate agent in ScenePrivate.GetAgents())
            {
                float agentDist = (ScenePrivate.FindObject(agent.AgentInfo.ObjectId).Position - ObjectPrivate.Position).Length();
                //Log.Write("Agent: " + agent.AgentInfo.Name + " agentDist: " + agentDist + " DetectionRange: " + DetectionRange);
                //Add Agent to List of Agents

                if (agentDist <= DetectionRange)
                {
                    Contestants.Add(agent);
                    NumberofContestants++;
                    PeopleInRange++;
                }
            }
            
        }
        catch (Exception ex)
        {
            if (__SimpleDebugging)
            {
                Log.Write(LogLevel.Error, __SimpleTag, "Proximity Detection Failed: " + ex.Message);
            }
        }
        if (PeopleInRange > 0)
        {
            //Randomly Select an agent
            //Log.Write("Contestants in Array: " + Contestants.Count());
            //Log.Write("NumberofContestants: " + NumberofContestants);

            NumberofContestants++;
            int ChosenContestantNumber = r.Next(1, NumberofContestants);
            //Log.Write("ChosenContestantNumber: " + ChosenContestantNumber);
            //Log.Write("Agent: " + Contestants[ChosenContestantNumber - 1]);
            ChosenContestant = Contestants[ChosenContestantNumber - 1];
            GivePrize();
            //Send Message to Message Board
            //DisplayContestant(ChosenContestant.AgentInfo.Name);
        }
    }

/*
    private void DisplayContestant(string inCurrentText)
    {
        string CharToSendOut;

        Log.Write("In Display Clue");
        string CurrentText = "";
        //Log.Write("LineLength: " + inCurrentText[rowCntr].Length);
        CurrentText = inCurrentText;

        //int tempLength = BoardColumns;
        BoardColumns = 25;

        CurrentText = CenterText(CurrentText);
 
        CurrentText = BlackSpacesReplace(CurrentText);

        int textLength = CurrentText.Length;

        //Log.Write("inCurrentText[" + rowCntr + "]: " + inCurrentText + "  Length: " + textLength);
        string MessageEventOut = "Line2";
        Log.Write("MessageEventOut: " + MessageEventOut);
        Log.Write("CurrentText: " + CurrentText);

        for (int i = 0; i < BoardColumns; i++)
        {
            //Log.Write("A");
            SendChar sendCharClr = new SendChar();
            //Log.Write("B");
            sendCharClr.CharIndex = i;
            //Log.Write("CharIndex: " + sendCharClr.CharIndex);
            sendCharClr.CharToSend = " ";
            //Log.Write("SendCharClr: " + sendCharClr.CharToSend);
            PostScriptEvent(ScriptId.AllScripts, MessageEventOut, sendCharClr);
        }

        int columnCntr = 0;
        do
        {
            //Log.Write("columnCntr: " + columnCntr);
            CharToSendOut = CurrentText.Substring(columnCntr, 1);
            //Log.Write("CharToSendOut: " + CharToSendOut);
            SendChar sendChar = new SendChar();
            sendChar.CharIndex = columnCntr;
            sendChar.CharToSend = CharToSendOut;
            //Log.Write("MessageEvent: " + MessageEvent + " Sending Letter: " + CharToSendOut + " To Letter #: " + sendChar.CharIndex);
            PostScriptEvent(ScriptId.AllScripts, MessageEventOut, sendChar);
            columnCntr++;
        } while (columnCntr < textLength);

    }

    private string CenterText(string textToCenter)
    {

        //Log.Write("textToCenter: " + textToCenter);

        int indent = (BoardColumns - textToCenter.Length) / 2;
        string indentSpace = "";
        //Log.Write("indent: " + indent);

        for (int i = 0; i < indent; i++)
        {
            indentSpace = indentSpace + " ";
        }

        //Log.Write("indentSpace Length: " + indentSpace.Length);

        string centeredText = indentSpace + textToCenter;
        //Log.Write("centeredText1: " + centeredText);

        //Fill the back with blanks
        string trailerSpace = "";
        int trailerCount = BoardColumns - centeredText.Length;
        //Log.Write("trailerCount: " + trailerCount);

        for (int i = 0; i < trailerCount; i++)
        {
            trailerSpace = trailerSpace + " ";
        }
        centeredText = centeredText + trailerSpace;
        //Log.Write("centeredText: " + centeredText);
        //Log.Write("centeredText Length: " + centeredText.Length);

        return centeredText;
    }

    private string BlackSpacesReplace(string inString)
    {
        string fixedString = "";
        Char Blank = ' ';
        Char BlackChar = '*';
        fixedString = inString.Replace(Blank, BlackChar);

        return fixedString;
    }
*/

    private void LoadPrizes()
    {
        int loopCntr = 1;
        Log.Write("Prizes1To20.Count: " + Prizes1To20.Count());
        if (Prizes1To20.Count() > 0)
        {
            do
            {
                AllPrizes.Add(Prizes1To20[loopCntr]);
                loopCntr++;
            } while (loopCntr < Prizes1To20.Count());
            NumberofPrizes = loopCntr;
        }
        if (Prizes21To30.Count() > 0)
        {
            loopCntr = 0;
            do
            {
                AllPrizes.Add(Prizes1To20[loopCntr]);
                loopCntr++;
            } while (loopCntr < Prizes21To30.Count());
            NumberofPrizes = NumberofPrizes + loopCntr;
        }
    }

    /*
    private void DisplayRandomPrize(ScriptEventData data)
    {
        ISimpleData sdIn = data.Data?.AsInterface<ISimpleData>();

        PrizeNumber = r.Next(1, NumberofPrizes);
        Log.Write("PrizeNumber: " + PrizeNumber);
        //Display Prize
        SimpleData sd = new SimpleData(this);
        sd.SourceObjectId = ObjectPrivate.ObjectId;
        sd.AgentInfo = sdIn.AgentInfo; //ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        //sd.ObjectId = sd.AgentInfo != null ? sd.AgentInfo.ObjectId : ObjectId.Invalid;
        SendToAll(PrizeEventName+PrizeNumber, sd); 
    }
    */

    private void GivePrize()
    {
        Log.Write("In GivePrize");

        //Log.Write("sendBuyInfo.HitmanIn: " + sendBuyInfo.HitmanIn + " ItemChosen: " + ItemChosen + " ProductID: " + AllPrizes[ItemChosen]);
        Hitman = ChosenContestant;
        Log.Write("Hitman: " + Hitman);
        string ProductId;

        PrizeNumber = r.Next(1, NumberofPrizes);
        Log.Write("PrizeNumber: " + PrizeNumber + " AllPrizes.Count: " + AllPrizes.Count()); 
        ProductId = AllPrizes[PrizeNumber-1];

        Log.Write("Give Prize To: " + Hitman.AgentInfo.Name + " Prize: " + ProductId);

        if (!Guid.TryParse(ProductId, out ProductGuid))
        {
            bool foundId = false;
            // Find the ID from the store listing url. Very generic, will just find the first url segment or query arg it can convert to a UUID.
            // https://store.sansar.com/listings/9eb72eb2-38c1-4cd3-a9eb-360e2f19e403/female-pirate-hat-r3d
            string[] segments = ProductId.Split(new string[] { "/", "?", "&", "=" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string segment in segments)
            {
                if (segment.Length >= 32
                    && Guid.TryParse(segment, out ProductGuid))
                {
                    foundId = true;
                    break;
                }
            }

            if (!foundId)
            {
                Log.Write("Not Found in Store");
            }
            else
            {
            }
        }

        Log.Write("ProductID: " + ProductId);
        Log.Write("Product GUID: " + ProductGuid);
        Hitman.Client.OpenStoreListing(ProductGuid);
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

}