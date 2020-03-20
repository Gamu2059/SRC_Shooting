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

    /// <summary>
    /// このトランスフォームのローカル座標系を基準として、指定したワールド座標をローカル座標に変換する。<br/>
    /// ex : transform.position => transform.localPosition --> 自身のワールド座標は自身のローカル座標と同じになる。<br/>
    /// ex : transform.parent.position => (0, 0, 0) --> 親トランスフォームのワールド座標は自身のローカル座標系の原点になる。<br/>
    /// ex : (0, 0, 0) => -transform.parent.position --> ワールド座標系の原点は親トランスフォームのワールド座標の逆位置になる。
    /// </summary>
    /// 
    /// <param name="worldPosition">ワールド座標</param>
    public static Vector3 WorldPositionToLocalPosition(this Transform transform, Vector3 worldPosition)
    {
        if (transform.parent == null)
        {
            return worldPosition;
        }
        return transform.parent.worldToLocalMatrix.MultiplyPoint(worldPosition);
    }

    /// <summary>
    /// このトランスフォームのローカル座標系におけるローカル座標をワールド座標に変換する。<br/>
    /// ex : transform.localPosition => transform.position --> 自身のローカル座標は自身のワールド座標と同じになる。<br/>
    /// ex : (0, 0, 0) => transform.parent.position --> 自身のローカル座標系の原点は親トランスフォームのワールド座標と同じになる。<br/>
    /// </summary>
    /// 
    /// <param name="localPosition">ローカル座標</param>
    public static Vector3 LocalPositionToWorldPosition(this Transform transform, Vector3 localPosition)
    {
        if (transform.parent == null)
        {
            return localPosition;
        }
        return transform.parent.localToWorldMatrix.MultiplyPoint(localPosition);
    }

    /// <summary>
    /// このトランスフォームのローカル座標系を基準として、指定したワールド回転をローカル回転に変換する。<br/>
    /// ex : transform.eulerAngles => transform.localEulerAngles --> 自身のワールド回転は自身のローカル回転と同じになる。<br/>
    /// ex : transform.parent.eulerAngles => (0, 0, 0) --> 親トランスフォームのワールド回転は自身にとって無回転と同じになる。
    /// </summary>
    /// 
    /// <param name="worldEulerAngles">ワールド回転(オイラー角)</param>
    public static Vector3 WorldEulerAnglesToLocaEulerlAngles(this Transform transform, Vector3 worldEulerAngles)
    {
        if (transform.parent == null)
        {
            return worldEulerAngles;
        }

        var parentInverseRotation = Quaternion.Inverse(transform.parent.rotation);
        return (parentInverseRotation * Quaternion.Euler(worldEulerAngles)).eulerAngles;
    }

    /// <summary>
    /// このトランスフォームのローカル座標系を基準として、指定したワールド回転をローカル回転に変換する。<br/>
    /// ex : transform.rotation => transform.localRotation --> 自身のワールド回転は自身のローカル回転と同じになる。<br/>
    /// ex : transform.parent.rotation => (0, 0, 0, 1) --> 親トランスフォームのワールド回転は自身にとって無回転と同じになる。
    /// </summary>
    /// 
    /// <param name="worldRotation">ワールド回転(クォータニオン)</param>
    public static Quaternion WorldRotationToLocaRotation(this Transform transform, Quaternion worldRotation)
    {
        if (transform.parent == null)
        {
            return worldRotation;
        }

        var parentInverseRotation = Quaternion.Inverse(transform.parent.rotation);
        return parentInverseRotation * worldRotation;
    }

    /// <summary>
    /// このトランスフォームのローカル座標系におけるローカル回転をワールド回転に変換する。<br/>
    /// ex : transform.localEulerAngles => transform.eulerAngles --> 自身のローカル回転は自身のワールド回転と同じになる。<br/>
    /// ex : (0, 0, 0) => transform.parent.eulerAngles --> 自身のローカル座標系における無回転は親トランスフォームのワールド回転と同じになる。<br/>
    /// </summary>
    /// 
    /// <param name="localEulerAngles">ローカル回転(オイラー角)</param>
    public static Vector3 LocalEulerAnglesToWorldEulerAngles(this Transform transform, Vector3 localEulerAngles)
    {
        if (transform.parent == null)
        {
            return localEulerAngles;
        }

        return (transform.parent.rotation * Quaternion.Euler(localEulerAngles)).eulerAngles;
    }

    /// <summary>
    /// このトランスフォームのローカル座標系におけるローカル回転をワールド回転に変換する。<br/>
    /// ex : transform.localRotation => transform.rotation --> 自身のローカル回転は自身のワールド回転と同じになる。<br/>
    /// ex : (0, 0, 0, 1) => transform.parent.rotation --> 自身のローカル座標系における無回転は親トランスフォームのワールド回転と同じになる。<br/>
    /// </summary>
    /// 
    /// <param name="localRotation">ローカル回転(クォータニオン)</param>
    public static Quaternion LocalRotationToWorldRotation(this Transform transform, Quaternion localRotation)
    {
        if (transform.parent == null)
        {
            return localRotation;
        }

        return transform.parent.rotation * localRotation;
    }
}
