using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSystem : MonoBehaviour
{
    [SerializeField] BonusBall ball;
    [SerializeField] Controller controller;
    [SerializeField] GameObject ballGo;
    public Vector3 camPososition, camRotation;

    public float rotateSpeed;

    public Transform arrow;
    float currentAngle = -90;
    bool process = false;
    public void StartProcess()
    {
        ballGo.SetActive(true);
        controller.BlockMovement();
        controller.MakeHugeBall(ball, OnBallsTransformed);
    }

    private void OnBallsTransformed()
    {
        CreateArrow();
    }

    private void CreateArrow()
    {
        StartCoroutine(arrowMovingProcess());
    }
    IEnumerator arrowMovingProcess()
    {
      
       
        ChangeCameraView();
        yield return new WaitForSeconds(1);
        arrow.gameObject.SetActive(true);
        process = true;
        // while (!Input.GetMouseButtonDown(0))
        while (true)
        {
            yield return null;
            while (currentAngle != 90)
            {
               // arrow.rotation = Quaternion.RotateTowards(arrow.rotation, Quaternion.Euler(0,90,0),Time.deltaTime * rotateSpeed);
             currentAngle = Mathf.MoveTowards(currentAngle, 90, Time.deltaTime * rotateSpeed);
             arrow.rotation = Quaternion.Euler(0, currentAngle, 0);
                yield return null;
            }
            while (currentAngle != -90)
            {
               // arrow.rotation = Quaternion.RotateTowards(arrow.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * rotateSpeed);
                 currentAngle = Mathf.MoveTowards(currentAngle, -90, Time.deltaTime * rotateSpeed);
                 arrow.rotation = Quaternion.Euler(0, currentAngle, 0);
                yield return null;
            }

        }



    }
    public void Push()
    {
        StopAllCoroutines();
        arrow.gameObject.SetActive(false);
        ball.Push(arrow.forward.normalized);
       
    }

    private void ChangeCameraView()
    {
        CameraController.instance.ChangeViweToShoot(camPososition,camRotation);
    }

    void Update()
    {
        if (process)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Push();
                process = false;
                GameController.instance.OnShoot();
            }
        }
    }
}
