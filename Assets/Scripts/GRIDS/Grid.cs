using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu()]
public class Grid : ScriptableObject
{
    public float angle = 60;
    public List<Form> forms;
    public int formCount;


    void ResetGrid()
    {
       
        foreach (var item in forms)
        {
            item.outside = false;
            item.used = false;
        }
    }
    public List<Form> Create(Transform transform)
    {
        if (forms.Count > 0)
        {
       
            ResetGrid();
            foreach (var item in forms)
            {
                item.mainTransform = transform;
            }
            return forms;
        }
        AddToMainList(CreatrForms(transform.position, transform));//создаем основную форму
        while (true)
        {
            for (int q = 0; q < forms.Count; q++)
            {
                AddToMainList(CreatrForms(forms[q].point, transform));
                if (forms.Count > formCount) break;
            }
            if (forms.Count > formCount) break;

        }

        forms = forms.OrderBy((f) => Vector3.Distance(f.point, transform.position)).ToList();//сортировка форм по дистанции к центру
        for (int i = 0; i < forms.Count; i++)
        {
            SetChildsToForm(forms[i], transform);
        }
        return forms;
    }
    void AddToMainList(List<Form> newPoints)
    {
        foreach (var item in newPoints)
        {
            forms.Add(item);
        }
    }
    List<Form> CreatrForms(Vector3 center, Transform transform)
    {
        List<Form> createdForms = new List<Form>();
        Vector3 dir = Vector3.forward;
        for (int i = 0; i < 6; i++)
        {
            dir = GetNewDirectionWithOffset(dir, angle);
            Form form = new Form(transform);
            form.point = center + dir;
            form.point = RoundVector(form.point);
            if (!IsMainListContainsFrom(form))
            {
                createdForms.Add(form);
            }
        }
        return createdForms;
    }
    private void SetChildsToForm(Form form, Transform transform)
    {
        for (int i = 0; i < forms.Count; i++)
        {
            if (Vector3.Distance(forms[i].point, form.point) < 1.5f)
            {
                form.childs.Add(forms[i].point);
                if (form.childs.Count > 5) return;

            }
        }

        form.childs = form.childs.OrderBy((c) => Vector3.Distance(c, transform.position)).ToList();
        //сориировка
    }






    public static Vector3 GetNewDirectionWithOffset(Vector3 dir, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up) * dir;
    }

    Vector3 RoundVector(Vector3 point)
    {
        float x = (float)System.Math.Round(point.x, 2);
        float y = (float)System.Math.Round(point.y, 2);
        float z = (float)System.Math.Round(point.z, 2);
        point = new Vector3(x, y, z);
        return point;
    }
    bool IsMainListContainsFrom(Form form)
    {
        for (int i = 0; i < forms.Count; i++)
        {
            if (form.Equals(forms[i]))
            {
                return true;
            }
        }
        return false;
    }
}
