using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DCAtlas : ISerializationCallbackReceiver
{
    [Serializable]
    public class Tile
    {
        public string originalName;
        public string name;
        public Rect rect;
        public int index;

        public Vector2 offset;
        public Vector2 originalSize;

        [NonSerialized]
        public DCAtlas atlas;

        public int atlasId;

        public Texture2D atlasTexture
        {
            get
            {
                return atlas.atlasTextures[atlasId];
            }
        }

        public Sprite GenerateSprite()
        {
            return Sprite.Create(atlasTexture, 
                new Rect(rect.x, atlasTexture.height - rect.y - rect.height, rect.width, rect.height), 
                new Vector2(0.5f, 0.5f), 22,
                0, SpriteMeshType.FullRect);
        }
    }

    [Serializable]
    public class Clip
    {
        public string name;
        public List<int> frames = new List<int>();

        [NonSerialized]
        public DCAtlas atlas;

        public Tile GetFrame(int id) => atlas.tiles[frames[id]];
    }

    public List<string> atlasTextureNames = new List<string>();
    public List<Texture2D> atlasTextures = new List<Texture2D>();
    public List<Tile> tiles = new List<Tile>();

    public List<string> animNames = new List<string>();
    public List<Clip> clips = new List<Clip>();

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        foreach (var tile in tiles)
        {
            tile.atlas = this;
        }
        foreach (var clip in clips)
        {
            clip.atlas = this;
        }
    }
}
