using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁を管理する。
/// </summary>
public class CommandWallManager : SingletonMonoBehavior<CommandWallManager>
{
    public const string HOLDER_NAME = "[CommandWallHolder]";

    #region Field

    private Transform m_WallHolder;

    /// <summary>
    /// STANDBY状態の壁を保持するリスト。
    /// </summary>
    private List<CommandWallController> m_StandbyWalls;

    /// <summary>
    /// UPDATE状態の壁を保持するリスト。
    /// </summary>
    private List<CommandWallController> m_UpdateWalls;

    /// <summary>
    /// POOL状態の壁を保持するリスト。
    /// </summary>
    private List<CommandWallController> m_PoolWalls;

    /// <summary>
    /// UPDATE状態に遷移する壁のリスト。
    /// </summary>
    private List<CommandWallController> m_GotoUpdateWalls;

    /// <summary>
    /// POOL状態に遷移する壁のリスト。
    /// </summary>
    private List<CommandWallController> m_GotoPoolWalls;

    #endregion

    #region Get

    /// <summary>
    /// STANDBY状態の壁を保持するリストを取得する。
    /// </summary>
    public List<CommandWallController> GetStandbyWalls()
    {
        return m_StandbyWalls;
    }

    /// <summary>
    /// UPDATE状態の壁を保持するリストを取得する。
    /// </summary>
    public List<CommandWallController> GetUpdateWalls()
    {
        return m_UpdateWalls;
    }

    /// <summary>
    /// POOL状態の壁を保持するリストを取得する。
    /// </summary>
    public List<CommandWallController> GetPoolWalls()
    {
        return m_PoolWalls;
    }

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        m_StandbyWalls = new List<CommandWallController>();
        m_UpdateWalls = new List<CommandWallController>();
        m_PoolWalls = new List<CommandWallController>();
        m_GotoUpdateWalls = new List<CommandWallController>();
        m_GotoPoolWalls = new List<CommandWallController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_StandbyWalls.Clear();
        m_UpdateWalls.Clear();
        m_PoolWalls.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (CommandStageManager.Instance != null && CommandStageManager.Instance.GetWallHolder() != null)
        {
            m_WallHolder = CommandStageManager.Instance.GetWallHolder().transform;
        }
        else if (m_WallHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_WallHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var Wall in m_StandbyWalls)
        {
            if (Wall == null)
            {
                continue;
            }

            Wall.OnStart();
            m_GotoUpdateWalls.Add(Wall);
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var Wall in m_UpdateWalls)
        {
            if (Wall == null)
            {
                continue;
            }

            Wall.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var Wall in m_UpdateWalls)
        {
            if (Wall == null)
            {
                continue;
            }

            Wall.OnLateUpdate();
        }

        GotoPoolFromUpdate();
    }



    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        int count = m_GotoUpdateWalls.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var Wall = m_GotoUpdateWalls[idx];
            m_GotoUpdateWalls.RemoveAt(idx);
            m_StandbyWalls.Remove(Wall);
            m_UpdateWalls.Add(Wall);
            Wall.SetWallCycle(E_Wall_CYCLE.UPDATE);
        }

        m_GotoUpdateWalls.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolWalls.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var Wall = m_GotoPoolWalls[idx];
            Wall.SetWallCycle(E_Wall_CYCLE.POOLED);
            m_GotoPoolWalls.RemoveAt(idx);
            m_UpdateWalls.Remove(Wall);
            m_PoolWalls.Add(Wall);
        }

        m_GotoPoolWalls.Clear();
    }

    /// <summary>
    /// 壁をSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyWall(CommandWallController Wall)
    {
        if (Wall == null || !m_PoolWalls.Contains(Wall))
        {
            Debug.LogError("指定された壁を追加できませんでした。");
            return;
        }

        m_PoolWalls.Remove(Wall);
        m_StandbyWalls.Add(Wall);
        Wall.gameObject.SetActive(true);
        Wall.SetWallCycle(E_Wall_CYCLE.STANDBY_UPDATE);
        Wall.OnInitialize();
    }

    /// <summary>
    /// 指定した壁を制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolWall(CommandWallController Wall)
    {
        if (Wall == null || m_GotoPoolWalls.Contains(Wall))
        {
            Debug.LogError("指定した壁を削除できませんでした。");
            return;
        }

        Wall.SetWallCycle(E_Wall_CYCLE.STANDBY_POOL);
        Wall.OnFinalize();
        m_GotoPoolWalls.Add(Wall);
        Wall.gameObject.SetActive(false);
    }

    /// <summary>
    /// プールから壁を取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="WallPrefab">取得や生成の情報源となる壁のプレハブ</param>
    public CommandWallController GetPoolingWall(CommandWallController WallPrefab)
    {
        if (WallPrefab == null)
        {
            return null;
        }

        string WallId = WallPrefab.GetWallGroupId();
        CommandWallController Wall = null;

        foreach (var b in m_PoolWalls)
        {
            if (b != null && b.GetWallGroupId() == WallId)
            {
                Wall = b;
                break;
            }
        }

        if (Wall == null)
        {
            Wall = Instantiate(WallPrefab);
            Wall.transform.SetParent(m_WallHolder);
            m_PoolWalls.Add(Wall);
        }

        return Wall;
    }
}
