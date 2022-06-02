using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer healthBar;//SpriteRenderer代表图片
    public float health = 100f;//一百滴血
    public float repeatDamagePeriod = 2f;//伤害间隔为两秒
    public float hurtForce = 10f;//碰撞后受到的反向作用力
    public float damageAmount = 10f;//每次减血量为10

    private float lastHitTime;//最后一次受伤的时间
    private Vector3 healthScale;//血条的长度根据血量缩放
    private PlayerCtrl playerControl;//控制主角运动的脚本
    private Animator anim;//获取英雄的动画
    void Awake()
    {
        playerControl = GetComponent<PlayerCtrl>();//获得PlayerCtrl
        healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();//两个脚本在不同物体上时，不能直接使用GetComponent，需要使用GameObject.Find("...").GetComponent...
        anim = GetComponent<Animator>();

        healthScale = healthBar.transform.localScale;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            //可以再次减血
            if (Time.time > lastHitTime + repeatDamagePeriod)
            {
                if (health > 0f)
                {
                    TakeDamage(col.transform);
                    lastHitTime = Time.time;
                }

            }
        }
    }
    void death()
    {
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";
        }

        //GetComponent<PlayerCtrl>().enabled = false;
        playerControl.enabled = false;
        GetComponentInChildren<Gun>().enabled = false;
        //anim.SetTrigger("Die");

        //销毁血条
        //GameObject go = GameObject.Find("UI_HealthBar");
        //Destroy(go);
    }
    void TakeDamage(Transform enemy)
    {
        playerControl.bJump = false;
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
        GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        health -= damageAmount;
        if (health <= 0)
        {
            death();
            anim.SetTrigger("Die");
            // return;
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
        healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
    }

}
