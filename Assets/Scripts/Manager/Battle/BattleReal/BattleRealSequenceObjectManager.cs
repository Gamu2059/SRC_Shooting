using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SequenceControllerで制御されるオブジェクトに対してループ処理を与えるマネージャ。
/// </summary>
public class BattleRealSequenceObjectManager : Singleton<BattleRealSequenceObjectManager>
{
    #region Field

    private Transform m_ObjectHolder;

    /// <summary>
    /// STANDBY_UPDATE状態のオブジェクトのリスト。
    /// </summary>
    private LinkedList<BattleRealSequenceObjectController> m_StandbyUpdateObjects;

    /// <summary>
    /// UPDATE状態のオブジェクトのリスト。
    /// </summary>
    private LinkedList<BattleRealSequenceObjectController> m_UpdateObjects;

    /// <summary>
    /// STANDBY_DESTROY状態のオブジェクトのリスト。
    /// </summary>
    private LinkedList<BattleRealSequenceObjectController> m_StandbyDestroyObjects;

    #endregion

    public static BattleRealSequenceObjectManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyUpdateObjects = new LinkedList<BattleRealSequenceObjectController>();
        m_UpdateObjects = new LinkedList<BattleRealSequenceObjectController>();
        m_StandbyDestroyObjects = new LinkedList<BattleRealSequenceObjectController>();
    }

    public override void OnFinalize()
    {
        DestoryAllObjects();
        m_StandbyDestroyObjects = null;
        m_UpdateObjects = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        var stageManager = BattleRealStageManager.Instance;
        m_ObjectHolder = stageManager.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.SEQUENCE_OBJECT);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Start処理
        foreach (var obj in m_StandbyUpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }
            else if (obj.Cycle != E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckStandbyDestroy(obj);
                continue;
            }

            obj.OnStart();
        }

        GotoUpdateFromStandbyUpdate();

        // Update処理
        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }
            else if (obj.Cycle != E_OBJECT_CYCLE.UPDATE)
            {
                CheckStandbyDestroy(obj);
                continue;
            }

            obj.OnUpdate();
        }
    }


    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        // LateUpdate処理
        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }
            else if (obj.Cycle != E_OBJECT_CYCLE.UPDATE)
            {
                CheckStandbyDestroy(obj);
                continue;
            }

            obj.OnLateUpdate();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // FixedUpdate処理
        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }
            else if (obj.Cycle != E_OBJECT_CYCLE.UPDATE)
            {
                CheckStandbyDestroy(obj);
                continue;
            }

            obj.OnFixedUpdate();
        }
    }

    #endregion

    /// <summary>
    /// 破棄フラグが立っているものを破棄する。
    /// </summary>
    public void GotoDestroy()
    {
        DestroyObjects();
    }

    /// <summary>
    /// STANDBY_UPDATEからUPDATEに遷移させる
    /// </summary>
    private void GotoUpdateFromStandbyUpdate()
    {
        foreach (var obj in m_StandbyUpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }
            else if (obj.Cycle != E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckStandbyDestroy(obj);
                continue;
            }

            obj.Cycle = E_OBJECT_CYCLE.UPDATE;
            m_UpdateObjects.AddLast(obj);
        }

        m_StandbyUpdateObjects.Clear();
    }

    /// <summary>
    /// StandbyDestroyObjectsリストに登録されているものを全て破棄する。
    /// </summary>
    private void DestroyObjects()
    {
        foreach (var obj in m_StandbyDestroyObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            obj.Cycle = E_OBJECT_CYCLE.DESTROYED;
            m_StandbyUpdateObjects.Remove(obj);
            m_UpdateObjects.Remove(obj);
            GameObject.Destroy(obj.gameObject);
        }

        m_StandbyDestroyObjects.Clear();
    }

    /// <summary>
    /// STANDBY_UPDATE状態にする。
    /// </summary>
    /// <param name="obj"></param>
    public void CheckStandbyUpdate(BattleRealSequenceObjectController obj)
    {
        if (obj == null || m_StandbyUpdateObjects.Contains(obj))
        {
            return;
        }

        m_StandbyUpdateObjects.AddLast(obj);
        obj.Cycle = E_OBJECT_CYCLE.STANDBY_UPDATE;
        obj.OnInitialize();
    }

    /// <summary>
    /// STANDBY_DESTROYED状態にする。
    /// </summary>
    public void CheckStandbyDestroy(BattleRealSequenceObjectController obj)
    {
        if (obj == null || m_StandbyDestroyObjects.Contains(obj))
        {
            return;
        }

        obj.Cycle = E_OBJECT_CYCLE.STANDBY_DESTROYED;
        m_StandbyDestroyObjects.AddLast(obj);
    }

    /// <summary>
    /// プレハブを渡して複製物を生成する。
    /// </summary>
    /// <param name="sequenceObjectPrefab">シーケンス制御したいオブジェクトのプレハブ</param>
    /// <param name="rootGroup">オブジェクトに対しての制御内容</param>
    public BattleRealSequenceObjectController CreateSequenceObject(BattleRealSequenceObjectController sequenceObjectPrefab, SequenceGroup rootGroup)
    {
        if (sequenceObjectPrefab == null || rootGroup == null)
        {
            return null;
        }

        var obj = GameObject.Instantiate(sequenceObjectPrefab);
        obj.transform.SetParent(m_ObjectHolder);

        CheckStandbyUpdate(obj);
        obj.BuildSequence(rootGroup);

        return obj;
    }

    /// <summary>
    /// 全てのオブジェクトを破棄する。
    /// </summary>
    public void DestoryAllObjects()
    {
        foreach (var obj in m_StandbyUpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            GameObject.Destroy(obj.gameObject);
        }

        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            GameObject.Destroy(obj.gameObject);
        }

        foreach (var obj in m_StandbyDestroyObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            GameObject.Destroy(obj.gameObject);
        }

        m_StandbyUpdateObjects.Clear();
        m_UpdateObjects.Clear();
        m_StandbyDestroyObjects.Clear();
    }
}
