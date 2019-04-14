using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁のコントローラ。
/// </summary>
public class CommandWallController : BattleCommandObjectBase
{
    /// <summary>
    /// この壁のグループ名。
    /// 同じ名前同士の壁は、プールから再利用される。
    /// </summary>
    [SerializeField]
    private string m_WallGroupId = "Default Wall";

    /// <summary>
    /// この壁の状態。
    /// </summary>
    [SerializeField]
    private E_WALL_CYCLE m_WallCycle;

    /// <summary>
    /// 非表示になって破棄されるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;



    public string GetWallGroupId()
    {
        return m_WallGroupId;
    }

    public E_WALL_CYCLE GetWallCycle()
    {
        return m_WallCycle;
    }

    public void SetWallCycle(E_WALL_CYCLE cycle)
    {
        m_WallCycle = cycle;
    }

    /// <summary>
    /// このアイテムが生成された瞬間に呼び出される処理。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CanOutDestroy = false;
    }

    protected virtual void OnBecameVisible()
    {
        m_CanOutDestroy = true;
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            DestroyItem();
        }
    }

    /// <summary>
    /// この壁を破棄する。
    /// </summary>
    public virtual void DestroyItem()
    {
        if (m_WallCycle == E_WALL_CYCLE.UPDATE)
        {
            CommandWallManager.Instance.CheckPoolWall(this);
        }
    }

    /// <summary>
    /// 他の弾から当てられた時の処理。
    /// </summary>
    /// <param name="attackBullet">他の弾</param>
    /// <param name="attackData">他の弾の衝突情報</param>
    /// <param name="targetData">この壁の衝突情報</param>
    public virtual void SufferBullet(CommandBulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {

    }

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">この壁の衝突情報</param>
    /// <param name="targetData">他のキャラの衝突情報</param>
    public virtual void HitChara(CommandCharaController targetChara, ColliderData attackData, ColliderData targetData)
    {

    }
}
