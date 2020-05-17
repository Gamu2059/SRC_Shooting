using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBackChanger : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;

    [SerializeField]
    private Sprite[] m_Sprites;

    private void Awake()
    {
        var idx = Random.Range(0, m_Sprites.Length);
        m_Image.sprite = m_Sprites[idx];
    }
}
