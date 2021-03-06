﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    //make a variable for the enemy
    Enemy enemy;

    private Vector3 startPos;
    private float totalDist;
    private float curvePos;
    private float startTime;
    private int failSafe;
    private bool startMove;

    //assign enemy as the enemy that changed state
    public EnemyChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.currentState = "Chase";

        startPos = enemy.transform.position;
        totalDist = Vector3.Distance(startPos, Player.Instance.transform.position);
    }

    public void Execute()
    {
        //find the point to rotate towards
        Vector3 rotLocation = Player.Instance.transform.position - enemy.transform.position;
        //if the enemy can't fly, don't tilt up or down
        if (enemy.canFly == false)
        {
            rotLocation.y = 0;
        }

        //set the rotation using LookRotation
        Quaternion rotation = Quaternion.LookRotation(rotLocation);
        //use Slerp to gradually rotate towards the point, modified by turnSpeed
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);

        if (enemy.moveDelay != 0 && Vector3.Angle(rotLocation, enemy.transform.forward) < enemy.moveDelay && startMove == false)
        {
            startMove = true;
        }

        if (enemy.moveDelay == 0 || startMove == true)
        {
            if (enemy.animationTimeBased)
            {
                //for time-based animation, add time
                curvePos += Time.deltaTime;
            }

        }

        if (!enemy.canFly)
        {
            if (enemy.animationTimeBased)
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.chaseSpeed));
            }
            else
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * enemy.chaseSpeed);
            }

            //check if the enemy's x and z position are within the patrol point radius
            if (enemy.transform.position.x > Player.Instance.transform.position.x - enemy.stopRange && enemy.transform.position.x < Player.Instance.transform.position.x + enemy.stopRange && enemy.transform.position.z > Player.Instance.transform.position.z - enemy.stopRange && enemy.transform.position.z < Player.Instance.transform.position.z + enemy.stopRange)
            {
                if (!BattleManager.Instance.enemies.Contains(enemy.gameObject))
                {
                    BattleManager.Instance.AddEnemy(enemy.gameObject);
                }
                enemy.enemyStateAgent.ChangeState(new EnemyBattleState(enemy));
            }
        }
        else
        {
            if (enemy.animationTimeBased)
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * (enemy.animationCurve.Evaluate(curvePos) * enemy.chaseSpeed));
            }
            else
            {
                enemy.rig.MovePosition(enemy.transform.position + enemy.transform.forward * enemy.chaseSpeed);
            }

            //also check y because the enemy can fly
            if (enemy.transform.position.x > Player.Instance.transform.position.x - enemy.stopRange && enemy.transform.position.x < Player.Instance.transform.position.x + enemy.stopRange && enemy.transform.position.z > Player.Instance.transform.position.z - enemy.stopRange && enemy.transform.position.z < Player.Instance.transform.position.z + enemy.stopRange && enemy.transform.position.y > Player.Instance.transform.position.y - enemy.stopRange && enemy.transform.position.y < Player.Instance.transform.position.y + enemy.stopRange)
            {
                if (!BattleManager.Instance.enemies.Contains(enemy.gameObject))
                {
                    BattleManager.Instance.AddEnemy(enemy.gameObject);
                }
                enemy.enemyStateAgent.ChangeState(new EnemyBattleState(enemy));

            }
        }

        if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > enemy.fleeDistance)
        {
            BattleManager.Instance.RemoveEnemy(enemy.gameObject);
            enemy.enemyStateAgent.ChangeState(new EnemyWaitState(enemy));
        }
    }

    public void Exit()
    {

    }
}
