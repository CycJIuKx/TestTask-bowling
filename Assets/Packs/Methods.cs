using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace XOR
{
    public static class Methods
    {


        private static System.Random rng = new System.Random();
        /// <summary>
        /// Убирает нулевые ссылки, возвращая список Без них, НЕ МЕНЯЕТ САМ СЕБЯ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myList"></param>
        /// <returns></returns>
        public static List<T> RemoveNulls<T>(this List<T> myList)
        {
            myList = myList.Where(item => item != null).ToList();
            return myList;
        }
        /// <summary>
        /// возвращает рандомный элеметн списка
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myList"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this List<T> myList)
        {
            int r = Random.Range(0, myList.Count);
            return myList[r];
        }
        /// <summary>
        /// Возвращает позицию мышки в глобальных координатах, относительно камеры
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector3 GetMousePosAtWorldByScreen(Camera cam)
        {
            return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
        }
        /// <summary>
        /// Перемешивание Списка
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Возвращает рандомное число между X Y
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static float GetRandom(this Vector2 r)
        {
            return Random.Range(r.x, r.y);
        }


        /// <summary>
        /// возвращает нормаль где 1 - это цель перед вами, -1 позади, 0 сбоку
        /// /// </summary>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetDotDir2D(this Transform owner, Transform target)
        {
            Vector3 forward = owner.TransformDirection(Vector3.up);
            Vector3 toOther = target.transform.position - owner.position;
            toOther.Normalize();
            return Vector3.Dot(forward, toOther);
        }

        /// <summary>
        /// Поворачивает спрайт(Осью Y) в сторону направления 
        /// </summary>
        /// <param name="Dir">направление</param>
        /// <param name="myTrans">Обьект который поворачиваем</param>
        public static void TurnToPoint(Vector3 Dir, Transform myTrans)
        {
            Vector2 direction = Dir - myTrans.position;
            float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            myTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        /// <summary>
        /// Возвращает измененный угол нормали, по входному параметру( для 2д с видом сверху)
        /// </summary>
        /// <param name="dir">изначальное направление</param>
        /// <param name="angle">Угол изменения</param>
        public static Vector3 GetNewDirectionWithOffset(Vector3 dir, float angle)
        {
            return Quaternion.AngleAxis(45, Vector3.up) * dir;
        }
    }

   

    [System.Serializable]
    public class Int
    {

        public int normal, value;
        public void AddProcent(int procent)
        {
            value += (int)((float)normal * (float)procent / 100);
        }
        public static Int operator *(Int a, int b)
        {
            a.value *= b;
            a.normal *= b;
            return a;
        }
        public static Int operator /(Int a, int b)
        {
            a.value /= b;
            a.normal /= b;
            return a;
        }
    }
    [System.Serializable]
    public class Float
    {
        public float normal, value;
        public void AddProcent(float procent)
        {
            value += normal * procent / 100;
        }
        public static Float operator /(Float a, int b)
        {
            // сравниваем цену книг
            a.value /= b;
            a.normal /= b;
            return a;
        }
        public static Float operator *(Float a, int b)
        {
            // сравниваем цену книг
            a.value *= b;
            a.normal *= b;
            return a;
        }
    }
    [System.Serializable]
    public class Points
    {
        public int current, max;
        public int regen;
        public void RegenIteration()
        {
            current += regen;
            if (current > max) current = max;
        }
    }
}