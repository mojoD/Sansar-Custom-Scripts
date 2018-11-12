//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."
using System;
using Sansar.Script;
using Sansar.Simulation;

public class SceneReset : SceneObjectScript
{
    public string WelcomeMessage = "Welcome to the Interactive Grand Piano";
    private int NumOfTime = 0;
    //private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);

    private SessionId Jammer = new SessionId();

    public override void Init()
    {
        RigidBodyComponent rigidBody;
        if (ObjectPrivate.TryGetFirstComponent(out rigidBody)
            && rigidBody.IsTriggerVolume())
        {
            //CurPos = rigidBody.GetPosition();

            rigidBody.Subscribe(CollisionEventType.Trigger, OnCollide);

        }
        else
        {
        }
    }

    private void OnCollide(CollisionData Data)
    {
        AgentPrivate hit = ScenePrivate.FindAgent(Data.HitComponentId.ObjectId);
        if (Data.Phase == CollisionEventPhase.TriggerEnter)
        {
            Log.Write("Entered Volume");
            ResetScene(hit);
        }
        else
        {
            Log.Write("has left my volume!");
        }
    }

    private void ResetScene(AgentPrivate agent)
    {
        ModalDialog Dlg;
        //AgentPrivate agent = ScenePrivate.FindAgent(hitter);
        if (agent == null)
            return;

        Dlg = agent.Client.UI.ModalDialog;
        WaitFor(Dlg.Show, "Are you sure you want to reset the entire scene?", "YES", "NO");
        if (Dlg.Response == "YES")
        {
            StartCoroutine(() =>
            {
                ScenePrivate.Chat.MessageAllUsers("Resetting scene");
                Wait(TimeSpan.FromSeconds(1));
                ScenePrivate.ResetScene();
            });
        }
    }
}