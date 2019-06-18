using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayerCharaManager : BattleSingletonMonoBehavior<CommandPlayerCharaManager>
{
    public const string HOLDER_NAME = "[CommandPlayerCharaHolder]";

    #region Inspector

    [Header("Holder")]

    [SerializeField]
    private Transform m_PlayerCharaHolder;

    [Header("Chara")]

    [SerializeField]
    private CommandPlayerController m_CharaPrefab;

    [SerializeField]
    private Vector2 m_InitAppearViewportPosition;

    [SerializeField]
    private CommandPlayerController m_Controller;

    #endregion



    private Vector3 m_CharaMoveDir;



    #region Get Set

    public CommandPlayerController GetController()
    {
        return m_Controller;
    }

    #endregion



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

        if (CommandStageManager.Instance != null && CommandStageManager.Instance.GetPlayerCharaHolder() != null)
        {
            m_PlayerCharaHolder = CommandStageManager.Instance.GetPlayerCharaHolder().transform;
        }
        else if (m_PlayerCharaHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_PlayerCharaHolder = obj.transform;
        }

        // プレイヤーキャラは汎用的に用いるため、Startで作成する
        var chara = Instantiate(m_CharaPrefab);
        RegistChara(chara);
    }

    /// <summary>
    /// コマンドイベントが有効になった時に呼び出される。
    /// </summary>
    public override void OnEnableObject()
    {
        base.OnEnableObject();

        var pos = GetInitAppearPosition();
        var chara = GetController();

        if (chara != null) {
            chara.transform.position = pos;
            chara.OnStart();
        }
    }

    /// <summary>
    /// コマンドイベントが無効になった時に呼び出される。
    /// </summary>
    public override void OnDisableObject()
    {
        base.OnDisableObject();
        var chara = GetController();

        if (chara != null)
        {
            chara.OnFinalize();
        }
    }

    public override void OnUpdate()
    {
        if (m_Controller == null)
        {
            return;
        }

        // この移動処理はInputManagerがBaseSceneManagerよりも前にコールバックを返してくれるから実現できている
        // 二つのマネージャの実行順序を逆にするとキャラが動かなくなる
        m_Controller.Move(m_CharaMoveDir);
        m_Controller.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        if (m_Controller == null)
        {
            return;
        }

        RestrictCharaPosition();
        m_Controller.OnLateUpdate();
        m_CharaMoveDir = Vector3.zero;
    }

    public void RegistChara(CommandPlayerController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.transform.SetParent(m_PlayerCharaHolder);
        controller.OnInitialize();
        controller.gameObject.SetActive(true);
        m_Controller = controller;
    }

    public void RestrictCharaPosition()
    {
        var chara = GetController();

        if (chara == null)
        {
            return;
        }

        CommandStageManager.Instance.ClampMovingObjectPosition(chara.transform);
    }

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の初期出現座標を取得する。
    /// </summary>
    public Vector3 GetInitAppearPosition()
    {
        var minPos = CommandStageManager.Instance.GetMinLocalPositionField();
        var maxPos = CommandStageManager.Instance.GetMaxLocalPositionField();

        var factX = (maxPos.x - minPos.x) * m_InitAppearViewportPosition.x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * m_InitAppearViewportPosition.y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);
        pos += CommandStageManager.Instance.GetMoveObjectHolder().transform.position;

        return pos;
    }

    /// <summary>
    /// 横軸入力のコールバック。
    /// </summary>
    public void OnInputHorizontal(float value)
    {
        if (m_Controller == null) return;

        m_CharaMoveDir += Vector3.right * value;
    }

    /// <summary>
    /// 縦軸入力のコールバック。
    /// </summary>
    public void OnInputVertical(float value)
    {
        if (m_Controller == null) return;

        m_CharaMoveDir += Vector3.forward * value;
    }

    /// <summary>
    /// 弾を撃つコールバック。
    /// </summary>
    public void OnInputShot(InputManager.E_INPUT_STATE state)
    {
        if (m_Controller == null) return;

        m_Controller.ShotBullet();
    }
}
