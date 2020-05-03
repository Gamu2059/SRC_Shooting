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
    /// 不透明度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Opacity;
    public OperationFloatBase Opacity
    {
        set { m_Opacity = value; }
        get { return m_Opacity; }
    }

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_CanCollide;
    public OperationBoolBase CanCollide
    {
        set { m_CanCollide = value; }
        get { return m_CanCollide; }
    }

    /// <summary>
    /// 弾が生存しているかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_IsAlive;
    public OperationBoolBase IsAlive
    {
        set { m_IsAlive = value; }
        get { return m_IsAlive; }
    }


    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public TransformSimple GetResultTransform()
    {
        return new TransformSimple(
            m_Position.GetResultVector2(),
            m_Angle.GetResultFloat(),
            m_Scale.GetResultFloat(),
            m_Opacity != null ? m_Opacity.GetResultFloat() : 1,
            m_CanCollide != null ? m_CanCollide.GetResultBool() : true,
            m_IsAlive != null ? m_IsAlive.GetResultBool() : true
            );
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

        float opacity;

        if (m_Opacity == null)
        {
            opacity = shotParam.Opacity;
        }
        else
        {
            opacity = m_Opacity.GetResultFloat();
        }

        bool canCollide;

        if (m_CanCollide == null)
        {
            canCollide = shotParam.CanCollide;
        }
        else
        {
            canCollide = m_CanCollide.GetResultBool();
            //canCollide = false;
        }

        bool isAlive;

        if (m_IsAlive == null)
        {
            isAlive =
                -0.91 <= position.x && position.x <= 0.91 &&
                -1.1 <= position.y && position.y <= 1.1;
        }
        else
        {
            isAlive = m_IsAlive.GetResultBool();
        }

        return new TransformSimple(position, angle, scale, opacity, canCollide, isAlive);
    }
}