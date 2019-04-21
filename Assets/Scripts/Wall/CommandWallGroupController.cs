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
    private E_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// このグループに所属する壁のリスト。
    /// </summary>
    private List<CommandWallController> m_Walls;

    /// <summary>
    /// 非表示になって破棄されるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;

    public E_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    /// <summary>
    /// この壁グループを破棄する。
    /// </summary>
    public virtual void DestroyWallGroup()
    {
        if (m_Cycle == E_OBJECT_CYCLE.UPDATE)
        {
            //CommandWallManager.Instance.CheckPoolWall(this);
        }
    }
}
