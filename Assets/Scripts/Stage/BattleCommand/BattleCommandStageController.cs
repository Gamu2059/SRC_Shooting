using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// コマンドイベントのステージ移動を制御するコンポーネント。
/// </summary>
public class BattleCommandStageController : BattleControllableMonoBehavior
{
    [Header("Objects Holder")]

    [SerializeField]
    private GameObject m_ObjectsHolder;

    [Header("敵出現パラメータ")]

    [SerializeField]
    private CommandStageEnemyParam m_StageEnemyParam;

    [SerializeField]
    private XL_StageEnemyParam m_StageEnemyList;

    [SerializeField]
    private int m_ReferenceEnemyListIndex;

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

    [SerializeField]
    private float m_BuildEnemyTimeCount;

    private List<XL_StageEnemyParam.Param> m_StageEnemyAppearData;
    private List<XL_StageEnemyParam.Param> m_RemovingData;

    public float GetMoveSpeed()
    {
        return m_MoveSpeed;
    }
    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StageEnemyAppearData = new List<XL_StageEnemyParam.Param>();
        m_RemovingData = new List<XL_StageEnemyParam.Param>();
    }

    /// <summary>
    /// BattleCommandが有効になった時に呼び出される。
    /// </summary>
    public override void OnEnableObject()
    {
        base.OnEnableObject();

        m_BuildEnemyTimeCount = 0;
        BuildEnemyAppearData();
        m_ObjectsHolder.SetActive(true);
    }

    /// <summary>
    /// BattleCommandが無効になった時に呼び出される。
    /// </summary>
    public override void OnDisableObject()
    {
        base.OnDisableObject();
        m_ObjectsHolder.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        ControlViewMoving();
        AppearWall();
        AppearEnemy();
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

    private void AppearEnemy()
    {
        foreach (var data in m_StageEnemyAppearData)
        {
            //if (data.Time >= m_BuildEnemyTimeCount)
            //{
            //    continue;
            //}

            //var enemy = CommandEnemyCharaManager.Instance.CreateEnemy(m_StageEnemyParam.GetEnemyControllers()[data.EnemyMoveId], data.OtherParameters);

            //if (enemy == null)
            //{
            //    continue;
            //}

            //enemy.SetBulletSetParam(m_StageEnemyParam.GetBulletSets()[data.BulletSetId]);

            //var pos = CommandEnemyCharaManager.Instance.GetPositionFromFieldViewPortPosition(data.AppearViewportX, data.AppearViewportY);
            //pos.x += data.AppearOffsetX;
            //pos.y += data.AppearOffsetY;
            //pos.z += data.AppearOffsetZ;
            //enemy.transform.position = pos;

            //var rot = enemy.transform.eulerAngles;
            //rot.y = data.AppearRotateY;
            //enemy.transform.eulerAngles = rot;

            //m_RemovingData.Add(data);
        }

        RemoveEnemyAppearData();

        m_BuildEnemyTimeCount += Time.deltaTime;
    }
    private void BuildEnemyAppearData()
    {
        if (m_StageEnemyList == null)
        {
            return;
        }

        //m_StageEnemyAppearData.Clear();
        //m_StageEnemyAppearData.AddRange(m_StageEnemyList.param.FindAll(i => i.Time > 0f));
    }

    private void RemoveEnemyAppearData()
    {
        foreach (var data in m_RemovingData)
        {
            m_StageEnemyAppearData.Remove(data);
        }

        m_RemovingData.Clear();
    }
}
