using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TitleInputManager : InputManagerBase<TitleInputManager>
{
    private const string VERTICAL = "Vertical";
    private const string SUBMIT = "Submit";
    private const string CANCEL = "Cancel";

    public Vector2 MoveDir { get; private set; }

    public E_INPUT_STATE Submit { get; private set; }

    public E_INPUT_STATE Cancel { get; private set; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var y = GetAxis(VERTICAL);
        if (y == 0)
        {
            MoveDir = Vector2.zero;
        }
        else
        {
            MoveDir = new Vector2(0, y);
        }

        Submit = GetButton(SUBMIT);
        Cancel = GetButton(CANCEL);
    }

    public void RegistInput()
    {
        RegisterAxis(VERTICAL);
        RegisterButton(SUBMIT);
        RegisterButton(CANCEL);
    }

    public void RemoveInput()
    {
        RemoveAxis(VERTICAL);
        RemoveButton(SUBMIT);
        RemoveButton(CANCEL);
    }
}
