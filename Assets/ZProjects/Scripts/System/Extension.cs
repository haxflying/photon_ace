using UnityEngine;
using System.Collections;

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
}
