using UnityEngine;
using UnityEngine.Pool;

public class Cannon : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float spawnOffset = 0.2f;
    [SerializeField] int defaultCapacity = 10;
    [SerializeField] int maxPoolSize = 20;

    Camera cam;
    ObjectPool<Projectile> pool;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(
            createFunc: () =>
            {
                Projectile p = Instantiate(projectilePrefab);
                p.SetPool(pool.Release);
                return p;
            },
            actionOnGet:     p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => Destroy(p.gameObject),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize);
    }

    void Start() => cam = Camera.main;

    // Hook this to the FIRE button's OnClick
    public void Fire()
    {
        if (cam == null) cam = Camera.main;

        Vector3 pos = cam.transform.position + cam.transform.forward * spawnOffset;
        Projectile p = pool.Get();
        p.Launch(pos, cam.transform.forward);
    }
}