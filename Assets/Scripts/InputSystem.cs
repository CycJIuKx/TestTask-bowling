using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public Camera cam;
    public LayerMask mask;
    public Vector3 touchPoint;
    public Controller controller;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] cols = Physics.RaycastAll(ray, 50, mask);
            if (cols.Length>0)
            {
                touchPoint = cols[0].point;
                 controller.TranslateGroup(touchPoint);
               
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            controller.StopTranslateGroup();
        }


    }
}
