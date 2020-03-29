using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoControllerChanger
{
    string ControllerName { get; }
    bool ApplyEnableController { get; }
}
