using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CustomImageEffect : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private AnimationCurve m_ShifLevelCurve;

    private bool m_IsPush = false;
    private float m_Time = 0;

    void Start()
    {
        m_Material.SetFloat("_ShiftLevel", 0);
    }

    void Update()
    {

        if (m_IsPush)
        {
            // 一定時間後にShiftLevelを上げる
            var level = m_ShifLevelCurve.Evaluate(m_Time);

            m_Time += Time.deltaTime;
            // シェーダに数値を渡す
            m_Material.SetFloat("_ShiftLevel", level);
        }
        else if (Input.anyKey)
        {
            m_IsPush = true;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // イメージエフェクトの適用
        if (m_Material)
            Graphics.Blit(source, destination, m_Material);
    }
}