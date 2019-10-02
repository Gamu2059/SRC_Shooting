using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイムライン制御するオブジェクトのマネージャ。
/// </summary>
public class PlayableManager : BattleSingletonMonoBehavior<PlayableManager>
{
    #region Field

    private Transform m_Holder;

    /// <summary>
    /// UPDATE状態のオブジェクトを保持するリスト。
    /// </summary>
    private List<BattleMainPlayableBase> m_UpdateObjects;

    /// <summary>
    /// 破棄状態に遷移するオブジェクトのリスト。
    /// </summary>
    private List<BattleMainPlayableBase> m_GotoDestroyObjects;

    #endregion

    #region Get Set

    public List<BattleMainPlayableBase> GetUpdateObjects()
    {
        return m_UpdateObjects;
    }

    #endregion



    protected override void OnAwake()
    {
        base.OnAwake();

        m_UpdateObjects = new List<BattleMainPlayableBase>();
        m_GotoDestroyObjects = new List<BattleMainPlayableBase>();
    }

    public override void OnFinalize()
    {
        DestroyAllObjectImmediate();

        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (StageManager.Instance != null && StageManager.Instance.GetMoveObjectHolder() != null)
        {
            m_Holder = StageManager.Instance.GetMoveObjectHolder().transform;
        }
    }

    public override void OnUpdate()
    {
        // Update処理
        foreach (var playable in m_UpdateObjects)
        {
            if (playable == null)
            {
                m_GotoDestroyObjects.Add(playable);
                continue;
            }

            if (playable.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                playable.OnStart();
                playable.SetCycle(E_OBJECT_CYCLE.UPDATE);
            }

            playable.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var playable in m_UpdateObjects)
        {
            if (playable == null)
            {
                m_GotoDestroyObjects.Add(playable);
                continue;
            }

            playable.OnLateUpdate();
        }

        GotoDestroyFromUpdate();
    }

    private void GotoDestroyFromUpdate()
    {
        int count = m_GotoDestroyObjects.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var playable = m_GotoDestroyObjects[idx];

            if (playable == null)
            {
                continue;
            }

            m_GotoDestroyObjects.RemoveAt(idx);
            m_UpdateObjects.Remove(playable);
            playable.SetCycle(E_OBJECT_CYCLE.DESTROYED);
            playable.OnFinalize();
            Destroy(playable.gameObject);
        }

        m_GotoDestroyObjects.Clear();

        m_UpdateObjects.RemoveAll((e) => e == null);
    }

    /// <summary>
    /// オブジェクトを登録する。
    /// </summary>
    public BattleMainPlayableBase RegistObject(BattleMainPlayableBase playable)
    {
        if (playable == null || m_UpdateObjects.Contains(playable) || m_GotoDestroyObjects.Contains(playable))
        {
            return null;
        }

        playable.transform.SetParent(m_Holder);
        m_UpdateObjects.Add(playable);
        playable.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
        playable.OnInitialize();
        return playable;
    }

    /// <summary>
    /// オブジェクトを破棄する。
    /// </summary>
    public void DestroyObject(BattleMainPlayableBase playable)
    {
        if (playable == null || !m_UpdateObjects.Contains(playable))
        {
            return;
        }

        m_GotoDestroyObjects.Add(playable);
        playable.SetCycle(E_OBJECT_CYCLE.STANDBY_DESTROYED);
    }

    /// <summary>
    /// 全てのオブジェクトを破棄する。
    /// </summary>
    public void DestroyAllObject()
    {
        foreach(var playable in m_UpdateObjects)
        {
            DestroyObject(playable);
        }

        m_UpdateObjects.Clear();
    }

    /// <summary>
    /// オブジェクトを破棄する。
    /// これを呼び出したタイミングで即座に削除される。
    /// </summary>
    public void DestroyObjectImmediate(BattleMainPlayableBase playable)
    {
        if (playable == null)
        {
            return;
        }

        playable.OnFinalize();
        Destroy(playable.gameObject);
    }

    /// <summary>
    /// 全てのオブジェクトを破棄する。
    /// これを呼び出したタイミングで即座に削除される。
    /// </summary>
    public void DestroyAllObjectImmediate()
    {
        foreach(var playable in m_UpdateObjects)
        {
            DestroyObjectImmediate(playable);
        }

        m_UpdateObjects.Clear();
    }
}
