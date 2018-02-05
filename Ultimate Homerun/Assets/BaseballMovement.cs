using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballMovement : HittableMovement {

    private float colliderRadius;

    [Header("Baseball Movement")]
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float xScaleMultiplier;
    [SerializeField]
    private float yScaleMultiplier;

    [Header("Baseball Hit")]
    [SerializeField]
    private float speedTransferMultiplier;
    [SerializeField]
    private float damageMultiplier;

    protected override void Awake() {
        base.Awake();
        colliderRadius = GetComponent<Collider2D>().bounds.extents.x;
    }

    public override void Hit(float speed, Vector2 unitDirection) {
        base.Hit(speed, unitDirection);
    }

    private void Update() {
        float facingAngle = Mathf.Atan2(objectRB.velocity.y, objectRB.velocity.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, facingAngle);

        float xScale = colliderRadius * 2 + objectRB.velocity.magnitude * xScaleMultiplier;
        float yScale = colliderRadius * 2 + objectRB.velocity.magnitude * yScaleMultiplier;
        transform.localScale = new Vector3(xScale, yScale, transform.localScale.z);
    }

    private void FixedUpdate() {
        objectRB.velocity = Vector2.MoveTowards(objectRB.velocity, Vector2.zero, deceleration * Time.deltaTime);

        if (objectRB.velocity != Vector2.zero) {
            RaycastHit2D reboundHit = Physics2D.CircleCast(transform.position, colliderRadius, objectRB.velocity, objectRB.velocity.magnitude * Time.deltaTime + 0.005f, LayerMask.GetMask("Wall", "Runner"));
            if (reboundHit.collider != null) {
                Transform hitObjectTransform = reboundHit.transform;
                if (hitObjectTransform.gameObject.layer == LayerMask.NameToLayer("Runner")) {
                    hitObjectTransform.GetComponent<RunnerLife>().TakeDamage(objectRB.velocity.magnitude * damageMultiplier);
                    hitObjectTransform.GetComponent<Rigidbody2D>().velocity += objectRB.velocity * speedTransferMultiplier;
                }
                Vector2 reboundNormal = reboundHit.normal;
                Vector2 reboundVelocity = Vector2.Reflect(objectRB.velocity, reboundNormal);
                objectRB.velocity = reboundVelocity;

                CameraController.Instance.ShakeCamera(0.03125f, .25f);
            }
        }
    }

}
