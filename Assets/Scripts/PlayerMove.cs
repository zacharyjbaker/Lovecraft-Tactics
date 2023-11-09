using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rigid;
    public Animator animator;
    public GameObject cursor;
    public TurnManager turnManager;
    [SerializeField] public AudioClip[] sfx;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    } 

    // Update is called once per frame
    void Update()
    {
        /*var keyD = Input.GetKey(KeyCode.D);
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

        //Debug.Log(movement);

        rigid.MovePosition(rigid.position + movement * moveSpeed * Time.fixedDeltaTime);
        */
    }

    IEnumerator playSFX(AudioClip choice, float delay, float volume)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(choice, volume);
    }

    public void ShootAnimation()   
    {
        animator.SetTrigger("Shoot");
        StartCoroutine(playSFX(sfx[0], 1.36f, 0.7f));
    }
    
    public void FlipDirection(int direction)
    {
        if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
