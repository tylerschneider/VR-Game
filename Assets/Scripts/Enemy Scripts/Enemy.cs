using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{

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
    [RangeAttribute(0f, 20f)]
    public float speed;
    [Tooltip("Speed the enemy moves when flying")]
    [RangeAttribute(0f, 1f)]
    public float flySpeed;
    [Tooltip("Speed the enemy turns")]
    [RangeAttribute(0f, 20f)]
    public float turnSpeed;
    [Tooltip("Minimum angle between the enemy and target before it starts moving, 0 = moves immediately")]
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

    [Tooltip("Range the player must be in to initiate battle")]
    public float range;
    [Tooltip("Range from the player before stopping")]
    public float stopRange;

    [Space(10)]
    [Header("Battle")]
    [Space(25)]

    public int maxHealth;
    [HideInInspector]
    public int health;
    [Tooltip("Chance of each of the enemies' attacks")]
    public int[] attackChance;

    [Space(10)]
    [Header("Scripts")]
    [Space(25)]

    //[Tooltip("The enemy's home, where it will return to if moving too far")]
    //public GameObject home;
    [Tooltip("Script to control the enemies' states")]
    public EnemyStateAgent enemyStateAgent;
    public EnemyWaitState enemyWaitState;
    [Tooltip("Controls the enemies' movement")]
    public CharacterController enemyController;

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

    void Start()
    {
        health = maxHealth;

        col = this.gameObject.GetComponent<CapsuleCollider>();

        //the state the enemy starts in
        enemyStateAgent.ChangeState(new EnemyWaitState(this));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyStateAgent.ChangeState(new EnemyCallState(this));
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

    void FixedUpdate()
    {
        enemyStateAgent.FixedUpdate();
    }
}
