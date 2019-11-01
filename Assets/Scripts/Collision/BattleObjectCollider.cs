using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突情報を管理するコンポーネント。
/// </summary>
public class BattleObjectCollider : MonoBehaviour
{
    /// <summary>
    /// 衝突情報トランスフォームを保持する。
    /// </summary>
    [SerializeField]
    private ColliderTransform[] m_ColliderTransforms;
    public ColliderTransform[] ColliderTransforms => m_ColliderTransforms;

    /// <summary>
    /// 衝突情報をを生成する。
    /// </summary>
	public ColliderData[] CreateColliderData()
    {
        int hitNum = m_ColliderTransforms.Length;
        var colliders = new ColliderData[hitNum];

        for (int i = 0; i < hitNum; i++)
        {
            Transform t = m_ColliderTransforms[i].Transform;
            var c = new ColliderData();
            c.CenterPos = t.position.ToVector2XZ();
            c.Size = t.lossyScale.ToVector2XZ();
            c.Angle = -t.eulerAngles.y;
            c.Transform = m_ColliderTransforms[i];
            c.OutSideRect = Collision.GetOutSideCorner(c);

            colliders[i] = c;
        }

        return colliders;
    }

    /// <summary>
    /// 指定したトランスフォームの衝突判定をセットする。
    /// </summary>
    public void SetEnableCollider(Transform t, bool isEnable)
    {
        if (t == null || m_ColliderTransforms == null)
        {
            return;
        }

        for (int i = 0; i < m_ColliderTransforms.Length; i++)
        {
            var c = m_ColliderTransforms[i];
            if (t == c.Transform)
            {
                c.IsDisable = !isEnable;
            }
        }
    }

    public void SetEnableAllCollider(bool isEnable)
    {
        if (m_ColliderTransforms == null)
        {
            return;
        }

        for (int i = 0; i < m_ColliderTransforms.Length; i++)
        {
            var c = m_ColliderTransforms[i];
            c.IsDisable = !isEnable;
        }
    }
}
