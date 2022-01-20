using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{

    protected override void Death()
    {
        base.Death();
        GameManager.instance.winMenuAnim.SetTrigger("show");
    }
}
