using System.Collections;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject monsterPlantPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameManager gameManager;

    [Header("Config")] 
    [SerializeField] private float minSpawnRate = 4;
    [SerializeField] private float maxSpawnRate = 5;

    private void Start()
    {
        StartCoroutine(SpawnPlants());
    }

    private IEnumerator SpawnPlants()
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var plant = Instantiate(monsterPlantPrefab);
        plant.transform.position = spawnPoint.position;
        var controller = plant.GetComponent<EnemyController>();
        controller.player = player;
        controller.gameManager = gameManager;

        var spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnPlants());
    }
}