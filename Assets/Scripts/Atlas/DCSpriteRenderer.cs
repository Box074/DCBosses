using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DCSpriteRenderer : MonoBehaviour
{
    public DCAtlasInstance atlas;
    public int curSpriteId;
    private int prevSpriteId = -1;
    public GameObject renderGameObject;
    private SpriteRenderer m_sr;
    private Dictionary<int, Sprite> m_sprites = new Dictionary<int, Sprite>();

    public SpriteRenderer Renderer => m_sr;

    // Start is called before the first frame update
    void Awake()
    {
        m_sprites = new Dictionary<int, Sprite>();
    }
    private void OnDestroy()
    {
        foreach(var sprite in m_sprites.Values)
        {
            DestroyImmediate(sprite);
        }
        m_sprites.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(atlas == null)
        {
            return;
        }
        curSpriteId = Mathf.Clamp(curSpriteId, 0, atlas.atlas.tiles.Count - 1);
        if(renderGameObject == null)
        {
            renderGameObject = new GameObject("DCSprite Renderer");
            renderGameObject.transform.parent = transform;
            renderGameObject.transform.localScale = Vector3.one;
            m_sr = renderGameObject.AddComponent<SpriteRenderer>();
        }
        if(m_sr == null)
        {
            m_sr = renderGameObject.GetComponent<SpriteRenderer>();
        }
        if(m_sr == null)
        {
            Debug.LogError("Not found Sprite Renderer");
            return;
        }
        if(prevSpriteId == curSpriteId + 1)
        {
            return;
        }
        prevSpriteId = curSpriteId + 1;
        
        
        var tile = atlas.atlas.tiles[curSpriteId];
        if(!m_sprites.TryGetValue(curSpriteId, out var sprite) || sprite == null)
        {
            sprite = tile.GenerateSprite();
            m_sprites[curSpriteId] = sprite;
            //Debug.LogWarning("Missing Cache");
        }
        m_sr.sprite = sprite;

        if (Application.isPlaying || !Application.isEditor) SendMessage("OnUpdateSprite", this, SendMessageOptions.DontRequireReceiver);

        Vector2 pos = new Vector3();
        pos.x = tile.offset.x / sprite.pixelsPerUnit;
        pos.y = (tile.originalSize.y - tile.offset.y - tile.rect.height) / sprite.pixelsPerUnit;

        pos += (Vector2)sprite.bounds.size / 2f;

        var center = tile.originalSize / 2f / sprite.pixelsPerUnit;

        pos -= center;

        renderGameObject.transform.localPosition = pos;
    }
}
