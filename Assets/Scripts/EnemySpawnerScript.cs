using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [Tooltip("Prefabs for each enemy spawnable")]
    public GameObject[] enemyTypes = new GameObject[1];
    [Tooltip("Chance of each enemy spawning out of 100, in the same order as prefabs")]
    public int[] enemyFreq = new int[1];

    [Tooltip("Maximum number of enemies at a time, 0 = infinite")]
    public int maxEnemies = 3;
    [Tooltip("Total number of enemies the spawner should spawn, 0 = infinite")]
    public int totalEnemies = 0;
    [Tooltip("How often to spawn enemies in seconds")]
    public float spawnFreq = 3;
    [Tooltip("Radius enemies can spawn within")]
    [RangeAttribute(0.1f, 50f)]
    public float spawnRadius;

    public LayerMask terrainLayer;

    [HideInInspector] //List to hold the current enemies in the world
    public List<GameObject> enemyList;
    [HideInInspector] //Number of enemies that have been spawned
    public int enemiesSpawned = 0;

    [Header("Gizmos")]
    public bool showRange;
    public Color spawnRangeColor;

    void Start()
    { 
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

        //check if the total number of enemies has not been spawned
        if (totalEnemies == 0 || enemiesSpawned < totalEnemies)
        {
            //check to make sure the max amount of enemies are not in the world
            if(maxEnemies == 0 || enemyList.Count < maxEnemies)
            {
                //spawn a new enemy and wait, then restart CheckSpawn
                SpawnEnemy();
                yield return new WaitForSeconds(spawnFreq);
                StartCoroutine(CheckSpawn());
            }

            else
            {
                //wait and restart CheckSpawn
                yield return new WaitForSeconds(spawnFreq);
                StartCoroutine(CheckSpawn());
            }
        }

    }

    void SpawnEnemy()
    {
        //create a random number from 1-100
        int rand = Random.Range(1, 101);
        int freq = 0;

        //go through each frequency in the enemy frequency array
        for(var i = 0; i < enemyFreq.Length; i++)
        {
            //add it to a value
            freq += enemyFreq[i];

            //check that the random number is less than the frequency that the enemy should spawn
            if(rand <= freq)
            {
                RaycastHit hit;

                //create a new enemy variable corrosponding to which frequency was checked
                GameObject enemy = enemyTypes[i];
                //set the enemy's position somewhere random within the spawn radius
                enemy.transform.position = Random.insideUnitSphere * spawnRadius + this.gameObject.transform.position;
                //raycast downwards from a point twice the size above the spawn radius above the enemy, and place the enemy at the height it hits the terrain
                if (Physics.Raycast(enemy.transform.position + new Vector3(0, spawnRadius*2, 0), Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    enemy.transform.position = hit.point;
                }
                //set the enemy's home
                //enemy.GetComponent<EnemyScript>().home = this.gameObject;
                //create the enemy
                enemy = Instantiate(enemy);
                //add the enemy to the list of enemies spawned
                enemyList.Add(enemy);

                //stop running SpawnEnemy
                return;
            }
        }

    }

    void OnDrawGizmos()
    {
        if(showRange == true)
        {
            Gizmos.color = spawnRangeColor;
            Gizmos.DrawSphere(transform.position, spawnRadius);
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, spawnRadius*2, 0));
        }
        //creates a sphere indicating the range of spawning in the editor
    }
}
