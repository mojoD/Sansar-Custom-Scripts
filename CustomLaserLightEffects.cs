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

//
//   This script controls Laser Color/Type, Visibility and Blinking
//

public class CustomLaserLightEffects : ObjectScript
{

    #region ConstantsVariables
    // Public properties

    [Tooltip("Laser ID for Programs")]
    [DisplayName("Laser ID")]
    [DefaultValue("Laser1")]
    public readonly string LaserID;

    [Tooltip("Type of Laser")]
    [DefaultValue("RedLaser")]
    public string LaserType;

    [Tooltip("Turn Visibility On/Off")]
    [DefaultValue(true)]
    [DisplayName("Initial Visibility")]
    public bool initialVisibility;

    [Tooltip("Blink Laser")]
    [DefaultValue(false)]
    [DisplayName("Blink")]
    public bool blink;

    [Tooltip("Blink Rate")]
    [DefaultValue(1.0)]
    [DisplayName("Blink Rate")]
    public float blinkRateIn;

    private Quaternion initialRotation;
    private MeshComponent component;
    private bool currentVisibility;
    private bool inLaserBlink = false;
    private float currentBlinkRate = 1.0f;
    private float blinkRate;
    private string Change;

    #endregion

    #region Communication

    public interface ILaserTypeInfo
    {
        string LaserID { get; }
        string inLaserType { get; }
    }

    private void getLaserType(ScriptEventData gotLaserTypeInfo)
    {

        //Log.Write("In getLaserTypeInfo");
        if (gotLaserTypeInfo.Data == null)
        {
            return;
        }

        ILaserTypeInfo sendLaserTypeInfo = gotLaserTypeInfo.Data.AsInterface<ILaserTypeInfo>();
        if (sendLaserTypeInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }
        if ((sendLaserTypeInfo.LaserID == LaserID) || (sendLaserTypeInfo.LaserID == "ALL"))
        {
            if (sendLaserTypeInfo.inLaserType == LaserType)
            {
                currentVisibility = true;
                component.SetIsVisible(true);
            }
            else
            {
                currentVisibility = false;
                component.SetIsVisible(false);
            }
        }
        else
        {
            currentVisibility = false;
            component.SetIsVisible(false);
        }


        //Log.Write("sendLaserTypeInfo.inLaserType: " + sendLaserTypeInfo.inLaserType);

    }

    public interface IBlinkInfo
    {
        string LaserID { get; }
        bool LaserBlink { get; }
    }

    private void getBlinkInfo(ScriptEventData gotBlinkInfo)
    {

        //Log.Write("In getBlinkInfo");
        if (gotBlinkInfo.Data == null)
        {
            return;
        }

        IBlinkInfo sendBlinkInfo = gotBlinkInfo.Data.AsInterface<IBlinkInfo>();
        if (sendBlinkInfo == null)
        {
            Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
            return;
        }

        //Log.Write("sendBlinkInfo.LaserBlink: " + sendBlinkInfo.LaserBlink);
        if (sendBlinkInfo.LaserBlink)
        {
            blink = true;
        }
        else
        {
            blink = false;
        }
        //Log.Write("blink: " + blink);
        if (!inLaserBlink) LaserBlink();

    }

    public interface IBlinkRateInfo
    {
        string LaserID { get; }
        float LaserBlinkRate { get; }
    }

    private void getBlinkRate(ScriptEventData gotBlinkRate)
    {

        //Log.Write("In getPanRate gotPanRate.Data: " + gotPanRate.Data);
        if (gotBlinkRate.Data == null)
        {
            return;
        }

        IBlinkRateInfo sendBlinkRate = gotBlinkRate.Data.AsInterface<IBlinkRateInfo>();
        if ((sendBlinkRate.LaserID == LaserID) || (sendBlinkRate.LaserID == "ALL"))
        {
            if (sendBlinkRate == null)
            {
                Log.Write(LogLevel.Error, Script.ID.ToString(), "Unable to create interface, check logs for missing member(s)");
                return;
            }
            //Log.Write("inBlinkRate: " + sendBlinkRate.LaserBlinkRate + " currentBlinkRate: " + currentBlinkRate);
            //if (currentBlinkRate > 0.1)
            //{
            //    currentBlinkRate = currentBlinkRate + sendBlinkRate.LaserBlinkRate;
            //    blinkRate = blinkRateIn * currentBlinkRate;
            //}
            //else if (currentBlinkRate <= 0.1)
            //{
            //    currentBlinkRate = currentBlinkRate + sendBlinkRate.LaserBlinkRate;
            //    blinkRate = blinkRateIn * 0.1f * currentBlinkRate;
            //}
            //else Log.Write("Can't go any faster");
            blinkRate = sendBlinkRate.LaserBlinkRate;
        } 

        //Log.Write("blinkRate: " + blinkRate);
    }

    #endregion

    public override void Init()
    {
        if (!ObjectPrivate.TryGetFirstComponent<MeshComponent>(out component))
        {
            Log.Write("No mesh component found!");
            return;
        }

        if (initialVisibility)
        {
            component.SetIsVisible(true);
            currentVisibility = true;
        }
        else
        {
            component.SetIsVisible(false);
            currentVisibility = false;
        }

        blinkRate = blinkRateIn;

        //Log.Write("Found mesh component");
        //Log.Write("Visibility: " + component.GetIsVisible());

        if (!component.IsScriptable)
        { 
            Log.Write("Can't change visibility of non-scriptable mesh!");
        }

        SubscribeToScriptEvent("LaserType", getLaserType);
        SubscribeToScriptEvent("LaserBlink", getBlinkInfo);
        SubscribeToScriptEvent("LaserBlinkRate", getBlinkRate);

        if (blink)
        {
            LaserBlink();
        }
    }

    private void LaserBlink()
    {
        inLaserBlink = true;

        do
        {
            component.SetIsVisible(!component.GetIsVisible());
            Wait(TimeSpan.FromSeconds(blinkRate));
            //Log.Write("blink: " + blink + " currentVisibility: " + currentVisibility);
        } while (blink && currentVisibility);

        inLaserBlink = false;
        //Log.Write("Out of Blink - Initial Visibility: " + initialVisibility);
        if (initialVisibility && currentVisibility)
        {
            component.SetIsVisible(true);
            currentVisibility = true;
        }
        else
        {
            component.SetIsVisible(false);
            currentVisibility = false;
        }

    }
}
