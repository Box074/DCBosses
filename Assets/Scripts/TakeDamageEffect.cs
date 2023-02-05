using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int lastHP;
    public HealthManager hm;

    // Update is called once per frame
    void Update()
    {
        if(hm.hp < lastHP)
        {
            StartCoroutine(HitFlash());
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
