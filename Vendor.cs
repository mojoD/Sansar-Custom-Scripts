//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

public class Vendor : SceneObjectScript

{
    //public string PrizeCmd;
    public List<string> Prizes1To20 = new List<string>();
    public List<string> Prizes21To30 = new List<string>();

    private int NumberofPrizes = 0;
    private int ItemChosen = 0;
    private AgentPrivate Hitman;

    //private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    //ObjectId hitter;
    Guid ProductGuid;

    private List<string> AllPrizes = new List<string>();

    #region Communication
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


    #endregion

    public override void Init()
    {
        Log.Write("Prize  Giver Script Started");
        Script.UnhandledException += UnhandledException;
        LoadPrizes();
        SubscribeToScriptEvent("ItemChosen", getItemChosen);
        SubscribeToScriptEvent("BuyInfo", getBuyInfo);
    }

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

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

}