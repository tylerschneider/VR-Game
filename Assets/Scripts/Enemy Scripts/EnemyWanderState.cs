using System.Collections;
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

    //assign enemy as the enemy that changed state
    public EnemyWanderState(Enemy enemy)
    {
        Debug.Log("Wander State");
        this.enemy = enemy;
    }

    public void Enter()
    {

        if (enemy.wanderRadius > 0)
        {
            if (enemy.canFly)
            {
                randomPos = Random.insideUnitSphere * enemy.wanderRadius;
            }
            else
            {
                randomPos = Random.insideUnitSphere * enemy.wanderRadius;
                randomPos -= enemy.transform.position;
                randomPos.y = enemy.transform.position.y;
            }
        }

        //get the starting position of the enemy
        startPos = enemy.transform.position;

        totalDist = Vector3.Distance(startPos, randomPos);
    }

    public void Execute()
    {
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

            //check the x, y, and z because the enemy can fly. If the enemy hits the ground get a new point
            if(enemy.enemyController.isGrounded)
            {
                enemy.enemyStateAgent.ChangeState(new EnemyWanderState(enemy));
            }
            else if ( enemy.transform.position.x > randomPos.x - enemy.pointRadius && enemy.transform.position.x < randomPos.x + enemy.pointRadius && enemy.transform.position.z > randomPos.z - enemy.pointRadius && enemy.transform.position.z < randomPos.z + enemy.pointRadius && enemy.transform.position.y > randomPos.y - enemy.pointRadius && enemy.transform.position.y < randomPos.y + enemy.pointRadius)
            {
                enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
            }
        }

    }

    public void Exit()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = enemy.wanderPointColor;
        Gizmos.DrawSphere(randomPos, enemy.pointRadius);
    }

}
