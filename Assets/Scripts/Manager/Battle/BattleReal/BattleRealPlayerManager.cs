using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// リアルモードのプレイヤーキャラを管理する。
/// </summary>
public class BattleRealPlayerManager : ControllableObject, IUpdateCollider
{
    public static BattleRealPlayerManager Instance => BattleRealManager.Instance.PlayerManager;

    #region Inspector

    [Header("State")]

    [SerializeField]
    private PlayerState m_PlayerState;

    [SerializeField]
    private FloatReactiveProperty m_CurrentScore;

    [SerializeField]
    private IntReactiveProperty m_CurrentLevel;

    [SerializeField]
    private IntReactiveProperty m_CurrentExp;

    [SerializeField]
    private FloatReactiveProperty m_CurrentBombCharge;

    [SerializeField]
    private IntReactiveProperty m_CurrentBombNum;

    #endregion

    #region Field

    private BattleRealPlayerManagerParamSet m_ParamSet;

    private Transform m_PlayerCharaHolder;

    // 事前にシーンに存在していたプレイヤー
    private static BattleRealPlayerController m_RegisteredPlayer;

    private BattleRealPlayerController m_Player;
    public BattleRealPlayerController Player => m_Player;

    public bool IsNormalWeapon { get; private set; }

    public bool IsLaserType { get; private set; }

    #endregion

    #region Get Set

    public FloatReactiveProperty GetCurrentScore()
    {
        return m_CurrentScore;
    }

    public IntReactiveProperty GetCurrentLevel()
    {
        return m_CurrentLevel;
    }

    public IntReactiveProperty GetCurrentExp()
    {
        return m_CurrentExp;
    }

    public FloatReactiveProperty GetCurrentBombCharge()
    {
        return m_CurrentBombCharge;
    }

    public IntReactiveProperty GetCurrentBombNum()
    {
        return m_CurrentBombNum;
    }

    #endregion


    public BattleRealPlayerManager(BattleRealPlayerManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    /// <summary>
    /// プレイヤーキャラを登録する。
    /// デバッグ用。
    /// </summary>
    public static void RegistPlayer(BattleRealPlayerController player)
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

        IsLaserType = true;
        IsNormalWeapon = true;
    }

    public override void OnFinalize()
    {
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

        var pos = GetInitAppearPosition();
        m_Player.transform.SetParent(m_PlayerCharaHolder);
        m_Player.transform.position = pos;
        m_Player.OnInitialize();

        InitPlayerState();
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

        if (IsNormalWeapon)
        {
            if (input.Shot == E_INPUT_STATE.STAY)
            {
                Player.ShotBullet();
            }
        }
        else
        {
            if (input.Shot == E_INPUT_STATE.STAY)
            {
                if (IsLaserType)
                {
                    Debug.Log("Charging Laser...");
                }
                else
                {
                    Debug.Log("Charging Bomb...");
                }
            }
            else if (input.Shot == E_INPUT_STATE.UP)
            {
                if (IsLaserType)
                {
                    Debug.Log("Shot Laser!");
                }
                else
                {
                    Debug.Log("Shot Bomb!");
                }
            }
        }

        if (input.ChangeMode == E_INPUT_STATE.DOWN)
        {
            Debug.Log("Change Weapon");
            IsNormalWeapon = !IsNormalWeapon;
        }

        if (input.Cancel == E_INPUT_STATE.DOWN)
        {
            BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
        }

        m_Player.OnUpdate();
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

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    public Vector3 GetInitAppearPosition()
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

    /// <summary>
    /// プレイヤーステートを初期化する。
    /// </summary>
    public void InitPlayerState()
    {
        m_CurrentScore = new FloatReactiveProperty(0);
        m_CurrentLevel = new IntReactiveProperty(1);
        m_CurrentExp = new IntReactiveProperty(0);
        m_CurrentBombCharge = new FloatReactiveProperty(0f);
        m_CurrentBombNum = new IntReactiveProperty(0);
    }

    /// <summary>
    /// スコアを加算する。
    /// </summary>
    public void AddScore(float score)
    {
        m_CurrentScore.Value += score;
    }

    /// <summary>
    /// 経験値を加算する。
    /// </summary>
    public void AddExp(int exp)
    {
        var currentExp = m_CurrentExp.Value;
        currentExp += exp;

        var currentLevel = m_CurrentLevel.Value - 1;
        var needExp = m_PlayerState.NextNeedExpParams[currentLevel];

        if (currentExp >= needExp)
        {
            m_CurrentLevel.Value++;
            currentExp %= needExp;
            // Call LevelUp Action
        }

        m_CurrentExp.Value = currentExp;
    }

    /// <summary>
    /// ボムチャージを加算する。
    /// </summary>
    public void AddBombCharge(float charge)
    {
        var currentCharge = m_CurrentBombCharge.Value;
        currentCharge += charge;

        if (currentCharge >= m_PlayerState.BombCharge)
        {
            m_CurrentBombNum.Value++;
            currentCharge %= m_PlayerState.BombCharge;
        }

        m_CurrentBombCharge.Value = currentCharge;
    }

    public void UpdateCollider()
    {
        if (Player == null)
        {
            return;
        }

        Player.UpdateCollider();
    }
}
