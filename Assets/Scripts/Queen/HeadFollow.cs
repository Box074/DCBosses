using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[ExecuteAlways]
public class HeadFollow : MonoBehaviour
{
    public TextAsset headTrack;
    public GameObject head;
    public SpriteRenderer spriteRenderer;
    private Vector3 trackPos0 = new Vector3(-0.32100001f, 0.574999988f);
    private Vector3 trackPos1 = new Vector3(-0.395000011f, 0.620000005f);
    private Dictionary<string, Dictionary<string, List<int>>> headC = null;
    private void Update()
    {
        if(headC == null || headC.Count == 0)
        {
            var tok = (JObject) JToken.Parse(headTrack.text);
            headC = new Dictionary<string, Dictionary<string, List<int>>>();
            foreach(var v in tok)
            {
                var name = v.Key.ToLower().Trim();
                headC[name] = v.Value.ToObject<Dictionary<string, List<int>>>();
            }
        }
        /*if(animator.runtimeAnimatorController == null) return;
        var curClips = animator.GetCurrentAnimatorClipInfo(0);
        if(curClips.Length == 0) return;
        var curClip = curClips[0].clip;
        var curState = animator.GetCurrentAnimatorStateInfo(0);
        var curTime = curState.normalizedTime;
        var totalFrame = curClip.length / (1 / curClip.frameRate);
        var curFrame = (int)(Mathf.Floor(totalFrame * curTime) % totalFrame);*/
        var tex = spriteRenderer.sprite?.name;
        if(tex == null) return;
        var curClipName = tex.Split('_')[0];
        if(!headC.TryGetValue(curClipName.ToLower().Trim(), out var h))
        {
            Debug.LogError("Missing Head Track:" + curClipName);
            return;
        }

        var hp = h["Bip001 Head"];
        
        var id = int.Parse(tex.Split('_')[1], System.Globalization.NumberStyles.Integer) * 3;
        //Debug.Log($"{id}/{hp.Count}");
        if(hp.Count <= id + 2 || id < 0) return;
        var x = hp[id] - 215;
        var y = hp[id + 1] - 197;
        head.transform.localPosition = new Vector3(x * 0.009250000125f + trackPos0.x,
            y * (-0.009250000125f) + trackPos0.y);

    }
}
