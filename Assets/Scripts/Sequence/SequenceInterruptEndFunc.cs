using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主に、SequenceUnitにおいて割り込みで強制終了させたい時に追加するもの。
/// </summary>
public abstract class SequenceInterruptEndFunc : ScriptableObject
{
    public abstract bool IsInterruptEnd(Transform target, SequenceController controller);
}
