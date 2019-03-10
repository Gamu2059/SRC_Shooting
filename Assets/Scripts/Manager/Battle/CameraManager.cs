using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehavior<CameraManager>
{
	[SerializeField]
	private float m_Speed;

	[SerializeField]
	private Vector3 m_Direction;

	[SerializeField]
	private Camera m_TargetCamera;

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();

		m_TargetCamera.transform.Translate( m_Direction.normalized * m_Speed * Time.deltaTime, Space.World );
	}
}
