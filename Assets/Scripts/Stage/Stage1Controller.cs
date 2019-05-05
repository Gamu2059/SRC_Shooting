using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルメインのステージ1の動きを管理する。
/// </summary>
public class Stage1Controller : StageController
{
    private enum Stage1Phase
    {
        // 最初浮上してくる
        // キャラを軸に回転
        // 背景カメラが俯瞰位置に移動
        // バトルスタート
        // 直進1
        ST_1,
        // カーブ1
        CV_1,
        // 直進2
        ST_2,
        // カーブ2
        CV_2,
        // 直進3
        ST_3,

        CV_3,

    }

    private Camera m_C;

    private void aaaaa()
    {
        m_C.cullingMask -= LayerName.DefaultMask;
    }
}
