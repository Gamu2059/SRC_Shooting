using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private Transform[] m_Protectors;

	[SerializeField]
	private float m_ProtectorRadius;

	[SerializeField]
	private float m_ProtectorSpeed;

	private float m_Rad;

	private void Update()
	{
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

		if( m_Protectors == null || m_Protectors.Length < 1 )
		{
			return;
		}

		m_Rad += m_ProtectorSpeed * Time.deltaTime;
		m_Rad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_Rad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}
}
