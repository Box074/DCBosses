using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCut : MonoBehaviour
{
    public Material mat;
    private float _startTime;
    private bool isExit;
    public float cutWidth;
    public bool enable = false;
    public GameObject bindObj;
    public void EnterAnim()
    {
        enable = true;
        isExit = false;
        _startTime = Time.time;
    }
    public void ExitAnim()
    {
        enable = true;
        isExit = true;
        _startTime = Time.time;
    }
    private void Start() {
        mat = new Material(Shader.Find("DC/CutScreen"));
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(bindObj == null)
        {
            Graphics.Blit(src, dest);
            Destroy(this);
            return;
        }
        if(!enable || mat == null)
        {
            Graphics.Blit(src, dest);
            return;
        }
        var scale = Mathf.Clamp(Time.time - _startTime, 0f, 1f);
        
        if(isExit) scale = 1 - scale;
        var border = new Vector4();
        border.y = cutWidth * scale;
        border.w = cutWidth * scale;
        mat.SetVector("_Border", border);
        Graphics.Blit(src, dest, mat);
    }
}
