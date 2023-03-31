using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] spritesLarge;
    public Sprite[] spritesMedium;
    public Sprite[] spritesSmall;
    private SpriteRenderer _spriteRenderer;

    [Header("DestroyTime")]
    public float destroyTime;

    [SerializeField]
    [Header("Scores")]
    public int scoreSmall;
    public int scoreMedium, scoreLarge;

    [Header("Size")]
    public int currentAsteroidSize;
    public float maxSize, minSize, size, speed;

    private ScoreController scoreController;

    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        scoreController = FindObjectOfType<ScoreController>();
        SetAsteroid();
    }

    private void OnEnable() //Everytime the asteroid is spawned from the pool, it changes.
    {
        SetRotationAndScale();
        SetSprite();
        StopCoroutine("DestroyMyself"); //Since an object pool is used, we need to cancel the coroutine done before, to create a new one.
        StartCoroutine("DestroyMyself");
    }

    private void SetAsteroid()
    {
        this.transform.parent = GameObject.Find("Asteroids").transform;
        this.name = "Asteroid";
    }
    private void SetRotationAndScale() //Both rotation and size are randomized
    {
        size = Random.Range(minSize, maxSize);
        SetCurrentSize(size);
        transform.eulerAngles = new Vector3(0f,0f,Random.value * 360.0f);
        transform.localScale = Vector3.one * size;
        _rigidBody.mass = size;
    }

    private void SetCurrentSize(float size)
    {
        if(size >= maxSize * 0.5f)
        {
            currentAsteroidSize = 3;
        }
        else if (size >= maxSize * 0.3f && size < maxSize * 0.5f)
        {
            currentAsteroidSize = 2;
        }
        else
        {
            currentAsteroidSize = 1;
        }
    }

    private void SetSprite()//Depending on the size of the asteroid, a sprite is applied to it, randomized.
    {
        switch (currentAsteroidSize)
        {
            case 3:
                _spriteRenderer.sprite = spritesLarge[Random.Range(0, spritesLarge.Length)];
                break;

            case 2:
                _spriteRenderer.sprite = spritesMedium[Random.Range(0, spritesMedium.Length)];
                break;

            case 1:
                _spriteRenderer.sprite = spritesSmall[Random.Range(0, spritesSmall.Length)];
                break;
            default:
                _spriteRenderer.sprite = spritesLarge[Random.Range(0, spritesLarge.Length)];
                break;
        }
    }

    public void Trajectory(Vector2 direction)
    {
        _rigidBody.AddForce(direction * speed);
    }

    IEnumerator DestroyMyself()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            AsteroidKilledScore();
            ShouldSplit();
            ObjectPooler.Instance.GetPooledObject("Explosion", transform.position, transform.rotation);
            AudioManager.instance.PlaySFX2("Explosion");
            this.gameObject.SetActive(false);
        }
    }

    private void ShouldSplit() //If this asteroid is big enough, spawn 2 asteroids that are smaller.
    {
        if (size / 2 >= minSize)
        {
            for (int i = 0; i < 2; i++)
            {
                SplitAsteroid();
            }
        }
    }

    private void AsteroidKilledScore() //Add score by sending the score to the ScoreController, based on the size of the asteroid.
    {
        if(size >= maxSize / 2)
        {
            scoreController.AddScore(scoreLarge);
        }
        else if (size >= maxSize / 4 && size < maxSize / 2)
        {
            scoreController.AddScore(scoreMedium);
        }
        else
        {
            scoreController.AddScore(scoreSmall);
        }
    }

    private void SplitAsteroid()
    {
        Vector2 newAsteroidPosition = transform.position;
        newAsteroidPosition += (Random.insideUnitCircle / 2); //This is used to make the spawn position a bit more random.
        GameObject newAsteroidObject = ObjectPooler.Instance.GetPooledObject("Asteroid", newAsteroidPosition, transform.rotation);
        Asteroid newAsteroid = newAsteroidObject.GetComponent<Asteroid>();
        newAsteroid.currentAsteroidSize--; //New asteroid is smaller.
        newAsteroid.size = this.size / 2;
        newAsteroid.Trajectory(Random.insideUnitCircle.normalized * speed); //Randomize the trajectory
        newAsteroid.transform.localScale = Vector3.one * newAsteroid.size; 
    }
}
