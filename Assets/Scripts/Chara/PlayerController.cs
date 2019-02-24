using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerController : CharaControllerBase
{

	[System.Serializable]
	public enum E_PLAYER_LIFE_CYCLE
	{
		AHEAD,
		SORTIE,
		DEAD,
		DEAD_AHEAD,
	}

    [SerializeField, Range(1,3)]
    private int m_Lv;

	[SerializeField]
	private int m_Exp;

	[SerializeField]
	private int m_NowHp;

	[SerializeField]
	private int m_MaxHp;

	[SerializeField]
	private E_PLAYER_LIFE_CYCLE m_LifeCycle;

	[SerializeField]
	private float m_HitSize;

	[SerializeField]
	private GameObject m_BulletPrefab;

	[SerializeField]
	private GameObject m_BombPrefab;

    [SerializeField]
    protected bool IsReadyShotBullet;

    [SerializeField]
    private bool IsAutoShot;

    private IntReactiveProperty Level = new IntReactiveProperty(0);

    public void SetReadyShotBullet()
    {
        IsReadyShotBullet = true;
    }

    public bool GetIsAutoShot()
    {
        return IsAutoShot;
    }

    public override void OnAwake()
    {
        base.OnAwake();     
        Level.Subscribe(x => UpdateShotLevel(x));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Level.Value = m_Lv;
    }

    public virtual void UpdateShotLevel(int level)
    {
        // レベルによるショット変化の処理
    }
}
