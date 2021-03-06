﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMovement : HittableMovement {

    private float colliderRadius;

    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float xScaleMultiplier;
    [SerializeField]
    private float yScaleMultiplier;
    [SerializeField]
    private float speedIncrement;

    [SerializeField]
    private float minDirectionChangeInterval;
    [SerializeField]
    private float maxDirectionChangeInterval;

    private float currentInterval;
    private Vector2 moveDirection;

    protected override void Awake() {
        base.Awake();
        colliderRadius = GetComponent<Collider2D>().bounds.extents.x;
        moveDirection = (GameController.Instance.Batter.position - transform.position).normalized;
    }

    public override void Hit(float speed, Vector2 direction) {
        base.Hit(speed / 4, direction);
    }

    private void Update() {
        float facingAngle = Mathf.Atan2(objectRB.velocity.y, objectRB.velocity.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, facingAngle);

        float xScale = colliderRadius * 2 + objectRB.velocity.magnitude * xScaleMultiplier;
        float yScale = colliderRadius * 2 + objectRB.velocity.magnitude * yScaleMultiplier;
        transform.localScale = new Vector3(xScale, yScale, transform.localScale.z);

        maxSpeed += speedIncrement * Time.deltaTime;

        if (currentInterval <= 0) {
            moveDirection = (GameController.Instance.Batter.position - transform.position).normalized;
            currentInterval += Random.Range(minDirectionChangeInterval, maxDirectionChangeInterval);
        } else {
            currentInterval -= Time.deltaTime;
        }

        if (objectRB.velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2)) {
            objectRB.velocity = Vector2.MoveTowards(objectRB.velocity, moveDirection * maxSpeed, deceleration * Time.deltaTime);
        } else {
            objectRB.velocity = Vector2.MoveTowards(objectRB.velocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        
    }

    private void FixedUpdate() {

        RaycastHit2D reboundHit = Physics2D.Raycast(transform.position, objectRB.velocity, colliderRadius + objectRB.velocity.magnitude * Time.deltaTime, LayerMask.GetMask("Wall"));
        if (reboundHit.collider != null) {
            Vector2 reboundNormal = reboundHit.normal;
            Vector2 reboundVelocity = Vector2.Reflect(objectRB.velocity, reboundNormal);
            objectRB.velocity = reboundVelocity;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Batter")) {
            other.GetComponent<BatterLife>().TakeDamage(1 * Time.deltaTime);
        }
    }

}
