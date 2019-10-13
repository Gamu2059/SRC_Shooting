using UnityEngine;

public struct CollisionLine
{
    /// <summary>
    /// 線分の始点
    /// </summary>
    public Vector2 p;

    /// <summary>
    /// 線分の終点への位置ベクトル
    /// </summary>
    public Vector2 v;
}

public struct CollisionCircle
{
    /// <summary>
    /// 円の中心点
    /// </summary>
    public Vector2 p;

    /// <summary>
    /// 円の半径
    /// </summary>
    public float r;
}

public struct CollisionRect
{
    /// <summary>
    /// 矩形の中心点
    /// </summary>
    public Vector2 p;

    /// <summary>
    /// 矩形の中心に対して左下の座標
    /// </summary>
    public Vector2 leftDown;

    /// <summary>
    /// 矩形の中心に対して左上の座標
    /// </summary>
    public Vector2 leftUp;
}

public struct CollisionCapsule
{
    /// <summary>
    /// カプセルの直線
    /// </summary>
    public CollisionLine l;

    /// <summary>
    /// カプセルの半径
    /// </summary>
    public float r;
}
