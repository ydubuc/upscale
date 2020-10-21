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
        RespondToInputs();
        VerifyPlayerPosition();
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
        player.AddForce(Vector3.ClampMagnitude(force, 1100));
        AnimateJump(true);
    }

    private void AnimateJump(bool newState) {
        isGrounded = !newState;
        animator.SetBool("Grounded", !newState);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "Ground") { return; }
        AnimateJump(false);
    }

    private void VerifyPlayerPosition() {
        if (transform.position.y > -15f) { return; }
        gameObject.SetActive(false);
    }
}
