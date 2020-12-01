using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {
    // dependencies
    [SerializeField] Transform player = default;
    [SerializeField] Text textViewScore = default;

    // runtime variables
    private float highestPos = 0f;

    // functions
    void Start() {
        UpdateScore();
    }

    void Update() {
        if (player.transform.position.y > highestPos) {
            highestPos = player.transform.position.y;
            UpdateScore();
        }
    }

    private void UpdateScore() {
        textViewScore.text = Mathf.FloorToInt(highestPos).ToString();
    }
}
