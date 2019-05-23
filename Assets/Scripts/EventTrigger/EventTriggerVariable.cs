using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EventTriggerVariable
{
    public E_EVENT_TRIGGER_VARIABLE_TYPE Type;
    public string Name;
    public int IntInitValue;
    public float FloatInitValue;
    public bool BoolInitValue;
}
