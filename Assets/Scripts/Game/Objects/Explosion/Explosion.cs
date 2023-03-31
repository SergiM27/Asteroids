using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = GameObject.Find("Explosions").transform;
        gameObject.name = "Explosion";
    }
    public void ExplosionDone()
    {
        gameObject.SetActive(false);
    }
}
