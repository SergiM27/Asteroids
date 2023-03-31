using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    private PlayerController playerController;

    public float bonusFireRateTime;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void BonusToApply(int bonusType)
    {
        switch (bonusType)
        {
            case 0: //Fire Rate Bonus
                playerController.bulletCDShoot = playerController.bulletCDShoot / 2;
                StartCoroutine(BonusEnd(bonusFireRateTime, bonusType));
                break;
            case 1: //ExtraLife
                if (GameManager.instance.lives < 3) //If player has 3 lives, he is full hp.
                {
                    GameManager.instance.lives++;
                    GameManager.instance.UILivesUpdate();
                }
                break;
        }
    }

    IEnumerator BonusEnd(float bonusEndTime, int bonusType)
    {
        yield return new WaitForSeconds(bonusEndTime);
        switch (bonusType)
        {
            case 0: //Fire Rate Bonus
                playerController.bulletCDShoot = playerController.bulletCDShoot * 2;
                break;
        }
    }
}
