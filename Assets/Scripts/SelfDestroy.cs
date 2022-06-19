using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float delay;
    void Awake()
    {
        
    }

    
        
    void Start()
    {
        Destroy(gameObject, delay);
    }

    
    void Update()
    {
        
    }
}
