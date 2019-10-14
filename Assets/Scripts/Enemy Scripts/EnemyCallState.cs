using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    //assign enemy as the enemy that changed state
    public EnemyCallState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(enemy.transform.position, enemy.callRange, 1 << LayerMask.GetMask("Enemy"));

        for(int i = 0; i < enemy.callAmount; i++)
        {
            float minDist;

            foreach (Collider enemy in enemiesInRange)
            {
                if(enemy.gameObject.transform.parent != BattleManager.Instance.gameObject)
                {
                    float dist = Vector3.Distance(enemy.transform.position, Player.Instance.transform.position);

                    if(dist < minDist)
                    {
                        GameObject closestEnemy = enemy.gameObject.GetComponent<Enemy>();
                    }
                }

            }
        }

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
