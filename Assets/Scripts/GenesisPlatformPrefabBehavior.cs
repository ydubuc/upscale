using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenesisPlatformPrefabBehavior : MonoBehaviour {
    private float timer = 0f;

    void Update() {
        timer += Time.deltaTime;
        if (timer > 8) {
            OnCollisionExit(null);
        }
    }

    private void OnCollisionExit(Collision collision) {
        Destroy(this.gameObject);
    }
}
