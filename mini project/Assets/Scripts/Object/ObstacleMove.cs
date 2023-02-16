using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    GameManager gameManager;   
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        transform.position = curPos + Vector3.left * gameManager.environmentSpeed * Time.deltaTime;
    }
}
