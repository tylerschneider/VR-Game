using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Battle")]
    public int maxHealth;
    [HideInInspector]
    public int health;
    [Tooltip("Range the player must be in to initiate battle")]
    public float range;
    [Tooltip("Amount the range extends to call other enemies")]
    public float expandRange;
    [Tooltip("Chance of each of the enemies' attacks")]
    public int[] attackChance;

    [Header("Movement")]
    [Tooltip("How long the enemy waits during the wait state")]
    public float waitTime;
    [Tooltip("Animation of the enemy's movement")]
    public AnimationCurve animationCurve;
    [Tooltip("Changes the animation to time based (animation will keep repeating at the same rate) rather than distance based (animation changes based on how close the enemy is to its destination, from time 0 to 1)\n\nNOTE: Make sure the right gear in the animation curve is set to loop when this is on.")]
    public bool animationTimeBased;
    [Tooltip("Speed the enemy moves")]
    [RangeAttribute(0f, 20f)]
    public float speed;
    [Tooltip("Speed the enemy moves when flying")]
    [RangeAttribute(0f, 1f)]
    public float flySpeed;
    [Tooltip("Speed the enemy turns")]
    [RangeAttribute(0f, 20f)]
    public float turnSpeed;
    [Tooltip("Whether the enemy can move vertically, flying enemies need much slower speed")]
    public bool canFly;
    [Tooltip("Each point the enemy will go to for patrolling, 0 = no patrolling\n\nNOTE: Make sure points are near the ground if the enemy cannot fly to get accurate curve movement.")]
    public Vector3[] patrolPoints;

    [Tooltip("Range the enemy can wander (circular), 0 = use box")]
    [RangeAttribute(0f, 30f)]
    public float wanderRadius;
    [Tooltip("Range the enemy can wander (box), 0 = don't wander")]
    public Vector3 wanderRange;
    [Tooltip("How close the enemy can be to a point before moving to the next, raise this if the enemy is spinning around a point but you do not want to increase turn speed")]
    [RangeAttribute(0f, 1f)]
    public float pointRadius;
    [Tooltip("The enemy's home, where it will return to if moving too far")]
    public GameObject home;
    [Tooltip("Script to control the enemies' states")]
    public EnemyStateAgent enemyStateAgent;
    [Tooltip("Controls the enemies' movement")]
    public CharacterController enemyController;

    [HideInInspector]
    public int nextPatrolPoint = 0;

    [Header("Gizmos")]
    public bool showInnerRange;
    public Color innerRangeColor;
    public bool showOuterRange;
    public Color outerRangeColor;
    public bool showPatrolWander;
    public Gradient patrolGradient;
    public Color wanderColor;
    public Color wanderPointColor;

    [HideInInspector]
    public SphereCollider col;
    

    void Start()
    {
        health = maxHealth;

        col = this.gameObject.AddComponent<SphereCollider>();
        col.radius = range;
        col.isTrigger = true;

        //the state the enemy starts in
        enemyStateAgent.ChangeState(new EnemyWaitState(this));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(patrolPoints.Length == 0)
            {
                
            }
            else
            {
                
            }

        }
    }

    void OnDrawGizmos()
    {
        if(showInnerRange == true)
        {
            //creates a sphere indicating the range of spawning in the editor
            Gizmos.color = innerRangeColor;
            Gizmos.DrawSphere(transform.position, range);
        }
        if(showOuterRange == true)
        {
            Gizmos.color = outerRangeColor;
            Gizmos.DrawSphere(transform.position, expandRange);
        }


        if (patrolPoints.Length != 0 && showPatrolWander == true)
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
        else if (wanderRadius > 0)
        {
            Gizmos.color = wanderColor;
            Gizmos.DrawSphere(transform.position, wanderRadius);
        }
        else if (wanderRange.x != 0 && wanderRange.y != 0 && wanderRange.z != 0)
        {
            Gizmos.color = wanderColor;
            Gizmos.DrawCube(transform.position, wanderRange);
        }
    }

    void Update()
    {
        enemyStateAgent.Update();
    }
}
