using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAgent : MonoBehaviour
{
    public EnemyState currentState;

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.Execute();
    }
}
