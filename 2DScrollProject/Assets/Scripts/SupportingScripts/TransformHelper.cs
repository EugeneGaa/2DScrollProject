using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    public static Transform DeepFind(this Transform trans, string targetName)
    {
        Transform tempTrans = null;
        foreach(Transform child in trans)
        {
            
            if(child.name == targetName)
            {
                return child;
            }
            else
            {
                tempTrans =  DeepFind(child,targetName);
                if(tempTrans!=null)
                {
                    return tempTrans;
                }
            }
        }
        return null;
    }



}
