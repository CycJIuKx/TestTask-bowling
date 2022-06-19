using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHandler : MonoBehaviour
{

    [SerializeField] Vector3 rotateSpeed;
    private Transform myTransform;
    void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    
    void Update()
    {
        myTransform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
