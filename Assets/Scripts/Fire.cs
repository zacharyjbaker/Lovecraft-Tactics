using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool fireDamage = false;
    public int timer = 2;
    
    private void Start()
    {
        //Vector3 temp = this.transform.position;
        this.transform.position += new Vector3(0f, 0f, -2f);
    }
}
