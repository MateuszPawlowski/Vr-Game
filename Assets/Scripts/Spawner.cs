using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Variables
    [SerializeField] private float speedOfFruit;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeForSpawn = 2;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        // Have a clock that will check if every 2 seconds
        timer += Time.deltaTime;
        if(timer > timeForSpawn)
        {
            // Get one of the spawn points randomly
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            // Get a random prefab from the collection
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
            // Create that object in the scene
            GameObject spawnedPrefab = Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation);
            // Make the timer go back down
            timer -= timeForSpawn;

            // Move the fruit in a forward direction
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
            rb.velocity = randomPoint.forward * speedOfFruit;
        }
    }
}
