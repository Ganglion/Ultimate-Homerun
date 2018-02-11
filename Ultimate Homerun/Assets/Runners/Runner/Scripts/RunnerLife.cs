using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerLife : MonoBehaviour {

    private Animator runnerAnimator;
    private ParticleSystem runnerHitEffect;
    private ParticleSystemLightingController runnerPSLighting;

    [SerializeField]
    protected Transform healthIndicator;
    [SerializeField]
    protected GameObject healthDeathEffect;

    [SerializeField]
    protected float maxHealth;

    protected float currentHealth;

    [SerializeField]
    protected float runnerScore;

    protected virtual void Awake() {
        currentHealth = maxHealth;
        runnerAnimator = GetComponent<Animator>();
        runnerHitEffect = GetComponent<ParticleSystem>();
        runnerPSLighting = GetComponent<ParticleSystemLightingController>();
    }

    protected void Update() {
        healthIndicator.transform.localScale = new Vector3(1, currentHealth / maxHealth, 1);
    }

    public virtual void TakeDamage(float damage) {
        if (damage > 0) {
            currentHealth -= damage;
            if (currentHealth <= 0) {
                RunnerDeath();
            } else {
                runnerAnimator.SetTrigger("Take Damage");
                runnerHitEffect.Play();
                runnerPSLighting.RunLighting();
            }
        }
    }

    protected virtual void RunnerDeath() {
        Instantiate(healthDeathEffect, transform.position, Quaternion.Euler(transform.eulerAngles));
        ScoreController.Instance.AddScore(runnerScore);
        gameObject.SetActive(false);
    }

    public void SetMaxLife(float amount) {
        maxHealth = amount;
        currentHealth = amount;
    }

}
