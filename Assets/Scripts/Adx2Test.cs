#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adx2Test : MonoBehaviour
{
    [SerializeField]
    private CriAtomSource m_CriAtomSource;

    [SerializeField, Range(0, 1)]
    private float m_AiSAC;

    private Coroutine coroutine;
    private bool toHacking = false;

    private void Start()
    {
        m_CriAtomSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            coroutine = StartCoroutine(Aisac());
        }
    }

    private IEnumerator Aisac()
    {
        if (coroutine != null)
        {
            yield break;
        }

        toHacking = !toHacking;
        var duration = 1.4f;
        var nomalized = toHacking ? 0f : 1f;
        var time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            if (toHacking)
            {
                nomalized = time / duration;
            } else
            {
                nomalized = 1 - time / duration;
            }

            m_CriAtomSource.SetAisacControl("BGM_FadeControll", nomalized);

            if (time / duration > 1)
            {
                break;
            }

            yield return null;
        }

        coroutine = null;
    }
}
