#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの全てのアイテムオブジェクトの基礎クラス。
/// </summary>
public class BattleRealItemController : BattleRealObjectBase
{
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
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    private bool m_IsShowFirst;

    private bool m_IsAttract;

    private float m_AttractRate;

    public E_ITEM_TYPE ItemType { get; private set; }

    public int ItemPoint { get; private set; }

    private HitSufferController<BattleRealCharaController> m_CharaSuffer;

    #endregion

    #region Get &  Set

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

        m_CharaSuffer = new HitSufferController<BattleRealCharaController>();
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
        m_IsShowFirst = false;
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

        if (!m_IsAttract)
        {
            SetNowSpeed(GetNowAccel() * Time.deltaTime, E_RELATIVE.RELATIVE);
            SetPosition(transform.forward * speed, E_RELATIVE.RELATIVE);
        }
        else
        {
            var player = BattleRealPlayerManager.Instance.Player;
            if (player != null)
            {
                var nextPos = Vector3.Lerp(GetPosition(), player.transform.localPosition, m_AttractRate);
                SetPosition(nextPos);
            }
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

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        // 上方に移動している間は場外判定を行わない
        if (GetNowSpeed() >= 0)
        {
            return;
        }

        var isOutOfField = BattleRealItemManager.Instance.IsOutOfField(this);
        if (isOutOfField)
        {
            if (m_IsShowFirst)
            {
                Destroy();
            }
        }
        else
        {
            m_IsShowFirst = true;
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

    public void SetParam(Vector3 worldPos, E_ITEM_TYPE type, int point, float attractRate)
    {
        ItemType = type;
        ItemPoint = point;
        m_AttractRate = attractRate;
        transform.position = worldPos;
    }

    /// <summary>
    /// このアイテムの軌道情報を上書きする。
    /// </summary>
    public void ChangeOrbital(BulletOrbitalParam orbitalParam)
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
    public void SufferChara(BattleRealCharaController attackChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaSuffer.Put(attackChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.ITEM_GAIN)
        {
            Destroy();
        }
    }

    protected virtual void OnStaySufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {

    }

    protected virtual void OnExitSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {

    }

    #endregion

    /// <summary>
    /// このアイテムを破棄する。
    /// </summary>
    public void Destroy()
    {
        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            BattleRealItemManager.Instance.CheckPoolItem(this);
        }
    }
}
