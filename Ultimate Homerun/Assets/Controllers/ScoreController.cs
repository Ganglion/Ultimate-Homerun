using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : Singleton<ScoreController> {

    private Text scoreText;

    [SerializeField]
    private float scoreIncrementSpeed;

    private float targetScore = 0;
    private float score = 0;

    private void Awake() {
        scoreText = GetComponent<Text>();
    }

    private void Update() {
        score = Mathf.MoveTowards(score, targetScore, scoreIncrementSpeed * Time.deltaTime);
        scoreText.text = Mathf.Floor(score).ToString();
    }

    public void AddScore(float amount) {
        targetScore += amount;
    }

    public void ResetScore() {
        score = 0;
        targetScore = 0;
    }

}
