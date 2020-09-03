using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    private AudioSource boom;
    
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        boom = GetComponent<AudioSource>();
    }
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
    }
    private void Death()
    {
        Destroy(this.gameObject);
    }
    private void Explode()
    {
        boom.Play();
    }
}
