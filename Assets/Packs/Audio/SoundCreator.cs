using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XOR
{
    [System.Serializable]
    public class Clip
    {
        public AudioClip clip;
        public float volume = 0.5f;
    }
    public class SoundCreator : MonoBehaviour
    {
        private static SoundCreator instance;
        [SerializeField] private GameObject soundPrefab;

        private void Awake()
        {
            instance = this;
        }

        public static GameObject Create(Clip clip, Vector3 pos)
        {
            GameObject go = Instantiate(instance.soundPrefab, instance.transform);
            go.GetComponent<Sound>().Init(clip, pos, false);
            return go;
        }
        public static GameObject Create(Clip clip)
        {
            GameObject go = Instantiate(instance.soundPrefab, instance.transform);
            go.GetComponent<Sound>().Init(clip, Vector3.zero, true);

            return go;
        }
    }
}