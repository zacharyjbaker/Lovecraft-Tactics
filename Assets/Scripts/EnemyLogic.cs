using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float startingHitPoints = 0f;
    [SerializeField] private float hitPoints = 0f;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Shake()
    {
        for (int i = 0; i < 12; i++)
        {
            transform.localPosition += new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition -= new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int dmgValue)
    {
        hitPoints -= dmgValue;
        Debug.Log("Remaining HP" + hitPoints);

        if (hitPoints <= (startingHitPoints * 0.25f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.50f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.65f, 0.65f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.75f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f); }

        StartCoroutine(Shake());
        //this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        if (hitPoints <= 0f)
        {
            Destroy(gameObject, 0.8f);
        }
    }
}
