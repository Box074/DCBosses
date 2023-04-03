using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class ColliderControl : MonoBehaviour
{
    public PolygonCollider2D[] col;
    public SpriteRenderer render;
    private void FixedUpdate() {
        //OnUpdateSprite();
    }
    private void Update() {
        if(Application.isEditor && !Application.isPlaying) 
        {
            OnUpdateSprite();
        }
    }
    public void OnUpdateSprite()
    {
        Sprite sprite = render.sprite;
        if(sprite == null) return;
        var triangles = sprite.triangles;
        var vertices = sprite.vertices;

        var edges = new List<SpriteEdge>();

        for (int i = 0; i < triangles.Length; i = i + 3)
        {
            var a = triangles[i];
            var b = triangles[i + 1];
            var c = triangles[i + 2];
            var p0 = vertices[a];
            var p1 = vertices[b];
            var p2 = vertices[c];

            SpriteEdge s0 = edges.FirstOrDefault(x => x.Is(p0, p1));
            if(s0 == null)
            {
                s0 = new SpriteEdge(p0, p1);
                edges.Add(s0);
            }
            s0.count++;
            SpriteEdge s1 = edges.FirstOrDefault(x => x.Is(p1, p2));
            if(s1 == null)
            {
                s1 = new SpriteEdge(p1, p2);
                edges.Add(s1);
            }
            s1.count++;
            SpriteEdge s2 = edges.FirstOrDefault(x => x.Is(p0, p2));
            if(s2 == null)
            {
                s2 = new SpriteEdge(p0, p2);
                edges.Add(s2);
            }
            s2.count++;
        }

        var paths = new List<Vector2>();
        var offset = (Vector2) render.transform.localPosition;
        foreach(var v in edges)
        {
            if(v.count == 1)
            {
                paths.Add(v.p1 + offset);
                paths.Add(v.p2 + offset);
            }
        }
        foreach(var v in col) v.points = paths.ToArray();
    }



    class SpriteEdge
    {
        public SpriteEdge(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        public bool Is(Vector2 p1, Vector2 p2)
        {
            if(p1 == this.p1 && p2 == this.p2)
            {
                return true;
            }else if(p1 == this.p2 && p2 == this.p1)
            {
                return true;
            }
            return false;
        }
        public Vector2 p1;
        public Vector2 p2;
        public int count;
    }
}
