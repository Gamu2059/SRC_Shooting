using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演算を表すクラスの基底クラス。
/// </summary>
public abstract class OperationBase1<T> : object
{

    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public abstract T GetResult();
}




///// <summary>
///// ここまでの演算を表す
///// </summary>
//protected OperationBase<T> m_Operation;
