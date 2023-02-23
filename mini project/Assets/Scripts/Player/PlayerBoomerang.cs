using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoomerang : MonoBehaviour
{
    public int dmg = 1;
    public float speed = 20;
    public int status = 0; // 0: 오른쪽 1: 정지 2: 왼쪽
    // Start is called before the first frame update

    void Start()
    {
        Invoke("Turn", 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        switch(status) {
            case 0:
                transform.position = curPos + Vector3.right * speed * Time.deltaTime;
                break;
            case 1:
                break;
            case 2:
                transform.position = curPos + Vector3.left * speed * Time.deltaTime;
                break;
        }
        
    }

    void Turn() {
        if(status == 0) {
            status = 1;
            Invoke("Turn", 1f);
        }
        else if(status == 1)
            status = 2;
           
            
    }
}
