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
    private CameraController m_BackCamera;

    [SerializeField]
    private CameraController m_FrontCamera;

    #region Get Set

    public Camera GetBackCamera()
    {
        if (m_BackCamera != null)
        {
            return m_BackCamera.GetCamera();
        }

        return null;
    }

    public Camera GetFrontCamera()
    {
        if (m_FrontCamera != null)
        {
            return m_FrontCamera.GetCamera();
        }

        return null;
    }

    #endregion

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

    /// <summary>
    /// カメラを登録する。
    /// </summary>
    public void RegistCameraController(CameraController controller, E_CAMERA_TYPE cameraType)
    {
        if (controller == null)
        {
            return;
        }

        controller.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
        controller.OnInitialize();

        if (cameraType == E_CAMERA_TYPE.BACK_CAMERA)
        {
            m_BackCamera = controller;
        }
        else
        {
            m_FrontCamera = controller;
        }
    }

    private void UpdateCamera(CameraController controller)
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

    private void LateUpdateCamera(CameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.OnLateUpdate();
    }

    private void DestroyCamera(CameraController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.SetCycle(E_OBJECT_CYCLE.DESTROYED);
        controller.OnFinalize();
    }

    public CameraController GetCameraController(E_CAMERA_TYPE cameraType)
    {
        if (cameraType == E_CAMERA_TYPE.BACK_CAMERA)
        {
            return m_BackCamera;
        }
        else{
            return m_FrontCamera;
        }
    }

    /// <summary>
    /// ビューポート上の座標からワールド座標に変換する。
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GetViewportWorldPoint(float x, float y)
    {
        var camera = GetBackCamera();
        Vector3 farPos = camera.ViewportToWorldPoint(new Vector3(x, y, camera.nearClipPlane));
        Vector3 originPos = camera.transform.position;
        Vector3 dir = (farPos - originPos).normalized;

        Vector3 axis = Vector3.up;
        float h = Vector3.Dot(new Vector3(0, ParamDef.BASE_Y_POS, 0), axis);
        return originPos + dir * (h - Vector3.Dot(axis, originPos)) / (Vector3.Dot(axis, dir));
    }

    public Vector2 WorldToViewportPoint(Vector3 worldPosition)
    {
        var camera = GetBackCamera();
        return camera.WorldToViewportPoint(worldPosition);
    }
}
