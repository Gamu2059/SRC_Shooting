using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メイン画面のカメラの管理をする。
/// </summary>
public class CameraManager : SingletonMonoBehavior<CameraManager>
{
	[SerializeField]
	private Camera m_Camera;

	[SerializeField]
	private Transform m_TargetObject;

	[SerializeField]
	private Vector3 m_CameraOffsetPos;

	[SerializeField]
	private Vector3 m_TargetOffsetPos;


	public Camera GetCamera()
	{
		return m_Camera;
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();

		if( m_Camera == null || m_TargetObject == null )
		{
			return;
		}

		var cameraT = m_Camera.transform;
		cameraT.eulerAngles = m_TargetObject.eulerAngles;
		cameraT.position = m_TargetObject.position;
		cameraT.localPosition += m_TargetObject.right * m_CameraOffsetPos.x + m_TargetObject.up * m_CameraOffsetPos.y + m_TargetObject.forward * m_CameraOffsetPos.z;

		var lookPos = m_TargetObject.position;
		lookPos += m_TargetObject.right * m_TargetOffsetPos.x + m_TargetObject.up * m_TargetOffsetPos.y + m_TargetObject.forward * m_TargetOffsetPos.z;
		cameraT.LookAt( lookPos );
	}

	/// <summary>
	/// ビューポート上の座標からワールド座標に変換する。
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public Vector3 GetViewportWorldPoint( float x, float y )
	{
		var camera = GetCamera();
		Vector3 farPos = camera.ViewportToWorldPoint( new Vector3( x, y, camera.nearClipPlane ) );
		Vector3 originPos = camera.transform.position;
		Vector3 dir = ( farPos - originPos ).normalized;

		Vector3 axis = Vector3.up;
		float h = Vector3.Dot( new Vector3( 0, ParamDef.BASE_Y_POS, 0 ), axis );
		return originPos + dir * ( h - Vector3.Dot( axis, originPos ) ) / ( Vector3.Dot( axis, dir ) );
	}

	public Vector2 WorldToViewportPoint( Vector3 worldPosition )
	{
		var camera = GetCamera();
		return camera.WorldToViewportPoint( worldPosition );
	}
}
