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
using System.Collections.Generic;
using System;

//
//   This script controls the Laser Position and Rotation.
//

public class CustomLaserLightPosition : ObjectScript
{
    // Public properties

    [Tooltip("Laser ID for Programs")]
    [DisplayName("Laser ID")]
    [DefaultValue("Laser1")]
    public readonly string LaserID;

    [Tooltip("X, Y and Z Offsets for Relative Moves")]
    [DisplayName("Position Offset")]
    public readonly Vector PositionOffsetIn;

    [Tooltip("Position Transition Speed")]
    [DisplayName("Position Change Seconds")]
    [DefaultValue(1.0)]
    public double PositionSpeedIn;

    public List<Vector> Positions = new List<Vector>();

    [Tooltip("Rotation Transition Speed")]
    [DisplayName("Rotation Change Seconds")]
    [DefaultValue(0.1)]
    public double RotationSpeedIn;

    public List<Vector> StillRotations = new List<Vector>();

    private Vector currentPosition;
    private Quaternion currentRotation;

    private string Change;

    #region Communication

    public interface IRelPosInfo
    {
        string LaserID { get; }
        string PositionAxis { get;  }
        string Direction { get; }
    }

    private void getRelPosInfo(ScriptEventData gotRelPosInfo)
    {

        //Log.Write("In getRelPosInfo gotRelPosInfo.Data: " + gotRelPosInfo.Data);
        if (gotRelPosInfo.Data == null)
        {
            return;
        }

        IRelPosInfo sendRelPos = gotRelPosInfo.Data.AsInterface < IRelPosInfo>();
        if (sendRelPos == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendRelPos.LaserID == LaserID) || (sendRelPos.LaserID == "ALL"))
        {
            Vector NewPosition = currentPosition;

            if (sendRelPos.PositionAxis == "X")
                if (sendRelPos.Direction == "+")
                {
                    NewPosition.X = currentPosition.X + PositionOffsetIn.X;
                }
                else if (sendRelPos.Direction == "-")
                {
                    NewPosition.X = currentPosition.X - PositionOffsetIn.X;
                }
                else
                {
                    Log.Write("Direction is incorrect");
                }
            else if (sendRelPos.PositionAxis == "Y")
                if (sendRelPos.Direction == "+")
                {
                    NewPosition.Y = currentPosition.Y + PositionOffsetIn.Y;
                }
                else if (sendRelPos.Direction == "-")
                {
                    NewPosition.Y = currentPosition.Y - PositionOffsetIn.Y;
                }
                else
                {
                    Log.Write("Direction is incorrect");
                }
            else if (sendRelPos.PositionAxis == "Z")
                if (sendRelPos.Direction == "+")
                {
                    NewPosition.Z = currentPosition.Z + PositionOffsetIn.Z;
                }
                else if (sendRelPos.Direction == "-")
                {
                    NewPosition.Z = currentPosition.Z - PositionOffsetIn.Z;
                }
                else
                {
                    Log.Write("Direction is incorrect");
                }

            currentPosition = NewPosition;

            //Log.Write("currentPosition: " + currentPosition + " NewPosition: " + NewPosition);
            //Log.Write("PositionAxis: " + sendRelPos.PositionAxis + " Direction: " + sendRelPos.Direction);

            //WaitFor(ObjectPrivate.Mover.AddTranslate, NewPosition, PositionSpeedIn, MoveMode.Smoothstep);
            ObjectPrivate.Mover.AddTranslate(NewPosition, PositionSpeedIn, MoveMode.Smoothstep);
        }
    }

    public interface IPosChangeRateInfo
    {
        string LaserID { get; }
        double PositionChangeRate { get; }
    }

    private void getPosChangeRateInfo(ScriptEventData gotPosChangeRateInfo)
    {

        //Log.Write("In getPosChangeRateInfo gotPosChangeRateInfo.Data: " + gotPosChangeInfo.Data);
        if (gotPosChangeRateInfo.Data == null)
        {
            return;
        }

        IPosChangeRateInfo sendPosChangeRate = gotPosChangeRateInfo.Data.AsInterface<IPosChangeRateInfo>();
        if (sendPosChangeRate == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendPosChangeRate.LaserID == LaserID) || (sendPosChangeRate.LaserID == "ALL"))
        {
            PositionSpeedIn = sendPosChangeRate.PositionChangeRate;
        }

        //Log.Write("Position Change Rate: " + PositionSpeedIn);
    }

    public interface IWorldPosInfo
    {
        string LaserID { get; }
        int WorldPosition { get; }
    }

    private void getWorldPosInfo(ScriptEventData gotWorldPosInfo)
    {

        Log.Write("In getWorldPosInfo gotWorldPosInfo.Data: " + gotWorldPosInfo.Data);
        if (gotWorldPosInfo.Data == null)
        {
            return;
        }

        IWorldPosInfo sendWorldPos = gotWorldPosInfo.Data.AsInterface<IWorldPosInfo>();
        if (sendWorldPos == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendWorldPos.LaserID == LaserID) || (sendWorldPos.LaserID == "ALL"))
        {
            if (sendWorldPos.WorldPosition <= Positions.Count)
            {
                currentPosition = Positions[sendWorldPos.WorldPosition - 1];
                WaitFor(ObjectPrivate.Mover.AddTranslate, currentPosition, PositionSpeedIn, MoveMode.Smoothstep);
            }
        }

        //Log.Write("currentPosition: " + currentPosition);
    }

    public interface IDirectWorldPosInfo
    {
        string LaserID { get; }
        Vector DirectWorldPosition { get; }
    }

    private void getDirectWorldPosInfo(ScriptEventData gotDirectWorldPosInfo)
    {

        //Log.Write("In getDirectWorldPosInfo gotDirectWorldPosInfo.Data: " + gotDirectWorldPosInfo.Data);
        if (gotDirectWorldPosInfo.Data == null)
        {
            return;
        }

        IDirectWorldPosInfo sendDirectWorldPos = gotDirectWorldPosInfo.Data.AsInterface<IDirectWorldPosInfo>();
        if (sendDirectWorldPos == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendDirectWorldPos.LaserID == LaserID) || (sendDirectWorldPos.LaserID == "ALL"))
        {
            WaitFor(ObjectPrivate.Mover.AddTranslate, sendDirectWorldPos.DirectWorldPosition, PositionSpeedIn, MoveMode.Smoothstep);   
        }

        //Log.Write("currentPosition: " + sendDirectWorldPos.DirectWorldPosition);
    }

    public interface IWorldRotInfo
    {
        string LaserID { get; }
        int WorldRotation { get; }
    }

    public interface IRotChangeRateInfo
    {
        string LaserID { get; }
        double RotationChangeRate { get; }
    }

    private void getRotChangeRateInfo(ScriptEventData gotRotChangeRateInfo)
    {

        //Log.Write("In getRotChangeRateInfo gotRotChangeRateInfo.Data: " + gotRotChangeInfo.Data);
        if (gotRotChangeRateInfo.Data == null)
        {
            return;
        }

        IRotChangeRateInfo sendRotChangeRate = gotRotChangeRateInfo.Data.AsInterface<IRotChangeRateInfo>();
        if (sendRotChangeRate == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        if ((sendRotChangeRate.LaserID == LaserID) || (sendRotChangeRate.LaserID == "ALL"))
        {
            RotationSpeedIn = sendRotChangeRate.RotationChangeRate;
        }

        Log.Write("Rotation Change Rate: " + RotationSpeedIn);
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

        if((sendWorldRot.LaserID == LaserID) || (sendWorldRot.LaserID == "ALL"))
        {
            if (sendWorldRot.WorldRotation <= StillRotations.Count)
            {
                Quaternion Xrotation = Quaternion.FromAngleAxis(StillRotations[sendWorldRot.WorldRotation - 1].X * 0.0174533f, Vector.ObjectRight);
                Quaternion Yrotation = Quaternion.FromAngleAxis(StillRotations[sendWorldRot.WorldRotation - 1].Y * 0.0174533f, Vector.ObjectForward);
                Quaternion Zrotation = Quaternion.FromAngleAxis(StillRotations[sendWorldRot.WorldRotation - 1].Z * 0.0174533f, Vector.ObjectUp);

                WaitFor(ObjectPrivate.Mover.AddRotate, Xrotation * Yrotation * Zrotation, RotationSpeedIn, MoveMode.Smoothstep);
                Quaternion newRotation = Xrotation * Yrotation * Zrotation;
            }
        }
    }

    #endregion

    public override void Init()
    {
        
        currentPosition = ObjectPrivate.Position;
        currentRotation = ObjectPrivate.Rotation;

        if (!ObjectPrivate.IsMovable)
        {
            Log.Write($"MoverExample2 script can't move {ObjectPrivate.Name} because either the 'Movable from Script' flag was not set or the object does not have 'Keyframed' physics!");
            return;
        }

        SubscribeToScriptEvent("ChangeRelPos", getRelPosInfo);
        SubscribeToScriptEvent("ChangeWorldPos", getWorldPosInfo);
        SubscribeToScriptEvent("ChangeStillRot", getWorldRotInfo);
        SubscribeToScriptEvent("ChangePosChangeRate", getPosChangeRateInfo);
        SubscribeToScriptEvent("ChangeRotChangeRate", getRotChangeRateInfo);
    }
}
