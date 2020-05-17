using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CommonSoundPlayer : ControllableMonoBehavior
{
    [SerializeField]
    private bool m_CallOnStart;

    [SerializeField]
    private bool m_UseSoundParam;

    [SerializeField]
    private PlaySoundParam m_SoundParam;

    [SerializeField]
    private E_COMMON_SOUND m_SoundType;

    public override void OnStart()
    {
        base.OnStart();
        if (m_CallOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (m_UseSoundParam)
        {
            AudioManager.Instance.Play(m_SoundParam);
        }
        else
        {
            AudioManager.Instance.Play(m_SoundType);
        }
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(CommonSoundPlayer))]
    private class CommonSoundPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as CommonSoundPlayer;

            serializedObject.Update();

            t.m_CallOnStart = EditorGUILayout.Toggle("Call On Start", t.m_CallOnStart);

            EditorGUILayout.Space();
            t.m_UseSoundParam = EditorGUILayout.Toggle("Use Sound Param", t.m_UseSoundParam);
            if (t.m_UseSoundParam)
            {
                var sp = serializedObject.FindProperty("m_SoundParam");
                EditorGUILayout.PropertyField(sp);
            }
            else
            {
                var sp = serializedObject.FindProperty("m_SoundType");
                EditorGUILayout.PropertyField(sp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
