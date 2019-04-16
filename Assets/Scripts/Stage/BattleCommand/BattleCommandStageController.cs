using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントのステージ移動を制御するコンポーネント。
/// </summary>
public class BattleCommandStageController : BattleControllableMonoBehavior
{
    /// <summary>
    /// 移動速度。
    /// </summary>
    [SerializeField]
    private float m_MoveSpeed;

    /// <summary>
    /// 初期地点に戻るタイミングのz座標。
    /// </summary>
    [SerializeField]
    private float m_ResetPositionPoint;

    /// <summary>
    /// 壁のリスト。
    /// </summary>
    [SerializeField]
    private CommandWallController[] m_WallControllers;

    /// <summary>
    /// 壁の出現間隔。
    /// </summary>
    [SerializeField]
    private float m_WallAppearInterval;

    public float GetMoveSpeed()
    {
        return m_MoveSpeed;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        ControlViewMoving();
        AppearWall();
    }

    private void ControlViewMoving()
    {
        var moveRoot = CommandStageManager.Instance.GetMoveObjectHolder();
        moveRoot.transform.Translate(Vector3.forward * m_MoveSpeed * Time.deltaTime);

        var pos = moveRoot.transform.position;
        if (pos.z >= m_ResetPositionPoint)
        {
            pos.z = 0;
            moveRoot.transform.position = pos;
        }
    }

    private void AppearWall()
    {

    }
}
