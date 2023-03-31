using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifeTime;

    private void Awake()
    {
        SetBullet();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void SetBullet()
    {
        this.transform.parent = GameObject.Find("PlayerBullets").transform;
        this.name = "PlayerBullet";
    }
    public void Shoot(Vector2 direction)
    {
        _rigidBody.AddForce(direction * speed);
        AudioManager.instance.PlaySFX("Shoot");
        StopCoroutine(bulletDissapear()); //Since the bullet can be set to false when hitting an object, to avoid a problem when reusing it with the pool, we stop the dissappear coroutine before starting a new one.
        StartCoroutine(bulletDissapear());
    }
    IEnumerator bulletDissapear()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }
}
