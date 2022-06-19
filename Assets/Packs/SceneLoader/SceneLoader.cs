
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace XOR
{
    public class SceneLoader : MonoBehaviour
    {
        [System.Serializable]
        public struct Tip
        {
            [Tooltip("Загружаемая сцена ")]
            public string sceneName;
            [Tooltip("Подсказки на экране загрузки в эту сцену ")]
            [TextArea(5, 5)]
            public List<string> tipText;
            [Tooltip("Короткий звук проигрывания во время начала загрузки (опционально)")]
            public Clip clipIntro;
            [Tooltip("Доп картинка поверх бекграунда загрузки, опционально")]
            public Sprite sideSprite;

        }

        public static SceneLoader instance;

        [SerializeField] private List<Tip> tips;
        [Header("Drag & Drop")]
        [Tooltip("Text который появляетяс на загрузочном экране")]
        [SerializeField] private Text tipText;
        [Tooltip("Текс появляется на загрузочном экране, После загрузки новой сцены")]
        [SerializeField] private GameObject pressKey;
        [Tooltip("Кастумное изо")]
        [SerializeField] private Image custonImage;

        [Space(5)]
        [Tooltip("Включать экран загрузки на старте")]
        [SerializeField] private bool showLoadScreenOnStart = true;
        [Tooltip("Сколько секунд после загрузки сцены ждем, до возможности закрыть экран загрузки")]
        [SerializeField] private float waitBeforeLoadScene = 1;
        /// <summary>
        /// вызывается когда загрузилась сцена и мы нажали "любую клавишу"
        /// </summary>
        public event System.Action OnLoadScreenRemoved;

        private static string lastTipText = null;
        private static Sprite lastSprite = null;
        public bool loadProcess { get; private set; }

        [SerializeField] private Animator anim;
        [SerializeField] private float animatorSpeedFactor = 1;
        private void Awake()
        {
            instance = this;

        }

        void Start()
        {
            anim.SetFloat("SpeedFactor", animatorSpeedFactor);
            if (showLoadScreenOnStart) StartCoroutine(StartSceneProcess());
            else { anim.Play("ToLight"); };
        }

        IEnumerator StartSceneProcess()
        {
            loadProcess = true;
            tipText.text = lastTipText;
            custonImage.sprite = lastSprite;
            if (lastSprite != null) custonImage.enabled = true;
            yield return new WaitForSecondsRealtime(0.5f);
            pressKey.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            pressKey.gameObject.SetActive(false);
            anim.Play("ToLight");
            yield return new WaitForSecondsRealtime(1);
            loadProcess = false;
            OnLoadScreenRemoved?.Invoke();

        }
        IEnumerator LoadSceneProcess(string sceneName)
        {
            loadProcess = true;

            tipText.text = GetTipBySceneName(sceneName).tipText.GetRandomElement();
            custonImage.sprite = GetTipBySceneName(sceneName).sideSprite;
            lastSprite = custonImage.sprite;
            lastTipText = tipText.text;
            if (lastSprite != null) custonImage.enabled = true;

            anim.Play("ToDark");
           
            if (GetTipBySceneName(sceneName).clipIntro.clip != null) Music.instance.PlayThatMusicSlow(GetTipBySceneName(sceneName).clipIntro);
            yield return new WaitForSecondsRealtime(waitBeforeLoadScene);
            loadProcess = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        }
        /// <summary>
        /// Основаня функция, загружает другую сцену а прелюдией
        /// </summary>
        /// <param name="name"></param>
        public void LoadScene(string name)
        {
            if (loadProcess) return;
            loadProcess = true;
            StartCoroutine(LoadSceneProcess(name));

        }


        Tip GetTipBySceneName(string sceneName)
        {
            foreach (var item in tips)
            {
                if (item.sceneName == sceneName)
                {
                    return item;
                }
            }
            Debug.LogError("Не найдена подсказка с названием уровня " + sceneName);
            return new Tip();
        }

    }
}