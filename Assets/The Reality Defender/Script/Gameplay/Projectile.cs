using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float maxDistance = 5f;

    Rigidbody rb;
    Vector3 startPos;
    Action<Projectile> releaseToPool;
    bool inFlight;

    void Awake() => rb = GetComponent<Rigidbody>();

    // Called once by the pool when this projectile is created
    public void SetPool(Action<Projectile> release) => releaseToPool = release;

    public void Launch(Vector3 position, Vector3 direction)
    {
        transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
        startPos = position;

        // Reset physics state so no old velocity carries over from the last shot
        rb.linearVelocity = Vector3.zero;          // Unity 2022: rb.velocity
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = direction.normalized * speed;

        inFlight = true;
    }

    void Update()
    {
        // Miss: flew more than 5 m, so return it to the pool
        if (inFlight && (transform.position - startPos).sqrMagnitude > maxDistance * maxDistance)
            Despawn();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Hit anything: return to pool (Target handles its own destruction)
        if (inFlight) Despawn();
    }

    void Despawn()
    {
        inFlight = false;          // prevents double-release (hit + distance in the same frame)
        releaseToPool?.Invoke(this);
    }
}