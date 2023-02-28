using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int dmg = 1;
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            GameObject dieEffectObj = Instantiate(effect, transform.position, transform.rotation);
            ParticleSystem dieEffect = dieEffectObj.GetComponent<ParticleSystem>();
            Destroy(gameObject);
        }
            
    }
}

