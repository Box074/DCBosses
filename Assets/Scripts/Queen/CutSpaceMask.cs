using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSpaceMask : MonoBehaviour
{
    public new Renderer renderer;
    public Shader cutSpaceShader;
    public Vector3 start;
    public Vector3 end;
    public float left;
    public float right;
    private Material mat;
    private void Start() {
        cutSpaceShader ??= Shader.Find("DCQ/CutSpaceEffect");
        mat = new Material(cutSpaceShader);
    }
    private void Update() {
        var cam = Camera.main;
        var s = cam.WorldToViewportPoint(start - renderer.bounds.min);
        var e = cam.WorldToViewportPoint(end - renderer.bounds.min);
        mat.SetFloat("_CutX", s.x);
        mat.SetFloat("_CutY", s.y);
        mat.SetFloat("_CutXTo", e.x);
        mat.SetFloat("_CutYTo", e.y);
        mat.SetFloat("_LOffset", left);
        mat.SetFloat("_ROffset", right);
        renderer.sharedMaterial = mat;
    }
}
