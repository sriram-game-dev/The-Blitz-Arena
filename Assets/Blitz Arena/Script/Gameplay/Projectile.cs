using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float maxDistance = 5f;

    private Rigidbody rb;

    private Vector3 startPos;

    private Action<Projectile> releaseToPool;

    private bool inFlight;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void SetPool(Action<Projectile> release)
    {
        releaseToPool = release;
    }


    public void Launch(Vector3 position, Vector3 direction)
    {
        transform.SetPositionAndRotation(
            position,
            Quaternion.LookRotation(direction)
        );

        startPos = position;

        // Unity 2022
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.linearVelocity =
            direction.normalized * speed;

        inFlight = true;
    }


    private void Update()
    {
        if (inFlight &&
            (transform.position - startPos).sqrMagnitude >
            maxDistance * maxDistance)
        {
            Despawn();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (inFlight)
        {
            Despawn();
        }
    }


    private void Despawn()
    {
        inFlight = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        releaseToPool?.Invoke(this);
    }
}