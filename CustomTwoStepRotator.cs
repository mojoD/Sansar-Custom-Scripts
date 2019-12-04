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

public class CustomTwoStepRotator : SceneObjectScript
{

    //Script uses ObjectPrivate.Mover .... Moves from current position to a new relative position.  Triggered via simple messages.

    #region ConstantsVariables
    // Public properties

    //[DisplayName("Rotation Offset")]
    //public readonly Vector RotationOffset;

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
    //private Vector NewKeyRot;
    private string KeyIn = null;

    public interface ISendKeyInfo
    {
        string iChannelOut { get; }
        string iKeySent { get; }
    }

    #endregion


    // Logic!

    private void getKeyInfo(ScriptEventData gotKeyInfo)
    {

        //Log.Write("ActionSynthModule: In getKeyInfo");
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
        //Log.Write("KeyIn before Trim: " + KeyIn);

        if (KeyIn.Contains("U"))
        {
            //Log.Write("KeyUp: " + KeyIn);
            ObjectPrivate.Mover.AddRotate(initialRotation, Seconds, moveMode);

        }
        else
        {
            ObjectPrivate.Mover.AddRotate(fixedRotation, Seconds, moveMode);
        }
    }

    public override void Init()
    {
        initialRotation = ObjectPrivate.Rotation;
        
        //Log.Write("initialRotation: " + initialRotation);
        //Log.Write("Euler: " + ObjectPrivate.Rotation.GetEulerAngles().Z / 0.01745329);
        //float Zrot = initialRotation.Z;

        float Zrot = Convert.ToSingle(ObjectPrivate.Rotation.GetEulerAngles().Z / 0.01745329);
        //Log.Write("Zrot: " + Zrot);
        if (Zrot < 0)
        {
            Zrot = Zrot + 360;
        }
        //Log.Write("Zrot normalized: " + Zrot);
        //NewRot = <0.0, 0.0, 0.0>;  //RotationOffset;
        //Log.Write("NewRot Pre: " + NewRot);
        
        if ((Zrot >= 0) && (Zrot <= 90)) NewRot.X = -(90-Zrot) / 9;
        else if ((Zrot >= 90) && (Zrot <= 180)) NewRot.X = -(90-Zrot) / 9;
        else if ((Zrot >= 180) && (Zrot <= 270)) NewRot.X = (270-Zrot) / 9;
        else if ((Zrot >= 270) && (Zrot <= 360)) NewRot.X = (270-Zrot) / 9;
        Log.Write("NewRot.X: " + NewRot.X);
 
        if ((Zrot >= 0) && (Zrot <= 90)) NewRot.Y = -Zrot / 9;
        else if ((Zrot >= 90) && (Zrot <= 180)) NewRot.Y = (Zrot-180) / 9;
        else if ((Zrot >= 180) && (Zrot <= 270)) NewRot.Y = (Zrot-180) / 9;
        else if ((Zrot >= 270) && (Zrot <= 360)) NewRot.Y = (360-Zrot) / 9;
        //Log.Write("NewRot.Y: " + NewRot.Y);

        //Log.Write("Rotation Offset Angles: " + RotationOffset);
        NewRot.X = NewRot.X * 0.0174533f;
        NewRot.Y = NewRot.Y * 0.0174533f;
        NewRot.Z = 0 * 0.0174533f;
        //Log.Write("NewRot: " + NewRot);

        fixedRotation = Quaternion.FromEulerAngles(NewRot) * initialRotation;
        //Log.Write("fixedRotation: " + fixedRotation);

        if (StartMoved) ObjectPrivate.Mover.AddRotate(fixedRotation, Seconds, moveMode);

        if (OnEvent.Length > 0)
        {
            List<string> OnArray = new List<string>();
            OnArray.Clear();
            OnEvent.Replace(" ", string.Empty);
            OnArray = OnEvent.Split(',').ToList();
            int i = 0;
            do
            {
                SubscribeToScriptEvent(OnArray[i], getKeyInfo);
                //Log.Write("Subscribing to " + OnArray[i]);
                i++;
            } while (i < OnArray.Count());
        }

        if (OffEvent.Length> 0)
        {
            List<string> OffArray = new List<string>();
            OffArray.Clear();
            OffEvent.Replace(" ", string.Empty);
            OffArray = OffEvent.Split(',').ToList();
            int i = 0;
            do
            {
                SubscribeToScriptEvent(OffArray[i], getKeyInfo);
                //Log.Write("Subscribing to " + OffArray[i]);
                i++;
            } while (i < OffArray.Count());
        }

        //Log.Write("Past Init");
    }

}
