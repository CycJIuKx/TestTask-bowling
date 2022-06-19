using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XOR;
public class Skittle : MonoBehaviour
{
    [SerializeField] Vector2 forceX, forceY, forceZ;
  
    [SerializeField] Clip collisionClip;

    private Rigidbody rb;
    private bool activated = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            XOR.SoundCreator.Create(collisionClip);
            if (!activated) ball.Die();
            if (!activated) Push();
            activated = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball)
        {
            XOR.SoundCreator.Create(collisionClip);
            if (!activated) ball.Die();
            if (!activated) Push();
            activated = true;
        }
    }
    void Push()
    {
        Vector3 force = new Vector3(forceX.GetRandom(), forceY.GetRandom(), forceZ.GetRandom());

        rb.AddForce(force, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }
}
