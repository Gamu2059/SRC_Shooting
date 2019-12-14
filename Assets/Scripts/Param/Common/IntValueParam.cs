#pragma warning disable 0649

using UnityEngine;

[System.Serializable]
public class IntValueParam
{
    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    [SerializeField]
    private int m_ConstantValue;

    [SerializeField]
    private int m_RandomMinValue;

    [SerializeField]
    private int m_RandomMaxValue;

    public int GetValue()
    {
        if (m_ValueType == E_VALUE_TYPE.CONSTANT)
        {
            return m_ConstantValue;
        }

        return Random.Range(m_RandomMinValue, m_RandomMaxValue);
    }
}
