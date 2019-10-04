using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// リアルモードのプレイヤーキャラを管理する。
/// </summary>
public class BattleRealPlayerManager : ControllableObject
{
    public const string HOLDER_NAME = "[PlayerCharaHolder]";

    #region Inspector

    [Header("Holder")]

    [SerializeField]
    private Transform m_PlayerCharaHolder;


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

    // 事前にシーンに存在していたプレイヤー
    private static BattleRealPlayerController m_RegisterPlayer;

    private BattleRealPlayerController m_Player;
    public BattleRealPlayerController Player => m_Player;


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
    /// 後にprivateに変更する。
    /// </summary>
    public static void RegistPlayer(BattleRealPlayerController player)
    {
        if (player == null)
        {
            return;
        }

        m_RegisterPlayer = player;
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

        var stageManager = BattleManager.Instance.BattleRealStageManager;
        if (stageManager != null && stageManager.PlayerCharaHolder != null)
        {
            m_PlayerCharaHolder = stageManager.PlayerCharaHolder;
        }
        else if (m_PlayerCharaHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_PlayerCharaHolder = obj.transform;
        }

        if (m_RegisterPlayer != null)
        {
            m_Player = m_RegisterPlayer;
        }
        else
        {
            Debug.LogError(11111);
            m_Player = GameObject.Instantiate(m_ParamSet.PlayerPrefab);
        }

        var pos = GetInitAppearPosition();
        m_Player.transform.SetParent(m_PlayerCharaHolder);
        m_Player.transform.position = pos;
        m_Player.OnInitialize();

        InitPlayerState();

        Debug.Log("PlayerManager");
    }

    public override void OnUpdate()
    {
        if (m_Player == null)
        {
            return;
        }

        m_Player.OnUpdate();
        RestrictCharaPosition();
    }

    /// <summary>
    /// キャラの座標を動体フィールド領域に制限する。
    /// </summary>
    public void RestrictCharaPosition()
    {
        if (m_Player == null)
        {
            return;
        }

        var stageManager = BattleManager.Instance.BattleRealStageManager;
        stageManager.ClampMovingObjectPosition(m_Player.transform);
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    public Vector3 GetInitAppearPosition()
    {
        //var minPos = StageManager.Instance.GetMinLocalPositionField();
        //var maxPos = StageManager.Instance.GetMaxLocalPositionField();

        //var factX = (maxPos.x - minPos.x) * m_InitAppearViewportPosition.x + minPos.x;
        //var factZ = (maxPos.y - minPos.y) * m_InitAppearViewportPosition.y + minPos.y;
        //var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);
        //pos += StageManager.Instance.GetMoveObjectHolder().transform.position;

        //return pos;
        return Vector3.zero;
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

    /// <summary>
    /// 横軸入力のコールバック。
    /// </summary>
    public void OnInputHorizontal(float value)
    {
        //if (m_CurrentController == null) return;

        //m_CharaMoveDir += Vector3.right * value;
    }

    /// <summary>
    /// 縦軸入力のコールバック。
    /// </summary>
    public void OnInputVertical(float value)
    {
        //if (m_CurrentController == null) return;

        //m_CharaMoveDir += Vector3.forward * value;
    }

    /// <summary>
    /// キャラ切替のコールバック。
    /// </summary>
    public void OnInputChangeChara(float value)
    {
        //if (value == 0)
        //{
        //    return;
        //}

        //int charaNum = m_Controllers.Count;
        //if (value > 0)
        //{
        //    ChangeChara((m_CharaIndex + 1 + charaNum) % charaNum);
        //}
        //else if (value < 0)
        //{
        //    ChangeChara((m_CharaIndex - 1 + charaNum) % charaNum);
        //}
    }

    /// <summary>
    /// 弾を撃つコールバック。
    /// </summary>
    public void OnInputShot(InputExtension.E_INPUT_STATE state)
    {
        //if (m_CurrentController == null) return;

        //m_CurrentController.ShotBullet(state);
    }

    /// <summary>
    /// ボムを撃つコールバック。
    /// </summary>
    public void OnInputBomb(InputExtension.E_INPUT_STATE state)
    {
        //if (m_CurrentController == null) return;

        //if (state != InputExtension.E_INPUT_STATE.DOWN) return;

        //if (m_CurrentBombNum.Value < 1) return;

        //m_CurrentBombNum.Value--;
        //m_CurrentController.ShotBomb(state);
    }
}
