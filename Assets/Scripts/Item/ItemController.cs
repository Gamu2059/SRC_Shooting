using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全てのアイテムオブジェクトの基礎クラス。
/// </summary>
public class ItemController : BattleRealObjectBase
{
    public const string ATTRACT_COLLIDE = "ATTRACT COLLIDE";
    public const string GAIN_COLLIDE = "GAIN COLLIDE";

    [SerializeField]
    private E_ITEM_TYPE m_ItemType;

    [SerializeField]
    private int m_Point;

    [SerializeField]
    private Transform m_ViewTransform;

    #region Field

    /// <summary>
    /// アイテムのサイクル。
    /// </summary>
    private E_POOLED_OBJECT_CYCLE m_Cycle;

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

    private HitSufferController<CharaController> m_CharaSuffer;

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
    public E_POOLED_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    /// <summary>
    /// このアイテムのサイクルを設定する。
    /// </summary>
    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
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
    public void SetPosition(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
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
    public void SetRotation(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
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
    public void SetScale(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
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
    public void SetNowDeltaRotation(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
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
    public void SetNowSpeed(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
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
    public void SetNowAccel(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowAccel = GetRelativeValue(relative, GetNowAccel(), value);
    }

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();

        m_CharaSuffer = new HitSufferController<CharaController>();
    }

    protected override void OnDestroyed()
    {
        m_CharaSuffer.OnFinalize();
        m_CharaSuffer = null;
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CharaSuffer.OnEnter = OnEnterSufferChara;
        m_CharaSuffer.OnStay = OnStaySufferChara;
        m_CharaSuffer.OnExit = OnExitSufferChara;

        m_NowRotateAngle = 0;
        m_CanOutDestroy = false;
        m_IsAttract = false;
    }

    public override void OnFixedUpdate()
    {
        m_CharaSuffer.OnFinalize();
        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        var speed = GetNowSpeed() * Time.deltaTime;

        if (!m_IsAttract) {
            SetNowSpeed(GetNowAccel() * Time.deltaTime, E_RELATIVE.RELATIVE);
            SetPosition(transform.forward * speed, E_RELATIVE.RELATIVE);
        } else
        {
            //var player = BattleRealPlayerManager.Instance.GetCurrentController();
            //if (player != null)
            //{
            //    var nextPos = Vector3.Lerp(GetPosition(), player.transform.localPosition, BattleRealItemManager.Instance.GetItemAttractRate());
            //    SetPosition(nextPos);
            //}
        }

        m_NowRotateAngle += GetNowDeltaRotation().z * Time.deltaTime;
        if (speed <= 0)
        {
            m_NowRotateAngle = 0;
        }

        if (m_ViewTransform != null)
        {
            var angle = m_ViewTransform.localEulerAngles;
            angle.y = 0;
            angle.z = m_NowRotateAngle;
            m_ViewTransform.localEulerAngles = angle;
        }
    }

    #endregion

    protected Vector3 GetRelativeValue(E_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
        {
            return baseValue + relativeValue;
        }
        else
        {
            return relativeValue;
        }
    }

    protected Vector3 GetRelativeRotateValue(E_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
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

    protected float GetRelativeValue(E_RELATIVE relative, float baseValue, float relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
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
        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            //BattleRealItemManager.Instance.CheckPoolItem(this);
        }
    }

    /// <summary>
    /// プレイヤーに引き寄せられるようにする。
    /// </summary>
    public void AttractPlayer()
    {
        m_IsAttract = true;
    }

    #region Impl IColliderProcess

    public override void ClearColliderFlag()
    {
        m_CharaSuffer.ClearUpdateFlag();
    }

    public override void ProcessCollision()
    {
        m_CharaSuffer.ProcessCollision();
    }

    #endregion

    #region Suffer Chara

    /// <summary>
    /// 他のキャラから当てられた時の処理。
    /// </summary>
    /// <param name="attackChara">他のキャラ</param>
    /// <param name="attackData">他のキャラの衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void SufferChara(CharaController attackChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaSuffer.Put(attackChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferChara(HitSufferData<CharaController> sufferData)
    {

    }

    protected virtual void OnStaySufferChara(HitSufferData<CharaController> sufferData)
    {

    }

    protected virtual void OnExitSufferChara(HitSufferData<CharaController> sufferData)
    {

    }

    #endregion
}
