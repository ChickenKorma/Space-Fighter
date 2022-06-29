using UnityEngine;

public class EvadeState : AIBaseState
{
    public override void EnterState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Start Evading");
    }

    public override void UpdateState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Evading");
    }

    public override void ExitState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Stop Evading");
    }
}
