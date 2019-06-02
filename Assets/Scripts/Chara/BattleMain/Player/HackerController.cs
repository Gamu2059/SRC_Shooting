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

    [SerializeField]
    private int m_MaxBombAmount;

    private int currentBombAmount;

    [SerializeField]
    private float m_BombInterval;       // ボムの演出の長さで適宜変えてね

    private float bombDelay;

    public float GetCommandEventInterval()
    {
        return m_CommandEventInterval;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        currentBombAmount = m_MaxBombAmount;
        bombDelay = m_BombInterval;
    }

    public override void OnUpdate()
	{
		base.OnUpdate();
		shotDelay += Time.deltaTime;
        bombDelay += Time.deltaTime;
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
        if (currentBombAmount > 0 && bombDelay >= m_BombInterval)
        {
            currentBombAmount--;
            Debug.Log(string.Format("{0}Bomb!!!@{1}, Last{2}", this.name, this.transform.position, this.currentBombAmount));
            bombDelay = 0;
        }
    }
}
