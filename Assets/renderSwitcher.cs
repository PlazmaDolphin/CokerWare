#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

[InitializeOnLoad]
public class SceneRendererSwitcher
{
    private static readonly List<string> Scene3D = new List<string>{"screw", "defuse"}; // Replace with your actual 3D scene name
    private static readonly int Renderer2DIndex = 0; // Index of 2D Renderer in URP settings
    private static readonly int Renderer3DIndex = 1; // Index of 3D Renderer in URP settings

    static SceneRendererSwitcher()
    {
        EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
    }

    private static void OnSceneChanged(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
    {
        if (GraphicsSettings.defaultRenderPipeline is UniversalRenderPipelineAsset urpAsset)
        {
            SerializedObject serializedURP = new SerializedObject(urpAsset);
            SerializedProperty defaultRendererProp = serializedURP.FindProperty("m_DefaultRendererIndex");

            if (defaultRendererProp != null)
            {
                if (Scene3D.Contains(next.name))
                    defaultRendererProp.intValue = Renderer3DIndex; // Switch to 3D renderer
                else
                    defaultRendererProp.intValue = Renderer2DIndex; // Switch to 2D renderer

                serializedURP.ApplyModifiedProperties();
                Debug.Log($"Switched Scene View Renderer: {next.name} -> {defaultRendererProp.intValue}");
            }
        }
    }
}
#endif