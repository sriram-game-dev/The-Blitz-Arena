using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField, Range(0f, 1f)] private float explosionVolume = 1f;

    [Header("Gameplay")]
    [SerializeField] private int points = 10;

    private bool dead;

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore anything that isn't a projectile, and never score twice
        if (dead || collision.collider.GetComponent<Projectile>() == null)
            return;

        dead = true;

        // Explosion VFX
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Explosion sound (plays on its own temporary object, so Destroy won't cut it off)
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);

        // Score and wave logic
        if (GameManager.Instance != null)
            GameManager.Instance.OnTargetDestroyed(points);

        // Remove the target from the scene
        Destroy(gameObject);
    }
}