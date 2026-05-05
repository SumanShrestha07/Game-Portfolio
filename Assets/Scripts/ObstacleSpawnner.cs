using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnner : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclesPrefab;
    [SerializeField] private Transform obstacleSpawnPoint;

    [Header("Spawn Interval")]
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 4f;

    [Header("X Position Range")]
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;

    private void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            float interval = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            // Pick a random prefab from the list
            int randomIndex = UnityEngine.Random.Range(0, obstaclesPrefab.Count);
            GameObject prefab = obstaclesPrefab[randomIndex];

            // Randomize X position while keeping Y and Z from spawn point
            float randomX = UnityEngine.Random.Range(minX, maxX);
            Vector3 spawnPosition = new Vector3(
                obstacleSpawnPoint.position.x + randomX,
                obstacleSpawnPoint.position.y,
                obstacleSpawnPoint.position.z
            );

            Instantiate(prefab, spawnPosition, obstacleSpawnPoint.rotation);
        }
    }
}