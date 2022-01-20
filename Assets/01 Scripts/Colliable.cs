using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliable : MonoBehaviour
{
    public ContactFilter2D fitler;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        boxCollider.OverlapCollider(fitler, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i] == null)
            {
                continue;
            }
            // Debug.Log(hits[i].name);
            OnCollide(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in" + this.name);
    }
}
