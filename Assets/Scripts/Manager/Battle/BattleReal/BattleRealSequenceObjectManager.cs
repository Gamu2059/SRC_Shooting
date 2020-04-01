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
    /// 更新を掛けるオブジェクトのリスト。
    /// </summary>
    private LinkedList<SequenceController> m_UpdateObjects;

    /// <summary>
    /// 破棄するオブジェクトのリスト。
    /// </summary>
    private LinkedList<SequenceController> m_DestroyObjects;

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

        m_UpdateObjects = new LinkedList<SequenceController>();
        m_DestroyObjects = new LinkedList<SequenceController>();
    }

    public override void OnFinalize()
    {
        DestoryAllObjects();
        m_DestroyObjects = null;
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

        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnUpdate();
        }
    }


    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnLateUpdate();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        foreach (var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
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
    /// DestroyObjectsリストに登録されているものを全て破棄する。
    /// </summary>
    private void DestroyObjects()
    {
        foreach (var obj in m_DestroyObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            m_UpdateObjects.Remove(obj);
            GameObject.Destroy(obj.gameObject);
        }

        m_DestroyObjects.Clear();
    }

    /// <summary>
    /// DestroyObjectsリストに登録する。
    /// </summary>
    private void CheckDestroy(SequenceController obj)
    {
        if (obj == null || m_DestroyObjects.Contains(obj))
        {
            return;
        }

        m_DestroyObjects.AddLast(obj);
    }

    /// <summary>
    /// プレハブを渡して複製物を生成する。
    /// </summary>
    /// <param name="sequenceObjectPrefab">シーケンス制御したいオブジェクトのプレハブ</param>
    /// <param name="rootGroup">オブジェクトに対しての制御内容</param>
    public SequenceController CreateSequenceObject(SequenceController sequenceObjectPrefab, SequenceGroup rootGroup)
    {
        if (sequenceObjectPrefab == null || rootGroup == null)
        {
            return null;
        }

        var obj = GameObject.Instantiate(sequenceObjectPrefab);
        obj.OnInitialize();
        obj.BuildSequence(rootGroup);
        obj.OnEndSequence += () => CheckDestroy(obj);
        m_UpdateObjects.AddLast(obj);

        return obj;
    }

    /// <summary>
    /// 全てのオブジェクトを破棄する。
    /// </summary>
    public void DestoryAllObjects()
    {
        foreach(var obj in m_UpdateObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            GameObject.Destroy(obj.gameObject);
        }

        foreach (var obj in m_DestroyObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.OnFinalize();
            GameObject.Destroy(obj.gameObject);
        }

        m_UpdateObjects.Clear();
        m_DestroyObjects.Clear();
    }
}
