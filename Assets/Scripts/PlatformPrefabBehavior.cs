using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPrefabBehavior : MonoBehaviour {
    [SerializeField] float platformSpeed = 1.5f;
    private Vector3 speedVector;

    void Start() {
        speedVector.Set(0f, platformSpeed, 0f);
    }

    void Update() {
        MovePlatform();
        VerifyPlatformPosition();
    }

    private void MovePlatform() {
        transform.position -= speedVector * Time.deltaTime;
    }

    private void VerifyPlatformPosition() {
        if (transform.position.y > -15f) { return; }
        gameObject.SetActive(false);
    }
}
