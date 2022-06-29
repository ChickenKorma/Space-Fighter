using UnityEngine;

public abstract class AIBaseState
{
    public abstract void EnterState(AIStateMachine stateMachine, Spacecraft spacecraft);

    public abstract void UpdateState(AIStateMachine stateMachine, Spacecraft spacecraft);

    public abstract void ExitState(AIStateMachine stateMachine, Spacecraft spacecraft);
}
