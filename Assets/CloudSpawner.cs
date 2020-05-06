using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float cloudSizeMin;
    public float cloudSizeMax;
    public float variationMin;
    public float variationMax;
    public int startClouds = 10;
    public float cloudSpeed = 0.5f;
    private Collider col;
    public float cloudAlphaMax = 0.8f;
    public float cloudAlphaDist;

    private void Start()
    {
        col = GetComponent<BoxCollider>();

        for (int i = 0; i < startClouds; i++)
        {
            SpawnCloud(true);
        }
    }

    void Update()
    {
        foreach(Transform child in transform)
        {
            child.Translate(Vector3.right * cloudSpeed);

            float dist = Mathf.Abs(transform.position.x + child.position.x) + cloudAlphaDist;
            float alpha = Mathf.Min(1 - dist / col.bounds.extents.x, cloudAlphaMax);
            child.GetComponent<Renderer>().material.SetFloat("_Alpha", alpha);

            if (child.position.x >= col.bounds.extents.x + transform.position.x - cloudAlphaDist)
            {
                Destroy(child.gameObject);
                SpawnCloud(false);
            }
        }
    }

    void SpawnCloud(bool start)
    {
        Vector3 spawnLocation = Vector3.zero;

        if (start)
        {
            float spawnX = Random.Range(col.bounds.extents.x + transform.position.x, -col.bounds.extents.x + transform.position.x);
            float spawnY = Random.Range(col.bounds.extents.y + transform.position.y, -col.bounds.extents.y + transform.position.y);
            float spawnZ = Random.Range(col.bounds.extents.z + transform.position.z, -col.bounds.extents.z + transform.position.z);
            spawnLocation = new Vector3(spawnX, spawnY, spawnZ);
        }
        else
        {
            float spawnY = Random.Range(col.bounds.extents.y + transform.position.y, -col.bounds.extents.y + transform.position.y);
            float spawnZ = Random.Range(col.bounds.extents.z + transform.position.z, -col.bounds.extents.z + transform.position.z);
            spawnLocation = new Vector3(-col.bounds.extents.x + cloudAlphaDist + transform.position.x, spawnY, spawnZ);
        }

        GameObject newCloud = Instantiate(cloudPrefab, transform);
        newCloud.transform.position = spawnLocation;

        float size = Random.Range(cloudSizeMin, cloudSizeMax);
        float sizeX = Random.Range(variationMin, variationMax);
        float sizeY = Random.Range(variationMin, variationMax);
        float sizeZ = Random.Range(variationMin, variationMax);

        newCloud.transform.localScale = new Vector3(size + sizeX, size + sizeY, size + sizeZ);
    }
}
