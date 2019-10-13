using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleMainのカメラコントローラ。
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : BattleMainPlayableBase
{
    [SerializeField]
    private E_CAMERA_TYPE m_CameraType;

    private Camera m_Camera;

    public Camera GetCamera()
    {
        return m_Camera;
    }

    private void Start()
    {
        if (CameraManager.Instance == null)
        {
            //BattleManager.Instance.m_OnInitManagers += RegistCamera;
        }
        else
        {
            RegistCamera();
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_Camera = GetComponent<Camera>();
    }

    /// <summary>
    /// CameraManagerに登録する。
    /// </summary>
    private void RegistCamera()
    {
        CameraManager.Instance.RegistCameraController(this, m_CameraType);
    }
}
