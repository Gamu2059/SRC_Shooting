using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突判定処理をまとめるクラス。
/// </summary>
public static class Collision
{
    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// </summary>
    public static void CheckCollide(IColliderBase attack, IColliderBase target, Action<ColliderData, ColliderData> onCollideAction)
    {
        var attackDatas = attack.GetColliderData();
        var targetDatas = target.GetColliderData();

        foreach (var attackData in attackDatas)
        {
            foreach (var targetData in targetDatas)
            {
                //if (attackData.CollideName == "Command Player Bullet" && targetData.CollideName == "Command Wall")
                //{
                //    Debug.Log("===========");
                //    Debug.Log(attackData.CollideName + "," + attackData.CenterPos + "," + attackData.Size);
                //    Debug.Log(targetData.CollideName + "," + targetData.CenterPos + "," + targetData.Size);
                //    Debug.Log("Dist : " + (targetData.CenterPos - attackData.CenterPos).magnitude);
                //}
                if (IsCollide(attackData, targetData))
                {
                    EventUtility.SafeInvokeAction(onCollideAction, attackData, targetData);
                }
            }
        }
    }

    /// <summary>
    /// 二つの衝突情報が、互いに衝突しているかを判定する。
    /// </summary>
    public static bool IsCollide(ColliderData collider1, ColliderData collider2)
    {
        if (collider1.ColliderType == E_COLLIDER_SHAPE.RECT && collider2.ColliderType == E_COLLIDER_SHAPE.RECT)
        {
            //Debug.Log( 11 );
            return IsCollideRectAndRect(collider1, collider2);
        }
        else if (collider1.ColliderType == E_COLLIDER_SHAPE.ELLIPSE && collider2.ColliderType == E_COLLIDER_SHAPE.ELLIPSE)
        {
            //Debug.Log( 22 );
            return IsCollideEllipseAndEllipse(collider1, collider2);
        }
        else if (collider1.ColliderType == E_COLLIDER_SHAPE.RECT && collider2.ColliderType == E_COLLIDER_SHAPE.ELLIPSE)
        {
            //Debug.Log(33);
            return IsCollideRectAndEllipse(collider1, collider2);
        }
        else
        {
            //Debug.Log( 44 );
            return IsCollideRectAndEllipse(collider2, collider1);
        }
    }

    /// <summary>
    /// 矩形と矩形の衝突判定。
    /// </summary>
    private static bool IsCollideRectAndRect(ColliderData rect1, ColliderData rect2)
    {
        Vector2[] corners1 = GetCornerPosFromRect(rect1);
        Vector2[] corners2 = GetCornerPosFromRect(rect2);

        if (IsCollideRectAndRect(corners1, corners2))
        {
            return true;
        }

        return IsCollideRectAndRect(corners2, corners1);

    }

    /// <summary>
    /// 矩形と矩形の衝突判定の詳細処理。
    /// </summary>
    private static bool IsCollideRectAndRect(Vector2[] corners1, Vector2[] corners2)
    {
        for (int j = 0; j < corners2.Length; j++)
        {
            bool flag = true;

            for (int i = 0; i < corners1.Length; i++)
            {
                Vector2 baseV = corners1[(i + 1) % corners1.Length] - corners1[i];
                Vector2 targetV = corners2[j] - corners1[i];

                if (baseV.x * targetV.y - targetV.x * baseV.y > 0)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 矩形と楕円の衝突判定。
    /// </summary>
    private static bool IsCollideRectAndEllipse(ColliderData rect, ColliderData ellipse)
    {
        Vector2[] corners = GetCornerPosFromRect(rect);
        float cos = Mathf.Cos(ellipse.Angle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(ellipse.Angle * Mathf.Deg2Rad);
        float scaleRate = ellipse.Size.x / ellipse.Size.y;

        for (int i = 0; i < corners.Length; i++)
        {
            Vector2 offset = corners[i] - ellipse.CenterPos;
            float x = offset.x * cos + offset.y * sin;
            float y = scaleRate * (-offset.x * sin + offset.y * cos);

            if (x * x + y * y <= ellipse.Size.x * ellipse.Size.x)
            {
                return true;
            }
        }

        // 大きすぎる、小さすぎる時の対策のため矩形同士の判定処理を適用する
        return IsCollideRectAndRect(rect, ellipse);
    }

    /// <summary>
    /// 楕円と楕円の衝突判定。参考URL : http://marupeke296.com/COL_2D_No7_EllipseVsEllipse.html
    /// </summary>
    private static bool IsCollideEllipseAndEllipse(ColliderData ellipse1, ColliderData ellipse2)
    {
        ellipse1.Size /= 2;
        ellipse2.Size /= 2;

        float deffAngle = (ellipse1.Angle - ellipse2.Angle) * Mathf.Deg2Rad;
        Vector2 deltaPos = ellipse2.CenterPos - ellipse1.CenterPos;
        float deffCos = Mathf.Cos(deffAngle);
        float deffSin = Mathf.Sin(deffAngle);
        float nx = ellipse2.Size.x * deffCos;
        float ny = -ellipse2.Size.x * deffSin;
        float px = ellipse2.Size.y * deffSin;
        float py = ellipse2.Size.y * deffCos;

        float cos1 = Mathf.Cos(ellipse1.Angle * Mathf.Deg2Rad);
        float sin1 = Mathf.Sin(ellipse1.Angle * Mathf.Deg2Rad);
        float ox = cos1 * deltaPos.x + sin1 * deltaPos.y;
        float oy = -sin1 * deltaPos.x + cos1 * deltaPos.y;

        float rx_pow2 = 1f / (ellipse1.Size.x * ellipse1.Size.x);
        float ry_pow2 = 1f / (ellipse1.Size.y * ellipse1.Size.y);
        float A = rx_pow2 * nx * nx + ry_pow2 * ny * ny;
        float B = rx_pow2 * px * px + ry_pow2 * py * py;
        float D = 2 * rx_pow2 * nx * px + 2 * ry_pow2 * ny * py;
        float E = 2 * rx_pow2 * nx * ox + 2 * ry_pow2 * ny * oy;
        float F = 2 * rx_pow2 * px * ox + 2 * ry_pow2 * py * oy;
        float G = (ox / ellipse1.Size.x) * (ox / ellipse1.Size.x) + (oy / ellipse1.Size.y) * (oy / ellipse1.Size.y) - 1;

        float tmp1 = 1f / (D * D - 4 * A * B);
        float h = (F * D - 2 * E * B) * tmp1;
        float k = (E * D - 2 * A * F) * tmp1;
        float Th = Mathf.Atan2(D, B - A) * 0.5f;

        float CosTh = Mathf.Cos(Th);
        float SinTh = Mathf.Sin(Th);
        float A_tt = A * CosTh * CosTh + B * SinTh * SinTh - D * CosTh * SinTh;
        float B_tt = A * SinTh * SinTh + B * CosTh * CosTh + D * CosTh * SinTh;
        float KK = A * h * h + B * k * k + D * h * k - E * h - F * k + G;

        // 念のため
        if (KK > 0)
        {
            KK = 0;
        }

        float Rx_tt = 1 + Mathf.Sqrt(-KK / A_tt);
        float Ry_tt = 1 + Mathf.Sqrt(-KK / B_tt);
        float x_tt = CosTh * h - SinTh * k;
        float y_tt = SinTh * h + CosTh * k;
        float JudgeValue = x_tt * x_tt / (Rx_tt * Rx_tt) + y_tt * y_tt / (Ry_tt * Ry_tt);

        if (JudgeValue <= 1)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 指定した衝突判定の頂点の座標配列を取得する。
    /// </summary>
    private static Vector2[] GetCornerPosFromRect(ColliderData colliderData)
    {
        Vector2 halfSize = colliderData.Size / 2f;
        Vector2[] cornerPos = new Vector2[4];
        cornerPos[0] = new Vector2(halfSize.x, halfSize.y);
        cornerPos[1] = new Vector2(halfSize.x, -halfSize.y);
        cornerPos[2] = new Vector2(-halfSize.x, -halfSize.y);
        cornerPos[3] = new Vector2(-halfSize.x, halfSize.y);

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
}
