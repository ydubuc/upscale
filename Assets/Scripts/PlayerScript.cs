using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    // dependencies
    [SerializeField] private Rigidbody player = default;

    // lateinit dependencies
    private Animator animator;
    
    // constants
    private float deactivationPos = 25f;

    // runtime variables
    private Vector3 touchOrigin;
    private Vector3 touchEnd;
    private float highestPos = 0f;
    private bool isGrounded = true;
    
    // functions
    void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("Grounded", true);
    }

    void Update() {
        VerifyPlayerPosition();
        RespondToInputs();
    }

    private void VerifyPlayerPosition() {
        if (transform.position.y > highestPos) {
            highestPos = transform.position.y;
        }
        if (transform.position.y < highestPos - deactivationPos) {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        } else if (touch.phase == TouchPhase.Ended) {
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
        if (force.y < 0f || force.magnitude < 50) { return; }
        transform.parent = null;
        player.constraints = RigidbodyConstraints.None;
        player.constraints = RigidbodyConstraints.FreezeRotation;
        player.AddForce(Vector3.ClampMagnitude(force, 650));

        if (force.x > 0) {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                90,
                transform.eulerAngles.z
            );
        } else {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                -90,
                transform.eulerAngles.z
            );
        }

        SetJumpAnimation(true);
    }

    private void OnTriggerEnter(Collider collider) {
        if (player.velocity.y > Mathf.Epsilon ||
            collider.gameObject.tag != "Platform") { return; }
        Land(collider);
    }

    private void Land(Collider collider) {
        player.constraints = RigidbodyConstraints.FreezeAll;
        player.velocity = Vector3.zero;
        transform.SetParent(collider.transform);
        SetJumpAnimation(false);
    }

    private void SetJumpAnimation(bool isJumping) {
        isGrounded = !isJumping;
        animator.SetBool("Grounded", !isJumping);
    }
}
