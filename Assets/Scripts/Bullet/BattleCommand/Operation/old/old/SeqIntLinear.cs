using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の数の並び（数列）を表すクラス。インスペクター上でforループを表すときに使う（？）。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/seq/int/linear", fileName = "SeqIntLinear", order = 0)]
[System.Serializable]
public class SeqIntLinear : ScriptableObject
{

    /// <summary>
    /// 初期値
    /// </summary>
    [SerializeField]
    public int m_Init;


    /// <summary>
    /// 増分
    /// </summary>
    [SerializeField]
    public int m_Inc;

    /// <summary>
    /// 条件をこの値未満にする。
    /// </summary>
    [SerializeField]
    public int m_End;
}
