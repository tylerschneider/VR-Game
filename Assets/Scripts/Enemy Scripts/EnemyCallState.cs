using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private float minDist;
    private Enemy closestEnemy;

    //assign enemy as the enemy that changed state
    public EnemyCallState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        BattleManager.Instance.AddEnemy(this.enemy);

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] enemiesInRange = Physics.OverlapSphere(enemy.transform.position, enemy.callRange, enemyLayer);

        for(int i = 0; i < enemy.callAmount && i < enemiesInRange.Length - 1; i++)
        {
            minDist = enemy.callRange;

            foreach (Collider enemyCol in enemiesInRange)
            {
                if(!BattleManager.Instance.enemies.Contains(enemyCol.gameObject))
                {
                    float dist = Vector3.Distance(enemy.transform.position, enemyCol.transform.position);

                    Debug.Log(dist);

                    if(dist < minDist)
                    {
                        closestEnemy = enemyCol.gameObject.GetComponent<Enemy>();
                        minDist = dist;

                        Debug.Log("Min dist " + minDist);
                    }
                }

            }

            Debug.Log("added enemy ", closestEnemy);

            BattleManager.Instance.AddEnemy(closestEnemy);
        }

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
