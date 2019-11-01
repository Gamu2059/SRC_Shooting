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

    private BattleHackingPlayerManagerParamSet m_ParamSet;

    public BattleHackingPlayerController Player { get; private set; }

    private bool m_IsShotNormal;

    #endregion

    public static BattleHackingPlayerManager Instance => BattleHackingManager.Instance.PlayerManager;

    public BattleHackingPlayerManager(BattleHackingPlayerManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
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
            Player = GameObject.Instantiate(m_ParamSet.PlayerPrefab);
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
                speed = m_ParamSet.PlayerSlowMoveSpeed;
            }
            else
            {
                speed = m_ParamSet.PlayerBaseMoveSpeed;
            }

            var move = moveDir.ToVector3XZ() * speed * Time.deltaTime;
            Player.transform.Translate(move, Space.World);
        }

        // 移動直後に位置制限を掛ける
        RestrictPlayerPosition();

        switch (input.Shot)
        {
            case E_INPUT_STATE.DOWN:
                break;
            case E_INPUT_STATE.STAY:
                if (!m_IsShotNormal)
                {
                    m_IsShotNormal = true;
                    AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_PlayerShot02");
                }
                Player.ShotBullet();
                break;
            case E_INPUT_STATE.UP:
                m_IsShotNormal = false;
                AudioManager.Instance.StopSe(AudioManager.E_SE_GROUP.PLAYER);
                break;
            case E_INPUT_STATE.NONE:
                break;
        }

        /* 
        if (input.Cancel == E_INPUT_STATE.DOWN)
        {
            // リアルモードと違って、暫定でハッキングモードをクリアしたことにする
            GameManager.Instance.PlayerData.IncreaseHackingSucceedCount();
            if(GameManager.Instance.PlayerData.m_HackingSucceedCount >= 1){
                BattleRealPlayerManager.Instance.AddScore(1000 * GameManager.Instance.PlayerData.m_HackingSucceedCount);
            }
            BattleHackingManager.Instance.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
        }

        if(input.Slow == E_INPUT_STATE.DOWN){
            // 暫定でハッキングモードに失敗したことにする
            GameManager.Instance.PlayerData.ResetHackingSucceedCount();
            //Debug.Log("Hacking failed... : " + GameManager.Instance.m_HackingSucceedCount);
        }
        */
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

    public void ResetShotFlag()
    {
        m_IsShotNormal = false;
    }
}
