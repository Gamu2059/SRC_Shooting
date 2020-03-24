using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

/// <summary>
/// リアルモードのカメラの管理をする。
/// </summary>
[Serializable]
public class BattleRealCameraManager : SingletonMonoBehavior<BattleRealCameraManager>
{
    [SerializeField]
    private BattleRealCameraController m_BackCamera;
    
    [SerializeField]
    private BattleRealCameraController m_FrontCamera;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_BackCamera.OnInitialize();
        m_FrontCamera.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_FrontCamera.OnFinalize();
        m_BackCamera.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        m_BackCamera.OnStart();
        m_FrontCamera.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_BackCamera.OnUpdate();
        m_FrontCamera.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_BackCamera.OnLateUpdate();
        m_FrontCamera.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_BackCamera.OnFixedUpdate();
        m_FrontCamera.OnFixedUpdate();
    }

    #endregion

    public BattleRealCameraController GetCameraController(E_CAMERA_TYPE cameraType)
    {
        if (cameraType == E_CAMERA_TYPE.BACK_CAMERA)
        {
            return m_BackCamera;
        }
        else
        {
            return m_FrontCamera;
        }
    }

    /// <summary>
    /// ビューポート上の座標からワールド座標に変換する。
    /// </summary>
    public Vector3 GetViewportWorldPoint(float x, float y, E_CAMERA_TYPE cameraType = E_CAMERA_TYPE.BACK_CAMERA)
    {
        var camera = GetCameraController(cameraType).Camera;
        Vector3 farPos = camera.ViewportToWorldPoint(new Vector3(x, y, camera.nearClipPlane));
        Vector3 originPos = camera.transform.position;
        Vector3 dir = (farPos - originPos).normalized;

        Vector3 axis = Vector3.up;
        float h = Vector3.Dot(new Vector3(0, ParamDef.BASE_Y_POS, 0), axis);
        return originPos + dir * (h - Vector3.Dot(axis, originPos)) / (Vector3.Dot(axis, dir));
    }

    /// <summary>
    /// ワールド座標からビューポート上の座標に変換する。
    /// </summary>
    public Vector2 WorldToViewportPoint(Vector3 worldPosition, E_CAMERA_TYPE cameraType = E_CAMERA_TYPE.BACK_CAMERA)
    {
        var camera = GetCameraController(cameraType).Camera;
        return camera.WorldToViewportPoint(worldPosition);
    }

    public void Shake(CameraShakeParam shakeParam, E_CAMERA_TYPE cameraType = E_CAMERA_TYPE.BACK_CAMERA)
    {
        var camera = GetCameraController(E_CAMERA_TYPE.BACK_CAMERA);
        camera.Shake(shakeParam);
        camera = GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
        camera.Shake(shakeParam);
    }

    public void StopShake()
    {
        var camera = GetCameraController(E_CAMERA_TYPE.BACK_CAMERA);
        camera.StopShake();
        camera = GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
        camera.StopShake();
    }
}
