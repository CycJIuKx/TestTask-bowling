using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class Form
{
    public GameObject vusialGo;
    public Transform mainTransform;
    public Vector3 point;
    public bool used;
    public bool outside;
    public List<Vector3> childs = new List<Vector3>(6);
    public Form() { }
    public Form(Transform transform)
    {
        mainTransform = transform;
    }
    public bool isOutside()
    {

        if (mainTransform.position.x + point.x > 10 || mainTransform.position.x + point.x < -10)
        // if (point.x > 10 || point.x < -10)
        {
            MakeOutside(true);
            return true;
        }
        MakeOutside(false);
        return false;
    }
    public bool Equals(Form obj)
    {
        if (point == obj.point)
        {
            return true;
        }
        else return false;
    }
    public void InitTheVisual(GameObject visGo)
    {
        this.vusialGo = visGo;
    }
    void SwitchColor()
    {
        if (vusialGo == null) return;

        if (used)
        {
            vusialGo.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            vusialGo.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
    public void MakeUsed(bool flag)
    {
        used = flag;
        SwitchColor();
    }
    public void MakeOutside(bool flag)
    {
        outside = flag;
        SwitchColor();
    }
}
public class Controller : MonoBehaviour
{
    public bool debugGrid;
    [SerializeField] Grid grid;
    [SerializeField] GameObject pref;
  
    [SerializeField] float forwardSpeed = 10, sideSpeed = 20;
    public int createOnStart = 19;


    public List<Ball> balls;
    public List<Form> forms;



    private Vector3 mainPoint;
    private float borderRadius = 9;
    private Vector3 destenationPoint;
    private bool enableContol = false;
    private Transform myTransform;
    private int createIndex = 0;

    void Awake()
    {

    }


    void Start()
    {
        GameController.instance.StartGamplay += () =>
        {
            enableContol = true;
        };
        myTransform = GetComponent<Transform>();
        forms = grid.Create(myTransform);
        if (debugGrid)
        {
            for (int i = 0; i < forms.Count; i++)
            {
                GameObject m = ViualisePoint(forms[i].point);
                forms[i].InitTheVisual(m);

            }
        }

        CreateBalls(createOnStart);
       
    }

    void Update()
    {
        if (!enableContol) return;
        myTransform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
        ChangeMainPointOffset();

        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].OnUpdate();
        }
        for (int i = 0; i < forms.Count; i++)
        {
            forms[i].isOutside();
        }
      



        if (destenationPoint.x < myTransform.position.x - 1)
        {
            if (myTransform.position.x < -borderRadius) return;

            myTransform.Translate(Vector3.left * Time.deltaTime * sideSpeed);
        }
        if (destenationPoint.x > myTransform.position.x + 1)
        {
            if (myTransform.position.x > borderRadius) return;
            myTransform.Translate(Vector3.right * Time.deltaTime * sideSpeed);
        }

       

    }

    private void ChangeMainPointOffset()
    {
        if (myTransform.position.x < -8) mainPoint.x = 4;
        else if (myTransform.position.x < -7) mainPoint.x = 3;
        else if (myTransform.position.x < -6) mainPoint.x = 2;
        else if (myTransform.position.x < -5) mainPoint.x = 1;

        else if (myTransform.position.x > 8) mainPoint.x = -4;
        else if (myTransform.position.x > 7) mainPoint.x = -3;
        else if (myTransform.position.x > 6) mainPoint.x = -2;

        else if (myTransform.position.x > 5) mainPoint.x = -1;


        else { mainPoint.x = 0; }
    }


    public void TranslateGroup(Vector3 pos)
    {
        pos.y = 0;
        pos.z = 0;
        destenationPoint = pos;
    }
    public void StopTranslateGroup()
    {
        destenationPoint.x = myTransform.position.x;
    }



    public void CreateBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int q = 0; q < forms.Count; q++)
            {
                if (!forms[q].used)
                {
                    GameObject go = Instantiate(pref, myTransform);
                    go.name = "Ball" + createIndex;
                    createIndex++;
                    go.transform.localPosition = forms[q].point;
                    balls.Add(go.GetComponent<Ball>().Init(forms[q], this));
                    break;
                }
            }
        }
    }
    public void AddBalls(int value)
    {
        CreateBalls(value);
    }
    public void RemoveBalls(int value)
    {
        if (value >= balls.Count)
        {
            value = balls.Count - 1;
        }
        var removedBalls = new List<Ball>();
        for (int i = 0; i < value; i++)
        {
            removedBalls.Add(balls[i]);

        }
        foreach (var item in removedBalls)
        {
            item.Die();
        }
    }



    public void OnBallDie(Ball ball)
    {
        balls.Remove(ball);
        if (balls.Count==0)
        {
            BlockMovement();
            GameController.instance.OnAllBallsDie();
        }
    }
    public Form GetFormByPoint(Vector3 point)
    {
        for (int i = 0; i < forms.Count; i++)
        {
            if (forms[i].point == point)
            {
                return forms[i];
            }
        }
        Debug.LogError("Не найдена форма по точке " + point);
        return null;
    }





    public void MakeHugeBall(BonusBall ball, System.Action onFinal)
    {
        StartCoroutine(HugeBallMaking(ball,onFinal));
    }
    IEnumerator HugeBallMaking(BonusBall ball, System.Action onFinal)
    {
        foreach (var item in balls)
        {
            yield return new WaitForSeconds(0.03f);
            item.TransformToHugeBall(ball);
        }
        onFinal.Invoke();
    }


    public float GetDistanceToMainPoint(Vector3 checkPoint)
    {
        return Vector3.Distance(checkPoint, mainPoint);
    }
    public GameObject ViualisePoint(Vector3 point)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.parent = transform;
        go.name = "p " + point;
        go.transform.position = point;
        go.GetComponent<Collider>().enabled = false;

        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        return go;
    }


    public void BlockMovement()
    {
        enableContol = false;
    }

}
