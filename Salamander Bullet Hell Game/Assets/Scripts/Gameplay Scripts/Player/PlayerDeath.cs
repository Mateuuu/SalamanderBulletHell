using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerDeath : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] BoxCollider2D playerCollider;
    [SerializeField] BoxCollider2D recoilCollider;
    [SerializeField] CircleCollider2D shieldCollider;
    [Header("Renderers")]
    [SerializeField] SpriteRenderer shieldSpriteRenderer;
    [SerializeField] SpriteRenderer recoilSpriteRenderer;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [Header("Other")]
    [SerializeField] ParticleSystem playerDeathParticles;
    [SerializeField] CameraController cameraController;
    [SerializeField] Rigidbody2D playerRB;

    public static event Action playerDeath;

    public void Die()
    {
        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        playerDeath?.Invoke();
        playerSpriteRenderer.enabled = false;
        recoilSpriteRenderer.enabled = false;
        shieldSpriteRenderer.enabled = false;

        recoilCollider.enabled = false;
        shieldCollider.enabled = false;
        playerCollider.enabled = false;

        playerDeathParticles.Play();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("UpgradesMenu");
    }
}
