using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルにおけるオブジェクトの基底クラス。
/// </summary>
[RequireComponent(typeof(BattleObjectCollider))]
public abstract class BattleObjectBase : ControllableMonoBehavior, IColliderBase, IUpdateCollider, IRenderCollider
{
    /// <summary>
    /// 衝突情報コンポーネント。
    /// </summary>
    private BattleObjectCollider m_Collider;

    /// <summary>
    /// キャッシュしている衝突情報。
    /// </summary>
    private ColliderData[] m_ColliderDatas;

    /// <summary>
    /// 衝突情報コンポーネントを取得する。
    /// </summary>
    public BattleObjectCollider GetCollider()
    {
        return m_Collider;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_Collider == null)
        {
            m_Collider = GetComponent<BattleObjectCollider>();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 衝突情報を描画する。
    /// </summary>
    public abstract void OnRenderCollider();

    #endregion

    /// <summary>
    /// 衝突情報を更新する。
    /// </summary>
    public void UpdateCollider()
    {
        m_ColliderDatas = m_Collider.CreateColliderData();
    }

    /// <summary>
    /// 衝突情報を取得する。
    /// </summary>
    public ColliderData[] GetColliderData()
    {
        return m_ColliderDatas;
    }

    protected void DrawCollider()
    {
        var collisionMaterial = GetCollisionMaterial();
        if (collisionMaterial == null)
        {
            return;
        }

        if (GetColliderData() == null)
        {
            return;
        }

        collisionMaterial.SetPass(0);
        var colliderTransforms = m_Collider.ColliderTransforms;
        for (int i = 0; i < colliderTransforms.Length; i++)
        {
            var ct = colliderTransforms[i];
            var isCollide = m_ColliderDatas[i].IsCollide;

            var width = 1.0f / Screen.width * ct.ColliderWidth * 0.5f;

            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(isCollide ? Color.red : ct.ColliderColor);

            switch (ct.ColliderType)
            {
                case E_COLLIDER_SHAPE.RECT:
                    DrawRect(ct, width);
                    break;
                case E_COLLIDER_SHAPE.CIRCLE:
                    DrawCircle(ct, width);
                    break;
                case E_COLLIDER_SHAPE.ELLIPSE:
                    DrawEllipse(ct, width);
                    break;
                case E_COLLIDER_SHAPE.CAPSULE:
                    DrawCapsule(ct, width);
                    break;
                case E_COLLIDER_SHAPE.RAY:
                    break;
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    protected abstract Material GetCollisionMaterial();

    protected abstract Vector2 CalcViewportPos(Vector2 worldPos);

    /// <summary>
    /// 直線を描画する。
    /// </summary>
    private void DrawLine2D(Vector2 v0, Vector2 v1, float lineWidth)
    {
        Vector2 n = ((new Vector2(v1.y, v0.x)) - (new Vector2(v0.y, v1.x))).normalized * lineWidth;
        GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f);
        GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
        GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
        GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
    }

    /// <summary>
    /// 回転した座標を計算する。
    /// </summary>
    private Vector2 CalcRotatePos(Vector2 pos, float rad)
    {
        var nX = Mathf.Cos(rad) * pos.x - Mathf.Sin(rad) * pos.y;
        var nZ = Mathf.Sin(rad) * pos.x + Mathf.Cos(rad) * pos.y;
        return new Vector2(nX, nZ);
    }

    /// <summary>
    /// 矩形を描画する。
    /// </summary>
    private void DrawRect(ColliderTransform ct, float width)
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
            verts[i] = CalcRotatePos(verts[i], -rad) + pos;
            verts[i] = CalcViewportPos(verts[i]);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// 真円を描画する。
    /// </summary>
    private void DrawCircle(ColliderTransform ct, float width)
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
            verts[i] = CalcViewportPos(p);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// 楕円を描画する。
    /// </summary>
    private void DrawEllipse(ColliderTransform ct, float width)
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
            p = CalcRotatePos(p, -rad) + pos;
            verts[i] = CalcViewportPos(p);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }

    /// <summary>
    /// カプセルを描画する。
    /// </summary>
    private void DrawCapsule(ColliderTransform ct, float width)
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
            var p = CalcRotatePos(verts[i], -rad) + pos;
            verts[i] = CalcViewportPos(p);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            DrawLine2D(verts[i], verts[(i + 1) % verts.Length], width);
        }
    }
}

