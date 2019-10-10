using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突情報を管理するコンポーネント。
/// </summary>
public class BattleObjectCollider : MonoBehaviour
{
    /// <summary>
    /// 衝突情報トランスフォームを保持する。
    /// </summary>
    [SerializeField]
    private ColliderTransform[] m_ColliderTransforms;

    /// <summary>
    /// コライダーデータを生成する。
    /// </summary>
    /// <returns></returns>
	public ColliderData[] CreateColliderData()
    {
        int hitNum = m_ColliderTransforms.Length;
        var colliders = new ColliderData[hitNum];

        for (int i = 0; i < hitNum; i++)
        {
            Transform t = m_ColliderTransforms[i].Transform;
            var c = new ColliderData();
            c.CenterPos = new Vector2(t.position.x, t.position.z);
            c.Size = new Vector2(t.lossyScale.x, t.lossyScale.z);
            c.Angle = -t.eulerAngles.y;
            c.ColliderType = m_ColliderTransforms[i].ColliderType;
            c.CollideName = m_ColliderTransforms[i].CollideName;

            colliders[i] = c;
        }

        return colliders;
    }

    private static Shader m_CollisionShader;
    private Material m_lineMaterial;

    void Start()
    {
        if (m_CollisionShader == null)
        {
            m_CollisionShader = Shader.Find("Custom/CollisionRender");
        }

        // ライン描画用のマテリアルを生成.
        m_lineMaterial = new Material(m_CollisionShader);
        m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        m_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    private void DrawLine2D(Vector2 v0, Vector2 v1, float lineWidth)
    {
        Vector2 n = ((new Vector2(v1.y, v0.x)) - (new Vector2(v0.y, v1.x))).normalized * lineWidth;
        GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f);
        GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
        GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
        GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
    }

    private void OnRenderObject()
    {
        if (m_lineMaterial == null)
        {
            return;
        }

        if (m_ColliderTransforms == null)
        {
            return;
        }

        if (BattleManager.Instance == null || BattleRealManager.Instance == null || BattleRealStageManager.Instance == null)
        {
            return;
        }

        var stageManager = BattleRealStageManager.Instance;
        m_lineMaterial.SetPass(0);

        for (int i = 0; i < m_ColliderTransforms.Length; i++)
        {
            var ct = m_ColliderTransforms[i];
            var width = 1.0f / Screen.width * ct.ColliderWidth * 0.5f;

            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(ct.ColliderColor);

            switch (ct.ColliderType)
            {
                case E_COLLIDER_SHAPE.RECT:
                    DrawRect(ct, stageManager, width);
                    break;
                case E_COLLIDER_SHAPE.CIRCLE:
                    DrawCircle(ct, stageManager, width);
                    break;
                case E_COLLIDER_SHAPE.ELLIPSE:
                    DrawEllipse(ct, stageManager, width);
                    break;
                case E_COLLIDER_SHAPE.CAPSULE:
                    DrawCapsule(ct, stageManager, width);
                    break;
                case E_COLLIDER_SHAPE.RAY:
                    break;
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    /// <summary>
    /// 矩形を描画する。
    /// </summary>
    private void DrawRect(ColliderTransform ct, BattleRealStageManager stageManager, float width)
    {
        var t = ct.Transform;
        var pos = t.position.ToVector2XZ();
        var rad = t.eulerAngles.y * Mathf.Deg2Rad;
        var scl = t.lossyScale / 2f;

        var verts = new Vector2[] {
            new Vector2(-scl.x, -scl.z),
            new Vector2(-scl.x, scl.z),
            new Vector2(scl.x, scl.z),
            new Vector2(scl.x, -scl.z),
        };

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = CalcRotatePos(verts[i].x, verts[i].y, -rad) + pos;
            verts[i] = stageManager.CalcViewportPosFromWorldPosition(verts[i].x, verts[i].y);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// 真円を描画する。
    /// </summary>
    private void DrawCircle(ColliderTransform ct, BattleRealStageManager stageManager, float width)
    {
        var t = ct.Transform;
        var pos = t.position.ToVector2XZ();
        float scl = 0;
        if (ct.Direction == ColliderTransform.E_DIRECTION.HORIZONTAL)
        {
            scl = t.lossyScale.x / 2f;
        }
        else
        {
            scl = t.lossyScale.z / 2f;
        }

        var verts = new Vector2[16];
        var unitRad = Mathf.PI * 2 / verts.Length;
        for (int i = 0; i < verts.Length; i++)
        {
            var p = new Vector2(Mathf.Cos(unitRad * i), Mathf.Sin(unitRad * i)) * scl + pos;
            verts[i] = stageManager.CalcViewportPosFromWorldPosition(p.x, p.y);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// 楕円を描画する。
    /// </summary>
    private void DrawEllipse(ColliderTransform ct, BattleRealStageManager stageManager, float width)
    {
        var t = ct.Transform;
        var pos = t.position.ToVector2XZ();
        var rad = t.eulerAngles.y * Mathf.Deg2Rad;
        var scl = t.lossyScale.ToVector2XZ() / 2f;

        var verts = new Vector2[16];
        var unitRad = Mathf.PI * 2 / verts.Length;
        for (int i = 0; i < verts.Length; i++)
        {
            var p = new Vector2(Mathf.Cos(unitRad * i) * scl.x, Mathf.Sin(unitRad * i) * scl.y);
            p = CalcRotatePos(p.x, p.y, -rad) + pos;
            verts[i] = stageManager.CalcViewportPosFromWorldPosition(p.x, p.y);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// カプセルを描画する。
    /// </summary>
    private void DrawCapsule(ColliderTransform ct, BattleRealStageManager stageManager, float width)
    {
        var t = ct.Transform;
        var pos = t.position.ToVector2XZ();
        var rad = t.eulerAngles.y * Mathf.Deg2Rad;
        var scl = t.lossyScale.ToVector2XZ() / 2f;
        var radius = Mathf.Min(scl.x, scl.y);
        var halfLength = Mathf.Max(scl.x, scl.y) - radius;
        var isXLonger = scl.x > scl.y;

        var pointNum = 16;
        var semicirclePointNum = pointNum / 2 + 1;
        var unitRad = Mathf.PI * 2 / pointNum;

        var vertsA = new List<Vector2>(semicirclePointNum * 2);
        var vertsB = new List<Vector2>(semicirclePointNum);

        if (isXLonger)
        {
            var baseRad = -Mathf.PI / 2;
            for (int i = 0; i < semicirclePointNum; i++)
            {
                var r = unitRad * i + baseRad;
                var p = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * radius;
                p.x += halfLength;
                vertsA.Add(p);
            }

            baseRad = Mathf.PI / 2;
            for (int i = 0; i < semicirclePointNum; i++)
            {
                var r = unitRad * i + baseRad;
                var p = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * radius;
                p.x -= halfLength;
                vertsB.Add(p);
            }
        }
        else
        {
            var baseRad = 0f;
            for (int i = 0; i < semicirclePointNum; i++)
            {
                var r = unitRad * i + baseRad;
                var p = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * radius;
                p.y += halfLength;
                vertsA.Add(p);
            }

            baseRad = Mathf.PI;
            for (int i = 0; i < semicirclePointNum; i++)
            {
                var r = unitRad * i + baseRad;
                var p = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * radius;
                p.y -= halfLength;
                vertsB.Add(p);
            }
        }

        vertsA.AddRange(vertsB);
        var verts = vertsA.ToArray();
        for (int i = 0; i < verts.Length; i++)
        {
            var p = CalcRotatePos(verts[i].x, verts[i].y, -rad) + pos;
            verts[i] = stageManager.CalcViewportPosFromWorldPosition(p.x, p.y);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// 回転した座標を計算する。
    /// </summary>
    private Vector2 CalcRotatePos(float x, float z, float rad)
    {
        var nX = Mathf.Cos(rad) * x - Mathf.Sin(rad) * z;
        var nZ = Mathf.Sin(rad) * x + Mathf.Cos(rad) * z;
        return new Vector2(nX, nZ);
    }
}
