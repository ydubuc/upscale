using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject platformPrefab = default;
    [SerializeField] GameObject genesisPlatformPrefab = default;
    GameObject[] platforms = new GameObject[8];
    [SerializeField] float spawnHeight = 15f;
    [SerializeField] float minSpawnRate = 2f;
    [SerializeField] float maxSpawnRate = 5f;
    float spawnRate = 3f;
    float timeSinceLastSpawn = 3f;
    int nextInLine = 0;

    void Start() {
        InstantiatePlatforms();
    }

    void InstantiatePlatforms() {
        for (int i = 0; i < platforms.Length; i++) {
            Vector3 spawnPosition = new Vector3(0f, -spawnHeight, 0f);
            platforms[i] = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }

        Instantiate(genesisPlatformPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    void Update() {
        PoolPlatforms();
    }

    void PoolPlatforms() {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnRate) {
            timeSinceLastSpawn = 0f;
            PoolPlatform(nextInLine);
        }
    }

    void PoolPlatform(int platformNum) {
        platforms[platformNum].SetActive(true);
        platforms[platformNum].transform.position = transform.position + RandomPlatformPosition();
        spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        nextInLine += 1;
        if (nextInLine == platforms.Length) {
            nextInLine = 0;
        }
    }

    Vector3 RandomPlatformPosition() {
        return new Vector3(Random.Range(-3.75f, 3.75f), spawnHeight, 0f);
    }
}
