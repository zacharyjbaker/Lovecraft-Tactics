using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rigid;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    } 

    // Update is called once per frame
    void Update()
    {
        var keyD = Input.GetKey(KeyCode.D);
        var keyA = Input.GetKey(KeyCode.A);
        var keyW = Input.GetKey(KeyCode.W);
        var keyS = Input.GetKey(KeyCode.S);
        var keyJ = Input.GetKey(KeyCode.J);

        if (keyJ)
        {
            animator.SetTrigger("Shoot");
        }

        if (keyD || keyA)
        {
            if (keyD)
            {
                movement.x = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                movement.x = -1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            movement.x = 0;
        }

        if (keyW || keyS)
        {
            if (keyW)
            {
                movement.y = 1;
            }
            else
            {
                movement.y = -1;
            }
        }
        else
        {
            movement.y = 0;
        }

        Debug.Log(movement);

        rigid.MovePosition(rigid.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
