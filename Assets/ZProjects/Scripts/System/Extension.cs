using UnityEngine;
using System.Collections;
using DG.Tweening;
public static class Extension 
{

    public static Transform ZFindChild(this Transform original, string name)
    {
        if (original.childCount == 0)
            return null;

        Transform child = null;

        for (int i = 0; i < original.childCount; i++)
        {
            //Debug.Log("Search " + original.GetChild(i).name + " ing");
            if (original.GetChild(i).name == name)
            {
                //Debug.Log("find it");
                child = original.GetChild(i);
                return child;
            }
        }

        //广度优先搜索
        if (child == null)
        {
            //for(int i = 1; i < original.childCount; i++)
            //{
            //    child = original.GetChild(i).ZFindChild(name);
            //}

            int i = 0;
            while (i < original.childCount)
            {
                child = original.GetChild(i).ZFindChild(name);
                if (child == null)
                    child = original.GetChild(i++).ZFindChild(name);
                else
                    break;
            }
        }
        return child;
    }

    public static T SetDelay<T>(this T original, float time, System.Action callback, string ID = "")
    {
        GameObject go = new GameObject();
        go.hideFlags = HideFlags.HideAndDontSave;
        go.transform.position = Vector3.zero;
        go.transform.DOMoveX(1, time).OnComplete(new TweenCallback(callback));
        return original;
    }

}
