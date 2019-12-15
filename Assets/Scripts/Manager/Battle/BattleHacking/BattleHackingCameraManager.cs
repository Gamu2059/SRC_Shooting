using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

/// <summary>
/// ハッキングモードのカメラの管理をする。
/// </summary>
[Serializable]
public class BattleHackingCameraManager : ControllableObject
{
    public static BattleRealCameraManager Instance => BattleRealManager.Instance.CameraManager;

    private BattleHackingCameraController m_BackCamera;
    private BattleHackingCameraController m_FrontCamera;

    #region Get Set

    public Camera GetBackCamera()
    {
        if (m_BackCamera != null)
        {
            return m_BackCamera.Camera;
        }

        return null;
    }

    public Camera GetFrontCamera()
    {
        if (m_FrontCamera != null)
        {
            return m_FrontCamera.Camera;
        }

        return null;
    }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        DestroyCamera(m_FrontCamera);
        DestroyCamera(m_BackCamera);

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        UpdateCamera(m_BackCamera);
        UpdateCamera(m_FrontCamera);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        LateUpdateCamera(m_BackCamera);
        LateUpdateCamera(m_FrontCamera);
    }

    #endregion

    /// <summary>
    /// カメラを登録する。
    /// </summary>
    public void RegisterCamera(BattleHackingCameraController camera, E_CAMERA_TYPE cameraType)
    {
        if (camera == null)
        {
            return;
        }

        camera.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
        camera.OnInitialize();

        if (cameraType == E_CAMERA_TYPE.BACK_CAMERA)
        {
            m_BackCamera = camera;
        }
        else
        {
            m_FrontCamera = camera;
        }
    }

    private void UpdateCamera(BattleHackingCameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        if (controller.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
        {
            controller.OnStart();
            controller.SetCycle(E_OBJECT_CYCLE.UPDATE);
        }

        controller.OnUpdate();
    }

    private void LateUpdateCamera(BattleHackingCameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.OnLateUpdate();
    }

    private void FixedUpdateCamera(BattleHackingCameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.OnFixedUpdate();
    }

    private void DestroyCamera(BattleHackingCameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.SetCycle(E_OBJECT_CYCLE.DESTROYED);
        controller.OnFinalize();
    }

    public BattleHackingCameraController GetCameraController(E_CAMERA_TYPE cameraType)
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

    public void Shake(CameraShakeParam shakeParam)
    {
        var camera = GetCameraController(E_CAMERA_TYPE.BACK_CAMERA);
        camera.Shake(shakeParam);
        camera = GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
        camera.Shake(shakeParam);
    }
}
