using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPrefabBehavior : MonoBehaviour {
    [SerializeField] float platformSpeed = 1.5f;
    Vector3 platformPosition;

    void Start() {
        platformPosition.Set(0f, platformSpeed, 0f);
    }

    void Update() {
        MovePlatform();
        if (transform.position.y <= -10f) {
            gameObject.SetActive(false);
        }
    }

    void MovePlatform() {
        transform.position -= platformPosition * Time.deltaTime;
    }
}
