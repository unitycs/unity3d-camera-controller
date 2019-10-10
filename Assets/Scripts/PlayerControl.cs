 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour {
    public Transform FollowedCamera;
    public float m_Speed;
    public float m_JumpForce;
    public float gravity = 20;
    public float margin = 0.1f;

    protected Animator m_Anim;
    CharacterController m_Ctrl;
    
    private int m_waiting=0;
	// Use this for initialization
	void Start () {
        m_Anim = GetComponent<Animator>();
        m_Ctrl = GetComponent<CharacterController>();
	}

    // 通过射线检测主角是否落在地面或者物体上  
    bool IsGrounded()
    {
        //据需要修改margin的值
        return Physics.Raycast(transform.position, -Vector3.up, margin);
    }

    // Update is called once per frame
    void Update () {
        AnimatorStateInfo animatorInfo;
        animatorInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
        //移动方向  
        Vector3 mDir = Vector3.zero;
        
        if (m_Ctrl.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Input.GetMouseButtonUp(0))
            {
                m_Anim.SetBool("Waiting", false);
                m_waiting = 0;
                m_Anim.SetTrigger("Attack");
                Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f, LayerMask.GetMask("Enemy"));
                if (colliders.Length <= 0) return;
                for (int i = 0; i < colliders.Length; i++)
                {
                    Animator enemyAnim = colliders[i].gameObject.GetComponent<Animator>();
                    enemyAnim.SetTrigger("Damaged");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Anim.SetBool("Waiting", false);
                m_waiting = 0;
                m_Anim.SetTrigger("Jump");
                mDir.y = m_JumpForce;
            }
            else if(h!=0 || v!=0)
            {
                m_waiting = 0;
                m_Anim.SetBool("Waiting", false);
                if (v > 0)
                {
                    mDir = new Vector3(FollowedCamera.transform.forward.x, 0, FollowedCamera.transform.forward.z);

                    Quaternion newRotation = Quaternion.LookRotation(mDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
                }
                if (v < 0)
                {
                    mDir = new Vector3(-FollowedCamera.transform.forward.x, 0, -FollowedCamera.transform.forward.z);
                    Quaternion newRotation = Quaternion.LookRotation(mDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
                }
                if (h > 0)
                {
                    mDir = new Vector3(FollowedCamera.transform.right.x, 0, FollowedCamera.transform.right.z);
                    Quaternion newRotation = Quaternion.LookRotation(mDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);

                }
                if (h < 0)
                {
                    mDir = new Vector3(-FollowedCamera.transform.right.x, 0, -FollowedCamera.transform.right.z);
                    Quaternion newRotation = Quaternion.LookRotation(mDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100);
                }
                m_Anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v), 0.01f, Time.deltaTime);
            }
            else if ((h == 0)&&(v==0))
            {
                if (!m_Anim.GetBool("Waiting")) m_waiting++;
                if (m_waiting > 500)
                {
                    m_Anim.SetBool("Waiting", true);
                    if ((animatorInfo.normalizedTime > 1.0f) && (animatorInfo.IsName("Waiting")))//normalizedTime：0-1在播放、0开始、1结束 MyPlay为状态机动画的名字
                    {
                        m_Anim.SetBool("Waiting", false);
                        m_waiting = 0;
                    }
                }
            }

        }

        m_Ctrl.SimpleMove(mDir * m_Speed);
    }
}
