using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] XOR.Clip onDieClip;
    [SerializeField] GameObject destroyEffect;
    public bool useLog = false;
    [Range(1, 3)]
    [SerializeField] float ChangePositonRateFactor = 1;
    [SerializeField] float moveSpeed = 10;



    private Vector3 jumpOffset;
    private Controller controller;
    private Transform myTransform;
    private bool transformMakingProcess = false;
    private Form form;

    private bool isDead;
    public Ball Init(Form form, Controller controller)

    {
        this.form = form;
        form.MakeUsed(true);
        this.controller = controller;
        return this;
    }
    void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(FindNearPointProcess());
        StartCoroutine(AwakeProcess());
    }
    public void OnUpdate()
    {
        if (transformMakingProcess == false)
        {
            myTransform.Rotate(moveSpeed / 2, 0, 0);
            myTransform.localPosition = Vector3.MoveTowards(myTransform.localPosition, form.point + jumpOffset, Time.deltaTime * moveSpeed);
        }
    }
    IEnumerator AwakeProcess()
    {
        float timer = 0;
        while (timer < 1)
        {
            myTransform.localScale = Vector3.one * timer;
            timer += Time.deltaTime * 4;
            yield return null;
        }
        myTransform.localScale = Vector3.one;

    }
    IEnumerator FindNearPointProcess()
    {
        while (!isDead)
        {
            FindNearFormToMid();
            yield return new WaitForSeconds(1 / moveSpeed * ChangePositonRateFactor);
        }
    }
    private void FindNearFormToMid()
    {
        form.MakeUsed(false);
        form = GetClosestFreeForm();
        form.MakeUsed(true);
    }

    private Form GetClosestFreeForm()
    {
        if (IsMyFormIsNear() && !form.outside)//если наша форма самая близкая и не в ауте
        {
            return form;
        }
        List<Form> freeForms = GetFreeForms();
        Form nearForm = null;
        float nearDist = controller.GetDistanceToMainPoint(form.point);
        if (!form.outside)
        {
            Log("мы НЕ В аутсайде ");
            for (int i = 0; i < freeForms.Count; i++)
            {
                float childDistanceToMain = controller.GetDistanceToMainPoint(freeForms[i].point);
                if (childDistanceToMain < nearDist)
                {
                    nearForm = freeForms[i];
                    nearDist = childDistanceToMain;
                }
            }
        }
        else if (freeForms.Count > 0)
        {
            Log("мы в аутсайде, и список доступных точек не пуст ");
            nearForm = freeForms[0];
            for (int i = 0; i < freeForms.Count; i++)
            {
                float childDistanceToMain = controller.GetDistanceToMainPoint(freeForms[i].point);
                if (childDistanceToMain < nearDist)
                {
                    nearForm = freeForms[i];
                    nearDist = childDistanceToMain;
                }
            }
        }
        else
        {
            nearForm = GetNearClosedForm();
            Log("Ближайшая форма из заблоченых = " + nearForm);
        }

        if (nearForm == null)
        {
            Log("Не нашли свободную форму по близости");
            nearForm = form;
        }
        return nearForm;

    }


    private bool IsMyFormIsNear()
    {
        float currentDistanceToMain = controller.GetDistanceToMainPoint(form.point);
        List<Form> freeForms = GetFreeForms();
        for (int i = 0; i < freeForms.Count; i++)
        {
            if (controller.GetDistanceToMainPoint(freeForms[i].point) < currentDistanceToMain)
            {
                return false;
            }
        }
        return true;
    }

    private List<Form> GetFreeForms()
    {
        List<Form> freeFOrms = new List<Form>();
        for (int i = 0; i < form.childs.Count; i++)
        {
            Form childFrom = controller.GetFormByPoint(form.childs[i]);
            if (childFrom.outside || childFrom.used) continue;
            freeFOrms.Add(childFrom);
        }
        return freeFOrms;
    }
    private Form TryGetPointFromSide()
    {
        for (int i = form.childs.Count - 1; i > 0; i--)
        {
            Form childForm = controller.GetFormByPoint(form.childs[i]);
            if (!childForm.used)
            {
                if (myTransform.position.x >= 0)//если мы справа
                {
                    if (childForm.point.x < form.point.x)
                    {
                        return childForm;
                    }
                }
                if (myTransform.position.x < 0)//если мы слева
                {
                    if (childForm.point.x > form.point.x)
                    {
                        return childForm;
                    }
                }
            }
        }
        return null;
    }
    private Form GetNearClosedForm()
    {
        Form sideForm = TryGetPointFromSide();
        if (sideForm != null)
        {
            Log("Side form");
            return sideForm;
        }
        for (int i = 0; i < form.childs.Count; i++)
        {
            Form childForm = controller.GetFormByPoint(form.childs[i]);
            if (!childForm.used && childForm.outside)
            {
                if (myTransform.localPosition.z <= 0)
                {
                    if (childForm.point.z < form.point.z)
                    {
                        return childForm;
                    }
                }
                else
                {
                    if (childForm.point.z > form.point.z)
                    {
                        Log($"Варнули самую верх точку вокруг нас Мы={form.point} точка " + childForm.point);
                        return childForm;
                    }
                }
            }
        }
        Log("Не найдена ближайшая закрытая точка ");
        return null;
    }






    public void PushToUp(float jumpHeight = 3, float jumpSpeed = 5)
    {
        StartCoroutine(jumpCor(jumpHeight, jumpSpeed));
    }
    IEnumerator jumpCor(float jumpHeight, float jumpSpeed)
    {
        while (jumpOffset.y < jumpHeight)
        {
            // float speed = Time.deltaTime * jumpSpeed / jumpOffset.y + 1f * Time.deltaTime;
            float speed = Time.deltaTime * jumpSpeed * 2;
            jumpOffset = Vector3.MoveTowards(jumpOffset, new Vector3(0, jumpHeight, 0), speed);
            yield return null;
        }
        while (jumpOffset.y > 0)
        {
            float speed = Time.deltaTime * jumpSpeed / jumpOffset.y + 1f * Time.deltaTime;
            jumpOffset = Vector3.MoveTowards(jumpOffset, new Vector3(0, 0, 0), speed);
            yield return null;
        }
        jumpOffset = Vector3.zero;
    }



    public void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} die form used ={form.used}");
        form.MakeUsed(false);
        XOR.SoundCreator.Create(onDieClip);
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        Debug.Log($"{gameObject.name} die process form used ={form.used}");
        GetComponent<Collider>().enabled = false;
        controller.OnBallDie(this);

        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        form.MakeUsed(false);
        yield return null;
        form.MakeUsed(false);
        Debug.Log($"{gameObject.name} die end form used ={form.used}");
        Destroy(gameObject);
    }




    public void TransformToHugeBall(BonusBall ball)
    {
        StartCoroutine(transformProcess(ball));
    }
    IEnumerator transformProcess(BonusBall ball)
    {
        while (true)
        {
            yield return null;
            myTransform.position = Vector3.MoveTowards(myTransform.position, ball.transform.position, Time.deltaTime * 10);
            if (Vector3.Distance(myTransform.position, ball.transform.position) < 1)
            {
                break;
            }
        }
        ball.AddBall();
        gameObject.SetActive(false);
    }


    void Log(string s)
    {
        if (useLog)
        {
            Debug.Log(s);
        }
    }
}
