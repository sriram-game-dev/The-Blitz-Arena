using UnityEngine;
using UnityEngine.Pool;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float spawnOffset = 0.3f;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 20;

    [Header("Effects")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private AudioClip fireSound;
    [SerializeField, Range(0f, 1f)] private float fireVolume = 0.7f;

    private Camera cam;
    private ObjectPool<Projectile> pool;

    private bool canFire = false;

    private void Awake()
    {
        pool = new ObjectPool<Projectile>(
            createFunc: () =>
            {
                Projectile p = Instantiate(projectilePrefab);
                p.SetPool(pool.Release);
                return p;
            },
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => Destroy(p.gameObject),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize
        );
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void SetCanFire(bool value)
    {
        canFire = value;
    }

    // FIRE BUTTON
    public void Fire()
    {
        if (!canFire)
            return;

        if (cam == null)
            cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Cannon: Camera not found.");
            return;
        }

        UiManager.Instance.FiredOnce(true);

        Vector3 pos = cam.transform.position + cam.transform.forward * spawnOffset;

        Projectile p = pool.Get();
        p.Launch(pos, cam.transform.forward);

        // Muzzle flash
        if (muzzleFlashPrefab != null)
            Instantiate(muzzleFlashPrefab, pos, cam.transform.rotation);

        // Fire sound
        if (fireSound != null)
            AudioSource.PlayClipAtPoint(fireSound, pos, fireVolume);
    }
}