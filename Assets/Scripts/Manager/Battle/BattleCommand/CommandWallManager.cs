using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁を管理する。
/// </summary>
public class CommandWallManager : BattleSingletonMonoBehavior<CommandWallManager>
{
    public const string HOLDER_NAME = "[CommandWallHolder]";

    #region Field

    private Transform m_WallHolder;

    /// <summary>
    /// UPDATE状態の壁を保持するリスト。
    /// </summary>
    private List<CommandWallController> m_UpdateWalls;

    /// <summary>
    /// POOL状態の壁を保持するリスト。
    /// </summary>
    private List<CommandWallController> m_PoolWalls;

    /// <summary>
    /// POOL状態に遷移する壁のリスト。
    /// </summary>
    private List<CommandWallController> m_GotoPoolWalls;

    #endregion

    #region Get

    /// <summary>
    /// UPDATE状態の壁を保持するリストを取得する。
    /// </summary>
    public List<CommandWallController> GetUpdateWalls()
    {
        return m_UpdateWalls;
    }

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        m_UpdateWalls = new List<CommandWallController>();
        m_PoolWalls = new List<CommandWallController>();
        m_GotoPoolWalls = new List<CommandWallController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
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
        // Update処理
        foreach (var wall in m_UpdateWalls)
        {
            if (wall == null)
            {
                continue;
            }

            if (wall.GetCycle() == E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                wall.OnStart();
                wall.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            }

            wall.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var wall in m_UpdateWalls)
        {
            if (wall == null)
            {
                continue;
            }

            wall.OnLateUpdate();
        }

        GotoPoolFromUpdate();
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
            var wall = m_GotoPoolWalls[idx];
            wall.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            m_GotoPoolWalls.RemoveAt(idx);
            m_UpdateWalls.Remove(wall);
            m_PoolWalls.Add(wall);
        }

        m_GotoPoolWalls.Clear();
    }

    /// <summary>
    /// 壁をSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyWall(CommandWallController wall)
    {
        if (wall == null || !m_PoolWalls.Contains(wall))
        {
            Debug.LogError("指定された壁を追加できませんでした。");
            return;
        }

        m_PoolWalls.Remove(wall);
        m_UpdateWalls.Add(wall);
        wall.gameObject.SetActive(true);
        wall.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        wall.OnInitialize();
    }

    /// <summary>
    /// 指定した壁を制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolWall(CommandWallController wall)
    {
        if (wall == null || m_GotoPoolWalls.Contains(wall))
        {
            Debug.LogError("指定した壁を削除できませんでした。");
            return;
        }

        wall.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        wall.OnFinalize();
        m_GotoPoolWalls.Add(wall);
        wall.gameObject.SetActive(false);
    }

    /// <summary>
    /// プールから壁を取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="wallPrefab">取得や生成の情報源となる壁のプレハブ</param>
    public CommandWallController GetPoolingWall(CommandWallController wallPrefab)
    {
        if (wallPrefab == null)
        {
            return null;
        }

        string wallId = wallPrefab.GetWallGroupId();
        CommandWallController wall = null;

        foreach (var b in m_PoolWalls)
        {
            if (b != null && b.GetWallGroupId() == wallId)
            {
                wall = b;
                break;
            }
        }

        if (wall == null)
        {
            wall = Instantiate(wallPrefab);
            wall.transform.SetParent(m_WallHolder);
            m_PoolWalls.Add(wall);
        }

        return wall;
    }

    /// <summary>
    /// 壁を直接登録する。
    /// </summary>
    public void RegistWall(CommandWallController wall)
    {
        if (wall == null)
        {
            return;
        }

        wall.transform.SetParent(m_WallHolder);
        m_PoolWalls.Add(wall);
        CheckStandbyWall(wall);
    }
}
