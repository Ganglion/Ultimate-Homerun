using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterLife : MonoBehaviour {

    [SerializeField]
    private Transform lifeIndicator;
    [SerializeField]
    private ParticleSystem heartHitEffect;
    [SerializeField]
    private Animator batterAnimator;

    [SerializeField]
    private float maxLife;

    private float currentLife;

    private void Awake() {
        currentLife = maxLife;
    }

    protected void Update() {
        lifeIndicator.transform.localScale = new Vector3(1, currentLife / maxLife, 1);
    }

    public void TakeDamage(float damage) {
        if (damage > 0) {
            currentLife -= damage;
            if (currentLife <= 0) {
                BatterDeath();
            } else {
                //batterAnimator.SetTrigger("Take Damage");
                if (!heartHitEffect.isPlaying) {
                    heartHitEffect.Play();
                }
            }
        }
    }

    public void BatterDeath() {
        gameObject.SetActive(false);
        GameController.Instance.GameOver();
    }

}
