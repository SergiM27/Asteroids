using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnTime, substractSpawnTime, amountOfObjects, spawnDistance, angleOfVariation;

    [Header("Bonus")]
    public int spawnBonusPercentage;
    public int bonusTypes;

    // Start is called before the first frame update
    void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    public void Spawn()
    {
        int r = Random.Range(0, spawnBonusPercentage);
        for (int i = 0; i < amountOfObjects; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance; //Assigns the direction from the middle of the screen as a circle around the screen.
            Vector3 spawnPoint = this.transform.position + spawnDirection; //Assigns where the asteroid will be spawned based on the direction given by spawnDirection.

            float directionVariance = Random.Range(-angleOfVariation, angleOfVariation); //A little variation to the angle in which the asteroid enters the screen, so not all asteroids go to the center.
            Quaternion rotation = Quaternion.AngleAxis(directionVariance,Vector3.forward); //Variation is assigned to the rotation variable.

            if (r != 0) //If r!=0, spawn an asteroid.
            {
                GameObject newAsteroid = ObjectPooler.Instance.GetPooledObject("Asteroid", spawnPoint, rotation);
                Vector2 asteroidDirection = rotation * -spawnDirection; //The asteroid will go to negative spawn direction (to the center of the screen), with the variation that rotation has.
                newAsteroid.GetComponent<Asteroid>().Trajectory(asteroidDirection); //Add force to the asteroid.
            }
            else //If r==0, spawn a bonus instead of an asteroid.
            {
                int whatBonus = Random.Range(0, bonusTypes);
                Vector2 bonusDirection = rotation * -spawnDirection;
                GameObject newBonus;
                switch (whatBonus)
                {
                    case 0:
                        newBonus = ObjectPooler.Instance.GetPooledObject("FireRateBonus", spawnPoint, rotation);
                        break;
                    case 1:
                        newBonus = ObjectPooler.Instance.GetPooledObject("ExtraLifeBonus", spawnPoint, rotation);
                        break;
                    default:
                        newBonus = null;
                        break;
                }
                if(newBonus!=null) newBonus.GetComponent<Bonus>().Trajectory(bonusDirection);
            }   
        }
    }
}
