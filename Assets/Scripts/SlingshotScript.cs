using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour {
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
    }

    void RespondToInputs() {
        if (!isGrounded) { return; }
#if UNITY_IOS || UNITY_ANDROID
        RespondToMobileInput();
#elif UNITY_STANDALONE
        RespondToStandaloneInput();
#endif
    }

    void RespondToMobileInput() {
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
            AnimateJump(true);
        }
    }

    void RespondToStandaloneInput() {
        if (Input.GetButtonDown("Fire1")) {
            touchOrigin = Input.mousePosition;
        } else if (Input.GetButtonUp("Fire1")) {
            touchEnd = Input.mousePosition;
            Vector3 jumpForce = touchOrigin - touchEnd;
            player.AddForce(Vector3.ClampMagnitude(jumpForce, 1100));
            AnimateJump(true);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "Ground") { return; }
        AnimateJump(false);
    }

    void AnimateJump(bool newState) {
        isGrounded = !newState;
        animator.SetBool("Grounded", !newState);
    }
}
