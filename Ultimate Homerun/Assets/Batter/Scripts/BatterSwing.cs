using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterSwing : MonoBehaviour {

    private BatterMovement batterMovement;
    private Animator batterAnimator;
    private AudioSource batterAudioSource;

    [SerializeField]
    private AudioClip batHitSFX;
    [SerializeField]
    private AudioClip smashHitSFX;
    [SerializeField]
    private AudioClip batGlintSFX;

    [SerializeField]
    private GameObject swingHitEffect;
    [SerializeField]
    private ParticleSystem fullChargeEffect;

    [SerializeField]
    private float minHitSpeed;
    [SerializeField]
    private float maxHitSpeed;
    [SerializeField]
    private float hitRadius;
    [SerializeField]
    private float hitAngle;
    [SerializeField]
    private float minSpeedMultiplier;
    [SerializeField]
    private float maxChargeTime;
    [SerializeField]
    private float minChargeTimeForEffect;
    [SerializeField]
    private float angularAcceleration;

    private bool isCharging = false;
    private bool isSwinging = false;
    private float chargeTime = 0;

    private void Awake() {
        batterMovement = GetComponent<BatterMovement>();
        batterAnimator = GetComponent<Animator>();
        batterAudioSource = GetComponent<AudioSource>();
    }

	private void Update () {

        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 directionToMouse = mousePosition - transform.position;
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angleToMouse - 90, angularAcceleration * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, nextAngle);

        if (Input.GetMouseButtonDown(0)) {
            isCharging = true;
        }
        if (Input.GetMouseButton(0) && !isSwinging) {
            batterAnimator.SetTrigger("Wind Up");
            chargeTime += Time.deltaTime;
            if (isCharging && chargeTime >= maxChargeTime) {
                fullChargeEffect.Play();
                isCharging = false;
                batterAudioSource.PlayOneShot(batGlintSFX);
            }
            batterMovement.SetSpeedMultiplier(minSpeedMultiplier);

        } else if (Input.GetMouseButtonUp(0)) {

            batterAnimator.SetTrigger("Swing");
            batterAnimator.ResetTrigger("Wind Up");
            StartCoroutine(ApplySwingDelay());

        }
        
    }

    public void SwingBat() {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.transform.position.z;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 directionToMouse = mousePosition - transform.position;
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        Collider2D[] objectsWithinSwingHitRadius = Physics2D.OverlapCircleAll(transform.position, hitRadius, LayerMask.GetMask("Hittable", "Runner"));
        for (int i = 0; i < objectsWithinSwingHitRadius.Length; i++)
        {
            Vector2 directionToObject = objectsWithinSwingHitRadius[i].transform.position - transform.position;
            float angleToObject = Mathf.Atan2(directionToObject.y, directionToObject.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(Mathf.DeltaAngle(angleToObject, angleToMouse)) <= (hitAngle / 2))
            {
                Instantiate(swingHitEffect, objectsWithinSwingHitRadius[i].transform.position, Quaternion.Euler(new Vector3(0, 0, angleToMouse - 90)));
            }
        }

        if (objectsWithinSwingHitRadius.Length > 0) {
            batterAudioSource.PlayOneShot(batHitSFX);

            float effectMultiplier = Mathf.Clamp(chargeTime - minChargeTimeForEffect, 0, maxChargeTime - minChargeTimeForEffect) / (maxChargeTime - minChargeTimeForEffect);
            CameraController.Instance.ShakeCamera(0.125f * effectMultiplier, .5f);
            TimeController.Instance.SlowTime(0.01f, 0.5f * effectMultiplier);

            for (int i = 0; i < objectsWithinSwingHitRadius.Length; i++) {
                Vector2 directionToObject = objectsWithinSwingHitRadius[i].transform.position - transform.position;
                float angleToObject = Mathf.Atan2(directionToObject.y, directionToObject.x) * Mathf.Rad2Deg;
                if (Mathf.Abs(Mathf.DeltaAngle(angleToObject, angleToMouse)) <= (hitAngle / 2)) {
                    objectsWithinSwingHitRadius[i].GetComponent<HittableMovement>().Hit(minHitSpeed + (maxHitSpeed - minHitSpeed) * Mathf.Clamp(chargeTime, 0, maxChargeTime) / maxChargeTime, directionToMouse);
                }
            }
        }

        chargeTime = 0;
        batterMovement.ResetSpeedMultiplier();
    }

    protected IEnumerator ApplySwingDelay() {
        isSwinging = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(batterAnimator.GetCurrentAnimatorStateInfo(0).length);
        isSwinging = false;
    }

}
