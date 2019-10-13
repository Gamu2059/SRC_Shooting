using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingInputManager : InputManagerBase
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string CANCEL = "Cancel";
    private const string SHOT = "Shot";
    private const string SLOW = "Slow";
    private const string MENU = "Menu";

    public static BattleHackingInputManager Instance => BattleHackingManager.Instance.InputManager;

    public Vector2 MoveDir { get; private set; }

    public E_INPUT_STATE Cancel { get; private set; }

    public E_INPUT_STATE Shot { get; private set; }

    public E_INPUT_STATE Slow { get; private set; }

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

        Cancel = GetButton(CANCEL);
        Shot = GetButton(SHOT);
        Slow = GetButton(SLOW);
        Menu = GetButton(MENU);
    }

    public void RegistInput()
    {
        RegisterAxis(HORIZONTAL);
        RegisterAxis(VERTICAL);
        RegisterButton(CANCEL);
        RegisterButton(SHOT);
        RegisterButton(SLOW);
        RegisterButton(MENU);
    }

    public void RemoveInput()
    {
        RemoveAxis(HORIZONTAL);
        RemoveAxis(VERTICAL);
        RemoveButton(CANCEL);
        RemoveButton(SHOT);
        RemoveButton(SLOW);
        RemoveButton(MENU);
    }
}
