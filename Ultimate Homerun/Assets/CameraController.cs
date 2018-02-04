using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

	private Transform cameraTransform;
	private Vector3 initialCameraPosition;
	private List<ShakeInstance> shakeInstances;

	void Awake() {
		cameraTransform = transform.GetChild (0).transform;
		initialCameraPosition = cameraTransform.localPosition;
		shakeInstances = new List<ShakeInstance> ();
	}

	void Update() {
		float currentShakeIntensity = 0;
		for (int i = 0; i < shakeInstances.Count; i++) {
			ShakeInstance currentShakeInstance = shakeInstances [i];
			if (currentShakeInstance.Duration <= 0) {
				shakeInstances.RemoveAt (i);
			} else {
				currentShakeIntensity = Mathf.Max (currentShakeIntensity, currentShakeInstance.Intensity * currentShakeInstance.Duration / currentShakeInstance.FullDuration);
				currentShakeInstance.Duration = currentShakeInstance.Duration - Time.unscaledDeltaTime;
			}
		}
        if (shakeInstances.Count > 0) {
            cameraTransform.localPosition = initialCameraPosition + Random.insideUnitSphere * currentShakeIntensity;
        } else {
            cameraTransform.localPosition = initialCameraPosition;
        }
	}

	public void ShakeCamera(float intensity, float duration) {
		ShakeInstance newShakeInstance = new ShakeInstance (intensity, duration);
		shakeInstances.Add (newShakeInstance);
	}

}
