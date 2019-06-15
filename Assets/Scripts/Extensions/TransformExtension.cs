using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// Transformの拡張クラス。
/// </summary>
public static class TransformExtension
{
    /// <summary>
    /// 指定したトランスフォームから指定した名前のオブジェクトを見つける。
    /// </summary>
    /// 
    /// <param name="name">探す名前</param>
    /// <param name="isCompleteMatch">名前を完全一致で指定するかどうか</param>
    public static Transform Find(this Transform transform, string name, bool isCompleteMatch)
    {
        if (isCompleteMatch)
        {
            if (transform.name == name)
            {
                return transform;
            }
        }
        else
        {
            if (Regex.IsMatch(transform.name, name))
            {
                return transform;
            }
        }

        int num = transform.childCount;

        if (num < 1)
        {
            return null;
        }

        for (int i = 0; i < num; i++)
        {
            var child = transform.GetChild(i);
            var findObj = child.Find(name, isCompleteMatch);

            if (findObj != null)
            {
                return findObj;
            }
        }

        return null;
    }
}
