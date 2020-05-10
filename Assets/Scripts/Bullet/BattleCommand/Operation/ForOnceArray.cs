#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 毎回一度だけ処理を行うオブジェクトの配列を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/forOnce/array", fileName = "ForOnceArray", order = 0)]
[System.Serializable]
public class ForOnceArray : ForOnceBase
{

    /// <summary>
    /// 処理オブジェクトの配列
    /// </summary>
    [SerializeField]
    private ForOnceBase[] m_Array;


    public override void Do()
    {
        foreach (ForOnceBase forOnceBase in m_Array)
        {
            forOnceBase.Do();
        }
    }
}