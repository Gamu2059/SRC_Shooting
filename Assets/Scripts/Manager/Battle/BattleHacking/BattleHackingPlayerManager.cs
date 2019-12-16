using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// リアルモードのプレイヤーキャラを管理する。
/// </summary>
public class BattleHackingPlayerManager : ControllableObject
{
    #region Field

    private Transform m_PlayerCharaHolder;

    public BattleHackingPlayerManagerParamSet ParamSet { get; private set; }

    public BattleHackingPlayerController Player { get; private set; }

    #endregion

    public static BattleHackingPlayerManager Instance => BattleHackingManager.Instance.PlayerManager;

    public BattleHackingPlayerManager(BattleHackingPlayerManagerParamSet paramSet)
    {
        ParamSet = paramSet;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (m_PlayerCharaHolder == null)
        {
            m_PlayerCharaHolder = BattleHackingStageManager.Instance.GetHolder(BattleHackingStageManager.E_HOLDER_TYPE.PLAYER);
        }

        if (Player == null)
        {
            Player = GameObject.Instantiate(ParamSet.PlayerPrefab);
            Player.transform.SetParent(m_PlayerCharaHolder);
            Player.OnInitialize();
        }
    }

    public override void OnUpdate()
    {
        if (Player == null)
        {
            return;
        }

        var input = BattleHackingInputManager.Instance;

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

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleHackingEnemyManager.Instance.KillAllBoss();
        }
#endif

        Player.OnUpdate();
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

        var stageManager = BattleHackingStageManager.Instance;
        stageManager.ClampMovingObjectPosition(Player.transform);
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    private Vector3 GetInitAppearPosition(Vector2 viewportPos)
    {
        var stageManager = BattleHackingStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;

        var factX = (maxPos.x - minPos.x) * viewportPos.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * viewportPos.y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);
        pos += m_PlayerCharaHolder.position;

        return pos;
    }

    public void OnPrepare(BattleHackingLevelParamSet levelParamSet)
    {
        if (Player != null)
        {
            var pos = GetInitAppearPosition(levelParamSet.InitAppearViewportPosition);
            Player.transform.position = pos;
            Player.gameObject.SetActive(true);
            Player.OnStart();
        }
    }

    public void OnPutAway()
    {
        if (Player != null)
        {
            Player.gameObject.SetActive(false);
        }
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
}
