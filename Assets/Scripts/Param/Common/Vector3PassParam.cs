#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Vector3の成分ごとに無視するかどうかを保持する
/// </summary>
[Serializable]
public class Vector3PassParam
{
    [SerializeField, Tooltip("X成分を無視するか")]
    private bool m_IsPassX;
    public bool IsPassX => m_IsPassX;

    [SerializeField, Tooltip("Y成分を無視するか")]
    private bool m_IsPassY;
    public bool IsPassY => m_IsPassY;

    [SerializeField, Tooltip("Z成分を無視するか")]
    private bool m_IsPassZ;
    public bool IsPassZ => m_IsPassZ;
}
