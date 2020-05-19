using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private string m_Format = "version {0}";

    private void Awake()
    {
        m_Text.text = string.Format(m_Format, Application.version);
    }
}
