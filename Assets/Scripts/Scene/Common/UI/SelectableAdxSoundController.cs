using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SelectableAdxSoundController : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField]
    private bool m_UseSelectSound;

    [SerializeField]
    private E_COMMON_SOUND m_SelectSound;

    [SerializeField]
    private bool m_UseSubmitSound;

    [SerializeField]
    private E_COMMON_SOUND m_SubmitSound;

    public void OnSelect(BaseEventData e)
    {
        if (AudioManager.Instance != null && m_UseSelectSound)
        {
            AudioManager.Instance.Play(m_SelectSound);
        }
    }

    public void OnSubmit(BaseEventData e)
    {
        if (AudioManager.Instance != null && m_UseSubmitSound)
        {
            AudioManager.Instance.Play(m_SubmitSound);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(SelectableAdxSoundController))]
    private class SelectableAdxSoundControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as SelectableAdxSoundController;

            using (new EditorGUILayout.HorizontalScope())
            {
                t.m_UseSelectSound = EditorGUILayout.Toggle("Use Select Sound", t.m_UseSelectSound);
                if (t.m_UseSelectSound)
                {
                    t.m_SelectSound = (E_COMMON_SOUND) EditorGUILayout.EnumFlagsField("", t.m_SelectSound);
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                t.m_UseSubmitSound = EditorGUILayout.Toggle("Use Submit Sound", t.m_UseSubmitSound);
                if (t.m_UseSubmitSound)
                {
                    t.m_SubmitSound = (E_COMMON_SOUND)EditorGUILayout.EnumFlagsField("", t.m_SubmitSound);
                }
            }
        }
    }

#endif
}
