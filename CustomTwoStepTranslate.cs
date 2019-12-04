/* This content is licensed under the terms of the Creative Commons Attribution 4.0 International License.
 * When using this content, you must:
 * •    Acknowledge that the content is from the Sansar Knowledge Base.
 * •    Include our copyright notice: "© 2018 Linden Research, Inc."
 * •    Indicate that the content is licensed under the Creative Commons Attribution-Share Alike 4.0 International License.
 * •    Include the URL for, or link to, the license summary at https://creativecommons.org/licenses/by-sa/4.0/deed.hi (and, if possible, to the complete license terms at https://creativecommons.org/licenses/by-sa/4.0/legalcode.
 * For example:
 * "This work uses content from the Sansar Knowledge Base. © 2018 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."
 */
using Sansar;
using Sansar.Script;
using Sansar.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

public class CustomTwoStepTranslate : SceneObjectScript
{

    //Script uses ObjectPrivate.Mover .... Moves from current position to a new relative position.  Triggered via simple messages.

    #region ConstantsVariables
    // Public properties

    // Offset from base position, in objects local space
    [DisplayName("Position Offset")]
    public readonly Vector PositionOffset;

    // On hearing this event execute move
    [DisplayName("On Event")]
    public readonly string OnEvent;

    // On hearing this event execute return move
    [DisplayName("Off Event")]
    public readonly string OffEvent;

    [DefaultValue(2.0f)]
    public double Seconds;

    public MoveMode moveMode = MoveMode.Smoothstep;

    public bool UpdateFromMovePosition = false;

    public bool StartMoved = false;

    private Quaternion initialRotation;
    private Quaternion fixedRotation;
    private Vector NewRot;
    private string KeyIn = null;

    private Vector initialPosition;
    private Vector fixedPosition;

    //public class SendKeyInfo : Reflective
    //{
    //    public string iChannelOut { get; set; }
    //    public string iKeySent { get; set; }
    //}

    public interface ISendKeyInfo
    {
        string iChannelOut { get; }
        string iKeySent { get; }
    }

    #endregion


    // Logic!

    private void getKeyInfo(ScriptEventData gotKeyInfo)
    {

        Log.Write("In getKeyInfo");
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

        KeyIn = sendKeyInfo.iKeySent;
        Log.Write("KeyIn before Trim: " + KeyIn);

        if (KeyIn.Contains("U"))
        {
            Log.Write("KeyUp: " + KeyIn);
            //Log.Write("FixedPosition: " + fixedPosition);
            //Log.Write("Seconds: " + Seconds);
            //Log.Write("moveMode: " + moveMode);
            //ObjectPrivate.Mover.AddRotate(initialRotation, Seconds, moveMode);
            ObjectPrivate.Mover.AddTranslate(initialPosition, Seconds, moveMode);
        }
        else
        {
            Log.Write("FixedPosition: " + fixedPosition);
            Log.Write("Seconds: " + Seconds);
            Log.Write("moveMode: " + moveMode);
            //ObjectPrivate.Mover.AddRotate(fixedRotation, Seconds, moveMode);
            ObjectPrivate.Mover.AddTranslate(fixedPosition, Seconds, moveMode);
            Log.Write("After Move");
            //else
            //{
            //    WaitFor(ObjectPrivate.Mover.AddRotate, fixedRotation, Seconds, moveMode);
            //    ObjectPrivate.Mover.AddRotate(initialRotation, Seconds, moveMode);
            //}
        }
    }

    public override void Init()
    {
        initialPosition = ObjectPrivate.Position;
        fixedPosition = initialPosition + PositionOffset;
        if (OnEvent.Length > 0)
        {
            List<string> OnArray = new List<string>();
            OnArray.Clear();
            OnEvent.Replace(" ", string.Empty);
            OnArray = OnEvent.Split(',').ToList();
            int i = 0;
            Log.Write("OnArray Count: " + OnArray.Count());
            do
            {
                Log.Write("Subscribing To: " + OnArray[i]);
                SubscribeToScriptEvent(OnArray[i], getKeyInfo);
                i++;
            } while (i < OnArray.Count());
        }

        if (OffEvent.Length > 0)
        {
            List<string> OffArray = new List<string>();
            OffArray.Clear();
            OffEvent.Replace(" ", string.Empty);
            OffArray = OffEvent.Split(',').ToList();
            int i = 0;
            do
            {
                SubscribeToScriptEvent(OffArray[i], getKeyInfo);
                i++;
            } while (i < OffArray.Count());
        }
        //SubscribeToAll(OffEvent, OffEventExecute);


        Log.Write("Past Init");
    }

}
