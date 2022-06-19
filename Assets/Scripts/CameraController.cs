using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    public Vector3 positionOffset, rotationOffset;


    private Vector3 basePosOffset, baseRootationOffset;
    private Transform myTransform;
    private Vector3 targerPos = new Vector3();
    void Awake()
    {
        instance = this;
        myTransform = GetComponent<Transform>();
    }



    void Start()
    {
        basePosOffset = positionOffset;
        baseRootationOffset = rotationOffset;
    }


    void Update()
    {
        targerPos = target.position;
        targerPos.x = 0;

        myTransform.position = Vector3.MoveTowards(myTransform.position, targerPos + positionOffset, 33 * Time.deltaTime);
        myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, Quaternion.Euler(rotationOffset), 33 * Time.deltaTime);

    }
    public void ChangeViweToShoot(Vector3 pos, Vector3 rot)
    {
        positionOffset = pos;
        rotationOffset = rot;
    }
}
