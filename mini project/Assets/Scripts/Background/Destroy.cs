using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.transform.tag == "Icicle" || other.gameObject.transform.tag == "Spike"
        || other.gameObject.transform.tag == "PlayerBullet" || other.gameObject.transform.tag == "EnemyBullet"
        || other.gameObject.transform.tag == "Enemy"){
            Destroy(other.gameObject);
        }
    }
}
