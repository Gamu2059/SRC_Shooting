using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 衝突判定処理をまとめるクラス。
/// </summary>
public static class Collision
{
    private const float EPS = 0.00001f;

    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// 衝突点の計算を行うため低速。
    /// </summary>
    public static void CheckCollide(IColliderBase attack, IColliderBase target, Action<ColliderData, ColliderData, List<Vector2>> onCollide)
    {
        var attackDatas = attack.GetColliderData();
        var targetDatas = target.GetColliderData();

        foreach (var attackData in attackDatas)
        {
            foreach (var targetData in targetDatas)
            {
                List<Vector2> hitPosList;
                if (IsCollide(attackData, targetData, out hitPosList))
                {
                    onCollide.Invoke(attackData, targetData, hitPosList);
                }
            }
        }
    }

    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// 衝突点の計算を行うため低速。
    /// </summary>
    public static bool IsCollide(ColliderData collider1, ColliderData collider2, out List<Vector2> hitPosList)
    {
        hitPosList = null;

        if (collider1.Transform.IsDisable || collider2.Transform.IsDisable)
        {
            return false;
        }

        // 大雑把に衝突していそうかを事前に判定する
        if (collider1.OutSideRect == Rect.zero || collider2.OutSideRect == Rect.zero)
        {
            if (!IsCollideOutSide(collider1, collider2))
            {
                return false;
            }
        }
        else
        {
            if (!IsCollideOutSide(collider1.OutSideRect, collider2.OutSideRect))
            {
                return false;
            }
        }

        var colShape1 = collider1.Transform.ColliderShape;
        var colShape2 = collider2.Transform.ColliderShape;

        if (colShape1 == E_COLLIDER_SHAPE.LINE)
        {
            var l1 = CreateLine(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToLine(l1, l2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideLineToCircle(l1, c2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideLineToRect(l1, r2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideLineToCapsule(l1, cap2, out hitPosList);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.CIRCLE)
        {
            var c1 = CreateCircle(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideCircleToRect(c1, r2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToCircle(c1, c2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideCircleToCapsule(c1, cap2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToCircle(l2, c1, out hitPosList);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.RECT)
        {
            var r1 = CreateRect(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToRect(l2, r1, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToRect(c2, r1, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideRectToRect(r1, r2, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideRectToCapsule(r1, cap2, out hitPosList);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.CAPSULE)
        {
            var cap1 = CreateCapsule(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToCapsule(l2, cap1, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToCapsule(c2, cap1, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideRectToCapsule(r2, cap1, out hitPosList);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideCapsuleToCapsule(cap1, cap2, out hitPosList);
            }
        }

        return false;
    }

    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// 衝突点の計算を行わないため高速。
    /// </summary>
    public static void CheckCollideFast(IColliderBase attack, IColliderBase target, Action<ColliderData, ColliderData> onCollide)
    {
        var attackDatas = attack.GetColliderData();
        var targetDatas = target.GetColliderData();

        foreach (var attackData in attackDatas)
        {
            foreach (var targetData in targetDatas)
            {
                if (IsCollideFast(attackData, targetData))
                {
                    onCollide.Invoke(attackData, targetData);
                }
            }
        }
    }

    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// 衝突点の計算を行わないため高速。
    /// </summary>
    public static bool IsCollideFast(ColliderData collider1, ColliderData collider2)
    {
        if (collider1.Transform.IsDisable || collider2.Transform.IsDisable)
        {
            return false;
        }

        // 大雑把に衝突していそうかを事前に判定する
        if (collider1.OutSideRect == Rect.zero || collider2.OutSideRect == Rect.zero)
        {
            if (!IsCollideOutSide(collider1, collider2))
            {
                return false;
            }
        }
        else
        {
            if (!IsCollideOutSide(collider1.OutSideRect, collider2.OutSideRect))
            {
                return false;
            }
        }

        var colShape1 = collider1.Transform.ColliderShape;
        var colShape2 = collider2.Transform.ColliderShape;

        if (colShape1 == E_COLLIDER_SHAPE.LINE)
        {
            var l1 = CreateLine(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToLineFast(l1, l2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideLineToCircleFast(l1, c2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideLineToRectFast(l1, r2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideLineToCapsuleFast(l1, cap2);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.CIRCLE)
        {
            var c1 = CreateCircle(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideCircleToRectFast(c1, r2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToCircleFast(c1, c2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideCircleToCapsuleFast(c1, cap2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToCircleFast(l2, c1);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.RECT)
        {
            var r1 = CreateRect(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToRectFast(l2, r1);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToRectFast(c2, r1);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideRectToRectFast(r1, r2);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideRectToCapsuleFast(r1, cap2);
            }
        }

        if (colShape1 == E_COLLIDER_SHAPE.CAPSULE)
        {
            var cap1 = CreateCapsule(collider1);

            if (colShape2 == E_COLLIDER_SHAPE.LINE)
            {
                var l2 = CreateLine(collider2);
                return IsCollideLineToCapsuleFast(l2, cap1);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CIRCLE)
            {
                var c2 = CreateCircle(collider2);
                return IsCollideCircleToCapsuleFast(c2, cap1);
            }
            if (colShape2 == E_COLLIDER_SHAPE.RECT)
            {
                var r2 = CreateRect(collider2);
                return IsCollideRectToCapsuleFast(r2, cap1);
            }
            if (colShape2 == E_COLLIDER_SHAPE.CAPSULE)
            {
                var cap2 = CreateCapsule(collider2);
                return IsCollideCapsuleToCapsuleFast(cap1, cap2);
            }
        }

        return false;
    }


    private static Vector2 GetRotatePos(Vector2 p, float rad)
    {
        var x = Mathf.Cos(rad) * p.x - Mathf.Sin(rad) * p.y;
        var y = Mathf.Sin(rad) * p.x + Mathf.Cos(rad) * p.y;
        return new Vector2(x, y);
    }

    private static CollisionLine CreateLine(ColliderData collider)
    {
        var t = collider.Transform;
        Vector2 p0, p1;

        if (t.Direction == ColliderTransform.E_DIRECTION.HORIZONTAL)
        {
            p0 = new Vector2(-collider.Size.x / 2f, 0);
            p1 = new Vector2(collider.Size.x / 2f, 0);
        }
        else
        {
            p0 = new Vector2(0, -collider.Size.y / 2f);
            p1 = new Vector2(0, collider.Size.y / 2f);
        }

        var rad = collider.Angle * Mathf.Deg2Rad;
        p0 = GetRotatePos(p0, rad);
        p1 = GetRotatePos(p1, rad);

        var line = new CollisionLine();
        line.p = p0 + collider.CenterPos;
        line.v = p1 - p0;

        return line;
    }

    private static CollisionCircle CreateCircle(ColliderData collider)
    {
        var circle = new CollisionCircle();
        circle.p = collider.CenterPos;
        circle.r = Mathf.Min(collider.Size.x, collider.Size.y) / 2f;

        return circle;
    }

    private static CollisionRect CreateRect(ColliderData collider)
    {
        var rad = collider.Angle * Mathf.Deg2Rad;
        var s = collider.Size;
        var leftDown = GetRotatePos(-s / 2f, rad);
        var leftUp = GetRotatePos(new Vector2(-s.x, s.y) / 2f, rad);

        var rect = new CollisionRect();
        rect.p = collider.CenterPos;
        rect.leftDown = leftDown;
        rect.leftUp = leftUp;

        return rect;
    }

    private static CollisionCapsule CreateCapsule(ColliderData collider)
    {
        var t = collider.Transform;
        var line = CreateLine(collider);
        var n = line.v.normalized;
        float radius;
        if (t.Direction == ColliderTransform.E_DIRECTION.HORIZONTAL)
        {
            radius = collider.Size.y / 2;
            var p1 = line.p + n * radius;
            var p2 = line.p + n * (collider.Size.x - radius);
            line.p = p1;
            line.v = p2 - p1;
        }
        else
        {
            radius = collider.Size.x / 2;
            var p1 = line.p + n * radius;
            var p2 = line.p + n * (collider.Size.y - radius);
            line.p = p1;
            line.v = p2 - p1;
        }

        var capsule = new CollisionCapsule();
        capsule.l = line;
        capsule.r = radius;

        return capsule;
    }

    /// <summary>
    /// 指定した衝突判定の頂点の座標配列を取得する。
    /// </summary>
    private static Vector2[] GetCornerPosFromRect(ColliderData colliderData, float div = 2)
    {
        Vector2 size = colliderData.Size / div;
        Vector2[] cornerPos = new Vector2[4];
        cornerPos[0] = new Vector2(size.x, size.y);
        cornerPos[1] = new Vector2(size.x, -size.y);
        cornerPos[2] = new Vector2(-size.x, -size.y);
        cornerPos[3] = new Vector2(-size.x, size.y);

        float cos = Mathf.Cos(colliderData.Angle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(colliderData.Angle * Mathf.Deg2Rad);

        for (int i = 0; i < cornerPos.Length; i++)
        {
            float x = cornerPos[i].x;
            float y = cornerPos[i].y;
            cornerPos[i].x = x * cos - y * sin;
            cornerPos[i].y = x * sin + y * cos;

            cornerPos[i] += colliderData.CenterPos;
        }

        return cornerPos;
    }

    /// <summary>
    /// 衝突判定の外側の矩形を取得する。
    /// </summary>
    public static Rect GetOutSideCorner(ColliderData colliderData, float div = 2)
    {
        var corners = GetCornerPosFromRect(colliderData, div);
        if (corners == null || corners.Length < 1)
        {
            return Rect.zero;
        }

        var rect = new Rect(corners[0], corners[0]);
        for (int i = 1; i < corners.Length; i++)
        {
            rect.x = Mathf.Min(rect.x, corners[i].x);
            rect.y = Mathf.Min(rect.y, corners[i].y);
            rect.width = Mathf.Max(rect.width, corners[i].x);
            rect.height = Mathf.Max(rect.height, corners[i].y);
        }

        return rect;
    }

    /// <summary>
    /// 外側の矩形同士の衝突判定を取る。
    /// </summary>
    private static bool IsCollideOutSide(ColliderData collider1, ColliderData collider2)
    {
        var o1 = GetOutSideCorner(collider1);
        var o2 = GetOutSideCorner(collider2);

        return IsCollideOutSide(o1, o2);
    }

    private static bool IsCollideOutSide(Rect o1, Rect o2)
    {
        var horizontal = (o2.x < o1.width) && (o1.x < o2.width);
        var vertical = (o2.y < o1.height) && (o1.y < o2.height);
        return horizontal && vertical;
    }

    public static float Dot(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    public static float Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }

    /// <summary>
    /// 点が円の中に含まれているかを判定する。
    /// </summary>
    private static bool IsInCircle(Vector2 point, Vector2 circleCenter, float circleRadius, float threshold = EPS)
    {
        var p = point - circleCenter;
        var pr = p.sqrMagnitude;
        var cr = circleRadius * circleRadius;
        return pr <= cr + threshold;
    }

    /// <summary>
    /// 点が円の中に含まれているかを判定する。
    /// </summary>
    private static bool IsInCircle(Vector2 point, CollisionCircle circle, float threshold = EPS)
    {
        return IsInCircle(point, circle.p, circle.r, threshold);
    }

    /// <summary>
    /// 点が矩形の中に含まれているかを判定する。
    /// </summary>
    private static bool IsInRect(Vector2 point, Vector2 rectCenter, Vector2 rectLeftDown, Vector2 rectLeftUp, float threshold = EPS)
    {
        var verts = new Vector2[]
        {
            rectLeftDown + rectCenter,
            rectLeftUp + rectCenter,
            -rectLeftDown + rectCenter,
            -rectLeftUp + rectCenter,
        };

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[(i + 1) % verts.Length] - verts[i];
            var vp = point - verts[i];
            if (Cross(v, vp) - threshold > 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 点が矩形の中に含まれているかを判定する。
    /// </summary>
    private static bool IsInRect(Vector2 point, CollisionRect rect, float threshold = EPS)
    {
        return IsInRect(point, rect.p, rect.leftDown, rect.leftUp, threshold);
    }

    #region Slow Collide Check

    /// <summary>
    /// 線分同士の衝突を行う。
    /// </summary>
    public static bool IsCollideLineToLine(CollisionLine l1, CollisionLine l2, out List<Vector2> result)
    {
        result = null;

        var v = l2.p - l1.p;
        var v1 = l1.v;
        var v2 = l2.v;
        var crs_v1_v2 = Cross(v1, v2);
        if (crs_v1_v2 == 0.0f)
        {
            // 平行状態
            return false;
        }

        var crs_v_v1 = Cross(v, v1);
        var crs_v_v2 = Cross(v, v2);

        float t1 = crs_v_v2 / crs_v1_v2;
        float t2 = crs_v_v1 / crs_v1_v2;

        if (t1 + EPS < 0 || t1 - EPS > 1 || t2 + EPS < 0 || t2 - EPS > 1)
        {
            // 交差していない
            return false;
        }

        result = new List<Vector2>();
        result.Add(l1.p + l1.v * t1);
        return true;
    }

    /// <summary>
    /// 線と円の衝突を行う。
    /// </summary>
    private static bool IsCollideLineToCircle(CollisionLine l, CollisionCircle c, out List<Vector2> result)
    {
        result = null;

        if (c.r < 0.0f && l.v.x == 0.0f && l.v.y == 0.0f)
        {
            return false;
        }

        var a = l.p - c.p;
        var v = l.v.normalized;

        // 係数tを算出
        var dotAV = Dot(a, v);
        var dotAA = Dot(a, a);
        var s = dotAV * dotAV - dotAA + c.r * c.r;

        if (s < 0.0f)
        {
            // 衝突していない
            return false;
        }

        if (s < EPS)
        {
            // 誤差修正
            s = 0.0f;
        }

        var sq = Mathf.Sqrt(s);
        var len = l.v.magnitude;
        var t1 = (-dotAV - sq) / len;
        var t2 = (-dotAV + sq) / len;

        var isCollide1 = 0f <= t1 && t1 <= 1f;
        var isCollide2 = 0f <= t2 && t2 <= 1f;

        if (!isCollide1 && !isCollide2)
        {
            if (IsInCircle(l.p, c) && IsInCircle(l.p + l.v, c))
            {
                result = new List<Vector2>();
                result.Add(l.p + l.v / 2f);
                return true;
            }

            return false;
        }

        result = new List<Vector2>();
        if (0f <= t1 && t1 <= 1f)
        {
            result.Add(t1 * len * v + l.p);
        }
        if (0f <= t2 && t2 <= 1f)
        {
            result.Add(t2 * len * v + l.p);
        }

        return true;
    }

    /// <summary>
    /// 線と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideLineToRect(CollisionLine l, CollisionRect r, out List<Vector2> result)
    {
        result = null;

        var verts = new Vector2[] {
            r.leftDown + r.p,
            r.leftUp + r.p,
            -r.leftDown + r.p,
            -r.leftUp + r.p,
        };

        var l2 = new CollisionLine();
        var isCollide = false;
        result = new List<Vector2>();

        for (int i = 0; i < verts.Length; i++)
        {
            l2.p = verts[i];
            l2.v = verts[(i + 1) % verts.Length] - verts[i];
            List<Vector2> lineResult;

            if (IsCollideLineToLine(l, l2, out lineResult))
            {
                isCollide = true;
                result.AddRange(lineResult);
            }
        }

        if (!isCollide)
        {
            if (IsInRect(l.p, r) && IsInRect(l.p + l.v, r))
            {
                result.Add(l.p + l.v / 2f);
                return true;
            }

            return false;
        }

        result = result.OrderBy(p => (p - l.p).sqrMagnitude).ToList();

        return true;
    }

    /// <summary>
    /// 線とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideLineToCapsule(CollisionLine l, CollisionCapsule c, out List<Vector2> result)
    {
        result = null;

        var beginCircle = new CollisionCircle();
        beginCircle.p = c.l.p;
        beginCircle.r = c.r;

        var endCircle = new CollisionCircle();
        endCircle.p = c.l.p + c.l.v;
        endCircle.r = c.r;

        var n = new Vector2(-c.l.v.y, c.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = c.l.p + c.l.v / 2f;
        rect.leftDown = n * c.r - c.l.v / 2f;
        rect.leftUp = n * c.r + c.l.v / 2f;

        List<Vector2> beginCircleResult;
        List<Vector2> endCircleResult;
        List<Vector2> rectResult;

        var isCollide = false;
        isCollide |= IsCollideLineToCircle(l, beginCircle, out beginCircleResult);
        isCollide |= IsCollideLineToCircle(l, endCircle, out endCircleResult);
        isCollide |= IsCollideLineToRect(l, rect, out rectResult);

        if (!isCollide)
        {
            return false;
        }

        var pList = new List<Vector2>();
        if (beginCircleResult != null)
        {
            pList.AddRange(beginCircleResult);
        }
        if (endCircleResult != null)
        {
            pList.AddRange(endCircleResult);
        }
        if (rectResult != null)
        {
            pList.AddRange(rectResult);
        }

        var list = pList.OrderBy(p => (p - l.p).sqrMagnitude);
        var num = list.Count();

        var end = l.p + l.v;
        var isInLineBegin = IsInCircle(l.p, beginCircle) || IsInCircle(l.p, endCircle) || IsInRect(l.p, rect);
        var isInLineEnd = IsInCircle(end, beginCircle) || IsInCircle(end, endCircle) || IsInRect(end, rect);

        result = new List<Vector2>();

        if (isInLineBegin && isInLineEnd)
        {
            result.Add(l.p + l.v / 2f);
        }
        else
        {
            if (!isInLineBegin)
            {
                result.Add(list.First());
            }
            if (!isInLineEnd)
            {
                result.Add(list.Last());
            }
        }

        return true;
    }

    /// <summary>
    /// 円と円の衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToCircle(CollisionCircle c1, CollisionCircle c2, out List<Vector2> result)
    {
        result = null;

        var r = c1.r + c2.r;
        var d = c2.p - c1.p;
        var dSqrMag = d.sqrMagnitude;
        if (dSqrMag > r * r)
        {
            return false;
        }

        result = new List<Vector2>();
        var dMag = d.magnitude;
        var large = c1.r > c2.r ? c1 : c2;
        var small = c1.r < c2.r ? c1 : c2;
        if ((dMag + small.r) * (dMag + small.r) > large.r * large.r)
        {
            var rc = (dSqrMag + c1.r * c1.r - c2.r * c2.r) / (2 * dMag);
            var rs = Mathf.Sqrt(c1.r * c1.r - rc * rc);
            var dn = d.normalized;
            var n = new Vector2(-d.y, d.x).normalized;
            result.Add(c1.p + rc * dn + rs * n);
            result.Add(c1.p + rc * dn - rs * n);
        }
        else
        {
            result.Add(small.p);
        }

        return true;
    }

    /// <summary>
    /// 円と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToRect(CollisionCircle c, CollisionRect r, out List<Vector2> result)
    {
        result = null;

        var verts = new Vector2[] {
            r.leftDown + r.p,
            r.leftUp + r.p,
            -r.leftDown + r.p,
            -r.leftUp + r.p,
        };

        var l = new CollisionLine();
        var isCollide = false;
        result = new List<Vector2>();

        for (int i = 0; i < verts.Length; i++)
        {
            l.p = verts[i];
            l.v = verts[(i + 1) % verts.Length] - verts[i];
            List<Vector2> lineResult;

            if (IsCollideLineToCircle(l, c, out lineResult))
            {
                if (IsInCircle(l.p, c) && IsInCircle(l.p + l.v, c))
                {
                    continue;
                }

                isCollide = true;
                result.AddRange(lineResult);
            }
        }

        if (!isCollide)
        {
            var isInRect = IsInRect(c.p, r);
            var isInCircle = IsInCircle(r.p, c);
            var rD = (r.leftUp - r.leftDown).sqrMagnitude;
            var cD = c.r * c.r * 4;
            if (isInRect && rD >= cD)
            {
                // 円が矩形に内包されている場合
                result.Add(c.p);
                return true;
            }
            if (isInCircle && rD <= cD)
            {
                // 矩形が円に内包されている場合
                result.Add(r.p);
                return true;
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// 円とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToCapsule(CollisionCircle c, CollisionCapsule cap, out List<Vector2> result)
    {
        result = null;

        var beginLength = (cap.l.p - c.p).magnitude + cap.r;
        var endLength = (cap.l.p + cap.l.v - c.p).magnitude + cap.r;
        if (beginLength < c.r && endLength < c.r)
        {
            // カプセルが円に内包されている場合
            result = new List<Vector2>();
            result.Add(cap.l.p + cap.l.v / 2f);
            return true;
        }

        var beginCircle = new CollisionCircle();
        beginCircle.p = cap.l.p;
        beginCircle.r = cap.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap.l.p + cap.l.v;
        endCircle.r = cap.r;

        var n = new Vector2(-cap.l.v.y, cap.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap.l.p + cap.l.v / 2f;
        rect.leftDown = n * cap.r - cap.l.v / 2f;
        rect.leftUp = n * cap.r + cap.l.v / 2f;

        List<Vector2> beginCircleResult;
        List<Vector2> endCircleResult;
        List<Vector2> rectResult;

        var isCollide = false;
        isCollide |= IsCollideCircleToCircle(c, beginCircle, out beginCircleResult);
        isCollide |= IsCollideCircleToCircle(c, endCircle, out endCircleResult);
        isCollide |= IsCollideCircleToRect(c, rect, out rectResult);

        if (!isCollide)
        {
            return false;
        }

        var pList = new List<Vector2>();
        if (beginCircleResult != null)
        {
            beginCircleResult.RemoveAll(p => IsInCircle(p, endCircle) || IsInRect(p, rect));
            pList.AddRange(beginCircleResult);
        }
        if (endCircleResult != null)
        {
            endCircleResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInRect(p, rect));
            pList.AddRange(endCircleResult);
        }
        if (rectResult != null)
        {
            rectResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInCircle(p, endCircle));
            pList.AddRange(rectResult);
        }

        result = new List<Vector2>();
        if (pList.Count < 1)
        {
            // 円がカプセルに内包されている場合
            result.Add(c.p);
            return true;
        }

        result.AddRange(pList);
        return true;
    }

    /// <summary>
    /// 矩形と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideRectToRect(CollisionRect r1, CollisionRect r2, out List<Vector2> result, bool isFirstLoop = true)
    {
        result = null;

        var verts = new Vector2[] {
            r1.leftDown + r1.p,
            r1.leftUp + r1.p,
            -r1.leftDown + r1.p,
            -r1.leftUp + r1.p,
        };

        var l = new CollisionLine();
        var isCollide = false;
        result = new List<Vector2>();

        for (int i = 0; i < verts.Length; i++)
        {
            l.p = verts[i];
            l.v = verts[(i + 1) % verts.Length] - verts[i];
            List<Vector2> lineResult;

            if (IsCollideLineToRect(l, r2, out lineResult))
            {
                isCollide = true;
                if (IsInRect(l.p, r2) && IsInRect(l.p + l.v, r2))
                {
                    continue;
                }
                result.AddRange(lineResult);
            }
        }

        if (!isCollide)
        {
            if (!isFirstLoop)
            {
                return false;
            }
            return IsCollideRectToRect(r2, r1, out result, false);
        }

        if (result.Count < 1)
        {
            result.Add(r1.p);
        }

        return true;
    }

    /// <summary>
    /// 矩形とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideRectToCapsule(CollisionRect r, CollisionCapsule cap, out List<Vector2> result)
    {
        result = null;

        var beginCircle = new CollisionCircle();
        beginCircle.p = cap.l.p;
        beginCircle.r = cap.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap.l.p + cap.l.v;
        endCircle.r = cap.r;

        var n = new Vector2(-cap.l.v.y, cap.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap.l.p + cap.l.v / 2f;
        rect.leftDown = n * cap.r - cap.l.v / 2f;
        rect.leftUp = n * cap.r + cap.l.v / 2f;

        List<Vector2> beginCircleResult;
        List<Vector2> endCircleResult;
        List<Vector2> rectResult;

        var isCollide = false;
        isCollide |= IsCollideCircleToRect(beginCircle, r, out beginCircleResult);
        isCollide |= IsCollideCircleToRect(endCircle, r, out endCircleResult);
        isCollide |= IsCollideRectToRect(rect, r, out rectResult);

        if (!isCollide)
        {
            return false;
        }

        var pList = new List<Vector2>();
        if (beginCircleResult != null)
        {
            beginCircleResult.RemoveAll(p => IsInCircle(p, endCircle) || IsInRect(p, rect));
            pList.AddRange(beginCircleResult);
        }
        if (endCircleResult != null)
        {
            endCircleResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInRect(p, rect));
            pList.AddRange(endCircleResult);
        }
        if (rectResult != null)
        {
            rectResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInCircle(p, endCircle));
            pList.AddRange(rectResult);
        }

        result = new List<Vector2>();
        if (pList.Count < 1)
        {
            if (IsInRect(cap.l.p, r) && IsInRect(cap.l.p + cap.l.v, r))
            {
                // カプセルが矩形に内包されている場合
                result = new List<Vector2>();
                result.Add(cap.l.p + cap.l.v / 2f);
                return true;
            }

            // 矩形がカプセルに内包されている場合
            result.Add(r.p);
            return true;
        }

        result.AddRange(pList);
        return true;
    }

    /// <summary>
    /// カプセルとカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideCapsuleToCapsule(CollisionCapsule cap1, CollisionCapsule cap2, out List<Vector2> result)
    {
        result = null;

        var beginCircle = new CollisionCircle();
        beginCircle.p = cap1.l.p;
        beginCircle.r = cap1.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap1.l.p + cap1.l.v;
        endCircle.r = cap1.r;

        var n = new Vector2(-cap1.l.v.y, cap1.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap1.l.p + cap1.l.v / 2f;
        rect.leftDown = n * cap1.r - cap1.l.v / 2f;
        rect.leftUp = n * cap1.r + cap1.l.v / 2f;

        List<Vector2> beginCircleResult;
        List<Vector2> endCircleResult;
        List<Vector2> rectResult;

        var isCollide = false;
        isCollide |= IsCollideCircleToCapsule(beginCircle, cap2, out beginCircleResult);
        isCollide |= IsCollideCircleToCapsule(endCircle, cap2, out endCircleResult);
        isCollide |= IsCollideRectToCapsule(rect, cap2, out rectResult);

        if (!isCollide)
        {
            return false;
        }

        var pList = new List<Vector2>();
        if (beginCircleResult != null)
        {
            beginCircleResult.RemoveAll(p => IsInCircle(p, endCircle) || IsInRect(p, rect));
            pList.AddRange(beginCircleResult);
        }
        if (endCircleResult != null)
        {
            endCircleResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInRect(p, rect));
            pList.AddRange(endCircleResult);
        }
        if (rectResult != null)
        {
            rectResult.RemoveAll(p => IsInCircle(p, beginCircle) || IsInCircle(p, endCircle));
            pList.AddRange(rectResult);
        }

        result = new List<Vector2>();
        if (pList.Count < 1)
        {
            if (cap1.r <= cap2.r)
            {
                // カプセル1がカプセル2に内包されている場合
                result.Add(cap1.l.p + cap1.l.v / 2f);
            }
            else
            {
                // カプセル2がカプセル1に内包されている場合
                result.Add(cap2.l.p + cap2.l.v / 2f);
            }
            return true;
        }

        result.AddRange(pList);
        return true;
    }

    #endregion

    #region Fast Collide Check

    /// <summary>
    /// 線分同士の衝突を行う。
    /// </summary>
    public static bool IsCollideLineToLineFast(CollisionLine l1, CollisionLine l2)
    {
        var v = l1.v;
        var v1 = l2.p - l1.p;
        var v2 = l2.p + l2.v - l1.p;

        var crs_v_v1 = Cross(v, v1);
        var crs_v_v2 = Cross(v, v2);

        return crs_v_v1 * crs_v_v2 <= 0;
    }
    
    /// <summary>
    /// 線と円の衝突を行う。
    /// </summary>
    private static bool IsCollideLineToCircleFast(CollisionLine l, CollisionCircle c)
    {
        if (c.r < 0.0f && l.v.x == 0.0f && l.v.y == 0.0f)
        {
            return false;
        }

        var s = l.v;
        var a = c.p - l.p;
        var b = c.p - (l.p + l.v);

        var d = Cross(s, a) / s.magnitude;

        if (d <= c.r)
        {
            if (Dot(a,s) * Dot(b,s) <= 0)
            {
                return true;
            }

            var sqrR = c.r * c.r;
            if (sqrR > a.sqrMagnitude || sqrR > b.sqrMagnitude)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 線と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideLineToRectFast(CollisionLine l, CollisionRect r)
    {
        var verts = new Vector2[] {
            r.leftDown + r.p,
            r.leftUp + r.p,
            -r.leftDown + r.p,
            -r.leftUp + r.p,
        };

        var l2 = new CollisionLine();

        for (int i = 0; i < verts.Length; i++)
        {
            l2.p = verts[i];
            l2.v = verts[(i + 1) % verts.Length] - verts[i];

            if (IsCollideLineToLineFast(l, l2))
            {
                return true;
            }
        }

        return IsInRect(l.p, r) && IsInRect(l.p + l.v, r);
    }

    /// <summary>
    /// 線とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideLineToCapsuleFast(CollisionLine l, CollisionCapsule c)
    {
        var beginCircle = new CollisionCircle();
        beginCircle.p = c.l.p;
        beginCircle.r = c.r;

        var endCircle = new CollisionCircle();
        endCircle.p = c.l.p + c.l.v;
        endCircle.r = c.r;

        var n = new Vector2(-c.l.v.y, c.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = c.l.p + c.l.v / 2f;
        rect.leftDown = n * c.r - c.l.v / 2f;
        rect.leftUp = n * c.r + c.l.v / 2f;

        if (IsCollideLineToCircleFast(l, beginCircle))
        {
            return true;
        }

        if (IsCollideLineToCircleFast(l, endCircle))
        {
            return true;
        }

        return IsCollideLineToRectFast(l, rect);
    }

    /// <summary>
    /// 円と円の衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToCircleFast(CollisionCircle c1, CollisionCircle c2)
    {
        var r = c1.r + c2.r;
        var d = c2.p - c1.p;
        var dSqrMag = d.sqrMagnitude;
        return dSqrMag <= r * r;
    }

    /// <summary>
    /// 円と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToRectFast(CollisionCircle c, CollisionRect r)
    {
        var verts = new Vector2[] {
            r.leftDown + r.p,
            r.leftUp + r.p,
            -r.leftDown + r.p,
            -r.leftUp + r.p,
        };

        var l = new CollisionLine();

        for (int i = 0; i < verts.Length; i++)
        {
            l.p = verts[i];
            l.v = verts[(i + 1) % verts.Length] - verts[i];

            if (IsCollideLineToCircleFast(l, c))
            {
                if (IsInCircle(l.p, c) && IsInCircle(l.p + l.v, c))
                {
                    continue;
                }

                return true;
            }
        }

        var isInRect = IsInRect(c.p, r);
        var isInCircle = IsInCircle(r.p, c);
        var rD = (r.leftUp - r.leftDown).sqrMagnitude;
        var cD = c.r * c.r * 4;

        if (isInRect && rD >= cD)
        {
            // 円が矩形に内包されている場合
            return true;
        }

        if (isInCircle && rD <= cD)
        {
            // 矩形が円に内包されている場合
            return true;
        }

        return false;
    }

    /// <summary>
    /// 円とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideCircleToCapsuleFast(CollisionCircle c, CollisionCapsule cap)
    {
        var beginLength = (cap.l.p - c.p).magnitude + cap.r;
        var endLength = (cap.l.p + cap.l.v - c.p).magnitude + cap.r;
        if (beginLength < c.r && endLength < c.r)
        {
            // カプセルが円に内包されている場合
            return true;
        }

        var beginCircle = new CollisionCircle();
        beginCircle.p = cap.l.p;
        beginCircle.r = cap.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap.l.p + cap.l.v;
        endCircle.r = cap.r;

        var n = new Vector2(-cap.l.v.y, cap.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap.l.p + cap.l.v / 2f;
        rect.leftDown = n * cap.r - cap.l.v / 2f;
        rect.leftUp = n * cap.r + cap.l.v / 2f;

        if (IsCollideCircleToCircleFast(c, beginCircle))
        {
            return true;
        }

        if (IsCollideCircleToCircleFast(c, endCircle))
        {
            return true;
        }

        return IsCollideCircleToRectFast(c, rect);
    }

    /// <summary>
    /// 矩形と矩形の衝突を行う。
    /// </summary>
    private static bool IsCollideRectToRectFast(CollisionRect r1, CollisionRect r2, bool isFirstLoop = true)
    {
        var verts = new Vector2[] {
            r1.leftDown + r1.p,
            r1.leftUp + r1.p,
            -r1.leftDown + r1.p,
            -r1.leftUp + r1.p,
        };

        var l = new CollisionLine();

        for (int i = 0; i < verts.Length; i++)
        {
            l.p = verts[i];
            l.v = verts[(i + 1) % verts.Length] - verts[i];

            if (IsCollideLineToRectFast(l, r2))
            {
                return true;
            }
        }

        if (!isFirstLoop)
        {
            return false;
        }
        return IsCollideRectToRectFast(r2, r1, false);
    }

    /// <summary>
    /// 矩形とカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideRectToCapsuleFast(CollisionRect r, CollisionCapsule cap)
    {
        var beginCircle = new CollisionCircle();
        beginCircle.p = cap.l.p;
        beginCircle.r = cap.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap.l.p + cap.l.v;
        endCircle.r = cap.r;

        var n = new Vector2(-cap.l.v.y, cap.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap.l.p + cap.l.v / 2f;
        rect.leftDown = n * cap.r - cap.l.v / 2f;
        rect.leftUp = n * cap.r + cap.l.v / 2f;

        if (IsCollideCircleToRectFast(beginCircle, r))
        {
            return true;
        }

        if (IsCollideCircleToRectFast(endCircle, r))
        {
            return true;
        }

        return IsCollideRectToRectFast(rect, r);
    }

    /// <summary>
    /// カプセルとカプセルの衝突を行う。
    /// </summary>
    private static bool IsCollideCapsuleToCapsuleFast(CollisionCapsule cap1, CollisionCapsule cap2)
    {
        var beginCircle = new CollisionCircle();
        beginCircle.p = cap1.l.p;
        beginCircle.r = cap1.r;

        var endCircle = new CollisionCircle();
        endCircle.p = cap1.l.p + cap1.l.v;
        endCircle.r = cap1.r;

        var n = new Vector2(-cap1.l.v.y, cap1.l.v.x).normalized;
        var rect = new CollisionRect();
        rect.p = cap1.l.p + cap1.l.v / 2f;
        rect.leftDown = n * cap1.r - cap1.l.v / 2f;
        rect.leftUp = n * cap1.r + cap1.l.v / 2f;

        if (IsCollideCircleToCapsuleFast(beginCircle, cap2))
        {
            return true;
        }

        if (IsCollideCircleToCapsuleFast(endCircle, cap2))
        {
            return true;
        }

        return IsCollideRectToCapsuleFast(rect, cap2);
    }

    #endregion
}
