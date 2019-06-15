﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// Timelineのバインドパラメータ。
/// </summary>
[System.Serializable]
public struct TimelineParam
{
    /// <summary>
    /// トラックのバインドパラメータ。
    /// </summary>
    [System.Serializable]
    public struct TrackBindParam
    {
        /// <summary>
        /// トラックの名前
        /// </summary>
        public string TrackName;

        /// <summary>
        /// バインドしたいオブジェクトの名前
        /// </summary>
        public string BindTargetName;
    }

    [System.Serializable]
    public struct ReferenceBindParam
    {
        /// <summary>
        /// 参照名
        /// </summary>
        public string ReferenceName;

        /// <summary>
        /// バインドしたいオブジェクトの名前
        /// </summary>
        public string BindTargetName;

        /// <summary>
        /// コンポーネント名 ObjectやTransformは特殊とする
        /// </summary>
        public string BindTargetComponentType;
    }

    /// <summary>
    /// Timelineアセット
    /// </summary>
    public TimelineAsset TimelineAsset;

    /// <summary>
    /// トラックのバインドパラメータ
    /// </summary>
    public TrackBindParam[] TrackBindParams;

    /// <summary>
    /// アセットに対するバインドパラメータ
    /// </summary>
    public ReferenceBindParam[] ReferenceBindParams;

    /// <summary>
    /// タイムラインの終了によって自動的に破棄するかどうか
    /// </summary>
    public bool IsDestroyEndTimeline;
}