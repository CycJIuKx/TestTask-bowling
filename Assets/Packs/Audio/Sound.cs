using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XOR
{
    public class Sound : MonoBehaviour
    {
        private AudioSource source;
        private void Update()
        {
            if (!source.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        public void Init(Clip clip, Vector3 pos, bool Is2D)
        {

            source = GetComponent<AudioSource>();
            if (Is2D) source.spatialBlend = 0;
            else source.spatialBlend = 1;

            transform.position = pos;
            source.clip = clip.clip;
            source.volume = clip.volume;
            source.Play();
        }
    }
}