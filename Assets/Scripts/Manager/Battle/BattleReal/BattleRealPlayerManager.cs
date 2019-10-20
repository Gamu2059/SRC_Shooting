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
    public static BattleRealPlayerManager Instance
    {
        get
        {
            if (BattleRealManager.Instance == null)
            {
                return null;
            }

            return BattleRealManager.Instance.PlayerManager;
        }
    }

    #region Inspector

    [Header ("State")]

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

    private bool m_IsShotNormal;

    public static Action OnStartAction;

    #endregion

    #region Get Set

    public FloatReactiveProperty GetCurrentScore () {
        return m_CurrentScore;
    }

    public IntReactiveProperty GetCurrentLevel () {
        return m_CurrentLevel;
    }

    public IntReactiveProperty GetCurrentExp () {
        return m_CurrentExp;
    }

    public FloatReactiveProperty GetCurrentBombCharge () {
        return m_CurrentBombCharge;
    }

    public IntReactiveProperty GetCurrentBombNum () {
        return m_CurrentBombNum;
    }

    public BattleRealPlayerExpParamSet[] GetRealPlayerExpParamSet(){
        return m_ParamSet.BattleRealPlayerExpParamSets;
    }

    #endregion

    public BattleRealPlayerManager (BattleRealPlayerManagerParamSet paramSet) {
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

    public override void OnInitialize () {
        base.OnInitialize ();

        IsLaserType = m_ParamSet.IsLaserType;
        IsNormalWeapon = m_ParamSet.IsNormalWeapon;
    }

    public override void OnFinalize () {
        base.OnFinalize ();
    }

    public override void OnStart () {
        base.OnStart ();

        m_PlayerCharaHolder = BattleRealStageManager.Instance.GetHolder (BattleRealStageManager.E_HOLDER_TYPE.PLAYER);

        if (m_RegisteredPlayer != null) {
            m_Player = m_RegisteredPlayer;
        } else {
            m_Player = GameObject.Instantiate (m_ParamSet.PlayerPrefab);
        }

        var pos = GetInitAppearPosition ();
        m_Player.transform.SetParent (m_PlayerCharaHolder);
        m_Player.transform.position = pos;
        m_Player.OnInitialize ();

        InitPlayerState();

        OnStartAction?.Invoke();
        OnStartAction = null;
    }

    public override void OnUpdate () {
        if (m_Player == null) {
            return;
        }

        var input = BattleRealInputManager.Instance;

        var moveDir = input.MoveDir;
        if (moveDir.x != 0 || moveDir.y != 0) {
            float speed = 0;
            if (input.Slow == E_INPUT_STATE.STAY) {
                speed = m_ParamSet.PlayerSlowMoveSpeed;
            } else {
                speed = m_ParamSet.PlayerBaseMoveSpeed;
            }

            var move = moveDir.ToVector3XZ () * speed * Time.deltaTime;
            m_Player.transform.Translate (move, Space.World);
        }

        // 移動直後に位置制限を掛ける
        RestrictPlayerPosition ();

        if (IsNormalWeapon)
        {
            switch (input.Shot)
            {
                case E_INPUT_STATE.DOWN:
                    break;
                case E_INPUT_STATE.STAY:
                    if (!m_IsShotNormal)
                    {
                        m_IsShotNormal = true;
                        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_PlayerShot01");
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
        }
        else
        {
            if (input.Shot == E_INPUT_STATE.STAY)
            {
                if (IsLaserType)
                {
                    Player.ChargeLaser();
                }
                else
                {
                    Player.ChargeBomb();
                }
            }
            else if (input.Shot == E_INPUT_STATE.UP)
            {
                if (IsLaserType)
                {
                    Player.ShotLaser();
                }
                else
                {
                    Player.ShotBomb();
                }
            }
        }

        if (input.ChangeMode == E_INPUT_STATE.DOWN)
        {
            IsNormalWeapon = !IsNormalWeapon;
        }

        if (input.Cancel == E_INPUT_STATE.DOWN) {
            BattleManager.Instance.RequestChangeState (E_BATTLE_STATE.TRANSITION_TO_HACKING);
        }

        /* デバッグ用 */
        //Debug.Log("CurrentScore = " + m_CurrentScore);
        //Debug.Log("CurrentExp = " + m_CurrentExp);

        m_Player.OnUpdate();
    }

    /// <summary>
    /// キャラの座標を動体フィールド領域に制限する。
    /// </summary>
    private void RestrictPlayerPosition () {
        if (m_Player == null) {
            return;
        }

        var stageManager = BattleRealStageManager.Instance;
        stageManager.ClampMovingObjectPosition (m_Player.transform);
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    public Vector3 GetInitAppearPosition () {
        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        var initViewPos = m_ParamSet.InitAppearViewportPosition;

        var factX = (maxPos.x - minPos.x) * initViewPos.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * initViewPos.y + minPos.y;
        var pos = new Vector3 (factX, ParamDef.BASE_Y_POS, factZ);
        pos += m_PlayerCharaHolder.position;

        return pos;
    }

    /// <summary>
    /// プレイヤーステートを初期化する。
    /// </summary>
    public void InitPlayerState () {
        m_CurrentScore = new FloatReactiveProperty (0);
        m_CurrentLevel = new IntReactiveProperty (1);
        m_CurrentExp = new IntReactiveProperty (0);
        m_CurrentBombCharge = new FloatReactiveProperty (0f);
        m_CurrentBombNum = new IntReactiveProperty (0);
    }

    /// <summary>
    /// スコアを加算する。
    /// </summary>
    public void AddScore (float score) {
        m_CurrentScore.Value += score;
    }

    /// <summary>
    /// 経験値を加算する。
    /// </summary>
    public void AddExp (int exp) {
        var currentExp = m_CurrentExp.Value;
        var currentLevel = m_CurrentLevel.Value - 1;

        if (currentLevel == m_ParamSet.BattleRealPlayerExpParamSets.Length) {
            // スコア増加(レベルMAXの時)
            AddScore(exp * 1.0f);
        } else {
            // Exp増加(レベルMaxではない時)
            currentExp += exp;
            var expParamSet = m_ParamSet.BattleRealPlayerExpParamSets[currentLevel];

            if (currentExp >= expParamSet.NextLevelNecessaryExp) {
                m_CurrentLevel.Value++;
                currentExp %= expParamSet.NextLevelNecessaryExp;
            }
            
            m_CurrentExp.Value = currentExp;
        }
    }

    /// <summary>
    /// ボムチャージを加算する。
    /// </summary>
    public void AddBombCharge (float charge) {
        // var currentCharge = m_CurrentBombCharge.Value;
        // currentCharge += charge;

        // if (currentCharge >= m_PlayerState.BombCharge) {
        //     m_CurrentBombNum.Value++;
        //     currentCharge %= m_PlayerState.BombCharge;
        // }
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