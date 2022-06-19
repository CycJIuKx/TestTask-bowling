using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XOR
{
    public class SlowMotionEffect : MonoBehaviour
    {
        bool isProcess;
        public static SlowMotionEffect instance;
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            Time.timeScale = 1;
        }

        public void Slow(float time, float start = 1, float end = 0.2f, float changeSpeed = 1)
        {
            if (!isProcess) StartCoroutine(slow(time, start, end, changeSpeed));
        }
        IEnumerator slow(float time, float start = 1, float end = 0.2f, float changeSpeed = 1)
        {
            isProcess = true;
            while (Time.timeScale != end)
            {
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, end, changeSpeed * Time.unscaledDeltaTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1);
            while (Time.timeScale != start)
            {
                Time.timeScale = Mathf.MoveTowards(Time.timeScale, start, changeSpeed * Time.unscaledDeltaTime);
                yield return null;
            }
            isProcess = false;
        }
    }
}