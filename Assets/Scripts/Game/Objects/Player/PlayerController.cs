using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool forwardPressed;

    [SerializeField]
    private float turnDirection;

    private Rigidbody2D _rigidBody;

    [Header("Speeds")]
    public float movementSpeed;
    public float turnSpeed;

    [Header("Bullets")]
    public float bulletCDShoot;
    private float bulletCDCurrent;
    public GameObject bulletSpawner;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        bulletCDCurrent = bulletCDShoot;
        GameManager.instance.SetPlayer(this);
        GameManager.instance.SetLives();
    }

    void Update()
    {
        Inputs();

        bulletCDCurrent -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (forwardPressed)
        {
            _rigidBody.AddForce(transform.up * movementSpeed);
        }
        if (turnDirection != 0)
        {
            _rigidBody.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void Inputs()
    {

        //Movement and rotation inputs
        forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = turnSpeed;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -turnSpeed;
        }
        else
        {
            turnDirection = 0;
        }

        //Shoot input
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (bulletCDCurrent <= 0)
            {
                Shoot();
            }
        }
    }

    private void Shoot() //Get a bullet from the pool, and shoot it in the bullet spawner up direction (facing the same direction as the player does).
    {
        GameObject newBullet = ObjectPooler.Instance.GetPooledObject("PlayerBullet", bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        newBullet.GetComponent<Bullet>().Shoot(bulletSpawner.transform.up);
        bulletCDCurrent = bulletCDShoot;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid")) //If hit by an asteroid, stop movement, get an explosion from the pool, and call PlayerDeath function from GameManager.
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = 0f;
            ObjectPooler.Instance.GetPooledObject("Explosion", transform.position, transform.rotation);
            this.gameObject.SetActive(false);
            GameManager.instance.PlayerDeath();
        }
    }
}
