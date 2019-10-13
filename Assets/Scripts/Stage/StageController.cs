using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ全体を制御するためのコントローラ。
/// </summary>
public class StageController : BattleControllableMonoBehavior
{
    #region Field Inspector

    [Header("Objects Holder")]

    [SerializeField, Tooltip("バトルメイン全体のオブジェクトのホルダー")]
    protected GameObject m_ObjectsHolder;

    [Header("敵出現パラメータ")]

    [SerializeField, Tooltip("このステージで登場する敵データ")]
    protected StageEnemyParam m_StageEnemyParam;

    [SerializeField, Tooltip("このステージで使用する敵出現データ")]
    protected XL_StageEnemyParam m_XlStageEnemyParam;

    #endregion

    #region Field

    /// <summary>
    /// 指定した敵データを基に作成した、実際に使用する出現データ
    /// </summary>
    protected List<XL_StageEnemyParam.Param> m_StageEnemyAppearData;

    /// <summary>
    /// 削除する敵データ
    /// </summary>
    protected List<XL_StageEnemyParam.Param> m_RemovingData;

    /// <summary>
    /// 敵出現の参考にする経過時間
    /// </summary>
    protected float m_BuildEnemyTimeCount;

    #endregion



    /// <summary>
    /// BattleMainが有効になった時に呼び出される。
    /// </summary>
    public override void OnEnableObject()
    {
        base.OnEnableObject();
        m_ObjectsHolder.SetActive(true);
    }

    /// <summary>
    /// BattleMainが無効になった時に呼び出される。
    /// </summary>
    public override void OnDisableObject()
    {
        base.OnDisableObject();
        m_ObjectsHolder.SetActive(false);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StageEnemyAppearData = new List<XL_StageEnemyParam.Param>();
        m_RemovingData = new List<XL_StageEnemyParam.Param>();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_BuildEnemyTimeCount = 0;

        BuildEnemyAppearData();
        BattleMainAudioManager.Instance.PlayBGM(BattleMainAudioManagerKeyWord.Stage1);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        ControlViewMoving();
        AppearEnemy();
    }

    /// <summary>
    /// ステージの見た目を制御する。
    /// </summary>
    protected virtual void ControlViewMoving()
    {
    }

    /// <summary>
    /// 敵出現を制御する。
    /// 時間経過による出現イベントしか制御していない。
    /// </summary>
    protected virtual void AppearEnemy()
    {
        foreach (var data in m_StageEnemyAppearData)
        {
            //if (data.Time >= m_BuildEnemyTimeCount)
            //{
            //    continue;
            //}

            //var enemy = EnemyCharaManager.Instance.CreateEnemy(m_StageEnemyParam.GetEnemyControllers()[data.EnemyMoveId], data.OtherParameters);

            //if (enemy == null)
            //{
            //    continue;
            //}

            //enemy.SetBulletSetParam(m_StageEnemyParam.GetBulletSets()[data.BulletSetId]);

            //var pos = EnemyCharaManager.Instance.GetPositionFromFieldViewPortPosition(data.AppearViewportX, data.AppearViewportY);
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

    /// <summary>
    /// 敵出現データを実際に作成する。
    /// </summary>
    protected void BuildEnemyAppearData()
    {
        if (m_XlStageEnemyParam == null)
        {
            return;
        }

        //m_StageEnemyAppearData.Clear();        
        //m_StageEnemyAppearData.AddRange(m_XlStageEnemyParam.param.FindAll(i => i.Time >= 0f));
    }

    /// <summary>
    /// 削除予定の敵出現データを削除する。
    /// </summary>
    protected void RemoveEnemyAppearData()
    {
        foreach (var data in m_RemovingData)
        {
            m_StageEnemyAppearData.Remove(data);
        }

        m_RemovingData.Clear();
    }
}
