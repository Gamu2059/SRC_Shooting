using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーキャラの動作を制御するマネージャ。
/// とりあえずで作ってます。
/// </summary>
public class PlayerCharaManager : SingletonMonoBehavior<PlayerCharaManager>
{

    #region Inspector

    [Header("Holder")]

    [SerializeField]
    private Transform m_PlayerCharaHolder;

    [Header("Chara")]

    [SerializeField]
    private PlayerController[] m_CharaPrefabs;

    [SerializeField]
    private Vector2 m_InitAppearViewportPosition;

    [SerializeField]
    private List<PlayerController> m_Controllers;

    [SerializeField]
    private PlayerController m_CurrentController;

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
            var obj = new GameObject("[PlayerCharaHolder]");
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

    public void RegistChara(PlayerController controller)
    {
        if (controller == null || m_Controllers.Contains(controller))
        {
            return;
        }

        controller.transform.SetParent(m_PlayerCharaHolder);
        m_Controllers.Add(controller);
        controller.OnInitialize();

        // 最初のキャラだけONにする
        if (m_Controllers.Count > 1)
        {
            controller.gameObject.SetActive(false);
        }
    }

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

        m_CurrentController.ShotBullet();
    }

    /// <summary>
    /// ボムを撃つコールバック。
    /// </summary>
    public void OnInputBomb(InputManager.E_INPUT_STATE state)
    {
        if (m_CurrentController == null) return;

        m_CurrentController.ShotBomb();
    }
}
