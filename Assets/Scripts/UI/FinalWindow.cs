using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinalWindow : MonoBehaviour
{
    [SerializeField] Text scoreText, commentText;
    [SerializeField] GameObject tapText;
    [SerializeField] GameObject window;
    [SerializeField] XOR.Clip openlip;
    public void Open(int score)
    {
        XOR.SoundCreator.Create(openlip);
        window.SetActive(true);
        SetComment(score);
        scoreText.text = $"{score}/30 pins knocked down!";
        StartCoroutine(exitDelay());
    }
    private void SetComment(int score)
    {
        if (score > 27)
        {
            commentText.text = "PERFECT";
        }
        else
      if (score > 20)
        {
            commentText.text = "GOOD";
        }
        else
       if (score > 10)
        {
            commentText.text = "Maybe next time";
        }
        else
       if (score >= 5)
        {
            commentText.text = "Maybe ";
        }
        else
        {
            commentText.text = "Nice try, loser...";
        }
    }
    IEnumerator exitDelay()
    {
        yield return new WaitForSeconds(3);
        tapText.SetActive(true);
        while (true)
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                XOR.SceneLoader.instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                yield break;
                //restrt
            }
        }

    }
    void Awake()
    {

    }



    void Start()
    {

    }


    void Update()
    {

    }
}
