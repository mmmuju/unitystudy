using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate : MonoBehaviour
{
    public float delay;
    public GameObject icicle;
    public GameObject spike;
    // Start is called before the first frame update
    void Start()
    {
        generateSpike();
        Invoke("generateIcicle", delay/2);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void generateSpike() {
        Instantiate<GameObject>(spike, new Vector2(15, -3.8f), Quaternion.identity); 
        Invoke("generateSpike", delay);
    }

    void generateIcicle()
    {
        Instantiate<GameObject>(icicle, new Vector2(15, 1.3f), Quaternion.identity);
        Invoke("generateIcicle", delay);
    }
}
