using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // determined by children positions
    private List<Vector3> spawnPoints;

    public int initialMaxEnemies;
    public float timeBetweenMaxEnemyIncrease;

    [Range(0,1)]
    public float minPercentExistingEnemies;

    private int maxEnemies;
    private static int enemiesAlive;
    private float nextMaxEnemyIncrease;

    public GameObject[] enemyPrefabs;

    private void Start()
    {
        spawnPoints = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i).position);
        }

        enemiesAlive = 0;
        maxEnemies = initialMaxEnemies;
        nextMaxEnemyIncrease = Time.time + timeBetweenMaxEnemyIncrease;
    }

    private void Update()
    {
        if (enemiesAlive < maxEnemies * minPercentExistingEnemies)
        {
            SpawnEnemies(maxEnemies - enemiesAlive);
        }

        if (Time.time >= nextMaxEnemyIncrease)
        {
            maxEnemies++;
            nextMaxEnemyIncrease = Time.time + timeBetweenMaxEnemyIncrease;
        }
    }

    private void SpawnEnemies(int count)
    {
        Vector3 playerPos = Player.instance.position;
        List<Vector3> orderedSpawnPoints = spawnPoints.OrderBy(point => (playerPos - point).sqrMagnitude).ToList();

        for (int i = 0; i < count; i++)
        {
            int spawnPointIndex = Random.Range(spawnPoints.Count / 2, spawnPoints.Count);
            SpawnEnemy(orderedSpawnPoints[spawnPointIndex]);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefab = enemyPrefabs[index];

        Instantiate(prefab, position, Quaternion.identity, null);
        enemiesAlive++;
    }

    public static void DecrementLivingEnemies()
    {
        enemiesAlive--;
    }
}
