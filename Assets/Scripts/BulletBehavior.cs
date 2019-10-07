using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        Collider2D aux = other.collider;
        if (aux.tag == "Play") aux.GetComponent<PlayerInput>().TakeDamage(1, false, 0);
        Destroy(gameObject);
    }
}
