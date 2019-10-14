using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [Tooltip("Prefabs for each enemy spawnable")]
    public GameObject[] enemyTypes = new GameObject[1];
    [Tooltip("Chance of each enemy spawning out of 100, in the same order as prefabs")]
    public int[] enemyChance = new int[1];

    [Tooltip("Maximum number of enemies at a time, 0 = infinite")]
    public int maxEnemies;
    [Tooltip("Total number of enemies the spawner should spawn, 0 = infinite")]
    public int totalEnemies;
    [Tooltip("Minimum time before spawning an enemy")]
    public float spawnTimeMin;
    [Tooltip("Maximum time before spawning an enemy")]
    public float spawnTimeMax;
    [Tooltip("Radius enemies can spawn within")]
    [RangeAttribute(0.1f, 50f)]
    public float spawnRadius;
    [Tooltip("Set whether spawned enemies will wander around this spawner")]
    public bool setAsHome;

    private List<GameObject> enemyList;
    private int enemiesSpawned = 0;
    private LayerMask terrainLayer;

    [Header("Gizmos")]
    public bool showRange;
    public Color spawnRangeColor;

    void Start()
    {
        terrainLayer = LayerMask.GetMask("Terrain");
        enemyList = new List<GameObject>();

        //start a timer for spawning
        StartCoroutine(CheckSpawn());
    }

    IEnumerator CheckSpawn()
    {
        //check each enemy in the list and make sure they were not destroyed
        for(var i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i] == null)
            {
                //remove any destroyed enemies
                enemyList.Remove(enemyList[i]);
            }
        }

        //check if the total number of enemies has been spawned, continue spawning if not
        if (totalEnemies == 0 || enemiesSpawned < totalEnemies)
        {
            //check to make sure the max amount of enemies are not in the world
            if(maxEnemies == 0 || enemyList.Count < maxEnemies)
            {
                //spawn a new enemy and wait, then restart CheckSpawn
                SpawnEnemy();
                float rand = (Random.value * (spawnTimeMax - spawnTimeMin)) + spawnTimeMin;
                Debug.Log(rand);
                yield return new WaitForSeconds(rand);
                StartCoroutine(CheckSpawn());
            }
            else
            {
                //wait and restart CheckSpawn
                float rand = (Random.value * (spawnTimeMax - spawnTimeMin)) + spawnTimeMin;
                Debug.Log(rand);
                yield return new WaitForSeconds(rand);
                StartCoroutine(CheckSpawn());
            }
        }

    }

    void SpawnEnemy()
    {

        //create a random number from 1-100
        int rand = Random.Range(1, 101);
        int freq = 0;
        int failSafe = 0;
        
        //go through each frequency in the enemy frequency array
        for(var i = 0; i < enemyChance.Length; i++)
        {
            //add it to a value
            freq += enemyChance[i];

            //check that the random number is less than the frequency that the enemy should spawn
            if(rand <= freq)
            {

                //keep looking for a spawn point until successful or 100 tries
                while (failSafe < 100)
                {
                    failSafe++;

                    RaycastHit hit;
                    Vector3 spawnPoint;

                    //create a new enemy corrosponding to which frequency was checked
                    GameObject enemy = enemyTypes[i];

                    //set the enemy's position somewhere random within the spawn radius
                    spawnPoint = Random.insideUnitSphere * spawnRadius + transform.position;
                    //raycast downwards to check where the terrain is
                    if (Physics.Raycast(spawnPoint + new Vector3(0, spawnRadius*2, 0), Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                    {
                        spawnPoint = hit.point;
                    }

                    //create the enemy
                    enemy = Instantiate(enemy);

                    //get the bounds of the enemy's character conroller
                    Vector3 bounds = enemy.GetComponent<CharacterController>().bounds.size;

                    /* If enemy pivot point is centered, not at the bottom
                    float boundSize = (bounds.y / 2) + (enemy.GetComponent<CharacterController>().skinWidth);
                    spawnPoint.y += boundSize;
                    */

                    //Check if there is any colliders other than terrain where the enemy spawns
                    if (Physics.CheckBox(spawnPoint, Vector3.Scale(bounds, new Vector3(0.5f, 0.5f, 0.5f)), Quaternion.identity, 1 << terrainLayer))
                    {
                        DestroyImmediate(enemy);
                    }
                    else
                    {
                        if (setAsHome == true)
                        {
                            //set this spawner as the point the enemy will wander around
                            enemy.GetComponent<Enemy>().wanderTarget = this.gameObject;
                        }
                        enemy.transform.position = spawnPoint;
                        //add the enemy to the list of enemies spawned
                        enemyList.Add(enemy);

                        //stop running SpawnEnemy
                        return;
                    }

                    if(failSafe == 100)
                    {
                        Debug.LogError("Spawn failed, cannot find open area ", this.gameObject);
                    }

                }
            }
        }
    }

    void OnDrawGizmos()
    {        
        //creates a sphere indicating the range of spawning in the editor
        if(showRange == true)
        {
            Gizmos.color = spawnRangeColor;
            Gizmos.DrawSphere(transform.position, spawnRadius);
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, spawnRadius*2, 0));
        }
    }
}
