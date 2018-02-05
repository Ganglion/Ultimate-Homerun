using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    [SerializeField]
    private Transform batter;
    public Transform Batter { get { return batter; } }

    [SerializeField]
    private GameObject runner;
    [SerializeField]
    private float spawnInterval;

    private float currentSpawnInterval;

    private void Awake() {
        currentSpawnInterval = spawnInterval;
    }

    private void Update() {
        currentSpawnInterval -= Time.deltaTime;
        if (currentSpawnInterval <= 0) {
            Instantiate(runner, Random.insideUnitCircle * 4.5f, Quaternion.Euler(Vector3.zero));
            currentSpawnInterval += spawnInterval;
        }
    }

}
