using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaController : MonoBehaviour
{

	[SerializeField]
	private float m_MoveSpeed;

	[SerializeField]
	private KeyCode m_FrontKey;

	[SerializeField]
	private KeyCode m_BackKey;

	[SerializeField]
	private KeyCode m_LeftKey;

	[SerializeField]
	private KeyCode m_RightKey;

	[SerializeField]
	private KeyCode m_ShotKey;

	[SerializeField]
	private GameObject m_Bullet;

	[SerializeField]
	private float m_ShotInterval;

	private float shotDelay;

	[SerializeField]
	private Transform[] m_MainShotPosition;

	[SerializeField]
	private GameObject[] m_SubShotLv2;

	private bool m_SubShotLv2CanShot;

	[SerializeField]
	private GameObject[] m_SubShotLv3;

	private bool m_SubShotLv3CanShot;

	[SerializeField]
	private float m_SubShotLv3Radius;

	[SerializeField]
	private float m_SubShotLv3RotateSpeed;

	[SerializeField, Range(1,3)]
	private int m_ShotLevel;

	[SerializeField]
	private Transform[] m_Protectors;

	[SerializeField]
	private float m_ProtectorRadius;

	[SerializeField]
	private float m_ProtectorSpeed;

	private float m_ProtectorRad;

	private float m_SubShotLv3Rad;

	private void Update()
	{
		shotDelay += Time.deltaTime;

		if(m_ShotLevel >= 3){
			m_SubShotLv3CanShot = true;
			m_SubShotLv2CanShot = true;
		}else if(m_ShotLevel >= 2){
			m_SubShotLv3CanShot = false;
			m_SubShotLv2CanShot = true;
		}else{
			m_SubShotLv3CanShot = false;
			m_SubShotLv2CanShot = false;
		}

		for(int i=0; i< m_SubShotLv2.Length; i++){
			m_SubShotLv2[i].SetActive(m_SubShotLv2CanShot);
		}

		for(int i=0; i< m_SubShotLv3.Length; i++){
			m_SubShotLv3[i].SetActive(m_SubShotLv3CanShot);
		}

		if( Input.GetKey( m_FrontKey ) )
		{
			transform.Translate( Vector3.forward * m_MoveSpeed * Time.deltaTime );
		}

		if( Input.GetKey( m_BackKey ) )
		{
			transform.Translate( Vector3.back * m_MoveSpeed * Time.deltaTime );
		}

		if( Input.GetKey( m_LeftKey ) )
		{
			transform.Translate( Vector3.left * m_MoveSpeed * Time.deltaTime );
		}

		if( Input.GetKey( m_RightKey ) )
		{
			transform.Translate( Vector3.right * m_MoveSpeed * Time.deltaTime );
		}

		if(Input.GetKey(m_ShotKey)){
			if(shotDelay >= m_ShotInterval){
				ShootBullet();
				shotDelay = 0;
			}
		}

		m_SubShotLv3Rad += m_SubShotLv3RotateSpeed * Time.deltaTime;
		m_SubShotLv3Rad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_SubShotLv3.Length;

		if(m_SubShotLv3CanShot){
			for(int i=0; i< m_SubShotLv3.Length; i++){
				float angle = unitAngle * i + m_SubShotLv3Rad;
				float x = m_SubShotLv3Radius * Mathf.Cos( angle );
				float z = m_SubShotLv3Radius * Mathf.Sin( angle );
				m_SubShotLv3[i].GetComponent<Transform>().localPosition = new Vector3( x, 0, z );
				m_SubShotLv3[i].GetComponent<Transform>().LookAt( transform );
			}
		}

		if( m_Protectors == null || m_Protectors.Length < 1 )
		{
			return;
		}

		m_ProtectorRad += m_ProtectorSpeed * Time.deltaTime;
		m_ProtectorRad %= Mathf.PI * 2;
		unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_ProtectorRad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}		
	}

	private void ShootBullet(){
		for(int i=0; i < m_MainShotPosition.Length; i++){
					Instantiate(m_Bullet, m_MainShotPosition[i].position, m_MainShotPosition[i].rotation);
				}

		if(m_SubShotLv2CanShot){
			for(int i=0; i < m_SubShotLv2.Length; i++){
					Instantiate(m_Bullet, m_SubShotLv2[i].GetComponent<Transform>().position, m_SubShotLv2[i].GetComponent<Transform>().rotation);
				}
		}
		if(m_SubShotLv3CanShot){
			for(int i=0; i < m_SubShotLv3.Length; i++){
					Instantiate(m_Bullet, m_SubShotLv3[i].GetComponent<Transform>().position, m_SubShotLv3[i].GetComponent<Transform>().rotation);
				}
		}	
	}
}
