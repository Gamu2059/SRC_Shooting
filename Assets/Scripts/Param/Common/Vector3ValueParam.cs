#pragma warning disable 0649

using UnityEngine;

[System.Serializable]
public class Vector3ValueParam
{
    [SerializeField]
    private E_VALUE_TYPE m_ValueType = E_VALUE_TYPE.CONSTANT;

    [SerializeField]
    private Vector3 m_ConstantValue;

    [SerializeField]
    private Vector3 m_RandomMinValue;

    [SerializeField]
    private Vector3 m_RandomMaxValue;

    public Vector3 GetValue()
    {
        if (m_ValueType == E_VALUE_TYPE.CONSTANT)
        {
            return m_ConstantValue;
        }

        var x = Random.Range(m_RandomMinValue.x, m_RandomMaxValue.x);
        var y = Random.Range(m_RandomMinValue.y, m_RandomMaxValue.y);
        var z = Random.Range(m_RandomMinValue.z, m_RandomMaxValue.z);
        return new Vector3(x, y);
    }
}
