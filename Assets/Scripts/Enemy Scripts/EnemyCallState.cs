using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private float minDist;
    List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject closestEnemy;
    private float time;

    //assign enemy as the enemy that changed state
    public EnemyCallState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        if (time >= enemy.callTime)
        {
            //get the layer for enemies
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");

            //get all colliders which are in the enemy layer
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.callRange, enemyLayer);

            //go through each collider
            foreach(Collider collider in colliders)
            {
                //if the game object of the collider is not in the "enemies in range" list, add it
                if(!enemiesInRange.Contains(collider.gameObject))
                {
                    enemiesInRange.Add(collider.gameObject);
                }
            }

            //record how many enemies gameobjects are in range
            int enemiesFound = enemiesInRange.Count;

            //loop while i is less than the call amount or amount of enemies in range
            for (int i = 0; i < enemy.callAmount && i < enemiesFound; i++)
            {
                //reset values before looping
                closestEnemy = null;
                minDist = enemy.callRange;

                //go through each enemy found
                foreach (GameObject e in enemiesInRange)
                {
                    Debug.Log(e);

                    //get the distance between the enemy and the player
                    float dist = Vector3.Distance(enemy.transform.position, e.transform.position);

                    //if the distance is less than the shortest distance so far, set that enemy as closest
                    if (dist < minDist)
                    {
                        closestEnemy = e;
                        minDist = dist;
                    }
                }

                //add the enemy to the battle manager and make it start chasing
                BattleManager.Instance.AddEnemy(closestEnemy.gameObject);
                closestEnemy.GetComponent<Enemy>().enemyStateAgent.ChangeState(new EnemyChaseState(closestEnemy.GetComponent<Enemy>()));

                //remove the enemy from the list so that it is not added again
                enemiesInRange.Remove(closestEnemy);
            }
        }

        time += Time.deltaTime;
    }

    public void Exit()
    {

    }
}
