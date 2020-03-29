//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// ベジェ曲線上の点であるVector2型の値を、制御点を直接入力せずに生成する演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/bezierPV1", fileName = "OperationVector2BezierPV1", order = 0)]
//[System.Serializable]
//public class OperationVector2BezierPV1 : OperationVector2Base
//{

//    /// <summary>
//    /// 現在の時刻にあたる値
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase m_T;

//    /// <summary>
//    /// 始点の位置ベクトル
//    /// </summary>
//    [SerializeField]
//    private OperationVector2Base m_BeginPosition;

//    /// <summary>
//    /// 始点の速度ベクトル
//    /// </summary>
//    [SerializeField]
//    private OperationVector2Base m_BeginVelocity;

//    /// <summary>
//    /// ベジェ曲線を表すオブジェクトの配列
//    /// </summary>
//    [SerializeField]
//    private BezierPointPV1[] m_BezierArray;


//    public override Vector2 GetResultVector2()
//    {
//        // 現在時刻
//        float t = m_T.GetResultFloat();

//        // スキャンで進んだ所までの時刻（区切りの時刻）
//        float tempTime = 0;

//        // 配列のインデックス
//        int i = 0;

//        while (i < m_BezierArray.Length - 1 && tempTime + m_BezierArray[i].T.GetResultFloat() < t)
//        {
//            tempTime += m_BezierArray[i].T.GetResultFloat();

//            i++;
//        }

//        // 最初の区間の場合
//        if (i <= 0)
//        {
//            // 始点の位置ベクトル
//            Vector2 positionPrev = m_BeginPosition.GetResultVector2();

//            // 始点の速度ベクトル
//            Vector2 velocityPrev = m_BeginVelocity.GetResultVector2();

//            // 所要時間
//            float timeLength = m_BezierArray[0].T.GetResultFloat();

//            // 終点の位置ベクトル
//            Vector2 position = m_BezierArray[0].Position.GetResultVector2();

//            // 終点の速度ベクトル
//            Vector2 velocity = m_BezierArray[0].Velocity.GetResultVector2();

//            return GetBezier(
//                positionPrev,
//                positionPrev + velocityPrev * timeLength,
//                position - velocity * timeLength,
//                position,
//                (t - tempTime) / timeLength
//                );
//        }
//        // 2つ目以降の区間の場合
//        else
//        {
//            // 始点の位置ベクトル
//            Vector2 positionPrev = m_BezierArray[i - 1].Position.GetResultVector2();

//            // 始点の速度ベクトル
//            Vector2 velocityPrev = m_BezierArray[i - 1].Velocity.GetResultVector2();

//            // 所要時間
//            float timeLength = m_BezierArray[i].T.GetResultFloat();

//            // 終点の位置ベクトル
//            Vector2 position = m_BezierArray[i].Position.GetResultVector2();

//            // 終点の速度ベクトル
//            Vector2 velocity = m_BezierArray[i].Velocity.GetResultVector2();

//            return GetBezier(
//                positionPrev,
//                positionPrev + velocityPrev * timeLength,
//                position - velocity * timeLength,
//                position,
//                (t - tempTime) / timeLength
//                );
//        }
//    }


//    public Vector2 GetBezier(Vector2 vec1, Vector2 vec2, Vector2 vec3, Vector2 vec4, float t)
//    {
//        return (1 - t) * (1 - t) * (1 - t) * vec1 +
//            3 * (1 - t) * (1 - t) * t * vec2 +
//            3 * (1 - t) * t * t * vec3 +
//            t * t * t * vec4;
//    }
//}





////for (int i = 0;i < m_BezierArray.Length;i++)
////{
////    tempTime += m_BezierArray[i].T.GetResultFloat();

////    if (t <= tempTime)
////    {
////        // 最初の区間の場合
////        if (i <= 0)
////        {
////            Vector2 positionPrev = m_BezierArray[i - 1].Position.GetResultVector2();
////            Vector2 velocityPrev = m_BezierArray[i - 1].Velocity.GetResultVector2();
////            float timeLength = m_BezierArray[i].T.GetResultFloat();
////            Vector2 position = m_BezierArray[i].Position.GetResultVector2();
////            Vector2 velocity = m_BezierArray[i].Velocity.GetResultVector2();

////            return GetBezier(
////                positionPrev,
////                positionPrev + velocityPrev * timeLength,
////                position + velocity * timeLength,
////                position,
////                t
////                );
////        }
////        // 2つ目以降の区間の場合
////        else
////        {
////        }
////    }
////}
