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

    // 事前にシーンに存在していたプレイヤー
    private static BattleRealPlayerController m_RegisteredPlayer;

    public BattleRealPlayerController Player { get; private set; }

    #endregion

    public static BattleRealPlayerManager Builder(BattleRealManager realManager, BattleRealPlayerManagerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealPlayerManagerParamSet param)
    {
        ParamSet = param;
    }

    /// <summary>
    /// プレイヤーキャラを登録する。
    /// デバッグ用。
    /// </summary>
    public static void RegisterPlayer(BattleRealPlayerController player)
    {
        if (player == null)
        {
            return;
        }

        m_RegisteredPlayer = player;
    }

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        m_PlayerCharaHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.PLAYER);

        if (m_RegisteredPlayer != null)
        {
            Player = m_RegisteredPlayer;
        }
        else
        {
            Player = GameObject.Instantiate(ParamSet.PlayerPrefab);
        }

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
    /// リスポーン位置にプレイヤーをセットする。
    /// </summary>
    public void SetRespawnPlayerPosition()
    {
        if (Player == null)
        {
            return;
        }

        var pos = GetPosFromViewportPosition(ParamSet.RespawnViewportPosition);
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
        if (Player != null)
        {
            Player.ClearColliderFlag();
        }
    }

    public void UpdateCollider()
    {
        if (Player != null)
        {
            Player.UpdateCollider();
        }
    }

    public void ProcessCollision()
    {
        if (Player != null)
        {
            Player.ProcessCollision();
        }
    }

    public void SetPlayerActive(bool isEnable)
    {
        if (Player != null)
        {
            Player.gameObject.SetActive(isEnable);
        }
    }

    public void SetPlayerInvinsible()
    {
        if (Player != null)
        {
            Player.SetInvinsible();
        }
    }

    public void ChargeShot()
    {
        if (Player != null)
        {
            if (Player.IsLaserType)
            {
                Player.ShotLaser();
                BattleRealCameraManager.Instance.Shake(ParamSet.LaserShakeParam);
            }
            else
            {
                Player.ShotBomb();
                BattleRealCameraManager.Instance.Shake(ParamSet.BombShakeParam);
            }
        }
    }

    public void StopChargeShot()
    {
        if (Player != null)
        {
            Player.StopChargeShot();
        }
    }

    public void OnDeadPlayer()
    {

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