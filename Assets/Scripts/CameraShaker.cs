using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XOR
{

    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] float shakeTimeStap;
        public static CameraShaker instance;
        [SerializeField] Transform camTransform;
        private void Awake()
        {

            instance = this;
        }

        public void Shake(float time = 2f, float force = 8f)
        {

            StartCoroutine(shake(time, force));

        }

        IEnumerator shake(float time, float force)
        {
            float t = time;
            float z = camTransform.localEulerAngles.z;
            float s = shakeTimeStap;
            while (t > 0)
            {
                float zNew = z + Random.Range(-force, force);

                camTransform.localEulerAngles = new Vector3(camTransform.localEulerAngles.x, camTransform.localEulerAngles.y, zNew);
                t -= shakeTimeStap;
                yield return new WaitForSecondsRealtime(s);
                s *= 0.9f;
                force *= 0.9f;
                camTransform.localEulerAngles = new Vector3(camTransform.localEulerAngles.x, camTransform.localEulerAngles.y, 0);
            }



        }
    }
}