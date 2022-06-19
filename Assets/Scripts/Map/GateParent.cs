using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateParent : MonoBehaviour
{
    [SerializeField] List<Gate> childs;
        
    void Start()
    {
        foreach (var item in childs)
        {
            item.OnActivate += OnAnyGateActivated;
        }
    }

    void OnAnyGateActivated()
    {
        foreach (var item in childs)
        {
            item.RemoveGate();
        }
    }
}
