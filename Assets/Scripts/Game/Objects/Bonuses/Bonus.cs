using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private BonusController bonusController;
    public float speed;

    [Header("DestroyTime")]
    public float destroyTime;

    [Header("Bonus Type")]
    public int bonusType;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        bonusController = FindObjectOfType<BonusController>();
        switch (bonusType)
        {
            case 0:
                this.transform.parent = GameObject.Find("FireRateBonuses").transform;
                this.name = "FireRateBonus";
                break;
            case 1:
                this.transform.parent = GameObject.Find("ExtraLifesBonuses").transform;
                this.name = "ExtraLifeBonus";
                break;
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        StopCoroutine("DestroyMyself"); //Since an object pool is used, we need to cancel the coroutine done before, to create a new one.
        StartCoroutine("DestroyMyself");
    }

    public void Trajectory(Vector2 direction)
    {
        _rigidBody.AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bonusController.BonusToApply(bonusType);
            ObjectPooler.Instance.GetPooledObject("Explosion", transform.position, transform.rotation);
            switch (bonusType)
            {
                case 0:
                    AudioManager.instance.PlaySFX2("FireRateBonus");
                    break;
                case 1:
                    AudioManager.instance.PlaySFX2("ExtraLifeBonus");
                    break;
            }
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyMyself()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.SetActive(false);
    }
}
