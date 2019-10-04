using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealInputManager : InputManagerBase
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string SUBMIT = "Submit";
    private const string CANCEL = "Cancel";
    private const string SHOT = "Shot";
    private const string SLOW = "Slow";
    private const string CHANGE_WEAPON_TYPE = "ChangeWeaponType";
    private const string MENU = "Menu";

    public static BattleRealInputManager Instance => BattleRealManager.Instance.InputManager;

    public Vector2 MoveDir { get; private set; }

    public E_INPUT_STATE Submit { get; private set; }

    public E_INPUT_STATE Cancel { get; private set; }

    public E_INPUT_STATE Shot { get; private set; }

    public E_INPUT_STATE Slow { get; private set; }

    public E_INPUT_STATE ChangeMode { get; private set; }

    public E_INPUT_STATE Menu { get; private set; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var x = GetAxis(HORIZONTAL);
        var y = GetAxis(VERTICAL);
        if (x == 0 && y == 0)
        {
            MoveDir = Vector2.zero;
        }
        else
        {
            MoveDir = new Vector2(x, y).normalized;
        }

        Submit = GetButton(SUBMIT);
        Cancel = GetButton(CANCEL);
        Shot = GetButton(SHOT);
        Slow = GetButton(SLOW);
        ChangeMode = GetButton(CHANGE_WEAPON_TYPE);
        Menu = GetButton(MENU);
    }

    public void RegisterInput()
    {
        RegisterAxis(HORIZONTAL);
        RegisterAxis(VERTICAL);
        RegisterButton(SUBMIT);
        RegisterButton(CANCEL);
        RegisterButton(SHOT);
        RegisterButton(SLOW);
        RegisterButton(CHANGE_WEAPON_TYPE);
        RegisterButton(MENU);
    }

    public void RemoveInput()
    {
        RemoveAxis(HORIZONTAL);
        RemoveAxis(VERTICAL);
        RemoveButton(SUBMIT);
        RemoveButton(CANCEL);
        RemoveButton(SHOT);
        RemoveButton(SLOW);
        RemoveButton(CHANGE_WEAPON_TYPE);
        RemoveButton(MENU);
    }
}
