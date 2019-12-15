#pragma warning disable 0649

using UnityEngine;

[System.Serializable]
public class Vector2ValueParam
{
    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    [SerializeField]
    private Vector2 m_ConstantValue;

    [SerializeField]
    private Vector2 m_RandomMinValue;

    [SerializeField]
    private Vector2 m_RandomMaxValue;

    public Vector2 GetValue()
    {
        if (m_ValueType == E_VALUE_TYPE.CONSTANT)
        {
            return m_ConstantValue;
        }

        var x = Random.Range(m_RandomMinValue.x, m_RandomMaxValue.x);
        var y = Random.Range(m_RandomMinValue.y, m_RandomMaxValue.y);
        return new Vector2(x, y);
    }
}
