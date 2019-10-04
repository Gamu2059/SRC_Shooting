using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E_INPUT_STATE = InputExtension.E_INPUT_STATE;

public class BattleRealInputManager : ControllableObject
{
    private const string SUBMIT = "Submit";
    private const string CANCEL = "Cancel";
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string SHOT = "Shot";
    private const string BOMB = "Bomb";
    private const string MENU = "Menu";

    public Vector2 MoveDir { get; private set; }

    public bool IsPushShot { get; private set; }

    public bool IsPushChangeWeapon { get; private set; }

    public bool IsPushSlow { get; private set; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var x = InputExtension.GetAxisAction(HORIZONTAL);
        var y = InputExtension.GetAxisAction(VERTICAL);
        MoveDir = new Vector2(x, y);

        IsPushShot = InputExtension.GetButtonAction(SHOT) == E_INPUT_STATE.STAY;
        IsPushChangeWeapon = InputExtension.GetButtonAction(BOMB) == E_INPUT_STATE.DOWN;
        IsPushSlow = InputExtension.GetButtonAction(MENU) == E_INPUT_STATE.DOWN;
    }
}
