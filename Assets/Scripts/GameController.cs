using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] Controller controller;
    [SerializeField] List<BonusSkittle> skittles;
    [SerializeField] FinalWindow finalWindow;
    public System.Action StartGamplay;

    void Awake()
    {
        instance = this;
    }

    

    public void AddBalls(int value)
    {
        controller.AddBalls(value);
    }
    public void RemoveBalls(int value)
    {
        controller.RemoveBalls(value);
    }
    public int GetBallsCount()
    {
        return controller.balls.Count;
    }

    public void OnShoot()
    {
        StartCoroutine(finalCor());
    }
    IEnumerator finalCor()
    {
        yield return new WaitForSeconds(5);
        int score = 0;
        foreach (var item in skittles)
        {
            if (item.isDown) score++;
        }
        finalWindow.Open(score);
    }
    

    void Start()
    {
        XOR.SceneLoader.instance.OnLoadScreenRemoved += OnLevelLoaded;
    }

    public void OnAllBallsDie()
    {
        XOR.SceneLoader.instance.LoadScene("Game");
    }
    private void OnLevelLoaded()
    {
        StartGamplay?.Invoke();
    }

    void Update()
    {

    }
}
