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
    /// この壁は破壊不可能かどうか。
    /// </summary>
    [SerializeField]
    private bool m_IsUnbreakable;

    /// <summary>
    /// この壁の耐久値。
    /// </summary>
    [SerializeField]
    private int m_MaxHp;

    /// <summary>
    /// この壁の状態。
    /// </summary>
    private E_WALL_CYCLE m_WallCycle;

    /// <summary>
    /// 非表示になって破棄されるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;

    /// <summary>
    /// この壁の現在耐久値。
    /// </summary>
    private int m_NowHp;

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

    protected virtual void Start()
    {
        CommandWallManager.Instance.RegistWall(this);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CanOutDestroy = false;
        m_NowHp = m_MaxHp;
    }

    protected virtual void OnBecameVisible()
    {
        m_CanOutDestroy = true;
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            DestroyWall();
        }
    }

    /// <summary>
    /// この壁を破棄する。
    /// </summary>
    public virtual void DestroyWall()
    {
        if (m_WallCycle == E_WALL_CYCLE.UPDATE)
        {
            CommandWallManager.Instance.CheckPoolWall(this);
        }
    }

    public void Damage(int damage)
    {
        if (damage < 1)
        {
            return;
        }

        m_NowHp = Mathf.Clamp(m_NowHp - damage, 0, m_MaxHp);
        if (m_NowHp == 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {
        DestroyWall();
    }

    /// <summary>
    /// 他の弾から当てられた時の処理。
    /// </summary>
    /// <param name="attackBullet">他の弾</param>
    /// <param name="attackData">他の弾の衝突情報</param>
    /// <param name="targetData">この壁の衝突情報</param>
    public virtual void SufferBullet(CommandBulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {
        if (m_IsUnbreakable)
        {
            return;
        }


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
