using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dajjsand.View.CustomComponents
{
    public class CustomButton : Button
    {
        [SerializeField] private AudioSource _clickSoundSource;
        [SerializeField] private List<Graphic> _additionalTargetGraphics = new();

        protected override void Awake()
        {
            base.Awake();

            if (_clickSoundSource == null)
                Debug.LogError("No click sound");
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            PlayClickSound();
        }

        private void PlayClickSound()
        {
            _clickSoundSource.Play();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            Color tintColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => colors.normalColor
            };

            if (transition == Transition.ColorTint)
            {
                Color targetColor = tintColor * colors.colorMultiplier;

                foreach (var graphic in _additionalTargetGraphics)
                {
                    if (graphic == null)
                        continue;

                    graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }

        [ContextMenu("Find All Graphics")]
        public void FindAllGraphics()
        {
            _additionalTargetGraphics.Clear();
            Graphic[] graphics = GetComponentsInChildren<Graphic>();

            foreach (var graphic in graphics)
            {
                if (graphic == targetGraphic)
                    continue;

                _additionalTargetGraphics.Add(graphic);
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(CustomButton))]
        public class CustomButtonEditor : ButtonEditor
        {
            SerializedProperty _clickSoundSource;
            SerializedProperty _additionalTargetGraphics;

            protected override void OnEnable()
            {
                base.OnEnable();
                _clickSoundSource = serializedObject.FindProperty("_clickSoundSource");
                _additionalTargetGraphics = serializedObject.FindProperty("_additionalTargetGraphics");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI(); // Рисуем стандартный интерфейс Button

                serializedObject.Update();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Custom Button Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_clickSoundSource);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_additionalTargetGraphics, true);

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}