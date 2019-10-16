using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;


    //assign enemy as the enemy that changed state
    public EnemyBattleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        if(Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > enemy.startRange)
        {
            enemy.enemyStateAgent.ChangeState(new EnemyChaseState(enemy));
        }

    }

    public void Exit()
    {

    }
}
