using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject platformPrefab = default;
    [SerializeField] GameObject genesisPlatformPrefab = default;
    private GameObject[] platforms = new GameObject[8];
    [SerializeField] float spawnHeight = 15f;
    [SerializeField] float minSpawnRate = 2f;
    [SerializeField] float maxSpawnRate = 5f;
    private float spawnRate = 3f;
    private float timeSinceLastSpawn = 3f;
    private int nextPlatformIndex = 0;

    void Start() {
        InstantiatePlatforms();
    }

    private void InstantiatePlatforms() {
        for (int i = 0; i < platforms.Length; i++) {
            Vector3 spawnPosition = new Vector3(0f, -spawnHeight, 0f);
            platforms[i] = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }

        Instantiate(genesisPlatformPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    void Update() {
        PoolPlatforms();
    }

    private void PoolPlatforms() {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnRate) {
            timeSinceLastSpawn = 0f;
            PoolPlatform(nextPlatformIndex);
        }
    }

    private void PoolPlatform(int index) {
        platforms[index].SetActive(true);
        platforms[index].transform.position = transform.position + RandomPlatformPosition();
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        nextPlatformIndex += 1;
        if (nextPlatformIndex == platforms.Length) {
            nextPlatformIndex = 0;
        }
    }

    private Vector3 RandomPlatformPosition() {
        return new Vector3(Random.Range(-3.75f, 3.75f), spawnHeight, 0f);
    }
}
