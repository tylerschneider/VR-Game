using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private Vector3 startPos;
    private float totalDist;
    private Vector3 nextPoint;
    private Vector3 moveDir;
    private float curvePos = 0f;

    //get the enemy this state is conrolling
    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        //get the starting position of the enemy
        startPos = enemy.transform.position;

        //get the location of the next point as noted in the enemy's script
        nextPoint = enemy.patrolPoints[enemy.nextPatrolPoint];
        totalDist = Vector3.Distance(startPos, nextPoint);
    }

    public void Execute()
    {
        //find the point to rotate towards
        Vector3 rotLocation = nextPoint - enemy.transform.position;
        //if the enemy can't fly, don't tilt up or down
        if(enemy.canFly == false)
        {
            rotLocation.y = 0;
        }
        //set the rotation using LookRotation
        Quaternion rotation = Quaternion.LookRotation(rotLocation);
        //rotate using Slerp to gradually rotate towards the point, modified by turnSpeed
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);

        if(enemy.animationTimeBased == false)
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
            enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.speed));

            //check if the enemy's x and z position are within the patrol point radius
            if (enemy.transform.position.x > nextPoint.x - enemy.pointRadius && enemy.transform.position.x < nextPoint.x + enemy.pointRadius && enemy.transform.position.z > nextPoint.z - enemy.pointRadius && enemy.transform.position.z < nextPoint.z + enemy.pointRadius)
            {
                goToNext();
            }
        }
        else
        {
            enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.speed));

            //check the x, y, and z because the enemy can fly
            if (enemy.transform.position.x > nextPoint.x - enemy.pointRadius && enemy.transform.position.x < nextPoint.x + enemy.pointRadius && enemy.transform.position.z > nextPoint.z - enemy.pointRadius && enemy.transform.position.z < nextPoint.z + enemy.pointRadius && enemy.transform.position.y > nextPoint.y - enemy.pointRadius && enemy.transform.position.y < nextPoint.y + enemy.pointRadius)
            {
                goToNext();
            }
        }
    }

    private void goToNext()
    {
        //if at the last point, return to the first
        if (enemy.nextPatrolPoint + 1 > enemy.patrolPoints.Length - 1)
        {
            enemy.nextPatrolPoint = 0;
        }
        //otherwise add one to get the next point
        else
        {
            enemy.nextPatrolPoint += 1;
        }
        //return to the wait state
        enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
    }

    public void Exit()
    {

    }
}
