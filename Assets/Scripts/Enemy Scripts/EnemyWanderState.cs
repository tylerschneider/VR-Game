﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private Vector3 startPos;
    private float totalDist;
    private Vector3 moveDir;
    private float curvePos = 0f;
    private Vector3 randomPos;
    private float startTime;
    private int failSafe = 0;

    //assign enemy as the enemy that changed state
    public EnemyWanderState(Enemy enemy)
    {
        Debug.Log("Wander State");
        this.enemy = enemy;
    }

    public void Enter()
    {
        //get the starting position of the enemy
        startPos = enemy.wanderTarget.transform.position;
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

        totalDist = Vector3.Distance(enemy.transform.position, randomPos);
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


        if (enemy.canFly == false)
        {
            //if the enemy can't fly, use simplemove to move the enemy with gravity, based on the enemy's progress related to the animation curve
            enemy.enemyController.SimpleMove(enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.speed));

            //check if the enemy's x and z position are within the patrol point radius
            if (enemy.transform.position.x > randomPos.x - enemy.pointRadius && enemy.transform.position.x < randomPos.x + enemy.pointRadius && enemy.transform.position.z > randomPos.z - enemy.pointRadius && enemy.transform.position.z < randomPos.z + enemy.pointRadius)
            {
                enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
            }
        }
        else
        {
            //if the enemy can move, use Move instead because it doesn't not apply gravity
            enemy.enemyController.Move(enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.flySpeed));

            //check the x, y, and z because the enemy can fly
            if ( enemy.transform.position.x > randomPos.x - enemy.pointRadius && enemy.transform.position.x < randomPos.x + enemy.pointRadius && enemy.transform.position.z > randomPos.z - enemy.pointRadius && enemy.transform.position.z < randomPos.z + enemy.pointRadius && enemy.transform.position.y > randomPos.y - enemy.pointRadius && enemy.transform.position.y < randomPos.y + enemy.pointRadius)
            {
                enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
            }
        }

    }

    public void Exit()
    {

    }

    private void GetWanderPoint()
    {
        //get the layers for enemies and terrain
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        LayerMask terrainLayer = LayerMask.GetMask("Terrain");

        //loop until finding a valid random position, or after trying 100 times
        while (randomPos == new Vector3(0, 0, 0) || failSafe < 100)
        {
            failSafe++;
            Vector3 rand;

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

            float dist = Vector3.Distance(startPos, rand) + enemy.enemyController.bounds.size.x / 2;

            RaycastHit hit;

            if (!enemy.canFly)
            {
                rand.y += enemy.wanderRadiusMax;

                if(Physics.Raycast(rand, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    rand = hit.point;
                    rand.y += enemy.enemyController.bounds.size.y / 2;
                    dist = Vector3.Distance(startPos, rand) + enemy.enemyController.bounds.size.x / 2;
                }
            }

            if (!Physics.Linecast(enemy.transform.position, rand, out hit, ~enemyLayer))
            {
                if (enemy.wanderRadiusMax > 0 && dist >= enemy.wanderRadiusMin || enemy.wanderRadiusMax == 0)
                {
                    randomPos = rand;
                    Debug.DrawLine(startPos, randomPos, Color.blue, 100f);
                    return;
                }
            }

            if(failSafe > 100)
            {
                Debug.Log("Enemy could not find a valid location to move ", enemy.gameObject);
                enemy.wanderRadiusMax = 0;
                enemy.wanderRadiusMin = 0;
                enemy.wanderRange = new Vector3(0, 0, 0);
                enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
            }
        }

    }

}