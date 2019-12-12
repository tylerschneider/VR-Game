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
        enemy.currentState = "Battle";

        enemy.rig.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Execute()
    {
        //look at player
        Vector3 rotLocation = Player.Instance.transform.position - enemy.transform.position;

        if (enemy.canFly == false)
        {
            rotLocation.y = 0;
        }

        Quaternion rotation = Quaternion.LookRotation(rotLocation);

        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);


        if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > enemy.startRange)
        {
            enemy.enemyStateAgent.ChangeState(new EnemyChaseState(enemy));
        }

    }

    public void Exit()
    {
        enemy.rig.constraints = RigidbodyConstraints.None;
    }
}
