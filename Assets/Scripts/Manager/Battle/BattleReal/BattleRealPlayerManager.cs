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

    #region Inspector

    [Header("State")]

    //[SerializeField]
    //private IntReactiveProperty m_CurrentLevel;

    //[SerializeField]
    //private IntReactiveProperty m_CurrentExp;

    //[SerializeField]
    //private FloatReactiveProperty m_CurrentBombCharge;

    //[SerializeField]
    //private IntReactiveProperty m_CurrentBombNum;

    #endregion

    #region Field

    private BattleRealPlayerManagerParamSet m_ParamSet;
    public BattleRealPlayerManagerParamSet ParamSet => m_ParamSet;

    private Transform m_PlayerCharaHolder;

    // 事前にシーンに存在していたプレイヤー
    private static BattleRealPlayerController m_RegisteredPlayer;

    private BattleRealPlayerController m_Player;
    public BattleRealPlayerController Player => m_Player;

    public bool IsLaserType { get; private set; }

    public Action<bool> OnChangeWeaponType;

    #endregion

    public BattleRealPlayerManager(BattleRealPlayerManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
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

        IsLaserType = m_ParamSet.IsLaserType;
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
            m_Player = m_RegisteredPlayer;
        }
        else
        {
            m_Player = GameObject.Instantiate(m_ParamSet.PlayerPrefab);
        }

        m_Player.transform.SetParent(m_PlayerCharaHolder);
        InitPlayerPosition();
        m_Player.OnInitialize();
        m_Player.OnStart();
    }

    public override void OnUpdate()
    {
        if (m_Player == null)
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
                speed = m_ParamSet.PlayerSlowMoveSpeed;
            }
            else
            {
                speed = m_ParamSet.PlayerBaseMoveSpeed;
            }

            var move = moveDir.ToVector3XZ() * speed * Time.deltaTime;
            m_Player.transform.Translate(move, Space.World);
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
                Player.ChargeShot();
                break;
        }

        if (input.ChangeMode == E_INPUT_STATE.DOWN)
        {
            IsLaserType = !IsLaserType;
            OnChangeWeaponType?.Invoke(IsLaserType);
        }

        m_Player.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_Player == null)
        {
            return;
        }

        m_Player.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (m_Player == null)
        {
            return;
        }

        m_Player.OnFixedUpdate();
    }

    /// <summary>
    /// キャラの座標を動体フィールド領域に制限する。
    /// </summary>
    private void RestrictPlayerPosition()
    {
        if (m_Player == null)
        {
            return;
        }

        var stageManager = BattleRealStageManager.Instance;
        stageManager.ClampMovingObjectPosition(m_Player.transform);
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
        var initViewPos = m_ParamSet.InitAppearViewportPosition;

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
            }
            else
            {
                Player.ShotBomb();
            }

            BattleRealCameraManager.Instance.Shake(m_ParamSet.CameraShakeParam);
        }
    }
}