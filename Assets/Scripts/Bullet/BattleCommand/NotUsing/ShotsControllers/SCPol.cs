//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 発射パラメータを操作するクラス。多角形全方位弾にする。弾数が増える。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamListControllerBase/Pol", fileName = "SCPol", order = 0)]
//[System.Serializable]
//public class SCPol : ShotsControllerInc
//{

//    [SerializeField, Tooltip("正何角形か")]
//    public int m_VertexNum;

//    [SerializeField, Tooltip("一辺あたりの弾の数")]
//    public int m_Density;


//    public override void GetShotParamListIn(List<ShotParam> array, ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
//    {
//        //float piDivN = Mathf.PI / m_VertexNum;
//        //float twoPiDivN = Calc.TWO_PI / m_VertexNum;

//        //float largeR = shotParam.Speed;

//        //float r = largeR * Mathf.Cos(piDivN);
//        //float b = largeR * Mathf.Sin(piDivN);

//        //for (int nI = 0; nI < m_VertexNum; nI++)
//        //{
//        //    float mTheta = twoPiDivN * nI + piDivN;

//        //    for (int mI = 0; mI < m_Density; mI++)
//        //    {
//        //        float c = b * (1 - (float)mI * 2 / m_Density);

//        //        float d = Mathf.Sqrt(r * r + c * c);

//        //        float nTheta = Mathf.Atan(c / r);

//        //        float shotTheta = twoPiDivN * nI + piDivN - nTheta;

//        //        //ShotParam anotherShotParam = new ShotParam(shotParam);

//        //        //anotherShotParam.Angle += shotTheta;
//        //        //anotherShotParam.Speed = d;

//        //        //array.Add(anotherShotParam);
//        //    }
//        //}
//    }
//}




////array.Add(new ShotParam(shotParam.BulletIndex, shotParam.Position, shotParam.Angle + shotTheta, d));


////public override List<ShotParam> GetshotsParam(List<ShotParam> array, ShotTimer shotTimer, HackingBossPhaseState1 state)
////{
////    float piDivN = Mathf.PI / m_N;
////    float twoPiDivN = Calc.TWO_PI / m_N;

////    float largeR = shotParam.Speed;

////    float r = largeR * Mathf.Cos(piDivN);
////    float b = largeR * Mathf.Sin(piDivN);

////    for (int nI = 0; nI < m_N; nI++)
////    {
////        float mTheta = twoPiDivN * nI + piDivN;

////        for (int mI = 0; mI < m_M; mI++)
////        {
////            float c = b * (1 - (float)mI * 2 / m_M);

////            float d = Mathf.Sqrt(r * r + c * c);

////            float nTheta = Mathf.Atan(c / r);

////            float shotTheta = twoPiDivN * nI + piDivN - nTheta;

////            ShotParam anotherShotParam = new ShotParam(shotParam);

////            anotherShotParam.Angle += shotTheta;
////            anotherShotParam.Speed = d;

////            array.Add(anotherShotParam);

////        }
////    }

////    return array;
////}
