using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterMovement : MonoBehaviour {

    private Rigidbody2D batterRB;

    // Variables
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;

    private float speedMultiplier = 1;

    private void Awake() {
        batterRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Vector2 directionVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            directionVector.y += 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            directionVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            directionVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            directionVector.x += 1;
        }
        Vector2 targetVelocity = directionVector.normalized * maxSpeed * speedMultiplier;
        batterRB.velocity = Vector2.MoveTowards(batterRB.velocity, targetVelocity, acceleration * Time.deltaTime);
    }

    public void SetSpeedMultiplier(float multiplier) {
        speedMultiplier = multiplier;
    }

    public void ResetSpeedMultiplier() {
        speedMultiplier = 1;
    }

}
