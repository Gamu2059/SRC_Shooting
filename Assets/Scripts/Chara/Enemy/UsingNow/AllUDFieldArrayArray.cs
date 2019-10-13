using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/Danmaku/All UD Field Array Array", fileName = "AUDFAA", order = 0)]
[System.Serializable]
public class AllUDFieldArrayArray : ScriptableObject
{
    [SerializeField, Tooltip("全単位弾幕フィールド配列の配列")]
    public AllUDFieldArray[] m_AllUDFieldArrayArray;


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