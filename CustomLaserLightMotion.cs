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

//
//   This script controls if the Laser Spins and Pans 
//

public class CustomLaserLightMotion : ObjectScript
{
    #region ConstantsVariables
    // Public properties

    [Tooltip("Laser ID for Programs")]
    [DisplayName("Laser ID")]
    [DefaultValue("Laser1")]
    public readonly string LaserID;

    [DisplayName("Rotation Offset")]
    public readonly Vector RotationOffsetIn;

   // [Tooltip("Turns per second")]
   // [DisplayName("Spin Speed")]
   // [DefaultValue(1.0f)]
   // public float SpinSpeedIn;

    [DisplayName("Pan Angles")]
    public readonly Vector PanAngles;

    [Tooltip("Pan/Spin per second")]
    [DisplayName("Pan/Spin Speed")]
    [DefaultValue(1.0f)]
    public float PanSpeedIn;

    public List<Vector> Rotations = new List<Vector>();

    [Tooltip("Angle Change Speed")]
    [DisplayName("Angle Change Speed")]
    [DefaultValue(0.0f)]
    public float AngleChangSpeedIn;

    //private Quaternion initialRotation;
    //private Quaternion currentRotation;
    private Vector currentRotationOffset;

    //private float SpinSpeed;
    private float PanSpeed;
    private float currentAngleChangeSpeed;
    private float currentSpinRate = 1.0f;
    private float currentPanRate = 1.0f;
    private float SpinAngle;
    private float currentSpinAngle;

    private float currentXPanAngle;
    private float currentYPanAngle;
    private float currentZPanAngle;

    private bool PanSpin = false;
    private bool NoSpin = false;
    private bool FullSpin = false;

    private Quaternion RotationChange;
    private int TargetWorldRotation = 0;
    private Quaternion TargetRotationChange;
    private float pctLoopsCompleteRotation;

    private string Change;
    #endregion

    #region Communication

    public interface IFullSpinInfo
    {
        string LaserID { get; }
        string inFullSpin { get; }
    }

    private void getSpinType(ScriptEventData gotSpinInfo)
    {

        //Log.Write("In getSpinInfo gotSpinInfo.Data: " + gotSpinInfo.Data);
        if (gotSpinInfo.Data == null)
        {
            return;
        }

        IFullSpinInfo sendFullSpinInfo = gotSpinInfo.Data.AsInterface<IFullSpinInfo>();
        if (sendFullSpinInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        //Log.Write("inFullSpin: " + sendFullSpinInfo.inFullSpin);

        if ((sendFullSpinInfo.LaserID == LaserID) || (sendFullSpinInfo.LaserID == "ALL"))
        {
            if (sendFullSpinInfo.inFullSpin == "FullSpin")
            {
                FullSpin = true;
                NoSpin = false;
                PanSpin = false;
                FullSpinning();
            }
            else if (sendFullSpinInfo.inFullSpin == "PanSpin")
            {
                FullSpin = false;
                NoSpin = false;
                PanSpin = true;
                PanSpinning();
            }
            else if (sendFullSpinInfo.inFullSpin == "NoSpin")
            {
                FullSpin = false;
                NoSpin = true;
                PanSpin = false;
                NoSpinning();
            }
        }

        //Log.Write("FullSpin: " + FullSpin + " PanSpin: " + PanSpin + " NoSpin: " + NoSpin);
    }
/*
    public interface ISpinRateInfo
    {
        string LaserID { get; }
        float inSpinRate { get; }
    }

    private void getSpinRate(ScriptEventData gotSpinRate)
    {

        //Log.Write("In getSpinRate gotSpinRate.Data: " + gotSpinRate.Data);
        if (gotSpinRate.Data == null)
        {
            return;
        }

        ISpinRateInfo sendSpinRate = gotSpinRate.Data.AsInterface<ISpinRateInfo>();
        if (sendSpinRate == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        if ((sendSpinRate.LaserID == LaserID) || (sendSpinRate.LaserID == "ALL"))
        {
            //Log.Write("inSpinRate: " + sendSpinRate.inSpinRate);
            currentSpinRate = currentSpinRate + sendSpinRate.inSpinRate;
            SpinSpeed = SpinSpeedIn * currentSpinRate;
        }

        Log.Write("SpinSpeed: " + SpinSpeed);
    }

    public interface ISpinAngleInfo
    {
        string LaserID { get; }
        float inSpinAngle { get; }
    }

    private void getSpinAngle(ScriptEventData gotSpinAngle)
    {

        //Log.Write("In getSpinAngle gotSpinAngle.Data: " + gotSpinAngle.Data);
        if (gotSpinAngle.Data == null)
        {
            return;
        }

        ISpinAngleInfo sendSpinAngle = gotSpinAngle.Data.AsInterface<ISpinAngleInfo>();
        if (sendSpinAngle == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendSpinAngle.LaserID == LaserID) || (sendSpinAngle.LaserID == "ALL"))
        {
            //Log.Write("inSpinAngle: " + sendSpinAngle.inSpinAngle);
            SpinAngle = currentSpinAngle + sendSpinAngle.inSpinAngle;
            currentSpinAngle = SpinAngle;
        }
        //Log.Write("CurrentSpinAngle: " + currentSpinAngle);
    }
*/

    public interface IPanRateInfo
    {
        string LaserID { get; }
        float inPanRate { get; }
    }

    private void getPanRate(ScriptEventData gotPanRate)
    {

        //Log.Write("In getPanRate gotPanRate.Data: " + gotPanRate.Data);
        if (gotPanRate.Data == null)
        {
            return;
        }

        IPanRateInfo sendPanRate = gotPanRate.Data.AsInterface<IPanRateInfo>();
        if (sendPanRate == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        //Log.Write("inPanRate: " + sendPanRate.inPanRate + " currentPanRate: " + currentPanRate);

        if ((sendPanRate.LaserID == LaserID) || (sendPanRate.LaserID == "ALL"))
        {
            /*
            if (currentPanRate > 0.1)
            {
                currentPanRate = currentPanRate + sendPanRate.inPanRate;
                PanSpeed = PanSpeedIn * currentPanRate;

            }
            else if (currentPanRate <= 0.1)
            {
                if (currentPanRate > 0.0)
                {
                    currentPanRate = currentPanRate + sendPanRate.inPanRate * 0.1f;
                    PanSpeed = PanSpeedIn * currentPanRate;
                }
            }
            else Log.Write("Can't go any faster");
            */
            PanSpeed = sendPanRate.inPanRate;
        }

        //Log.Write("PanSpeed: " + PanSpeed);
    }

    public interface IPanAngleInfo
    {
        string LaserID { get; }
        string PanAxis { get; } 
        float PanAngle { get; }
    }

    private void getPanAngle(ScriptEventData gotPanAngle)
    {

        //Log.Write("In getPanAngle gotPanAngle.Data: " + gotPanAngle.Data);
        if (gotPanAngle.Data == null)
        {
            return;
        }

        IPanAngleInfo sendPanAngle = gotPanAngle.Data.AsInterface<IPanAngleInfo>();
        if (sendPanAngle == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        //Log.Write("PanAxis: " + sendPanAngle.PanAxis +" PanAngle: " + sendPanAngle.PanAngle);

        if ((sendPanAngle.LaserID == LaserID) || (sendPanAngle.LaserID == "ALL"))
        {
            if (sendPanAngle.PanAxis == "X")
            {
                //currentXPanAngle = currentXPanAngle + sendPanAngle.PanAngle;
                currentXPanAngle = sendPanAngle.PanAngle;
                //Log.Write("CurrentXPanAngle: " + currentXPanAngle);
            }
            else if (sendPanAngle.PanAxis == "Y")
            {
                //currentYPanAngle = currentYPanAngle + sendPanAngle.PanAngle;
                currentYPanAngle = sendPanAngle.PanAngle;
                //Log.Write("CurrentYPanAngle: " + currentYPanAngle);
            }
            else if (sendPanAngle.PanAxis == "Z")
            {
                //currentZPanAngle = currentZPanAngle + sendPanAngle.PanAngle;
                currentZPanAngle = sendPanAngle.PanAngle;
                //Log.Write("CurrentZPanAngle: " + currentZPanAngle);
            }
        }
    }

    public interface IWorldRotInfo
    {
        string LaserID { get; }
        int WorldRotation { get; }
        int TargetWorldRotation { get; }
        float PctLoopComplete { get; }
    }

    private void getWorldRotInfo(ScriptEventData gotWorldRotInfo)
    {

        //Log.Write("In getWorldRotInfo gotWorldRotInfo.Data: " + gotWorldRotInfo.Data);
        if (gotWorldRotInfo.Data == null)
        {
            return;
        }

        IWorldRotInfo sendWorldRot = gotWorldRotInfo.Data.AsInterface<IWorldRotInfo>();
        if (sendWorldRot == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        //Log.Write("Original: " + Rotations[sendWorldRot.WorldRotation - 1] + " Target: " + Rotations[sendWorldRot.TargetWorldRotation - 1] + " PctLoopComplete: " + sendWorldRot.PctLoopComplete);
        //Log.Write("newX: " + newRotationAngleX + " newY: " + newRotationAngleY + " newZ: " + newRotationAngleZ);
        if ((sendWorldRot.LaserID == LaserID) || (sendWorldRot.LaserID == "ALL"))
        {
            float newRotationAngleX;
            float newRotationAngleY;
            float newRotationAngleZ;

            if (Rotations[sendWorldRot.WorldRotation - 1].X > Rotations[sendWorldRot.TargetWorldRotation - 1].X)
            {
                newRotationAngleX = Rotations[sendWorldRot.WorldRotation - 1].X - ((Rotations[sendWorldRot.WorldRotation - 1].X - Rotations[sendWorldRot.TargetWorldRotation - 1].X) * sendWorldRot.PctLoopComplete);
            }
            else
            {
                newRotationAngleX = Rotations[sendWorldRot.WorldRotation - 1].X + ((Rotations[sendWorldRot.WorldRotation - 1].X - Rotations[sendWorldRot.TargetWorldRotation - 1].X) * sendWorldRot.PctLoopComplete);
            }
            if (Rotations[sendWorldRot.WorldRotation - 1].Y > Rotations[sendWorldRot.TargetWorldRotation - 1].Y)
            {
                newRotationAngleY = Rotations[sendWorldRot.WorldRotation - 1].Y - ((Rotations[sendWorldRot.WorldRotation - 1].Y - Rotations[sendWorldRot.TargetWorldRotation - 1].Y) * sendWorldRot.PctLoopComplete);
            }
            else
            {
                newRotationAngleY = Rotations[sendWorldRot.WorldRotation - 1].Y + ((Rotations[sendWorldRot.WorldRotation - 1].Y - Rotations[sendWorldRot.TargetWorldRotation - 1].Y) * sendWorldRot.PctLoopComplete);
            }
            if (Rotations[sendWorldRot.WorldRotation - 1].Z > Rotations[sendWorldRot.TargetWorldRotation - 1].Z)
            {
                newRotationAngleZ = Rotations[sendWorldRot.WorldRotation - 1].Z - ((Rotations[sendWorldRot.WorldRotation - 1].Z - Rotations[sendWorldRot.TargetWorldRotation - 1].Z) * sendWorldRot.PctLoopComplete);
            }
            else
            {
                newRotationAngleZ = Rotations[sendWorldRot.WorldRotation - 1].Z + ((Rotations[sendWorldRot.WorldRotation - 1].Z - Rotations[sendWorldRot.TargetWorldRotation - 1].Z) * sendWorldRot.PctLoopComplete);
            }

            if (sendWorldRot.WorldRotation <= Rotations.Count)
            {
                Quaternion Xrotation = Quaternion.FromAngleAxis(newRotationAngleX * 0.0174533f, Vector.ObjectRight);
                Quaternion Yrotation = Quaternion.FromAngleAxis(newRotationAngleY * 0.0174533f, Vector.ObjectForward);
                Quaternion Zrotation = Quaternion.FromAngleAxis(newRotationAngleZ * 0.0174533f, Vector.ObjectUp);
                RotationChange = Xrotation * Yrotation * Zrotation;
                //Log.Write("new RotationChange: " + RotationChange);
            }
        }
    }

    public interface IAngleChangeInfo
    {
        string LaserID { get; }
        float inAngleChangeSpeed { get; }
        int inTargetWorldRotation { get; }
    }

    private void getAngleChangeSpeed(ScriptEventData gotAngleChangeSpeed)
    {

        //Log.Write("In getAngleChangeSpeed gotangleChangeSpeed.Data: " + gotAngleChangeSpeed.Data);
        if (gotAngleChangeSpeed.Data == null)
        {
            return;
        }

        IAngleChangeInfo sendAngleChangeSpeed = gotAngleChangeSpeed.Data.AsInterface<IAngleChangeInfo>();
        if (sendAngleChangeSpeed == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        //Log.Write("inAngleChangeSpeeed: " + sendAngleChangeSpeed.inAngleChangeSpeed);

        if ((sendAngleChangeSpeed.LaserID == LaserID) || (sendAngleChangeSpeed.LaserID == "ALL"))
        {
            currentAngleChangeSpeed = sendAngleChangeSpeed.inAngleChangeSpeed;
            if (sendAngleChangeSpeed.inTargetWorldRotation <= Rotations.Count)
            {
                Quaternion Xrotation = Quaternion.FromAngleAxis(Rotations[sendAngleChangeSpeed.inTargetWorldRotation - 1].X * 0.0174533f, Vector.ObjectRight);
                Quaternion Yrotation = Quaternion.FromAngleAxis(Rotations[sendAngleChangeSpeed.inTargetWorldRotation - 1].Y * 0.0174533f, Vector.ObjectForward);
                Quaternion Zrotation = Quaternion.FromAngleAxis(Rotations[sendAngleChangeSpeed.inTargetWorldRotation - 1].Z * 0.0174533f, Vector.ObjectUp);
                TargetRotationChange = Xrotation * Yrotation * Zrotation;
                //Log.Write("new TargetRotationChange: " + TargetRotationChange);
            }
        }

        //Log.Write("currentAngleChangeSpeed: " + currentAngleChangeSpeed + " TargetRotationChange: " + TargetRotationChange);
    }

    #endregion

    public override void Init()
    {
        currentAngleChangeSpeed = AngleChangSpeedIn;
        PanSpeed = PanSpeedIn;
        currentSpinAngle = RotationOffsetIn.Y;
        SpinAngle = currentSpinAngle;
        currentXPanAngle = PanAngles.X;
        currentYPanAngle = PanAngles.Y;
        currentZPanAngle = PanAngles.Z;
        Vector ZeroVector = new Vector(0.0f, 0.0f, 0.0f);
        RotationChange = Quaternion.FromEulerAngles(ZeroVector);

        if (!ObjectPrivate.IsMovable)
        {
            Log.Write($"MoverExample2 script can't move {ObjectPrivate.Name} because either the 'Movable from Script' flag was not set or the object does not have 'Keyframed' physics!");
            return;
        }

        SubscribeToScriptEvent("SpinTypeMsg", getSpinType);
        //SubscribeToScriptEvent("SpinRateMsg", getSpinRate);
        //SubscribeToScriptEvent("SpinAngleMsg", getSpinAngle);
        SubscribeToScriptEvent("PanAngleMsg", getPanAngle);
        SubscribeToScriptEvent("PanRateMsg", getPanRate);
        SubscribeToScriptEvent("ChangeWorldRot", getWorldRotInfo);
        SubscribeToScriptEvent("ChangeAngleSpeed", getAngleChangeSpeed);
    }

    private void PanSpinning()
    {
        // Use this to keep track of the spin direction, and to choose whether to start in one direction or the other based on the sign of the spin speed
        float nextSpinSign = Math.Sign(PanSpeed);
        // Compute a quaternion to rotate the object around the up axis by the specified angle

        do
        {

            // Calculate the time for this rotation based on the specified spin speed

            double timeForRotation = PanSpeed;

            Quaternion Xrotation = Quaternion.FromAngleAxis(nextSpinSign * currentXPanAngle * 0.0174533f, Vector.ObjectRight);
            Quaternion Yrotation = Quaternion.FromAngleAxis(nextSpinSign * currentYPanAngle * 0.0174533f, Vector.ObjectForward);
            Quaternion Zrotation = Quaternion.FromAngleAxis(nextSpinSign * currentZPanAngle * 0.0174533f, Vector.ObjectUp);

            // Do the actual rotation, and wait for it to complete
            WaitFor(ObjectPrivate.Mover.AddRotate, Xrotation * Yrotation * Zrotation * RotationChange, timeForRotation, MoveMode.Smoothstep);

            nextSpinSign *= -1.0f;
        } while (PanSpin);
    }

    private void FullSpinning()
    {
        //Quaternion Xangle = Quaternion.FromAngleAxis(SpinAngle * 0.0174533f, Vector.ObjectRight * RotationChange;
        //Quaternion Yangle = Quaternion.FromAngleAxis(SpinAngle * 0.0174533f, Vector.ObjectForward) * RotationChange;
        //Quaternion Zangle = Quaternion.FromAngleAxis(SpinAngle * 0.0174533f, Vector.ObjectUp) * RotationChange;
        //Log.Write("RotationChange: " + RotationChange);
        //Log.Write("TargetRotationChange: " + TargetRotationChange);

        Quaternion calculatedRotationChange;
        calculatedRotationChange = RotationChange;

        do
        {
            // Calculate time for one rotation
            float timeForOneRotation = 1.0f / PanSpeed;

            //Log.Write("Spinning SpinAngle: " + SpinAngle);
            //Log.Write("Spin Speed: " + SpinSpeed + " Spin Rate: " + timeForOneRotation);
            //ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 1.5f / 6.0f, Vector.ObjectUp) * Yangle, timeForOneRotation * 0.25, MoveMode.Linear);
            //ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 3.0f / 6.0f, Vector.ObjectUp) * Yangle, timeForOneRotation * 0.25, MoveMode.Linear);
            //ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 4.5f / 6.0f, Vector.ObjectUp) * Yangle, timeForOneRotation * 0.25, MoveMode.Linear);
            //WaitFor(ObjectPrivate.Mover.AddRotate,      Quaternion.FromAngleAxis(Mathf.TwoPi, Vector.ObjectUp) * Yangle, timeForOneRotation * 0.25, MoveMode.Linear);
            if (RotationChange.Y != 0.0f)
            {
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 1.5f / 6.0f, Vector.ObjectUp) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 3.0f / 6.0f, Vector.ObjectUp) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 4.5f / 6.0f, Vector.ObjectUp) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                WaitFor(ObjectPrivate.Mover.AddRotate, Quaternion.FromAngleAxis(Mathf.TwoPi, Vector.ObjectUp) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
            }
            else if (RotationChange.Z != 0.0f)
            {
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 1.5f / 6.0f, Vector.ObjectForward) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 3.0f / 6.0f, Vector.ObjectForward) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 4.5f / 6.0f, Vector.ObjectForward) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                WaitFor(ObjectPrivate.Mover.AddRotate, Quaternion.FromAngleAxis(Mathf.TwoPi, Vector.ObjectForward) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
            }
            else if (RotationChange.X != 0.0f)
            {
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 1.5f / 6.0f, Vector.ObjectRight) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 3.0f / 6.0f, Vector.ObjectRight) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                ObjectPrivate.Mover.AddRotate(Quaternion.FromAngleAxis(Mathf.TwoPi * 4.5f / 6.0f, Vector.ObjectRight) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
                WaitFor(ObjectPrivate.Mover.AddRotate, Quaternion.FromAngleAxis(Mathf.TwoPi, Vector.ObjectRight) * RotationChange, timeForOneRotation * 0.25, MoveMode.Linear);
            }
            //Log.Write("FullSpin: " + FullSpin + " PanSpin: " + PanSpin + " SpinRate: " + SpinSpeed);
        } while (FullSpin);

        //Log.Write("Out of Spin Loop");
        
    }

    void NoSpinning()
    {
        //Log.Write("No Spinning");
        /*
        float amt = 10.0f;
        int i = 0;
        do
        {
            WaitFor(ObjectPrivate.Mover.AddRotate, Quaternion.FromAngleAxis(amt* 0.0174533f, Vector.ObjectForward), 1.0, MoveMode.Linear);
            i++;
            amt = i * 10;
            Log.Write("i: " + i);
            Wait(TimeSpan.FromSeconds(1));
        } while (i < 18) ;
       */
    }
}
