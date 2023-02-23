using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmite : MonoBehaviour
{
    public float duration = 0.2f;
    void Start()
    {
        Invoke("Life", duration);
    }

    void Life() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Icicle" || other.gameObject.tag == "Spike")
            Destroy(other.gameObject);
    }
}
