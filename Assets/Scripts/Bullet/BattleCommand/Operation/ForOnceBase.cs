using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一度しかループしないfor文を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class ForOnceBase : ForBase
{

    /// <summary>
    /// 条件判定をする
    /// </summary>
    private bool m_IsLoop;


    public override void Init()
    {
        // 実行する
        Do();
        
        // 一回はループするようにする
        m_IsLoop = true;
    }


    public override bool IsTrue()
    {
        return m_IsLoop;
    }


    public override void Process()
    {
        // 一回ループしたので、もうループしないようにする
        m_IsLoop = false;
    }


    /// <summary>
    /// 何かを実行する。
    /// </summary>
    public abstract void Do();
}
