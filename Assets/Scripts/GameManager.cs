using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // dependencies
    [SerializeField] Transform player = default;
    [SerializeField] GameObject platformPrefab = default;
    [SerializeField] GameObject tower1 = default;
    [SerializeField] GameObject tower2 = default;

    // constants
    private float spawnRate = 1f;
    private float minSpawnHeightDiff = 4f;
    private float maxSpawnHeightDiff = 7f;

    // injected variables
    [SerializeField] float platformSpeed = 1.5f;

    // runtime variables
    private GameObject[] platforms = new GameObject[8];
    
    private float timeSinceLastSpawn = 0f;
    private float spawnHeight = 0f;
    private int nextPlatformIndex = 0;

    private float towerScaleY = 0f;
    private float nextTowerPosY = 0f;
    private int nextTowerIndex = 1;

    // functions
    void Start() {
        towerScaleY = tower1.transform.localScale.y;
        nextTowerPosY = towerScaleY;
        ConfigureLevel();
    }

    private void ConfigureLevel() {
        player.gameObject.SetActive(false);

        for (int i = 0; i < platforms.Length; i++) {
            Vector3 spawnPosition = new Vector3(0f, -30f, 0f);
            platforms[i] = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }

        PoolPlatform(0);
        Vector3 offset = new Vector3(0f, 3f, 0f);
        player.position = platforms[0].transform.position + offset;
        player.gameObject.SetActive(true);
    }

    void Update() {
        spawnHeight -= platformSpeed * Time.deltaTime;
        UpdateTowerPos();
        PoolPlatforms();
    }

    private void UpdateTowerPos() {
        if (player.position.y < (nextTowerPosY - towerScaleY)) { return; }
        GameObject tower = nextTowerIndex == 0 ? tower1 : tower2;
        nextTowerIndex = nextTowerIndex == 0 ? 1 : 0;
        tower.transform.position = new Vector3(0f, nextTowerPosY, 0f);
        nextTowerPosY += towerScaleY;
    }

    private void PoolPlatforms() {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn < spawnRate) { return; }
        if ((spawnHeight - player.position.y) > 15f) { return; }
        timeSinceLastSpawn = 0f;
        PoolPlatform(nextPlatformIndex);
    }

    private void PoolPlatform(int index) {
        platforms[index].SetActive(true);
        platforms[index].transform.position = RandomPlatformPosition();
        nextPlatformIndex += 1;
        if (nextPlatformIndex == platforms.Length) {
            nextPlatformIndex = 0;
        }
    }

    private Vector3 RandomPlatformPosition() {
        spawnHeight += Random.Range(minSpawnHeightDiff, maxSpawnHeightDiff);
        return new Vector3(Random.Range(-3.75f, 3.75f), spawnHeight, 0f);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
