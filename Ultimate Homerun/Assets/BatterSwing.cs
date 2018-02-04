using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterSwing : MonoBehaviour {

    private BatterMovement batterMovement;

    [SerializeField]
    private float hitSpeed;
    [SerializeField]
    private float hitRadius;
    [SerializeField]
    private float hitAngle;
    [SerializeField]
    private float minSpeedMultiplier;
    [SerializeField]
    private float maxChargeTime;
    [SerializeField]
    private float angularAcceleration;

    private float chargeTime = 0;

    private void Awake() {
        batterMovement = GetComponent<BatterMovement>();
    }

	private void Update () {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 directionToMouse = mousePosition - transform.position;
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        //Debug.Log("ANGLAY MOUSO " + angleToMouse);
        float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angleToMouse - 90, angularAcceleration * Time.deltaTime);
        //Debug.Log("NEXTO ANGLAY " + nextAngle);
        transform.eulerAngles = new Vector3(0, 0, nextAngle);

        if (Input.GetMouseButton(0)) {
            chargeTime += Time.deltaTime;
            batterMovement.SetSpeedMultiplier(minSpeedMultiplier);
        } else if (Input.GetMouseButtonUp(0)) {

            CameraController.Instance.ShakeCamera(0.125f, .5f);
            TimeController.Instance.SlowTime(0f, 0.5f);

            Collider2D[] objectsWithinSwingHitRadius = Physics2D.OverlapCircleAll(transform.position, hitRadius, LayerMask.GetMask("Hittable"));
            for (int i = 0; i < objectsWithinSwingHitRadius.Length; i++) {
                Vector2 directionToObject = objectsWithinSwingHitRadius[i].transform.position - transform.position;
                float angleToObject = Mathf.Atan2(directionToObject.y, directionToObject.x) * Mathf.Rad2Deg;
                if (Mathf.Abs(Mathf.DeltaAngle(angleToObject, angleToMouse)) <= (hitAngle / 2)) {
                    objectsWithinSwingHitRadius[i].GetComponent<Rigidbody2D>().velocity = directionToObject.normalized * hitSpeed * Mathf.Clamp(chargeTime, 0, maxChargeTime) / maxChargeTime;
                }
            }
            chargeTime = 0;
            batterMovement.ResetSpeedMultiplier();
        }

        
    }
}
