using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵本体の動きを表す抽象クラス。
/// </summary>
public abstract class MovingPhaseBase : ScriptableObject
{

    /// <summary>
    /// 初期状態を代入する。
    /// </summary>
    public abstract void Init(TransformSimple transform);


    /// <summary>
    /// 与えられた時刻での状態を取得する。
    /// </summary>
    public abstract TransformSimple GetTransform(float time);


    /// <summary>
    /// 与えられた時刻での状態の時間微分を取得する。
    /// </summary>
    public abstract TransformSimple GetDdtTransform(float time);


    /// <summary>
    /// 所要時間を所得する。
    /// </summary>
    public abstract float GetDuration();


    /// <summary>
    /// この形態の終了時の状態を取得する。
    /// </summary>
    public abstract TransformSimple GetEndTransform();
}
