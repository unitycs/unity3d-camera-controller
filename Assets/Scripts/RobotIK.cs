using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RobotIK : MonoBehaviour {
    public Transform target;

    protected Animator m_Anim;
	// Use this for initialization
	void Start () {
        m_Anim = GetComponent<Animator>();
	}
	
    void OnAnimatorIK()
    {
        if (Vector3.Distance(target.position, transform.position) < 10)
        {
            Vector3 eye = new Vector3(target.position.x, target.position.y + 1.2f, target.position.z);
            m_Anim.SetLookAtPosition(eye);
            m_Anim.SetLookAtWeight(1f, 0.1f, 0.5f, 0.9f);
        }
        else return;
    }

	// Update is called once per frame
	void Update () {

        if(Vector3.Dot(transform.forward,transform.position-target.position)>0)
        {           
            m_Anim.SetBool("Turn",true);
            //transform.Rotate(0, 180, 0);
        }
        else if(m_Anim.GetBool("Turn"))
        {
            m_Anim.SetBool("Turn", false);
        }
	}
}
