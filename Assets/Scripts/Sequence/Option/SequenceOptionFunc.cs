using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SequenceUnitやSequenceGroupの特定のタイミングで呼び出したい関数をオプショナルで追加するためのもの。
/// </summary>
public abstract class SequenceOptionFunc : ScriptableObject
{
    public abstract void Call(Transform transform = null);
}
