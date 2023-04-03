using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DCSpriteAnimator))]
[RequireComponent(typeof(DCSpriteRenderer))]
public class DCSpriteAnimatorHost : MonoBehaviour
{
    public DCSpriteClipCollection collection;
    public string curClipName = "";
    public string CurrentClipName { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentClipName == curClipName)
        {
            return;
        }
        var clip = collection.m_clips.FirstOrDefault(x => x.m_name == curClipName);
        if(clip == null)
        {
            return;
        }
        CurrentClipName = curClipName;
        clip.m_actions[0].DoAction(collection, clip, 0,
            GetComponent<DCSpriteAnimator>(), GetComponent<DCSpriteRenderer>(), this);
    }
}
