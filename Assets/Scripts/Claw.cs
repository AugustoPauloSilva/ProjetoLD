using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        
    }
	
	public void reset(){
		transform.position = new Vector3(0, 0, 0);
	}
}
