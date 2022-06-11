using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public float MoveForce = 100.0f;
    public float MaxSpeed = 5;
    public Rigidbody2D HeroBody;
    public bool bFaceRight = true;
    public bool bJump = false;
    public float JumpForce = 100;
    public AudioClip[] JumpClips;//随机声音

    //public AudioSource audioSource;
    public AudioMixer audioMixer;

    private Transform mGroundCheck;
    Animator anim;
    float mVolume = 0;

    void Start()
    {
        HeroBody = GetComponent<Rigidbody2D>();
        mGroundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();//获取animator
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(HeroBody.velocity.x) < MaxSpeed)
        {
            HeroBody.AddForce(Vector2.right * h * MoveForce);
        }
        if (Mathf.Abs(HeroBody.velocity.x) > 5)
        {
            HeroBody.velocity = new Vector2(Mathf.Sign(HeroBody.velocity.x) * MaxSpeed, HeroBody.velocity.y);
        }
        anim.SetFloat("speed", Mathf.Abs(h));
        if (h > 0 && !bFaceRight)
        {
            flip();
        }
        if (h < 0 && bFaceRight)
        {
            flip();
        }
        //射线检测是通过按位与的操作进行而不是通过“==”操作进行判断
        if (Physics2D.Linecast(transform.position, mGroundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (Input.GetButtonDown("Jump"))
            {
                bJump = true;
            }
        }
        Debug.DrawLine(transform.position, mGroundCheck.position, Color.red);
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            mVolume++;
            audioMixer.SetFloat("MasterVolume", mVolume);
        }
        if (bJump)
        {
            int i = Random.Range(0, JumpClips. Length);
            //audioSource.clip = JumpClips[i];
            //audioSource.Play();
            AudioSource.PlayClipAtPoint(JumpClips[i], transform.position);
            HeroBody.AddForce(Vector2.up * JumpForce);
            bJump = false;
            anim.SetTrigger("jump");
        }
    }
    private void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        bFaceRight = !bFaceRight;
    }
}
