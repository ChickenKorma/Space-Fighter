using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private AIBaseState currentState;

    private SeekState seekState = new SeekState();
    private AttackState attackState = new AttackState();
    private EvadeState evadeState = new EvadeState();

    public SeekState SeekState
    {
        get { return seekState; }
    }
    public AttackState AttackState
    {
        get { return attackState; }
    }
    public EvadeState EvadeState
    {
        get { return evadeState; }
    }

    private Spacecraft spacecraft;

    void Start()
    {
        spacecraft = GetComponent<Spacecraft>();

        currentState = seekState;
        currentState.EnterState(this, spacecraft);
    }

    void Update()
    {
        currentState.UpdateState(this, spacecraft);
    }

    // Changes current state to a new state, invoking the correct transition methods
    public void SwitchState(AIBaseState newState)
    {
        currentState.ExitState(this, spacecraft);
        currentState = newState;
        currentState.EnterState(this, spacecraft);
    }
}
