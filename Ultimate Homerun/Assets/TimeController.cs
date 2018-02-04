using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController> {

    private float initialTimeScale = 1;
    private List<TimeInstance> timeInstances;

    void Awake() {
        timeInstances = new List<TimeInstance>();
    }

    void Update() {
        float currentTimeScale = initialTimeScale;
        for (int i = 0; i < timeInstances.Count; i++) {
            TimeInstance currentTimeInstance = timeInstances[i];
            if (currentTimeInstance.Duration <= 0) {
                timeInstances.RemoveAt(i);
            } else {
                currentTimeScale = Mathf.Min(currentTimeScale, currentTimeInstance.Scale);
                currentTimeInstance.Duration = currentTimeInstance.Duration - Time.unscaledDeltaTime;
            }
        }
        Time.timeScale = currentTimeScale;
    }

    public void SlowTime(float scale, float duration) {
        TimeInstance newTimeInstance = new TimeInstance(scale, duration);
        timeInstances.Add(newTimeInstance);
    }

}
