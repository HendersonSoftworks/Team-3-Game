using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hMovement = Input.GetAxis("Vertical");
        float vMovement = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(vMovement, hMovement, 0) * Time.deltaTime);

    }
}
