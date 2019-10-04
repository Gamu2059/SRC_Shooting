using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : BattleRealPlayerController
{
	[SerializeField]
	private Transform[] m_MainShotPosition;

    [SerializeField, Range( 0f, 1f )]
	private float m_ShotInterval;

    /// <summary>
    /// コマンドイベントの再発動にかかるインターバル
    /// </summary>
    [SerializeField]
    private float m_CommandEventInterval;

	private float shotDelay;

    public float GetCommandEventInterval()
    {
        return m_CommandEventInterval;
    }


	public override void OnUpdate()
	{
		base.OnUpdate();
		shotDelay += Time.deltaTime;
	}

	public override void ShotBullet(E_INPUT_STATE state)
	{
		if (shotDelay >= m_ShotInterval)
		{
		    for (int i = 0; i < m_MainShotPosition.Length; i++)
		    {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position - transform.parent.position;
                BulletController.ShotBullet(shotParam);
		    }
		    shotDelay = 0;
		}
	}
}
