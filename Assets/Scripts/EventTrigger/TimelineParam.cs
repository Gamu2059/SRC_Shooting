using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public struct TimelineParam
{
    [System.Serializable]
    public struct BindParam
    {
        public string TrackName;
        public string BindName;
    }

    public TimelineAsset TimelineAsset;

    public BindParam[] BindParams;
}
