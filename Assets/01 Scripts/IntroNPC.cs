using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNPC : Colliable
{
    public string message;

    private float cooldown = 4.0f;
    private float lastShout;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
    }

    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 15, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
        }
    }

  
}
