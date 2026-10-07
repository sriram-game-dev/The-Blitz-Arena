using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;   // optional, added in Part 2
    [SerializeField] int points = 10;

    bool dead;

    void OnCollisionEnter(Collision collision)
    {
        if (dead || collision.collider.GetComponent<Projectile>() == null) return;
        dead = true;

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.OnTargetDestroyed(points);
       gameObject.SetActive(false);
    }
}