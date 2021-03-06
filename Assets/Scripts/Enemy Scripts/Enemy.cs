﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    public string currentState;

    [Space(10)]
    [Header("Movement")]
    [Space(25)]

    [Tooltip("Minimum time the enemy will wait between moving")]
    public float waitMin;
    [Tooltip("Maximum time the enemy will wait between moving")]
    public float waitMax;
    [Tooltip("Animation of the enemy's movement")]
    public AnimationCurve animationCurve;
    [Tooltip("Changes the animation to time based (animation will keep repeating at the same rate) rather than distance based (animation changes based on how close the enemy is to its destination, from time 0 to 1)\n\nNOTE: Make sure the right gear in the animation curve is set to loop when this is on.")]
    public bool animationTimeBased;
    [Tooltip("Whether the enemy can move vertically, flying enemies need much slower speed")]
    public bool canFly;
    [Tooltip("Speed the enemy moves")]
    [RangeAttribute(0f, 10f)]
    public float speed;
    [Tooltip("Speed the enemy moves when flying")]
    [RangeAttribute(0f, 1f)]
    public float flySpeed;
    [Tooltip("Speed the enemy turns")]
    [RangeAttribute(0f, 20f)]
    public float turnSpeed;
    [Tooltip("Minimum turn angle between the enemy and target before it starts moving, 0 = moves immediately")]
    [RangeAttribute(0f, 20f)]
    public float moveDelay;

    [Space(20)]

    [Tooltip("How close the enemy can be to a point before moving to the next, raise this if the enemy is spinning around a point but you do not want to increase turn speed")]
    [RangeAttribute(0f, 1f)]
    public float pointRadius;
    [Tooltip("Each point the enemy will go to for patrolling, 0 = no patrolling\n\nNOTE: Make sure points are near the ground if the enemy cannot fly to get accurate curve movement.")]
    public Vector3[] patrolPoints;
    [Tooltip("Range the enemy can wander (circular), 0 = use box")]
    [RangeAttribute(0f, 30f)]
    public float wanderRadiusMin;
    [RangeAttribute(0f, 30f)]
    public float wanderRadiusMax;
    [Tooltip("Range the enemy can wander (box), 0 = don't wander")]
    public Vector3 wanderRange;
    [Tooltip("How long in seconds before the enemy will find a new point, to prevent getting stuck")]
    public float wanderTimeOut;
    [Tooltip("The object the enemy will wander around (for free movement, set it as itself")]
    public GameObject wanderTarget;

    [Space(10)]
    [Header("Calling")]
    [Space(25)]

    [Tooltip("How long the enemy will be calling for others")]
    public float callTime;
    [Tooltip("How many enemies can be called")]
    public float callAmount;
    [Tooltip("Amount the range extends to call other enemies")]
    public float callRange;

    [Space(10)]
    [Header("Chasing")]
    [Space(25)]

    [RangeAttribute(0f, 10f)]
    public float chaseSpeed;
    [Tooltip("Range from the player before stopping moving")]
    public float stopRange;
    [Tooltip("Range from the player to continue chase")]
    public float startRange;
    [Tooltip("Range from the player before stopping chase")]
    public float fleeDistance;
    [HideInInspector]
    public bool chasing;
    [Tooltip("Changes the animation to time based (animation will keep repeating at the same rate) rather than distance based (animation changes based on how close the enemy is to its destination, from time 0 to 1)\n\nNOTE: Make sure the right gear in the animation curve is set to loop when this is on.")]
    public bool chaseAnimationTimeBased;

    [Space(10)]
    [Header("Battle")]
    [Space(25)]

    public bool boss = false;
    public int maxHealth;
    public int health;
    [Tooltip("Damage of each attack")]
    public int[] attackDamage;
    [Tooltip("Chance of each of the enemies' attacks")]
    public int[] attackChance;
    public GameObject[] dropItems;

    [Space(10)]
    [Header("Scripts")]
    [Space(25)]

    [HideInInspector]
    public EnemyStateAgent enemyStateAgent;
    [HideInInspector]
    public Rigidbody rig;
    //public EnemyWaitState enemyWaitState;

    [Space(10)]
    [Header("Gizmos")]
    [Space(50)]

    public bool showCallRange;
    public Color innerRangeColor;
    public bool showStopRange;
    public Color outerRangeColor;
    public bool showPatrolWander;
    public Gradient patrolGradient;
    public Color wanderColor;

    [HideInInspector]
    public CapsuleCollider col;
    [HideInInspector]
    public int nextPatrolPoint = 0;

    void Awake()
    {
        enemyStateAgent = GetComponent<EnemyStateAgent>();
        rig = GetComponent<Rigidbody>();

        if (!canFly)
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = false;
        }

        health = maxHealth;

        col = this.gameObject.GetComponent<CapsuleCollider>();

        //the state the enemy starts in
        enemyStateAgent.ChangeState(new EnemyWaitState(this));
    }

    void OnTriggerEnter(Collider other)
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        if(other.tag == "Player" && BattleManager.Instance.enemies.Count == 0 && !Physics.Linecast(transform.position, Player.Instance.transform.position, 1 << enemyLayer))
        {
            enemyStateAgent.ChangeState(new EnemyChaseState(this));
        }
    }

    void FixedUpdate()
    {
        enemyStateAgent.FixedUpdate();
    }

    public void Attack()
    {
        int rand = Random.Range(0, 101);

        if(rand <= attackChance[0]) { Attack1(); }
        else if (rand <= attackChance[1]) { Attack2(); }
        else if (rand <= attackChance[2]) { Attack3(); }
        else if (rand <= attackChance[3]) { Attack4(); }
        else if (rand <= attackChance[4]) { Attack5(); }
    }

    public void killEnemy()
    {
        foreach(GameObject item in dropItems)
        {
            Instantiate(item, transform.position + Random.insideUnitSphere * 0.5f, transform.rotation);
        }

        if(GetComponent<EndGameScript>())
        {
            GetComponent<EndGameScript>().EndGame();
        }

        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if(showCallRange == true)
        {
            //creates a sphere indicating the range of spawning in the editor
            Gizmos.color = innerRangeColor;
            Gizmos.DrawSphere(transform.position, callRange);
        }
        if(showStopRange == true)
        {
            Gizmos.color = outerRangeColor;
            Gizmos.DrawSphere(transform.position, stopRange);
        }

        if (showPatrolWander == true)
        {
            if (patrolPoints.Length != 0)
            {
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    Gizmos.color = patrolGradient.Evaluate(i / (float)(patrolPoints.Length - 1));

                    if (i == patrolPoints.Length - 1)
                    {
                        Gizmos.DrawLine(patrolPoints[i], patrolPoints[0]);
                    }
                    else
                    {
                        Gizmos.DrawLine(patrolPoints[i], patrolPoints[i + 1]);
                    }

                    Gizmos.DrawSphere(patrolPoints[i], pointRadius);
                }
            }
            else if (wanderRadiusMax > 0)
            {
                Gizmos.color = wanderColor;
                Gizmos.DrawSphere(wanderTarget.transform.position, wanderRadiusMin);
                Gizmos.DrawSphere(wanderTarget.transform.position, wanderRadiusMax);
            }
            else if (wanderRange.x != 0 && wanderRange.y != 0 && wanderRange.z != 0)
            {
                Gizmos.color = wanderColor;
                Gizmos.DrawCube(wanderTarget.transform.position, wanderRange);
            }
        }

    }


    public virtual void Attack1()
    {

    }

    public virtual void Attack2()
    {

    }

    public virtual void Attack3()
    {

    }

    public virtual void Attack4()
    {

    }

    public virtual void Attack5()
    {

    }
}
