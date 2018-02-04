using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInstance {

    private float scale;
    private float duration;
    public float Duration { get { return duration; } set { duration = value; } }
    public float Scale { get { return scale; } }

    public TimeInstance(float timeScale, float timeDuration) {
        scale = timeScale;
        duration = timeDuration;
    }

}
