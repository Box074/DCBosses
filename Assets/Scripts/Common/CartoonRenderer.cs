using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonRenderer : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public Texture2D[] normals;
    void Update()
    {
        if(normals == null || renderer.sprite == null || renderer.sprite.texture == null) return;

        var texName = renderer.sprite.texture.name;
        var nName = texName + "_n";
        renderer.material.SetTexture("_BumpMap", normals.FirstOrDefault(x => x.name == nName));
    }
}
