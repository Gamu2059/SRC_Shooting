using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセット用単位弾幕パラメータの配列のクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/All UD Field Array", fileName = "AUDFA", order = 0)]
[System.Serializable]
public class AllUDFieldArray : ScriptableObject
{
    /// <summary>
    /// アセット用単位弾幕パラメータの配列。
    /// </summary>
    [SerializeField, Tooltip("")]
    private UDField[] m_UDFieldArray;


    /// <summary>
    /// 純粋な単位弾幕パラメータの配列を取得する。
    /// </summary>
    public UDParams[] GetAllUDParams()
    {
        List<UDParams> uDParamsList = new List<UDParams>();

        foreach (UDField uDOmnFields in m_UDFieldArray)
        {
            uDParamsList.Add(uDOmnFields.GetUDParams());
        }

        return uDParamsList.ToArray();
    }
}



//// これは要るのか分からない
//public UDParams GetUDFields(E_U_D eUD,int index)
//{
//    switch (eUD)
//    {
//        case E_U_D.OMN:
//            return m_UDField[index].GetFields();

//        default:
//            return m_UDField[index].GetFields();
//    }
//}