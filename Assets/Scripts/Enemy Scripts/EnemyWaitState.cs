using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaitState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    //variable for wait time remaining
    float timeRemaining;

    //make enemy the enemy's script
    public EnemyWaitState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Wait State");
        timeRemaining = (Random.value * (enemy.waitMax - enemy.waitMin)) + enemy.waitMin;
    }

    public void Execute()
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining < 0)
        {
            if(enemy.patrolPoints.Length > 0)
            {
                enemy.enemyStateAgent.ChangeState(new EnemyPatrolState(enemy));
            }

            else
            {
                enemy.enemyStateAgent.ChangeState(new EnemyWanderState(enemy));
            }

        }
    }

    public void Exit()
    {

    }
}
