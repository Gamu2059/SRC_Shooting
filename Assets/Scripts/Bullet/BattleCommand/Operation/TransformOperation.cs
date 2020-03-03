using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵や弾の物理的な状態を表す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/transform/transform", fileName = "TransformOperation", order = 0)]
[System.Serializable]
public class TransformOperation : ScriptableObject
{

    /// <summary>
    /// 位置
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Position;
    public OperationVector2Base Position
    {
        set { m_Position = value; }
        get { return m_Position; }
    }

    /// <summary>
    /// 角度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;
    public OperationFloatBase Angle
    {
        set { m_Angle = value; }
        get { return m_Angle; }
    }

    /// <summary>
    /// 大きさ
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Scale;
    public OperationFloatBase Scale
    {
        set { m_Scale = value; }
        get { return m_Scale; }
    }


    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public TransformSimple GetResultTransform()
    {
        return new TransformSimple(m_Position.GetResultVector2(), m_Angle.GetResultFloat(), m_Scale.GetResultFloat());
    }


    /// <summary>
    /// 演算結果を取得する（それぞれのパラメータがもしnullなら、それについては慣性に従って求める）
    /// </summary>
    public TransformSimple GetResultTransform(ShotParam shotParam, float time)
    {
        Vector2 position;

        if (m_Position == null)
        {
            position = shotParam.Position + shotParam.Velocity * time;
        }
        else
        {
            position = m_Position.GetResultVector2();
        }

        float angle;

        if (m_Angle == null)
        {
            angle = shotParam.Angle + shotParam.AngleSpeed * time;
        }
        else
        {
            angle = m_Angle.GetResultFloat();
        }

        float scale;

        if (m_Scale == null)
        {
            scale = shotParam.Scale + shotParam.ScaleSpeed * time;
        }
        else
        {
            scale = m_Scale.GetResultFloat();
        }

        return new TransformSimple(position, angle, scale);
    }
}