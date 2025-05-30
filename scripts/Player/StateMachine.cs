using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public PlayerStates currentState {  get; private set; }
    public void StartMachine(PlayerStates _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerStates _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
