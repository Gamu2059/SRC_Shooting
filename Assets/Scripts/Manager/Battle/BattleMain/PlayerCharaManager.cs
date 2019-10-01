using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// プレイヤーキャラの動作を制御するマネージャ。
/// </summary>
public class PlayerCharaManager : BattleSingletonMonoBehavior<PlayerCharaManager>
{
    public const string HOLDER_NAME = "[PlayerCharaHolder]";

    #region Inspector

    [Header("Holder")]

    [SerializeField]
    private Transform m_PlayerCharaHolder;

    [Header("Chara")]

    [SerializeField]
    private PlayerController[] m_CharaPrefabs;

    [SerializeField]
    private int m_InitSortieIndex;

    [SerializeField]
    private Vector2 m_InitAppearViewportPosition;

    [SerializeField]
    private List<PlayerController> m_Controllers;

    [SerializeField]
    private PlayerController m_CurrentController;

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



    // 現在出撃中のキャラのインデックス
    private int m_CharaIndex = 0;

    private float m_WaitChangeTime = 0;

    private Vector3 m_CharaMoveDir;


    #region Get Set

    public List<PlayerController> GetControllers()
    {
        return m_Controllers;
    }

    public PlayerController GetCurrentController()
    {
        return m_CurrentController;
    }

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



    protected override void OnAwake()
    {
        base.OnAwake();
        m_Controllers = new List<PlayerController>();
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_Controllers.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (StageManager.Instance != null && StageManager.Instance.GetPlayerCharaHolder() != null)
        {
            m_PlayerCharaHolder = StageManager.Instance.GetPlayerCharaHolder().transform;
        }
        else if (m_PlayerCharaHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_PlayerCharaHolder = obj.transform;
        }

        var pos = GetInitAppearPosition();
        foreach (var charaPrefab in m_CharaPrefabs)
        {
            var chara = Instantiate(charaPrefab);
            RegistChara(chara);
            chara.transform.position = pos;
        }

        ChangeChara(m_InitSortieIndex);

        InitPlayerState();
    }

    public override void OnUpdate()
    {
        if (m_CurrentController == null)
        {
            ChangeChara(0);
            return;
        }

        if (m_WaitChangeTime > 0f)
        {
            m_WaitChangeTime -= Time.deltaTime;
        }

        // この移動処理はInputManagerがBaseSceneManagerよりも前にコールバックを返してくれるから実現できている
        // 二つのマネージャの実行順序を逆にするとキャラが動かなくなる
        m_CurrentController.Move(m_CharaMoveDir);
        m_CurrentController.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        if (m_CurrentController == null)
        {
            return;
        }

        RestrictCharaPosition();
        m_CurrentController.OnLateUpdate();
        m_CharaMoveDir = Vector3.zero;
    }

    /// <summary>
    /// 指定したインデックスのキャラに交代する。
    /// </summary>
    private void ChangeChara(int index)
    {
        if (m_CharaIndex == index && m_CurrentController != null || m_WaitChangeTime > 0f)
        {
            return;
        }

        if (index < 0 || index >= m_Controllers.Count)
        {
            return;
        }

        m_CharaIndex = index;
        var nextController = m_Controllers[m_CharaIndex];

        if (m_CurrentController != null)
        {
            m_CurrentController.gameObject.SetActive(false);
            nextController.transform.localPosition = m_CurrentController.transform.localPosition;
        }

        m_CurrentController = nextController;
        m_CurrentController.gameObject.SetActive(true);
        m_WaitChangeTime = 1f;
    }

    /// <summary>
    /// プレイヤーキャラを登録する。
    /// 後にprivateに変更する。
    /// </summary>
    public void RegistChara(PlayerController controller)
    {
        if (controller == null || m_Controllers.Contains(controller))
        {
            return;
        }

        controller.transform.SetParent(m_PlayerCharaHolder);
        m_Controllers.Add(controller);
        controller.OnInitialize();
        controller.gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラの座標を動体フィールド領域に制限する。
    /// </summary>
    public void RestrictCharaPosition()
    {
        var chara = GetCurrentController();

        if (chara == null)
        {
            return;
        }

        StageManager.Instance.ClampMovingObjectPosition(chara.transform);
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    public Vector3 GetInitAppearPosition()
    {
        var minPos = StageManager.Instance.GetMinLocalPositionField();
        var maxPos = StageManager.Instance.GetMaxLocalPositionField();

        var factX = (maxPos.x - minPos.x) * m_InitAppearViewportPosition.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * m_InitAppearViewportPosition.y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);
        pos += StageManager.Instance.GetMoveObjectHolder().transform.position;

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

    /// <summary>
    /// 横軸入力のコールバック。
    /// </summary>
    public void OnInputHorizontal(float value)
    {
        if (m_CurrentController == null) return;

        m_CharaMoveDir += Vector3.right * value;
    }

    /// <summary>
    /// 縦軸入力のコールバック。
    /// </summary>
    public void OnInputVertical(float value)
    {
        if (m_CurrentController == null) return;

        m_CharaMoveDir += Vector3.forward * value;
    }

    /// <summary>
    /// キャラ切替のコールバック。
    /// </summary>
    public void OnInputChangeChara(float value)
    {
        if (value == 0)
        {
            return;
        }

        int charaNum = m_Controllers.Count;
        if (value > 0)
        {
            ChangeChara((m_CharaIndex + 1 + charaNum) % charaNum);
        }
        else if (value < 0)
        {
            ChangeChara((m_CharaIndex - 1 + charaNum) % charaNum);
        }
    }

    /// <summary>
    /// 弾を撃つコールバック。
    /// </summary>
    public void OnInputShot(InputManager.E_INPUT_STATE state)
    {
        if (m_CurrentController == null) return;

        m_CurrentController.ShotBullet(state);
    }

    /// <summary>
    /// ボムを撃つコールバック。
    /// </summary>
    public void OnInputBomb(InputManager.E_INPUT_STATE state)
    {
        if (m_CurrentController == null) return;

        if (state != InputManager.E_INPUT_STATE.DOWN) return;

        if (m_CurrentBombNum.Value < 1) return;

        m_CurrentBombNum.Value--;
        m_CurrentController.ShotBomb(state);
    }
}
