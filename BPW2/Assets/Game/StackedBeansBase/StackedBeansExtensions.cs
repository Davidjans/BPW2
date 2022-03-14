using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackedBeansUtils
{
    public static class StackedBeansExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        public static T GetRandomAndRemove<T>(this List<T> list)
        {
            T temp = list.GetRandom();
            list.Remove(temp);
            return temp;
        }

        public static void DestroyVisualChildren(this Transform thisTransform)
        {
            foreach (Transform childTransform in thisTransform.GetComponentsInChildren<Transform>())
            {
                if (childTransform != thisTransform)
                {
                    if (childTransform.GetComponentInChildren<MeshRenderer>() != null)
                        Object.Destroy(childTransform.GetComponentInChildren<MeshRenderer>());

                    if (childTransform.GetComponent<MeshFilter>() != null)
                        Object.Destroy(childTransform.GetComponent<MeshFilter>());
                }
            }
        }
    }

}
