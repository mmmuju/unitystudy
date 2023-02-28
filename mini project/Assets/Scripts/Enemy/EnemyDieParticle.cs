using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 1);
        
    }

    void Die() {
        Destroy(gameObject);
    }
}
