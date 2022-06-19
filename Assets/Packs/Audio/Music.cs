using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XOR
{

    public class Music : MonoBehaviour
    {
        public static Music instance;
        [System.Serializable]
        public class SceneCLips
        {
            public string sceneName;
            public List<Clip> clips;
        }
        /// <summary>
        /// список музыки на этой сцене
        /// </summary>
        [SerializeField] private List<SceneCLips> sceneClips;
        [Header("Drag & Drop")]
        [SerializeField] private AudioSource source;
        [SerializeField] private bool ReplayWhenStop = true;
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private float slowSwapSpeedFactor = 1;

        private Coroutine slowCor = null;
        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                if (instance != null)
                {
                    if (instance != this)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                    instance = this;
                }
            }
            else instance = this;
        }


        void Update()
        {
            if (!source.isPlaying && ReplayWhenStop)
            {
                Clip c = GetRandomClip();
                PlayThatMusicFast(c);
            }
        }


        IEnumerator SlowSwapClip(Clip clip)
        {
            while (source.volume > 0)
            {
                source.volume = Mathf.MoveTowards(source.volume, 0, Time.unscaledDeltaTime * slowSwapSpeedFactor);
                yield return null;
            }

            source.clip = clip.clip;
            source.Play();

            while (source.volume < clip.volume)
            {
                source.volume = Mathf.MoveTowards(source.volume, clip.volume, Time.unscaledDeltaTime * slowSwapSpeedFactor);
                yield return null;
            }

        }

        /// <summary>
        /// Переключает музыку на выбранную с плавным затуханием
        /// </summary>
        /// <param name="m"></param>
        public void PlayThatMusicSlow(Clip m)
        {

            if (slowCor != null) StopCoroutine(slowCor);
            slowCor = StartCoroutine(SlowSwapClip(m));
        }  /// <summary>
           /// Немедленно переключает музыку на выбранную
           /// </summary>
           /// <param name="m"></param>
        public void PlayThatMusicFast(Clip m)
        {
            if (slowCor != null) StopCoroutine(slowCor);
            source.clip = m.clip;
            source.volume = m.volume;
            source.Play();
        }

        private Clip GetRandomClip()
        {
            List<Clip> clips = null;
            foreach (var item in sceneClips)
            {
                if (item.sceneName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    clips = item.clips;
                    break;
                }
            }
            if (clips == null) { Debug.LogError("Не найдено музыки для этой сцены"); return null; }

            Clip c = clips.GetRandomElement();
            if (clips.Count > 1)
            {
                while (true)
                {
                    if (c.clip != source.clip)
                    {
                        return c;
                    }
                    else
                    {
                        c = clips.GetRandomElement();
                    }

                }
            }
            else
            {
                return c;
            }
        }
    }
}