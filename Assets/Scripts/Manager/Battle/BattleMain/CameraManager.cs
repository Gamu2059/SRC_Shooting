using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メイン画面のカメラの管理をする。
/// </summary>
public class CameraManager : SingletonMonoBehavior<CameraManager>
{
	[SerializeField]
	private Camera m_TargetCamera;

	[SerializeField]
	private Transform m_TargetObject;

	[SerializeField]
	private Vector3 m_CameraOffsetPos;

	[SerializeField]
	private Vector3 m_TargetOffsetPos;


	public Camera GetCamera()
	{
		return m_TargetCamera;
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();

		if( m_TargetCamera == null || m_TargetObject == null )
		{
			return;
		}

		var cameraT = m_TargetCamera.transform;
		cameraT.eulerAngles = m_TargetObject.eulerAngles;
		cameraT.position = m_TargetObject.position;
		cameraT.localPosition += m_TargetObject.right * m_CameraOffsetPos.x + m_TargetObject.up * m_CameraOffsetPos.y + m_TargetObject.forward * m_CameraOffsetPos.z;

		var lookPos = m_TargetObject.position;
		lookPos += m_TargetObject.right * m_TargetOffsetPos.x + m_TargetObject.up * m_TargetOffsetPos.y + m_TargetObject.forward * m_TargetOffsetPos.z;
		cameraT.LookAt( lookPos );
	}
}
