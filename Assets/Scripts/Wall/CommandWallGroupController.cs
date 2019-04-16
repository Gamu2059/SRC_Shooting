using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁のグループ管理を行うコントローラ。
/// </summary>
public class CommandWallGroupController : BattleCommandObjectBase
{
    /// <summary>
    /// 壁のグループのサイクル。
    /// </summary>
    private E_WALL_CYCLE m_WallGroupCycle;

    /// <summary>
    /// このグループに所属する壁のリスト。
    /// </summary>
    private List<CommandWallController> m_Walls;

    /// <summary>
    /// 非表示になって破棄されるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;

    public E_WALL_CYCLE GetWallGroupCycle()
    {
        return m_WallGroupCycle;
    }

    public void SetWallGroupCycle(E_WALL_CYCLE cycle)
    {
        m_WallGroupCycle = cycle;
    }

    /// <summary>
    /// この壁グループを破棄する。
    /// </summary>
    public virtual void DestroyWallGroup()
    {
        if (m_WallGroupCycle == E_WALL_CYCLE.UPDATE)
        {
            //CommandWallManager.Instance.CheckPoolWall(this);
        }
    }
}
