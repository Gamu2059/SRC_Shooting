using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁のグループの管理を行う。
/// </summary>
public class CommandWallGroupManager : BattleSingletonMonoBehavior<CommandWallGroupManager>
{
    public const string HOLDER_NAME = "[CommandWallHolder]";

    #region Field

    private Transform m_WallHolder;

    /// <summary>
	/// UPDATE状態の壁グループを保持するリスト。
	/// </summary>
	[SerializeField]
    private List<CommandWallGroupController> m_UpdateWallGroups;

    /// <summary>
    /// POOL状態に遷移する壁グループのリスト。
    /// </summary>
    [SerializeField]
    private List<CommandWallGroupController> m_GotoDestroyWallGroups;

    #endregion

    #region Get

    /// <summary>
    /// 全ての有効な壁のグループを取得する。
    /// </summary>
    public List<CommandWallGroupController> GetUpdateWallGroups()
    {
        return m_UpdateWallGroups;
    }

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        m_UpdateWallGroups = new List<CommandWallGroupController>();
        m_GotoDestroyWallGroups = new List<CommandWallGroupController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();

        m_UpdateWallGroups.Clear();
        m_GotoDestroyWallGroups.Clear();
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
        foreach (var wallGroup in m_UpdateWallGroups)
        {
            if (wallGroup == null)
            {
                continue;
            }

            if (wallGroup.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                wallGroup.OnStart();
                wallGroup.SetCycle(E_OBJECT_CYCLE.UPDATE);
            }

            wallGroup.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var wallGroup in m_UpdateWallGroups)
        {
            if (wallGroup == null)
            {
                continue;
            }

            wallGroup.OnLateUpdate();
        }

        //GotoPoolFromUpdate();
    }




    /// <summary>
    /// 壁をSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyWall(CommandWallController wall)
    {
        //if (wall == null || !m_PoolWalls.Contains(wall))
        //{
        //    Debug.LogError("指定された壁を追加できませんでした。");
        //    return;
        //}

        //m_PoolWalls.Remove(wall);
        //m_StandbyWalls.Add(wall);
        //wall.gameObject.SetActive(true);
        //wall.SetWallCycle(E_WALL_CYCLE.STANDBY_UPDATE);
        //wall.OnInitialize();
    }

    /// <summary>
    /// 指定した壁を制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolWall(CommandWallController wall)
    {
        //if (wall == null || m_GotoPoolWalls.Contains(wall))
        //{
        //    Debug.LogError("指定した壁を削除できませんでした。");
        //    return;
        //}

        //wall.SetWallCycle(E_WALL_CYCLE.STANDBY_POOL);
        //wall.OnFinalize();
        //m_GotoPoolWalls.Add(wall);
        //wall.gameObject.SetActive(false);
    }

    /// <summary>
    /// 壁を直接登録する。
    /// </summary>
    public void RegistWall(CommandWallGroupController wallGroup)
    {
        if (wallGroup == null)
        {
            return;
        }

        wallGroup.transform.SetParent(m_WallHolder);
        m_UpdateWallGroups.Add(wallGroup);
        //CheckStandbyWall(wallGroup);
    }
}
