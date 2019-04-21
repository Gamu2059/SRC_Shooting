using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全てのアイテムオブジェクトの基礎クラス。
/// </summary>
public class ItemController : BattleMainObjectBase
{
    public const string ATTRACT_COLLIDE = "ATTRACT COLLIDE";
    public const string GAIN_COLLIDE = "GAIN COLLIDE";

    [Serializable]
    public enum E_ITEM_CYCLE
    {
        /// <summary>
        /// 発射される直前。
        /// </summary>
        STANDBY_UPDATE,

        /// <summary>
        /// 発射された後、動いている状態。
        /// </summary>
        UPDATE,

        /// <summary>
        /// プールされる準備状態。
        /// </summary>
        STANDBY_POOL,

        /// <summary>
        /// プーリングされた状態。
        /// </summary>
        POOLED,
    }


    [SerializeField]
    private E_ITEM_TYPE m_ItemType;

    [SerializeField]
    private int m_Point;

    [SerializeField]
    private Transform m_LookCameraTransform;




    #region Field

    /// <summary>
    /// アイテムのサイクル。
    /// </summary>
    private E_ITEM_CYCLE m_ItemCycle;

    /// <summary>
	/// このアイテムのOrbitalParam。
	/// </summary>
	private BulletOrbitalParam m_OrbitalParam;

    /// <summary>
    /// このアイテムが1秒間にどれくらい回転するか。
    /// </summary>
    private Vector3 m_NowDeltaRotation;

    /// <summary>
    /// このアイテムが1秒間にどれくらい移動するか。
    /// </summary>
    private float m_NowSpeed;

    /// <summary>
    /// このアイテムが1秒間にどれくらい加速するか。
    /// </summary>
    private float m_NowAccel;

    /// <summary>
    /// 現在の回転。
    /// </summary>
    private float m_NowRotateAngle;

    /// <summary>
    /// 非表示になって破棄されるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;

    /// <summary>
    /// 引き寄せられるかどうか。
    /// </summary>
    private bool m_IsAttract;

    #endregion



    #region Get Set

    /// <summary>
    /// このアイテムの種類を取得する。
    /// </summary>
    public E_ITEM_TYPE GetItemType()
    {
        return m_ItemType;
    }

    /// <summary>
    /// このアイテムのサイクルを取得する。
    /// </summary>
    public E_ITEM_CYCLE GetItemCycle()
    {
        return m_ItemCycle;
    }

    /// <summary>
    /// このアイテムのサイクルを設定する。
    /// </summary>
    public void SetItemCycle(E_ITEM_CYCLE value)
    {
        m_ItemCycle = value;
    }

    /// <summary>
    /// このアイテムのポイントを取得する。
    /// </summary>
    public int GetPoint()
    {
        return m_Point;
    }

    /// <summary>
    /// このアイテムのOrbialParam。
    /// </summary>
    public BulletOrbitalParam GetOrbitalParam()
    {
        return m_OrbitalParam;
    }

    /// <summary>
    /// このアイテムのローカル座標を取得する。
    /// </summary>
    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }

    /// <summary>
    /// このアイテムのローカル座標を設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetPosition(Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        transform.localPosition = GetRelativeValue(relative, GetPosition(), value);
    }

    /// <summary>
    /// このアイテムのローカルオイラー回転を取得する。
    /// </summary>
    public Vector3 GetRotation()
    {
        return transform.localEulerAngles;
    }

    /// <summary>
    /// このアイテムのローカルオイラー回転を設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetRotation(Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        transform.localEulerAngles = GetRelativeRotateValue(relative, GetRotation(), value);
    }

    /// <summary>
    /// このアイテムのローカルスケールを取得する。
    /// </summary>
    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    /// <summary>
    /// このアイテムのローカルスケールを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetScale(Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        transform.localScale = GetRelativeValue(relative, GetScale(), value);
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい回転するかを取得する。
    /// </summary>
    public Vector3 GetNowDeltaRotation()
    {
        return m_NowDeltaRotation;
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい回転するかを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowDeltaRotation(Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        m_NowDeltaRotation = GetRelativeValue(relative, GetNowDeltaRotation(), value);
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい移動するかを取得する。
    /// </summary>
    public float GetNowSpeed()
    {
        return m_NowSpeed;
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい移動するかを取得する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowSpeed(float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        m_NowSpeed = GetRelativeValue(relative, GetNowSpeed(), value);
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい加速するかを取得する。
    /// </summary>
    public float GetNowAccel()
    {
        return m_NowAccel;
    }

    /// <summary>
    /// このアイテムが1秒間にどれくらい加速するかを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowAccel(float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE)
    {
        m_NowAccel = GetRelativeValue(relative, GetNowAccel(), value);
    }

    #endregion
    


    /// <summary>
    /// このアイテムが生成された瞬間に呼び出される処理。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        m_NowRotateAngle = 0;
        m_CanOutDestroy = false;
        m_IsAttract = false;
    }

    public override void OnUpdate()
    {
        var speed = GetNowSpeed() * Time.deltaTime;

        if (!m_IsAttract) {
            SetNowSpeed(GetNowAccel() * Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE);
            SetPosition(transform.forward * speed, E_ATTACK_PARAM_RELATIVE.RELATIVE);
        } else
        {
            var player = PlayerCharaManager.Instance.GetCurrentController();
            if (player != null)
            {
                var nextPos = Vector3.Lerp(GetPosition(), player.transform.localPosition, ItemManager.Instance.GetItemAttractRate());
                SetPosition(nextPos);
            }
        }

        m_NowRotateAngle += GetNowDeltaRotation().z * Time.deltaTime;
        if (speed <= 0)
        {
            m_NowRotateAngle = 0;
        }

        if (m_LookCameraTransform != null)
        {
            m_LookCameraTransform.LookAt(CameraManager.Instance.GetCamera().transform);
            var angle = m_LookCameraTransform.localEulerAngles;
            angle.y = 180;
            angle.z = m_NowRotateAngle;
            m_LookCameraTransform.localEulerAngles = angle;
        }
    }

    protected virtual void OnBecameVisible()
    {
        m_CanOutDestroy = true;
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            DestroyItem();
        }
    }

    protected Vector3 GetRelativeValue(E_ATTACK_PARAM_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_ATTACK_PARAM_RELATIVE.RELATIVE)
        {
            return baseValue + relativeValue;
        }
        else
        {
            return relativeValue;
        }
    }

    protected Vector3 GetRelativeRotateValue(E_ATTACK_PARAM_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_ATTACK_PARAM_RELATIVE.RELATIVE)
        {
            var value = baseValue + relativeValue;

            if (relativeValue.x != 0)
                value.x = value.x % 360 + 360;

            if (relativeValue.y != 0)
                value.y = value.y % 360 + 360;

            if (relativeValue.z != 0)
                value.z = value.z % 360 + 360;

            return value;
        }
        else
        {
            return relativeValue;
        }
    }

    protected float GetRelativeValue(E_ATTACK_PARAM_RELATIVE relative, float baseValue, float relativeValue)
    {
        if (relative == E_ATTACK_PARAM_RELATIVE.RELATIVE)
        {
            return baseValue + relativeValue;
        }
        else
        {
            return relativeValue;
        }
    }

    /// <summary>
    /// このアイテムの軌道情報を上書きする。
    /// </summary>
    public virtual void ChangeOrbital(BulletOrbitalParam orbitalParam)
    {
        m_OrbitalParam = orbitalParam;

        // 各種パラメータの上書き
        SetPosition(m_OrbitalParam.Position, m_OrbitalParam.PositionRelative);
        SetRotation(m_OrbitalParam.Rotation, m_OrbitalParam.RotationRelative);
        SetScale(m_OrbitalParam.Scale, m_OrbitalParam.ScaleRelative);
        SetNowDeltaRotation(m_OrbitalParam.DeltaRotation, m_OrbitalParam.DeltaRotationRelative);
        SetNowSpeed(m_OrbitalParam.Speed, m_OrbitalParam.SpeedRelative);
        SetNowAccel(m_OrbitalParam.Accel, m_OrbitalParam.AccelRelative);
    }

    /// <summary>
    /// このアイテムを破棄する。
    /// </summary>
    public virtual void DestroyItem()
    {
        if (m_ItemCycle == E_ITEM_CYCLE.UPDATE)
        {
            ItemManager.Instance.CheckPoolItem(this);
        }
    }

    /// <summary>
    /// プレイヤーに引き寄せられるようにする。
    /// </summary>
    public void AttractPlayer()
    {
        m_IsAttract = true;
    }

    /// <summary>
    /// 他のキャラから当てられた時の処理。
    /// </summary>
    /// <param name="attackChara">他のキャラ</param>
    /// <param name="attackData">他のキャラの衝突情報</param>
    /// <param name="targetData">このアイテムの衝突情報</param>
    public virtual void SufferChara(CharaController attackChara, ColliderData attackData, ColliderData targetData)
    {
        if (targetData.CollideName == ATTRACT_COLLIDE)
        {
            AttractPlayer();
        }
        else if (targetData.CollideName == GAIN_COLLIDE)
        {
            DestroyItem();
        }
    }
}
