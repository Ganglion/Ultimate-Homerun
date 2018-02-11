using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemLightingController : MonoBehaviour {

    [SerializeField]
    private Light pointLight;

    [SerializeField]
    private float zTransform;
    [SerializeField]
    private float timeStartFade;
    [SerializeField]
    private float timeEndFade;
    [SerializeField]
    private float initialLightIntensity;

    [SerializeField]
    private float timeFade = 0;

    private void Awake() {
        transform.position = new Vector3(transform.position.x, transform.position.y, zTransform);
    }

    private void Update() {
        timeFade += Time.deltaTime;
        float lightMultiplier = 1 - Mathf.Clamp((timeFade - timeStartFade) / (timeEndFade - timeStartFade), 0, 1);
        pointLight.intensity = lightMultiplier * initialLightIntensity;
    }

    public void RunLighting() {
        timeFade = 0;
    }

}
