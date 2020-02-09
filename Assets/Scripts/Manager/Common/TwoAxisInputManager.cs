using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TwoAxisInputManager : InputManagerBase
{
    private const string VERTICAL = "Vertical";
    private const string HORIZONTAL = "Horizontal";
    private const string SUBMIT = "Submit";
    private const string CANCEL = "Cancel";

    public Vector2 MoveDir { get; private set; }

    public E_INPUT_STATE Submit { get; private set; }

    public E_INPUT_STATE Cancel { get; private set; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var y = GetAxis(VERTICAL);
        var x = GetAxis(HORIZONTAL);

        MoveDir = Vector2.zero;

        if (y == 0)
        {

        }
        else
        {
            MoveDir += new Vector2(0, y);
        }

        if(x == 0)
        {

        }
        else
        {
            MoveDir += new Vector2(x, 0);
        }

        Submit = GetButton(SUBMIT);
        Cancel = GetButton(CANCEL);
    }

    public void RegistInput()
    {
        RegisterAxis(VERTICAL);
        RegisterAxis(HORIZONTAL);
        RegisterButton(SUBMIT);
        RegisterButton(CANCEL);
    }

    public void RemoveInput()
    {
        RemoveAxis(VERTICAL);
        RemoveAxis(HORIZONTAL);
        RemoveButton(SUBMIT);
        RemoveButton(CANCEL);
    }
}
