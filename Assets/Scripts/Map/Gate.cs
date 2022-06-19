using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gate : MonoBehaviour
{
    public enum ActionType
    {
        plus, minus, factorPlus, factorMinus
    }
    [SerializeField] ActionType type;
    [SerializeField] int value;
    [SerializeField] XOR.Clip activateClip;
    public System.Action OnActivate;

    private bool activated = false;


    public void RemoveGate()
    {
        Destroy(gameObject);
    }
    public void Activate()
    {
        if (activated) return;
        XOR.SoundCreator.Create(activateClip);
        activated = true;
        switch (type)
        {
            case ActionType.plus:
                GameController.instance.AddBalls(value);
                break;
            case ActionType.minus:
                GameController.instance.RemoveBalls(value);
                break;
            case ActionType.factorPlus:
                int add = GameController.instance.GetBallsCount();
                add *= value;
                GameController.instance.AddBalls(add);
                break;
            case ActionType.factorMinus:
                int take = GameController.instance.GetBallsCount();
                take /= value;
                GameController.instance.RemoveBalls(take);
                break;

        }
        ActivateEffect();
    }
    void ActivateEffect()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

        Ball ball = other.GetComponent<Ball>();
        if (ball)
        {
            Activate();
            OnActivate.Invoke();

        }
    }
}
