using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/Danmaku/All UD Field Array", fileName = "AUDFA", order = 0)]
[System.Serializable]
public class AllUDFieldArray : ScriptableObject
{
    [SerializeField, Tooltip(
        "単位弾幕OMNの配列"
        + "\n" +
        "全方位に弾を発射する。隣り合う角度は等しく、対称的である。"
        )]
    public UDField[] m_UDField;


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


    public UDParams[] GetAllUDParams()
    {
        List<UDParams> uDParamsList = new List<UDParams>();

        foreach (UDField uDOmnFields in m_UDField)
        {
            uDParamsList.Add(uDOmnFields.GetUDParams());
        }

        return uDParamsList.ToArray();
    }
}