using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    [SerializeField] private Rigidbody player = default;

    private Animator animator;
    private Vector3 touchOrigin;
    private Vector3 touchEnd;
    private bool isGrounded = true;

    void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", true);
    }

    void Update() {
        VerifyPlayerPosition();
        RespondToInputs();
    }

    private void VerifyPlayerPosition() {
        if (transform.position.y > -15f) { return; }
        gameObject.SetActive(false);
    }

    private void RespondToInputs() {
        if (!isGrounded) { return; }
#if UNITY_IOS || UNITY_ANDROID
        RespondToMobileInput();
#elif UNITY_STANDALONE
        RespondToStandaloneInput();
#endif
    }

    private void RespondToMobileInput() {
        if (Input.touchCount < 1) { return; }

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began) {
            touchOrigin = touch.position;
        } else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= Mathf.Epsilon) {
            touchEnd = touch.position;
            Jump(touchOrigin - touchEnd);
        }
    }

    private void RespondToStandaloneInput() {
        if (Input.GetButtonDown("Fire1")) {
            touchOrigin = Input.mousePosition;
        } else if (Input.GetButtonUp("Fire1")) {
            touchEnd = Input.mousePosition;
            Jump(touchOrigin - touchEnd);
        }
    }

    private void Jump(Vector3 force) {
        player.AddForce(Vector3.ClampMagnitude(force, 1300));
        transform.parent = null;
        player.constraints = RigidbodyConstraints.None;
        player.constraints = RigidbodyConstraints.FreezeRotation;
        AnimateJump(true);
    }

    private void OnTriggerEnter(Collider collider) {
        if (player.velocity.y > Mathf.Epsilon ||
        collider.gameObject.tag != "Platform") { return; }
        Land(collider);
    }

    private void Land(Collider collider) {
        transform.SetParent(collider.transform);
        player.constraints = RigidbodyConstraints.FreezeAll;
        player.velocity = Vector3.zero;
        AnimateJump(false);
    }

    private void AnimateJump(bool newState) {
        isGrounded = !newState;
        animator.SetBool("Grounded", !newState);
    }
}
