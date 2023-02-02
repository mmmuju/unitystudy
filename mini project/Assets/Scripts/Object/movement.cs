using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        transform.position = curPos + Vector3.left * speed * Time.deltaTime;
    }
}
