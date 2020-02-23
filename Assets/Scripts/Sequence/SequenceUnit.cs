using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 制御フローの単一処理機構。
/// </summary>
[Serializable]
public class SequenceUnit : SequenceElement
{
    /// <summary>
    /// これに入ってきた時に呼び出される。
    /// 初期化処理等を行う。
    /// </summary>
    public virtual void OnStart(Transform target) { }

    /// <summary>
    /// これに入った後、毎フレーム呼び出される。
    /// </summary>
    public virtual void OnUpdate(Transform target, float deltaTime) { }

    /// <summary>
    /// これから出ていく時に呼び出される。
    /// 終了処理等を行う。
    /// </summary>
    public virtual void OnEnd(Transform target) { }

    /// <summary>
    /// これが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public virtual bool IsEnd()
    {
        return true;
    }

    /// <summary>
    /// これに入ってきた時のトランスフォームの座標と回転を予測する。
    /// </summary>
    public virtual void GetStartTransform(Transform target, out Vector3 position, out Vector3 rotate)
    {
        position = target.position;
        rotate = target.eulerAngles;
    }
}
