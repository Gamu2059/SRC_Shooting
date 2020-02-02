using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// コマンドイベントの全ての弾オブジェクトの基礎クラス。
/// </summary>
[System.Serializable]
public class BattleHackingBulletController : BattleHackingObjectBase
{
    #region Field Inspector

    /// <summary>
    /// この弾のグループ名。
    /// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
    /// </summary>
    [SerializeField]
    protected string m_BulletGroupId = "Default Bullet";

    [SerializeField]
    protected bool m_IsDestoryOnCollide = false;

    #endregion



    #region Field

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているか。
    /// 同じ値同士を持つ弾やキャラには被弾しない。
    /// </summary>
    protected E_CHARA_TROOP m_Troop;

    /// <summary>
    /// この弾の状態。
    /// </summary>
    [SerializeField]
    protected E_POOLED_OBJECT_CYCLE m_Cycle;

    protected HitSufferController<CommandCharaController> m_CharaHit;

    #endregion



    #region Getter & Setter

    /// <summary>
    /// この弾のグループ名を取得する。
    /// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
    /// </summary>
    public string GetBulletGroupId()
    {
        return m_BulletGroupId;
    }

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているか。
    /// 同じ値同士を持つ弾やキャラには被弾しない。
    /// </summary>
    public E_CHARA_TROOP GetTroop()
    {
        return m_Troop;
    }

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているかを設定する。
    /// </summary>
    public void SetTroop(E_CHARA_TROOP troop)
    {
        m_Troop = troop;
    }

    /// <summary>
    /// この弾の状態を取得する。
    /// </summary>
    public E_POOLED_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    /// <summary>
    /// この弾の状態を設定する。
    /// </summary>
    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    /// <summary>
    /// この弾のローカル座標を取得する。
    /// </summary>
    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }

    #endregion


    #region Game Cycle


    protected override void OnAwake()
    {
        base.OnAwake();
        m_CharaHit = new HitSufferController<CommandCharaController>();
    }

    protected override void OnDestroyed()
    {
        m_CharaHit.OnFinalize();
        m_CharaHit = null;
        base.OnDestroyed();
    }

    public override void OnFinalize()
    {
        m_CharaHit.OnFinalize();
        base.OnFinalize();
    }

    #endregion

    #region Impl IColliderProcess

    public override void ClearColliderFlag()
    {
        m_CharaHit.ClearUpdateFlag();
    }

    public override void ProcessCollision()
    {
        m_CharaHit.ProcessCollision();
    }

    #endregion

    #region Hit Chara

    #endregion
}