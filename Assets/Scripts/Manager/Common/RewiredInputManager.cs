using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Integration.UnityUI;

/// <summary>
/// Rewiredを用いた単体プレイヤー用のInputManager
/// </summary>
public class RewiredInputManager : SingletonMonoBehavior<RewiredInputManager>
{
    #region Define

    private class InnerInputManager : RewiredSingleInputManagerBase<InnerInputManager> { }

    #endregion

    #region Field Inspector

    [SerializeField]
    private RewiredStandaloneInputModule m_StandaloneInputModule;

    [Header("Map Category")]

    [SerializeField]
    private string m_KeyConCategory;

    [SerializeField]
    private string m_UiCategory;

    [SerializeField]
    private string m_InGameCategory;

    [Header("KeyCon Input")]

    [SerializeField]
    private string m_KeyConHorizontal;

    [SerializeField]
    private string m_KeyConVertical;

    [SerializeField]
    private string m_KeyConSubmit;

    [SerializeField]
    private string m_KeyConCancel;

    [Header("UI Input")]

    [SerializeField]
    private string m_UiHorizontal;

    [SerializeField]
    private string m_UiVertical;

    [SerializeField]
    private string m_UiSubmit;

    [SerializeField]
    private string m_UiCancel;

    [Header("InGame Input")]

    [SerializeField]
    private string m_InGameHorizontal;

    [SerializeField]
    private string m_InGameVertical;

    [SerializeField]
    private string m_InGameShot;

    [SerializeField]
    private string m_InGameChargeShot;

    [SerializeField]
    private string m_InGameChangeWeapon;

    [SerializeField]
    private string m_InGameSlowly;

    [SerializeField]
    private string m_InGameOpenMenu;

    #endregion

    #region Field

    public Vector2 UiAxisDir { get; private set; }
    public E_REWIRED_INPUT_STATE UiSubmit { get; private set; }
    public E_REWIRED_INPUT_STATE UiCancel { get; private set; }

    public Vector2 AxisDir { get; private set; }
    public E_REWIRED_INPUT_STATE Shot { get; private set; }
    public E_REWIRED_INPUT_STATE ChargeShot { get; private set; }
    public E_REWIRED_INPUT_STATE ChangeWeapon { get; private set; }
    public E_REWIRED_INPUT_STATE Slowly { get; private set; }
    public E_REWIRED_INPUT_STATE OpenMenu { get; private set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        InnerInputManager.Create();
        InnerInputManager.Instance.OnInitialize();

        var player = ReInput.players.GetPlayer(0);
        InnerInputManager.Instance.SetPlayer(player);

        RegistInput();
    }

    public override void OnFinalize()
    {
        RemoveInput();

        InnerInputManager.Instance.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        InnerInputManager.Instance.OnUpdate();

        var uiX = InnerInputManager.Instance.GetAxis(m_UiHorizontal);
        var uiY = InnerInputManager.Instance.GetAxis(m_UiVertical);
        if (uiX == 0 && uiY == 0)
        {
            UiAxisDir = Vector2.zero;
        }
        else
        {
            UiAxisDir = new Vector2(uiX, uiY).normalized;
        }
        
        UiSubmit = InnerInputManager.Instance.GetButton(m_UiSubmit);
        UiCancel = InnerInputManager.Instance.GetButton(m_UiCancel);

        var ingameX = InnerInputManager.Instance.GetAxis(m_InGameHorizontal);
        var ingameY = InnerInputManager.Instance.GetAxis(m_InGameVertical);
        if (ingameX == 0 && ingameY == 0)
        {
            AxisDir = Vector2.zero;
        }
        else
        {
            AxisDir = new Vector2(ingameX, ingameY).normalized;
        }

        Shot = InnerInputManager.Instance.GetButton(m_InGameShot);
        ChargeShot = InnerInputManager.Instance.GetButton(m_InGameChargeShot);
        ChangeWeapon = InnerInputManager.Instance.GetButton(m_InGameChangeWeapon);
        Slowly = InnerInputManager.Instance.GetButton(m_InGameSlowly);
        OpenMenu = InnerInputManager.Instance.GetButton(m_InGameOpenMenu);
    }

    #endregion

    private void RegistInput()
    {
        InnerInputManager.Instance.RegisterAxis(m_UiHorizontal);
        InnerInputManager.Instance.RegisterAxis(m_UiVertical);
        InnerInputManager.Instance.RegisterButton(m_UiSubmit);
        InnerInputManager.Instance.RegisterButton(m_UiCancel);

        InnerInputManager.Instance.RegisterAxis(m_InGameHorizontal);
        InnerInputManager.Instance.RegisterAxis(m_InGameVertical);
        InnerInputManager.Instance.RegisterButton(m_InGameShot);
        InnerInputManager.Instance.RegisterButton(m_InGameChargeShot);
        InnerInputManager.Instance.RegisterButton(m_InGameChangeWeapon);
        InnerInputManager.Instance.RegisterButton(m_InGameSlowly);
        InnerInputManager.Instance.RegisterButton(m_InGameOpenMenu);
    }

    private void RemoveInput()
    {
        InnerInputManager.Instance.RemoveButton(m_InGameOpenMenu);
        InnerInputManager.Instance.RemoveButton(m_InGameSlowly);
        InnerInputManager.Instance.RemoveButton(m_InGameChangeWeapon);
        InnerInputManager.Instance.RemoveButton(m_InGameChargeShot);
        InnerInputManager.Instance.RemoveButton(m_InGameShot);
        InnerInputManager.Instance.RemoveAxis(m_InGameVertical);
        InnerInputManager.Instance.RemoveAxis(m_InGameHorizontal);

        InnerInputManager.Instance.RemoveButton(m_UiCancel);
        InnerInputManager.Instance.RemoveButton(m_UiSubmit);
        InnerInputManager.Instance.RemoveAxis(m_UiVertical);
        InnerInputManager.Instance.RemoveAxis(m_UiHorizontal);
    }

    //public void ChangeToUIInput()
    //{
    //    InnerInputManager.Instance.SetActionCategory(m_UiCategory, true);
    //    InnerInputManager.Instance.SetActionCategory(m_InGameCategory, false);
    //}

    //public void ChangeToInGameInput()
    //{
    //    InnerInputManager.Instance.SetActionCategory(m_UiCategory, false);
    //    InnerInputManager.Instance.SetActionCategory(m_InGameCategory, true);
    //}

    public void ChangeToKeyConInputModule()
    {
        m_StandaloneInputModule.horizontalAxis = m_KeyConHorizontal;
        m_StandaloneInputModule.verticalAxis = m_KeyConVertical;
        m_StandaloneInputModule.submitButton = m_KeyConSubmit;
        m_StandaloneInputModule.cancelButton = m_KeyConCategory;

        InnerInputManager.Instance.SetActionCategory(m_UiCategory, false);
        InnerInputManager.Instance.SetActionCategory(m_KeyConCategory, true);
    }

    public void ChangeToUIInputModule()
    {
        m_StandaloneInputModule.horizontalAxis = m_UiHorizontal;
        m_StandaloneInputModule.verticalAxis = m_UiVertical;
        m_StandaloneInputModule.submitButton = m_UiSubmit;
        m_StandaloneInputModule.cancelButton = m_UiCancel;

        InnerInputManager.Instance.SetActionCategory(m_UiCategory, true);
        InnerInputManager.Instance.SetActionCategory(m_KeyConCategory, false);
    }
}
