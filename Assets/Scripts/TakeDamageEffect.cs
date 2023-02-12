using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int lastHP;
    public HealthManager hm;
    public EnemyDreamnailReaction dream;
    public bool trigDream;
    private FieldInfo f_cooldownTimeRemaining = typeof(EnemyDreamnailReaction).GetField("cooldownTimeRemaining", BindingFlags.Instance | BindingFlags.NonPublic);
    // Update is called once per frame
    void Update()
    {
        if(hm.hp < lastHP)
        {
            StartCoroutine(HitFlash());
        }
        if(dream != null && f_cooldownTimeRemaining != null) 
        {
            var cld = (float)f_cooldownTimeRemaining.GetValue(dream);
            if(cld > 0)
            {
                if(!trigDream)
                {
                    StartCoroutine(HitFlash());
                    trigDream = true;
                }
            }
            else
            {
                trigDream = false;
            }
        }
        lastHP = hm.hp;
    }
    private IEnumerator HitFlash()
    {
        yield return null;
        spriteRenderer.material.SetFloat("_HurtFlash", 0.75f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material.SetFloat("_HurtFlash", 0);
    }
}
