#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセット用単位弾幕パラメータの配列の配列のクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/All UD Field Array Array", fileName = "AUDFAA", order = 0)]
[System.Serializable]
public class AllUDFieldArrayArray : ScriptableObject
{
    /// <summary>
    /// アセット用単位弾幕パラメータの配列の配列。
    /// </summary>
    [SerializeField, Tooltip("")]
    private AllUDFieldArray[] m_AllUDFieldArrayArray;


    /// <summary>
    /// 純粋な単位弾幕パラメータの配列の配列を取得する。
    /// </summary>
    public UDParams[][] GetAllUDParams()
    {
        List<UDParams[]> uDParamsArrayList = new List<UDParams[]>();

        foreach (AllUDFieldArray allUDFieldArray in m_AllUDFieldArrayArray)
        {
            uDParamsArrayList .Add(allUDFieldArray.GetAllUDParams());
        }

        return uDParamsArrayList.ToArray();
    }
}