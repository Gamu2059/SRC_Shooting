using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 衝突処理を管理する基底クラス。
/// </summary>
public abstract class BattleCollisionManagerBase<T> : Singleton<T> where T : BattleCollisionManagerBase<T>, new()
{
    private Material m_CollisionMaterial;
    private MaterialPropertyBlock m_PropertyBlock;
    private int m_ColorId;
    private LinkedList<Mesh> m_Meshes;

    public BattleCollisionManagerBase()
    {
        m_CollisionMaterial = null;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PropertyBlock = new MaterialPropertyBlock();
        m_ColorId = Shader.PropertyToID("_Color");
        m_Meshes = new LinkedList<Mesh>();
    }

    public override void OnFinalize()
    {
        DestroyDrawingColliderMeshes();
        m_Meshes = null;
        base.OnFinalize();
    }

    public abstract void CheckCollision();

    public void DestroyDrawingColliderMeshes()
    {
        if (m_Meshes != null)
        {
            foreach (var m in m_Meshes)
            {
                m.Clear(false);
                GameObject.Destroy(m);
            }
            m_Meshes.Clear();
        }
    }

    public abstract void DrawCollider();

    protected void DrawCollider(BattleObjectBase battleObject)
    {
        if (m_CollisionMaterial == null)
        {
            return;
        }

        if (battleObject == null)
        {
            return;
        }

        var collider = battleObject.GetCollider();
        var colliderDatas = battleObject.GetColliderData();
        if (collider == null || colliderDatas == null)
        {
            return;
        }

        var transforms = collider.ColliderTransforms;
        for (int i = 0; i < transforms.Length; i++)
        {
            var ct = transforms[i];
            var cData = colliderDatas[i];
            var color = ct.ColliderColor;
            var width = ct.ColliderWidth;

            if (cData.IsCollide)
            {
                color = Color.red;
                width *= 2;
            }

            width /= Screen.width * 2f;
            m_PropertyBlock.SetColor(m_ColorId, color);

            switch (ct.ColliderShape)
            {
                case E_COLLIDER_SHAPE.RECT:
                    DrawRect(ct, width);
                    break;
                case E_COLLIDER_SHAPE.CIRCLE:
                    DrawCircle(ct, width);
                    break;
                case E_COLLIDER_SHAPE.CAPSULE:
                    DrawCapsule(ct, width);
                    break;
                case E_COLLIDER_SHAPE.LINE:
                    DrawRay(ct, width);
                    break;
            }

            //if (BattleManager.Instance.m_IsDrawOutSideColliderArea)
            //{
            //    DrawOutSideRect(cData);
            //}
        }
    }

    protected abstract Vector2 CalcViewportPos(Vector2 worldPos);

    private void DrawMesh(Vector2[] verts, float lineWidth, Transform t, bool isLoop = true)
    {
        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var createNum = isLoop ? verts.Length : verts.Length - 1;

        for (int i = 0; i < createNum; i++)
        {
            var currentIndex = i;
            var nextIndex = (i + 1) % verts.Length;

            var v0 = verts[currentIndex];
            var v1 = verts[nextIndex];
            var n = ((new Vector2(v1.y, v0.x)) - (new Vector2(v0.y, v1.x))).normalized * lineWidth;

            vertices.Add(new Vector3(v0.x - n.x, 0, v0.y - n.y));
            vertices.Add(new Vector3(v0.x + n.x, 0, v0.y + n.y));
            vertices.Add(new Vector3(v1.x - n.x, 0, v1.y - n.y));
            vertices.Add(new Vector3(v1.x + n.x, 0, v1.y + n.y));

            var t0 = (0 + currentIndex * 4);
            var t1 = (1 + currentIndex * 4);
            var t2 = (2 + currentIndex * 4);
            var t3 = (3 + currentIndex * 4);

            triangles.Add(t2);
            triangles.Add(t1);
            triangles.Add(t0);
            triangles.Add(t1);
            triangles.Add(t2);
            triangles.Add(t3);
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        var angle = Quaternion.Euler(0, t.eulerAngles.y, 0);
        Graphics.DrawMesh(mesh, t.position, angle, m_CollisionMaterial, 0, null, 0, m_PropertyBlock);
        m_Meshes.AddFirst(mesh);
    }

    private void DrawOutSideRect(ColliderData cData)
    {
        var rect = Collision.GetOutSideCorner(cData, 1);
        var verts = new Vector2[]
        {
                new Vector2(rect.x, rect.y),
                new Vector2(rect.x, rect.height),
                new Vector2(rect.width, rect.height),
                new Vector2(rect.width, rect.y),
        };
        for (int x = 0; x < verts.Length; x++)
        {
            verts[x] = CalcViewportPos(verts[x] - cData.CenterPos);
        }

        var lineWidth = cData.Transform.ColliderWidth / (Screen.width * 2);
        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var createNum = verts.Length;

        for (int i = 0; i < createNum; i++)
        {
            var currentIndex = i;
            var nextIndex = (i + 1) % verts.Length;

            var v0 = verts[currentIndex];
            var v1 = verts[nextIndex];
            var n = ((new Vector2(v1.y, v0.x)) - (new Vector2(v0.y, v1.x))).normalized * lineWidth;

            vertices.Add(new Vector3(v0.x - n.x, 0, v0.y - n.y));
            vertices.Add(new Vector3(v0.x + n.x, 0, v0.y + n.y));
            vertices.Add(new Vector3(v1.x - n.x, 0, v1.y - n.y));
            vertices.Add(new Vector3(v1.x + n.x, 0, v1.y + n.y));

            var t0 = (0 + currentIndex * 4);
            var t1 = (1 + currentIndex * 4);
            var t2 = (2 + currentIndex * 4);
            var t3 = (3 + currentIndex * 4);

            triangles.Add(t2);
            triangles.Add(t1);
            triangles.Add(t0);
            triangles.Add(t1);
            triangles.Add(t2);
            triangles.Add(t3);
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        var pos = cData.Transform.Transform.position;
        Graphics.DrawMesh(mesh, pos, Quaternion.Euler(0, 0, 0), m_CollisionMaterial, 0, null, 0, m_PropertyBlock);
    }

    /// <summary>
    /// 矩形を描画する。
    /// </summary>
    private void DrawRect(ColliderTransform ct, float width)
    {
        var t = ct.Transform;
        var scl = t.lossyScale;

        var verts = new Vector2[] {
            new Vector2(-scl.x, -scl.z),
            new Vector2(-scl.x, scl.z),
            new Vector2(scl.x, scl.z),
            new Vector2(scl.x, -scl.z),
        };

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = CalcViewportPos(verts[i]);
        }

        DrawMesh(verts, width, t);
    }

    /// <summary>
    /// 真円を描画する。
    /// </summary>
    private void DrawCircle(ColliderTransform ct, float width)
    {
        var t = ct.Transform;
        var scl = Mathf.Min(t.lossyScale.x, t.lossyScale.z);

        var verts = new Vector2[16];
        var unitRad = Mathf.PI * 2 / verts.Length;
        for (int i = 0; i < verts.Length; i++)
        {
            var rad = unitRad * i;
            var p = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * scl;
            verts[i] = CalcViewportPos(p);
        }

        DrawMesh(verts, width, t);
    }

    /// <summary>
    /// カプセルを描画する。
    /// </summary>
    private void DrawCapsule(ColliderTransform ct, float width)
    {
        var t = ct.Transform;
        var scl = t.lossyScale.ToVector2XZ();

        var pointNum = 16;
        var semicirclePointNum = pointNum / 2 + 1;
        var unitRad = Mathf.PI * 2 / pointNum;

        var vertsA = new List<Vector2>(semicirclePointNum * 2);
        var vertsB = new List<Vector2>(semicirclePointNum);

        float radius, halfLength;
        if (ct.Direction == ColliderTransform.E_DIRECTION.HORIZONTAL)
        {
            radius = scl.y;
            halfLength = scl.x - radius;

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
            radius = scl.x;
            halfLength = scl.y - radius;

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
            verts[i] = CalcViewportPos(verts[i]);
        }

        DrawMesh(verts, width, t);
    }

    /// <summary>
    /// 直線を描画する。
    /// </summary>
    private void DrawRay(ColliderTransform ct, float width)
    {
        var t = ct.Transform;
        var scl = t.lossyScale;
        Vector2[] verts;

        if (ct.Direction == ColliderTransform.E_DIRECTION.HORIZONTAL)
        {
            verts = new Vector2[] {
                new Vector2(-scl.x, 0),
                new Vector2(scl.x, 0),
            };
        }
        else
        {
            verts = new Vector2[] {
                new Vector2(0, -scl.z),
                new Vector2(0, scl.z),
            };
        }

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = CalcViewportPos(verts[i]);
        }

        DrawMesh(verts, width, t);
    }
}
