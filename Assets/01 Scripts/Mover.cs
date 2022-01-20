using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;


    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

    }
    protected virtual void UpdateMotor(Vector3 input)
    {
        if (GameManager.instance.isPause == true)
            return;
        //Reset moveDelta
        // moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);
        moveDelta = input;
        //swap sprite direction, wether you're going right or left
        if (moveDelta.x > 0)
        {
            transform.localScale = originalSize;
        }
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y , originalSize.z);

        //add push vector, if any
        moveDelta += pushDirection;
        //reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
        //make sure we can move in this direction by casting a box there first if the box return null we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make player move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make player move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
