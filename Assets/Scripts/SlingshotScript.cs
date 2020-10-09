using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour {
    [SerializeField] Rigidbody player = default;

    Vector3 touchOrigin;
    Vector3 touchEnd;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        ListenForInputs();
    }

    void ListenForInputs() {
        if (Input.touchCount < 1) { return; }

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began) {
            touchOrigin = touch.position;
        } else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= Mathf.Epsilon) {
            touchEnd = touch.position;
            Vector3 jumpForce = touchOrigin - touchEnd;

            // transform.parent = null;
            // player.constraints = RigidbodyConstraints.None;
            // player.constraints = RigidbodyConstraints.FreezeRotation;

            player.AddForce(Vector3.ClampMagnitude(jumpForce, 1100));
        }
    }
}
