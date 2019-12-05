using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private Vector3 startPos;
    private float totalDist;
    private float curvePos;
    private Vector3 randomPos;
    private float startTime;
    private int failSafe;
    private bool startMove;

    //assign enemy as the enemy that changed state
    public EnemyWanderState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.currentState = "Wander";

        //get the starting position of the enemy
        startPos = enemy.transform.position;
        startTime = Time.time;

        //if using radius (sphere) to find a random position
        if (enemy.wanderRadiusMax > 0 || enemy.wanderRange.x > 0 || enemy.wanderRange.y > 0 || enemy.wanderRange.z > 0)
        {
            GetWanderPoint();

        }
        else
        {
            enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
        }

        totalDist = Vector3.Distance(startPos, randomPos);
    }

    private void GetWanderPoint()
    {
        //get the layers for enemies and terrain
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        LayerMask terrainLayer = LayerMask.GetMask("Terrain");

        //loop until finding a valid random position, or after trying 100 times
        while (randomPos == new Vector3(0, 0, 0))
        {
            failSafe++;
            Vector3 rand;
            RaycastHit hit;

            //if using radius, get a random point within the maximum radius sphere
            if (enemy.wanderRadiusMax > 0)
            {
                rand = (Random.insideUnitSphere * enemy.wanderRadiusMax) + enemy.wanderTarget.transform.position;
            }
            //if using range (box), get a random position for x, y, and z
            else
            {
                rand.x = ((Random.value * enemy.wanderRange.x) - enemy.wanderRange.x / 2) + enemy.wanderTarget.transform.position.x;
                rand.y = ((Random.value * enemy.wanderRange.y) - enemy.wanderRange.y / 2) + enemy.wanderTarget.transform.position.y;
                rand.z = ((Random.value * enemy.wanderRange.z) - enemy.wanderRange.z / 2) + enemy.wanderTarget.transform.position.z;
            }

            //get the distance from the target's origin to the random point
            float dist = Vector3.Distance(enemy.wanderTarget.transform.position, rand);

            if (!enemy.canFly)
            {
                //if the enemy can't fly, raise the y position of the random point and raycast down to find the point on the terrain to walk towards
                rand.y += enemy.wanderRadiusMax;

                if (Physics.Raycast(rand, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    rand = hit.point;
                    dist = Vector3.Distance(enemy.wanderTarget.transform.position, rand);
                }
            }

            //check for any terrain or objects other than enemies that are in the way
            if (!Physics.Linecast(enemy.transform.position, rand, out hit, terrainLayer) && !Physics.Linecast(enemy.transform.position, rand, out hit, 1 << enemyLayer))
            {
                if (enemy.wanderRadiusMax > 0 && dist >= enemy.wanderRadiusMin || enemy.wanderRadiusMax == 0)
                {
                    randomPos = rand;
                    return;
                }
            }

            //if the enemy could not find somewhere to move after 100 tries, stop it from moving
            if (failSafe == 100)
            {
                Debug.Log("Enemy could not find a valid location to move ", enemy.gameObject);
                enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
                enemy.wanderRadiusMax = 0;
                enemy.wanderRadiusMin = 0;
                enemy.wanderRange = new Vector3(0, 0, 0);
                return;
            }
        }

    }


    public void Execute()
    {
        //change to wait state if the enemy could not reach its target in time
        if (Time.time - startTime >= enemy.wanderTimeOut)
        {
            enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
        }

        //find the point to rotate towards
        Vector3 rotLocation = randomPos - enemy.transform.position;
        //if the enemy can't fly, don't tilt up or down
        if (enemy.canFly == false)
        {
            rotLocation.y = 0;
        }

        //set the rotation using LookRotation
        Quaternion rotation = Quaternion.LookRotation(rotLocation);
        //rotate using Slerp to gradually rotate towards the point, modified by turnSpeed
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);

        if (enemy.moveDelay != 0 && Vector3.Angle(rotLocation, enemy.transform.forward) < enemy.moveDelay && startMove == false)
        {
            startMove = true;
        }

        if(enemy.moveDelay == 0 || startMove == true)
        {
            if (enemy.animationTimeBased == false)
            {
                //get the distance between the starting and current location
                float distFromStart = Vector3.Distance(startPos, enemy.transform.position);
                //find how far the enemy has moved from 0 to 1
                curvePos = distFromStart / totalDist;
            }
            else
            {
                //for time-based animation, add time
                curvePos += Time.deltaTime;
            }

            if (!enemy.canFly)
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.speed));

                //check if the enemy's x and z position are within the patrol point radius
                if (enemy.transform.position.x > randomPos.x - enemy.pointRadius && enemy.transform.position.x < randomPos.x + enemy.pointRadius && enemy.transform.position.z > randomPos.z - enemy.pointRadius && enemy.transform.position.z < randomPos.z + enemy.pointRadius)
                {
                    enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
                }
            }
            else
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.speed));

                //also check y because the enemy can fly
                if (enemy.transform.position.x > randomPos.x - enemy.pointRadius && enemy.transform.position.x < randomPos.x + enemy.pointRadius && enemy.transform.position.z > randomPos.z - enemy.pointRadius && enemy.transform.position.z < randomPos.z + enemy.pointRadius && enemy.transform.position.y > randomPos.y - enemy.pointRadius && enemy.transform.position.y < randomPos.y + enemy.pointRadius)
                {
                    enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
                }
            }
        }



    }

    public void Exit()
    {

    }

}
