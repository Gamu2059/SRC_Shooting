#pragma warning disable 0649

using UnityEngine;

[System.Serializable]
public class FloatValueParam
{
    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    [SerializeField]
    private float m_ConstantValue;

    [SerializeField]
    private float m_RandomMinValue;

    [SerializeField]
    private float m_RandomMaxValue;

    public float GetValue()
    {
        if (m_ValueType == E_VALUE_TYPE.CONSTANT)
        {
            return m_ConstantValue;
        }

        return Random.Range(m_RandomMinValue, m_RandomMaxValue);
    }
}
