using UnityEngine;

public class AttackState : AIBaseState
{

    public override void EnterState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Start Attacking");
    }

    public override void UpdateState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("Attacking");

        /*
        if (!isTargetVisible(stateMachine))
        {
            stateMachine.SwitchState(stateMachine.SeekState);
        }
        */
    }

    public override void ExitState(AIStateMachine stateMachine, Spacecraft spacecraft)
    {
        Debug.Log("End Attacking");
    }
}
