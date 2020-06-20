#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多重forループを表すクラス。（forループをメタ的に見ている）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/MultiForLoop", fileName = "MultiForLoop", order = 0)]
[System.Serializable]
public class MultiForLoop : ForBase
{

    [SerializeField]
    public ForBase[] m_ForArray;

    private int m_Index;

    private bool isLooping;


    /// <summary>
    /// 攻撃が始まる前の初期化をする。
    /// </summary>
    public override void Setup()
    {
        foreach (ForBase forBase in m_ForArray)
        {
            forBase.Setup();
        }
    }


    /// <summary>
    /// 初期化する。
    /// </summary>
    public override void Init()
    {
        m_Index = 0;
        isLooping = false;
    }


    /// <summary>
    /// 進める（全てのループを抜け終わった場合はfalseを返す。）
    /// </summary>
    public override void Process()
    {
        m_Index = m_ForArray.Length - 1;
        isLooping = true;
    }


    public override bool IsTrue()
    {
        // どのforループよりも内側にいるなら
        if (m_Index > m_ForArray.Length - 1)
        {
            return true;
        }
        // どのforループよりも外側に行ったなら
        else if (m_Index < 0)
        {
            return false;
        }

        // 増分の方から来たなら
        if (isLooping)
        {
            // 増分の処理をする
            m_ForArray[m_Index].Process();
        }
        // 初期化の方から来たなら
        else
        {
            // 初期化の処理をする
            m_ForArray[m_Index].Init();
        }

        if (m_ForArray[m_Index].IsTrue())
        {
            // 条件判定がtrueなら、1つ内側へ行ってそこで初期化をする
            m_Index++;
            isLooping = false;
            return IsTrue();
        }
        else
        {
            // 条件判定がfalseなら、このループは終わって、1つ外側へ行ってループする
            m_Index--;
            isLooping = true;
            return IsTrue();
        }
    }
}