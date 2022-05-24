using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// Start is called before the first frame update
	public float moveSpeed = 4f;//敌人的移动速度
	public int HP = 2;//血量为2
	public Sprite deadEnemy;//Sprite存放无死亡形象
	public Sprite damagedEnemy;//Sprite存放无头盔形象
	public float deathSpinMin = -100f;//敌人死亡时受到随机的旋转扭力
	public float deathSpinMax = 100f;//同上

	//敌人显示的图片
	private SpriteRenderer ren;
	private Transform frontCheck;
	private bool dead = false;


	void Awake()
	{
		ren = transform.Find("body").GetComponent<SpriteRenderer>();//根据血量换图像
		frontCheck = transform.Find("frontCheck").transform;
	}

	void FixedUpdate()
	{
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		foreach (Collider2D c in frontHits)
		{
			if (c.tag == "wall")
			{
				Flip();//转身
				break;
			}
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
		if (HP == 1 && damagedEnemy != null)
			ren.sprite = damagedEnemy;
		if (HP <= 0 && !dead)
			Death();
	}

	public void Hurt()
	{
		HP--;
	}

	void Death()
	{
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;//设置成不可见
		}
		ren.enabled = true;//先设false再设true，双缓冲，防止程序变慢
		ren.sprite = deadEnemy;//给图片赋值
		dead = true;
		Rigidbody2D rd2d = GetComponent<Rigidbody2D>();
		rd2d.freezeRotation = false;
		rd2d.AddTorque(Random.Range(deathSpinMin, deathSpinMax));
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach (Collider2D c in cols)
		{
			c.isTrigger = true;
		}
	}

	public void Flip()
	{
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
