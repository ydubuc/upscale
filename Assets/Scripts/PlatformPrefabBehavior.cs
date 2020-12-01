using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPrefabBehavior : MonoBehaviour {
    // lateinit constants
    private Vector3 platformSpeedVector;

    // injected variables
    [SerializeField] float platformSpeed = 1.5f;

    // functions
    void Start() {
        platformSpeedVector.Set(0f, platformSpeed, 0f);
    }

    void Update() {
        MovePlatform();
    }

    private void MovePlatform() {
        transform.position -= platformSpeedVector * Time.deltaTime;
    }
}
