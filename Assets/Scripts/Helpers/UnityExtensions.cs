using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace GSGAddOns.UnityExtensions
{
    public static class UnityExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }

        public static void ResetTransformation(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        public static void Freeze(this Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0;
            rigidbody2D.isKinematic = true;
        }


        public static List<T> ChainableAdd<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

     

        public static void Require<T>(this T obj, Object context)
        {
            if (obj == null)
            {
                Debug.LogError($"<color=red> {typeof(T)} </color> Missing on - <color=green> {context} </color>");

            }
        }

        public static string ToHex(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
          
        }
    }
}