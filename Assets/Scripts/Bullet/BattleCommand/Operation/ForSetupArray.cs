#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃が始まる前の初期化の処理の配列を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/forSetup/array", fileName = "ForSetupArray", order = 0)]
[System.Serializable]
public class ForSetupArray : ForSetupBase
{

    /// <summary>
    /// 初期化処理オブジェクトの配列
    /// </summary>
    [SerializeField]
    private ForSetupBase[] m_Array;


    public override void Setup()
    {
        foreach (ForSetupBase forSetupBase in m_Array)
        {
            forSetupBase.Setup();
        }
    }
}