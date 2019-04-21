using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーの入力を管理する。
/// </summary>
public class InputManager : SingletonMonoBehavior<InputManager>
{
    /// <summary>
    /// マウスやボタンの入力状態。
    /// </summary>
    public enum E_INPUT_STATE
    {
        DOWN,
        STAY,
        UP,
    }

    public const string SUBMIT = "Submit";
    public const string CANCEL = "Cancel";
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";
    public const string SHOT = "Shot";
    public const string BOMB = "Bomb";
    public const string CHANGE_CHARA = "Change Chara";
    public const string MENU = "Menu";

    public Action<E_INPUT_STATE> SubmitAction;
    public Action<E_INPUT_STATE> CancelAction;
    public Action<float> HorizontalAction;
    public Action<float> VerticalAction;
    public Action<E_INPUT_STATE> ShotAction;
    public Action<E_INPUT_STATE> BombAction;
    public Action<float> ChangeCharaAction;
    public Action<E_INPUT_STATE> MenuAction;

    public override void OnUpdate()
	{
		base.OnUpdate();

        DetectButtonAction(SUBMIT, SubmitAction);
        DetectButtonAction(CANCEL, CancelAction);
        DetectButtonAction(SHOT, ShotAction);
        DetectButtonAction(BOMB, BombAction);
        DetectButtonAction(MENU, MenuAction);
        DetectAxisAction(HORIZONTAL, HorizontalAction);
        DetectAxisAction(VERTICAL, VerticalAction);
        DetectAxisAction(CHANGE_CHARA, ChangeCharaAction);
    }

    private void DetectButtonAction(string name, Action<E_INPUT_STATE> callback)
    {
        if (Input.GetButtonDown(name))
        {
            EventUtility.SafeInvokeAction(callback, E_INPUT_STATE.DOWN);
        } else if (Input.GetButton(name))
        {
            EventUtility.SafeInvokeAction(callback, E_INPUT_STATE.STAY);
        } else if (Input.GetButtonUp(name))
        {
            EventUtility.SafeInvokeAction(callback, E_INPUT_STATE.UP);
        }
    }

    private void DetectAxisAction(string name, Action<float> callback)
    {
        float value = Input.GetAxis(name);
        EventUtility.SafeInvokeAction(callback, value);
    }
}
