#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の符号に応じて-1,0,1のint値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/sign", fileName = "OperationIntSign", order = 0)]
[System.Serializable]
public class OperationIntSign : OperationIntBase
{

    /// <summary>
    /// float型の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;


    public override float GetResultFloat()
    {
        return GetResultInt();
    }


    public override int GetResultInt()
    {
        float floatValue = m_Float.GetResultFloat();

        if (floatValue < 0)
        {
            return -1;
        }
        else if(0 < floatValue)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}