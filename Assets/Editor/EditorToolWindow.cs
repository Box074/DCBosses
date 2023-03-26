using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorToolWindow : EditorWindow
{
    [MenuItem("DCBosses/Tool Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorToolWindow window = (EditorToolWindow)GetWindow(typeof(EditorToolWindow));
        window.Show();
    }

    public List<Texture2D> m_SA_Textures;
    public List<Texture2D> m_SA_Normals;
    public string m_SA_OutputPath;
    public Material m_SA_Material;

    SerializedObject serObj;
    SerializedProperty p_SA_Textures;
    SerializedProperty p_SA_Normals;

    private void OnEnable()
    {
        serObj = new SerializedObject(this);
        p_SA_Textures = serObj.FindProperty(nameof(m_SA_Textures));
        p_SA_Normals = serObj.FindProperty(nameof(m_SA_Normals));
    }

    void ShadowApply()
    {

        var mat = Instantiate(m_SA_Material);
        Directory.CreateDirectory(m_SA_OutputPath);
        for(int i = 0; i < m_SA_Textures.Count; i++)
        {
            var tex = m_SA_Textures[i];
            var normal = i >= m_SA_Normals.Count ? Texture2D.normalTexture : m_SA_Normals[i];

            var rtex = new RenderTexture(tex.width, tex.height, 0);

            mat.mainTexture = tex;
            mat.SetTexture("_BumpMap", normal);

            Graphics.Blit(tex, rtex, mat);

            var prev = RenderTexture.active;
            RenderTexture.active = rtex;

            var otex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
            otex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            otex.Apply();
            RenderTexture.active = prev;

            rtex.Release();
            File.WriteAllBytes(Path.Combine(m_SA_OutputPath, tex.name + ".png"), otex.EncodeToPNG());
            DestroyImmediate(otex);
        }

        DestroyImmediate(mat);
    }

    Vector2 scroll;
    void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.BeginVertical();
        

        EditorGUILayout.Space(10);
        GUILayout.Label("[Texture] Processing", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(p_SA_Textures, new GUIContent("Texture2Ds"));
        EditorGUILayout.PropertyField(p_SA_Normals, new GUIContent("Normals"));

        if(GUILayout.Button("Clear"))
        {
            p_SA_Normals.ClearArray();
            p_SA_Textures.ClearArray();
            serObj.ApplyModifiedProperties();
        }

        GUILayout.Label("Output Folder");
        m_SA_OutputPath = GUILayout.TextField(m_SA_OutputPath);

        m_SA_Material = (Material)EditorGUILayout.ObjectField(m_SA_Material, typeof(Material), false);

        if (GUILayout.Button("Start"))
        {
            ShadowApply();
        }

        GUILayout.EndVertical();

        serObj.Update();
        EditorGUI.BeginChangeCheck();
        if (EditorGUI.EndChangeCheck())
        {
            serObj.ApplyModifiedProperties();
        }

        GUILayout.EndScrollView();
    }
}
