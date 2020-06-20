#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある1つの弾幕を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/Danmaku", fileName = "Danmaku", order = 0)]
[System.Serializable]
public class Danmaku : BulletShotParamBase
{

    [SerializeField, Tooltip("単位弾幕の配列")]
    private BulletShotParams[] m_BulletShotParamsArray;


    public override void OnStarts()
    {
        foreach (BulletShotParams bulletShotParams in m_BulletShotParamsArray)
        {
            bulletShotParams.OnStarts();
        }
    }


    public override void OnUpdates(CommandCharaController owner)
    {
        foreach (BulletShotParams bulletShotParams in m_BulletShotParamsArray)
        {
            bulletShotParams.OnUpdates(owner);
        }
    }
}





//[SerializeField, Tooltip("単位弾幕の配列")]
//private UnitDanmaku[] m_UnitDanmakuArray;


//foreach (UnitDanmaku unitDanmaku in m_UnitDanmakuArray)
//{
//    unitDanmaku.OnStarts();
//}

//if (m_BulletShotParamsArray != null)
//{
//    foreach (BulletShotParams bulletShotParams in m_BulletShotParamsArray)
//    {
//        bulletShotParams.OnStarts();
//    }
//}


//foreach (UnitDanmaku unitDanmaku in m_UnitDanmakuArray)
//{
//    unitDanmaku.OnUpdates(boss, commonOperationVariable);
//}

//if (m_BulletShotParamsArray != null)
//{
//    foreach (BulletShotParams bulletShotParams in m_BulletShotParamsArray)
//    {
//        bulletShotParams.OnUpdates(boss.GetEnemy(), commonOperationVariable, E_COMMON_SOUND.ENEMY_SHOT_MEDIUM_02);
//    }
//}


//bulletShotParams.OnUpdates(boss.GetEnemy(), E_COMMON_SOUND.ENEMY_SHOT_MEDIUM_02);
