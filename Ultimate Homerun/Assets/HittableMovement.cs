using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableMovement : MonoBehaviour {

    protected Rigidbody2D objectRB;

    protected virtual void Awake() {
        objectRB = GetComponent<Rigidbody2D>();
    }

    public virtual void Hit(float speed, Vector2 direction) {
        objectRB.velocity = speed * direction.normalized;
    }

}
