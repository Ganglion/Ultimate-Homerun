using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

    [SerializeField]
    private Transform batter;
    public Transform Batter { get { return batter; } }

    [SerializeField]
    private GameObject runner;
    [SerializeField]
    private float runnerBaseHealth;
    [SerializeField]
    private float runnerHealthIncrement;

    [SerializeField]
    private float antiSpawnRadius;
    [SerializeField]
    private float spawnInterval;

    private float currentRunnerHealth;
    private float currentSpawnInterval;

    private void Awake() {
        currentRunnerHealth = runnerBaseHealth;
        currentSpawnInterval = spawnInterval;
    }

    private void Update() {
        currentSpawnInterval -= Time.deltaTime;
        if (currentSpawnInterval <= 0) {
            Vector3 spawnPoint = Random.insideUnitCircle * 4.5f;
            while ((spawnPoint - batter.position).sqrMagnitude < Mathf.Pow(antiSpawnRadius, 2)) {
                spawnPoint = Random.insideUnitCircle * 4.5f;
            }
            GameObject newRunner = Instantiate(runner, spawnPoint, Quaternion.Euler(Vector3.zero));
            newRunner.GetComponent<RunnerLife>().SetMaxLife(currentRunnerHealth);
            currentRunnerHealth += runnerHealthIncrement;
            currentSpawnInterval += spawnInterval;
        }
    }

    public void GameOver() {
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame() {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
