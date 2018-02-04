using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballMovement : HittableMovement {

    private float colliderRadius;

    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float xScaleMultiplier;
    [SerializeField]
    private float yScaleMultiplier;

    protected override void Awake() {
        base.Awake();
        colliderRadius = GetComponent<Collider2D>().bounds.extents.x;
    }

    protected override void Hit(float speed, Vector2 unitDirection) {
        objectRB.velocity = speed * unitDirection;
    }

    private void Update() {
        float facingAngle = Mathf.Atan2(objectRB.velocity.y, objectRB.velocity.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, facingAngle);

        float xScale = colliderRadius * 2 + objectRB.velocity.magnitude * xScaleMultiplier;
        float yScale = colliderRadius * 2 + objectRB.velocity.magnitude * yScaleMultiplier;
        transform.localScale = new Vector3(xScale, yScale, transform.localScale.z);
    }

    private void LateUpdate() {
        objectRB.velocity = Vector2.MoveTowards(objectRB.velocity, Vector2.zero, deceleration * Time.deltaTime);

        RaycastHit2D reboundHit = Physics2D.Raycast(transform.position, objectRB.velocity, colliderRadius + objectRB.velocity.magnitude * Time.deltaTime, LayerMask.GetMask("Wall"));
        if (reboundHit.collider != null) {
            Vector2 reboundNormal = reboundHit.normal;
            Vector2 reboundVelocity = Vector2.Reflect(objectRB.velocity, reboundNormal);
            objectRB.velocity = reboundVelocity;
        }
    }

}
