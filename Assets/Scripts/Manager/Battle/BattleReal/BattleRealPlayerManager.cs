using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// リアルモードのプレイヤーキャラを管理する。
/// </summary>
public class BattleRealPlayerManager : Singleton<BattleRealPlayerManager>, IColliderProcess
{
    #region Field

    public BattleRealPlayerManagerParamSet ParamSet { get; private set; }

    private Transform m_PlayerCharaHolder;

    public BattleRealPlayerController Player { get; private set; }

    #endregion

    public static BattleRealPlayerManager Builder(BattleRealManager realManager, BattleRealPlayerManagerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.SetCallback(realManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealPlayerManagerParamSet param)
    {
        ParamSet = param;
    }

    private void SetCallback(BattleRealManager realManager)
    {
        realManager.ChangeStateAction += OnChangeStateBattleRealManager;
    }

    #region Game Cycle

    public override void OnFinalize()
    {
        Player?.OnFinalize();
        Player = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_PlayerCharaHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.PLAYER);

        Player = GameObject.Instantiate(ParamSet.PlayerPrefab);
        Player.transform.SetParent(m_PlayerCharaHolder);
        SetInitPlayerPosition();
        Player.SetParam(ParamSet);
        Player.OnInitialize();
        Player.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Player?.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        Player?.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Player?.OnFixedUpdate();
    }

    #endregion

    private void OnChangeStateBattleRealManager(E_BATTLE_REAL_STATE state)
    {
        Player?.OnChangeStateBattleRealManager(state);
    }

    /// <summary>
    /// ゲーム開始時位置にプレイヤーをセットする。
    /// </summary>
    public void SetInitPlayerPosition()
    {
        if (Player == null)
        {
            return;
        }

        var pos = GetPosFromViewportPosition(ParamSet.InitAppearViewportPosition);
        Player.transform.position = pos;
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    private Vector3 GetPosFromViewportPosition(Vector2 viewportPos)
    {
        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;

        var factX = (maxPos.x - minPos.x) * viewportPos.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * viewportPos.y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);
        pos += m_PlayerCharaHolder.position;

        return pos;
    }

    public void ClearColliderFlag()
    {
        Player?.ClearColliderFlag();
    }

    public void UpdateCollider()
    {
        Player?.UpdateCollider();
    }

    public void ProcessCollision()
    {
        Player?.ProcessCollision();
    }

    public void StopChargeShot()
    {
        Player?.StopChargeShot();
    }

    /// <summary>
    /// プレイヤーを死亡させる。
    /// </summary>
    public void DeadPlayer()
    {
        Player?.Dead();
        Player?.RequestChangeToDeadState();
    }

    /// <summary>
    /// プレイヤーを復活させる。
    /// </summary>
    /// <param name="enableInvinsible">無敵状態を適用するかどうか</param>
    public void RespawnPlayer(bool enableInvinsible)
    {
        MovePlayerBySequence(ParamSet.RespawnSequence);

        if (enableInvinsible)
        {
            MakeInvinsiblePlayer();
        }
    }

    /// <summary>
    /// プレイヤーを無敵状態にする。
    /// </summary>
    public void MakeInvinsiblePlayer()
    {
        Player?.SetInvinsible(5f);
    }

    public void MovePlayerBySequence(SequenceGroup sequenceGroup)
    {
        Player?.MoveBySequence(sequenceGroup);
    }

    public void RestrictPlayerPosition()
    {
        if (Player != null)
        {
            Player.IsRestrictPosition = true;
        }
    }
}