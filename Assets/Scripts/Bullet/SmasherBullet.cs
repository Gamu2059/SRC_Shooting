using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBullet : Bullet
{
    [SerializeField]
    private float m_Speed;

    private void OnBecameInvisible()
    {
        //DestroyBullet();
    }
}
