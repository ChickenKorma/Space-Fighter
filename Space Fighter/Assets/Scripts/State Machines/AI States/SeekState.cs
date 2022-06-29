using UnityEngine;

public class SeekState : AIBaseState
{
    public override void EnterState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Start Seeking");
    }

    public override void UpdateState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Seeking");
    }

    public override void ExitState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Stop Seeking");
    }
}
