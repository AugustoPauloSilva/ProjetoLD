using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behavior : MonoBehaviour
{
    public GameObject dropAttack; 
    public float frequency = 60f;
    public Vector3 offset = new Vector3(0,-2,0);
    float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (count > 0) count--;
        else {
            count = frequency;
            Instantiate(dropAttack,transform.position+offset,transform.rotation);
        }

    }
}
