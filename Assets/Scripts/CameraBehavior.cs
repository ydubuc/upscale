using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {
	// dependencies
    [SerializeField] Transform player = default;

	// constants
	private Vector3 velocity = Vector3.zero;

	// injected variables
	[SerializeField] float smoothSpeed = 0.25f;

	// runtime variables
	private Vector3 offset;

	// functions
	void Start() {
		float zOffset = transform.position.z - player.position.z;
		offset.Set(0f, 0f, zOffset);
	}

	void LateUpdate() {
		SmoothFollowTarget();
	}

	private void SmoothFollowTarget() {
		Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, TargetPos(), ref velocity, smoothSpeed);
		transform.position = smoothedPos;
	}

	private Vector3 TargetPos() {
		Vector3 targetPos = player.position + offset;
		targetPos.x = 0f;
		return targetPos;
	}
}
