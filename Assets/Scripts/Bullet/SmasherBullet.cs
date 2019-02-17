using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBullet : Bullet
{
    [SerializeField]
    private float m_Speed;

    public override void OnUpdate()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible()
    {
        DestroyBullet();
    }
}
