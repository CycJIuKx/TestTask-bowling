using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        Ball ball = other.GetComponent<Ball>();
        if (ball)
        {
            ball.Die();
        }
    }
}
