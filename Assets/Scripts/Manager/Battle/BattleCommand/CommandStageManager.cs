using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStageManager : SingletonMonoBehavior<CommandStageManager>
{
    #region Inspector

    [Header("Holder")]

    /// <summary>
    /// 動物体を保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_MoveObjectHolder;

    /// <summary>
    /// 静止物体を保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_FixedObjectHolder;

    /// <summary>
    /// ステージ上の背景オブジェクトを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_StageObjectHolder;

    /// <summary>
    /// プレイヤーキャラを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_PlayerCharaHolder;

    /// <summary>
    /// 敵キャラを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_EnemyCharaHolder;

    /// <summary>
    /// 弾を保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_BulletHolder;

    /// <summary>
    /// 壁を保持するためのホルダー
    /// </summary>
    [SerializeField]
    private GameObject m_WallHolder;

    [Header("Filed")]

    [SerializeField]
    private Vector2 m_MinLocalFieldPosition;

    [SerializeField]
    private Vector2 m_MaxLocalFieldPosition;

    #endregion



    #region Get

    public GameObject GetMoveObjectHolder()
    {
        return m_MoveObjectHolder;
    }

    public GameObject GetFixedObjectHolder()
    {
        return m_FixedObjectHolder;
    }

    public GameObject GetStageObjectHolder()
    {
        return m_StageObjectHolder;
    }

    public GameObject GetPlayerCharaHolder()
    {
        return m_PlayerCharaHolder;
    }

    public GameObject GetEnemyCharaHolder()
    {
        return m_EnemyCharaHolder;
    }

    public GameObject GetBulletHolder()
    {
        return m_BulletHolder;
    }

    public GameObject GetWallHolder()
    {
        return m_WallHolder;
    }

    public Vector2 GetMinLocalPositionField()
    {
        return m_MinLocalFieldPosition;
    }

    public Vector2 GetMaxLocalPositionField()
    {
        return m_MaxLocalFieldPosition;
    }

    #endregion



    /// <summary>
    /// MoveObjectHolderのピボットを基準にした座標に変換する。
    /// </summary>
    public Vector3 GetMoveObjectHolderBasePosition(Vector3 position)
    {
        return position - m_MoveObjectHolder.transform.position;
    }

    /// <summary>
    /// MoveObjectHolderのピボットを基準にした回転に変換する。
    /// </summary>
    public Vector3 GetMoveObjectHolderBaseEulerAngles(Vector3 eulerAngles)
    {
        return eulerAngles - m_MoveObjectHolder.transform.eulerAngles;
    }

    /// <summary>
    /// 指定したオブジェクトの座標が、動体オブジェクトホルダーのフィールド領域の外にあるかどうかを判定する。
    /// </summary>
    public bool IsOutOfFieldMovingObjectPosition(Transform movingObj)
    {
        var localPos = movingObj.position - m_MoveObjectHolder.transform.position;
        return
            localPos.x < m_MinLocalFieldPosition.x ||
            localPos.x > m_MaxLocalFieldPosition.x ||
            localPos.z < m_MinLocalFieldPosition.y ||
            localPos.z > m_MaxLocalFieldPosition.y;
    }

    /// <summary>
    /// 指定したオブジェクトの座標を動体オブジェクトホルダーのフィールド領域に入れ込む。
    /// </summary>
    public void ClampMovingObjectPosition(Transform movingObj)
    {
        if (!IsOutOfFieldMovingObjectPosition(movingObj))
        {
            return;
        }

        var pos = movingObj.position - m_MoveObjectHolder.transform.position;
        pos.x = Mathf.Clamp(pos.x, m_MinLocalFieldPosition.x, m_MaxLocalFieldPosition.x);
        pos.z = Mathf.Clamp(pos.z, m_MinLocalFieldPosition.y, m_MaxLocalFieldPosition.y);
        movingObj.localPosition = pos;
    }
}
