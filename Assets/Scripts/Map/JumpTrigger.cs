using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] XOR.Clip clip;
    public float JumpHeight = 3;
    public float JumpSpeed = 5;

    private bool used;


    private void OnTriggerEnter(Collider other)
    {

        Ball ball = other.GetComponent<Ball>();
        if (ball)
        {
            if(!used)XOR.SoundCreator.Create(clip);
             used = true;

            ball.PushToUp(JumpHeight, JumpSpeed);
        }
    }
}
