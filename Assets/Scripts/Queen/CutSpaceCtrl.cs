using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSpaceCtrl : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    private GameObject _maskGO;
    private Mesh _maskMesh;
    public CutSpaceMask mask;
    public float distance;
    void Start()
    {
        _maskGO = new GameObject("Cutspace Mask");

        _maskMesh = new Mesh();
        _maskMesh.vertices = new Vector3[] {
            new Vector3(-50,-50),
            new Vector3(-50, 50),
            new Vector3(50, -50),
            new Vector3(50, 50)
        };
        _maskMesh.triangles = new int[] {
            0, 1, 2,
            2, 3, 1
        };
        _maskMesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1,1)
        };
        var maskSR = _maskGO.AddComponent<MeshRenderer>();
        var filter = _maskGO.AddComponent<MeshFilter>();
        filter.sharedMesh = _maskMesh;
        mask = _maskGO.AddComponent<CutSpaceMask>();
        mask.renderer = maskSR;
    }
    private void OnDestroy() {
        Destroy(_maskGO);
        Destroy(_maskMesh);
    }
    private void OnDisable() {
        if(mask != null) mask.enabled = false;
    }
    private void OnEnable() {
        if(mask != null) mask.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(mask == null) return;
        mask.start = start.transform.position;
        mask.end = end.transform.position;
        var pos = (end.transform.position + start.transform.position) / 2;
        mask.transform.position = pos;
        mask.left = mask.right = 0;
        if(transform.localScale.x < 0)
        {
            mask.left = distance;
        }
        else 
        {
            mask.right = distance;
        }
    }
}
