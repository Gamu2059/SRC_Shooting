using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBullet : Bullet
{
    [SerializeField]
    private float m_Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUpdate()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible()
    {
        DestroyBullet();
    }
}
