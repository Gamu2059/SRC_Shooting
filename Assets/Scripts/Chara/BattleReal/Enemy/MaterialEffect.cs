using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のマテリアルを切り替えるのに使用するコンポーネント
/// </summary>
public class MaterialEffect : ControllableMonoBehavior
{
    [SerializeField]
    private Renderer[] m_Renderers;

    private Dictionary<Renderer, Material> m_OriginMaterialDict;
    private float m_Duration;
    private float m_NowTime;
    private bool m_IsChanging;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (m_Renderers != null)
        {
            m_OriginMaterialDict = new Dictionary<Renderer, Material>();
            foreach (var r in m_Renderers)
            {
                // データの上書きが怖いので複製しておく
                m_OriginMaterialDict.Add(r, Instantiate(r.sharedMaterial));
            }
        }
    }

    protected override void OnDestroyed()
    {
        if (m_OriginMaterialDict != null)
        {
            foreach (var m in m_OriginMaterialDict.Values)
            {
                Destroy(m);
            }

            m_OriginMaterialDict.Clear();
        }

        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_NowTime = 0;
        m_IsChanging = false;
        ResetMaterial();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsChanging)
        {
            if (m_NowTime > m_Duration)
            {
                ResetMaterial();
                m_IsChanging = false;
            }
            else
            {
                m_NowTime += Time.deltaTime;
            }
        }
    }

    public void ChangeMaterial(Material material, float duration)
    {
        if (m_IsChanging)
        {
            return;
        }

        if (material == null || m_Renderers == null)
        {
            return;
        }

        foreach (var r in m_Renderers)
        {
            r.sharedMaterial = material;
        }

        m_Duration = duration;
        m_IsChanging = true;
        m_NowTime = 0;
    }

    public void ResetMaterial()
    {
        if (m_OriginMaterialDict == null)
        {
            return;
        }

        foreach (var set in m_OriginMaterialDict)
        {
            set.Key.sharedMaterial = set.Value;
        }
    }
}
