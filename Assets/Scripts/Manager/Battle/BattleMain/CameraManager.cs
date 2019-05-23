using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// メイン画面のカメラの管理をする。
/// </summary>
public class CameraManager : BattleSingletonMonoBehavior<CameraManager>
{
	[SerializeField]
	private Camera m_Camera;

    [SerializeField]
    private GameObject m_CameraParent;

	[SerializeField]
	private Transform m_TargetObject;

	[SerializeField]
	private Vector3 m_CameraOffsetPos;

	[SerializeField]
	private Vector3 m_TargetOffsetPos;

    [SerializeField]
    private TimelineAsset m_CameraTimeline;


	public Camera GetCamera()
	{
		return m_Camera;
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();


        if(Input.GetKeyDown(KeyCode.H))
        {
            //foreach(var root in m_CameraTimeline.GetRootTracks())
            //{
            //    Debug.Log("Root name " + root.name);
            //    foreach(var clip in root.GetClips())
            //    {
            //        Debug.Log("Clip name " + clip.displayName +", asset "+ clip.asset.name);
            //        if (clip.asset is BattleAnimationPlayableAsset battleAnimAsset)
            //        {
            //            battleAnimAsset.SetTarget(m_Camera.transform);
            //        }
            //    }
            //}

            var dir = m_Camera.GetComponent<PlayableDirector>();
            dir.playableAsset = m_CameraTimeline;
            dir.Play();
        }
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
