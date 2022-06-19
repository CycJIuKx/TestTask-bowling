using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
  [SerializeField]  ShotSystem shootSystem;
    [SerializeField] XOR.Clip onStartClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            XOR.SoundCreator.Create(onStartClip);
            shootSystem.StartProcess();
        }
    }
}
