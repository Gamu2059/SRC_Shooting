using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// リアルモードのプレイヤーキャラを管理する。
/// </summary>
public class BattleRealPlayerManager : ControllableObject, IColliderProcess
{
    public static BattleRealPlayerManager Instance {
        get {
            if (BattleRealManager.Instance == null)
            {
                return null;
            }

            return BattleRealManager.Instance.PlayerManager;
        }
    }

    #region Field

    public BattleRealPlayerManagerParamSet ParamSet { get; private set; }

    private Transform m_PlayerCharaHolder;

    // 事前にシーンに存在していたプレイヤー
    private static BattleRealPlayerController m_RegisteredPlayer;

    public BattleRealPlayerController Player { get; private set; }

    public bool IsLaserType { get; private set; }

    public Action<bool> OnChangeWeaponType;

    #endregion

    public BattleRealPlayerManager(BattleRealPlayerManagerParamSet paramSet)
    {
        ParamSet = paramSet;
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

    public override void OnInitialize()
    {
        base.OnInitialize();

        IsLaserType = ParamSet.IsLaserType;
    }

    public override void OnFinalize()
    {
        OnChangeWeaponType = null;
        base.OnFinalize();
    }

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
        InitPlayerPosition();
        Player.OnInitialize();
        Player.OnStart();
    }

    public override void OnUpdate()
    {
        if (Player == null)
        {
            return;
        }

        var input = BattleRealInputManager.Instance;

        var moveDir = input.MoveDir;
        if (moveDir.x != 0 || moveDir.y != 0)
        {
            float speed = 0;
            if (input.Slow == E_INPUT_STATE.STAY)
            {
                speed = ParamSet.PlayerSlowMoveSpeed;
            }
            else
            {
                speed = ParamSet.PlayerBaseMoveSpeed;
            }

            var move = moveDir.ToVector3XZ() * speed * Time.deltaTime;
            Player.transform.Translate(move, Space.World);
        }

        // 移動直後に位置制限を掛ける
        RestrictPlayerPosition();

        if (input.Shot == E_INPUT_STATE.STAY)
        {
            Player.ShotBullet();
        }

        switch (input.ChargeShot)
        {
            case E_INPUT_STATE.DOWN:
            case E_INPUT_STATE.STAY:
                Player.ChargeUpdate();
                break;
            case E_INPUT_STATE.UP:
                Player.ChargeRelease();
                break;
        }

        if (input.ChangeMode == E_INPUT_STATE.DOWN)
        {
            IsLaserType = !IsLaserType;
            Player.ChangeWeapon();
            OnChangeWeaponType?.Invoke(IsLaserType);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
        }

        Player.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (Player == null)
        {
            return;
        }

        Player.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (Player == null)
        {
            return;
        }

        Player.OnFixedUpdate();
    }

    /// <summary>
    /// キャラの座標を動体フィールド領域に制限する。
    /// </summary>
    private void RestrictPlayerPosition()
    {
        if (Player == null)
        {
            return;
        }

        var stageManager = BattleRealStageManager.Instance;
        stageManager.ClampMovingObjectPosition(Player.transform);
    }

    public void InitPlayerPosition()
    {
        if (Player == null)
        {
            return;
        }

        var pos = GetInitAppearPosition();
        Player.transform.position = pos;
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    private Vector3 GetInitAppearPosition()
    {
        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        var initViewPos = ParamSet.InitAppearViewportPosition;

        var factX = (maxPos.x - minPos.x) * initViewPos.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * initViewPos.y + minPos.y;
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
            if (IsLaserType)
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
}