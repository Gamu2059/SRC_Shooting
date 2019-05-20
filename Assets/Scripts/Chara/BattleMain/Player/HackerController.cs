using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : PlayerController
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
        UpdateShotLevel(GetLevel());
	}

    private void UpdateShotLevel(int level)
    {
        if(level >= 3)
        {
            m_CommandEventInterval = 3.0f;
        }
        else if (level >= 2)
        {
            m_CommandEventInterval = 4.0f;
        }
        else
        {
            m_CommandEventInterval = 5.0f;
        }
    } 

	public override void ShotBullet(InputManager.E_INPUT_STATE state)
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

    public override void ShotBomb(InputManager.E_INPUT_STATE state)
    {
        Debug.Log(string.Format("{0}Bomb!!!@{1}", this.name, this.transform.position));
    }
}
