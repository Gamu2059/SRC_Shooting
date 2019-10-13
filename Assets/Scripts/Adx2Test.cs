using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adx2Test : MonoBehaviour
{
    [SerializeField]
    private CriWareInitializer m_Initializer;

    [SerializeField]
    private CriAtomSource criAtomSource;

    // Start is called before the first frame update
    void Start()
    {
        m_Initializer.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
