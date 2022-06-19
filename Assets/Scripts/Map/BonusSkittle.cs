using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSkittle : MonoBehaviour
{
    [SerializeField] Transform topPoint;
    public bool isDown = false;
    private bool check = false;
  



    void Update()
    {
        if (check)
        {
            if (topPoint.position.y < 7)
            {
                isDown = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<BonusBall>() || collision.gameObject.GetComponent<BonusSkittle>())
        {
            check = true;
        }
    }
}
