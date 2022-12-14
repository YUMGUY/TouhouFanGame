using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineSpawner : MonoBehaviour
{
    [Header("spawnResource")]
    public GameObject bulletPrefab;
     public GameObject bulletPoolSine;
    public float lifeTimeM;



    [Header("Spawn Variables")]
    public bool autoSpawn;
    public int numberOfBulletColumns;
    public float bulletmSpeed;
    public float mCompression;
    public float mAmplitude;
    public bool doubleHelix;
    public float spawnTimer;
    public float spawnRate;

    [Header("Acceleration Variables")]
    public bool sineSpeedAcceleration;
    public AnimationCurve sineSpeedC;


    [Header("Rotations Info")]
    public bool customRotations;
    public bool parentControl;
    public float[] rotations;
    public float minRotation;
    public float maxRotation;



    [Header("custom bullet variables")]
    public bool reverseBullets;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    GameObject[] bulletsInScene = GameObject.FindGameObjectsWithTag("sineBullet1");
        //    print(bulletsInScene.Length);
        //}
       


        if (spawnTimer <= 0)
        {   
            if(autoSpawn)
            {
                SpawnRegularBullets(mAmplitude, mCompression, bulletmSpeed);
            }
          

            // spawn two helixes
            if (doubleHelix)
            {
                SpawnRegularBullets(-1 * mAmplitude, mCompression, bulletmSpeed);
            }

            // reset spawnTimer after everything is finished
            spawnTimer = spawnRate;
        }



    }

    // simple way of spawning bullets
    public void SpawnRegularBullets(float amplitude, float compression, float speed)
    {
        // setting angle later in DistributedRotations, figure out how to resize array accordingly, maybe old bulletsize, new bulletsize, 
        // check if new bullet size > old bullet size, then make rotation = new?
        float angle = 0;
        float increment = 0;
        if(numberOfBulletColumns == 1) { increment = minRotation; }
        increment = ((maxRotation - minRotation) / (numberOfBulletColumns - 1));
        for (int i = 0; i < numberOfBulletColumns; ++i)
        {
            GameObject bullet1 = bulletPoolSine.GetComponent<simpleBulletPool>().GetPooledObject();
            if(bullet1 != null)
            {

                bullet1.transform.position = transform.position;

                if (parentControl == true)
                {   
                    // so that min and max rotation affect where the bullets are going
                    bullet1.transform.eulerAngles = new Vector3(0, 0, angle + minRotation + transform.eulerAngles.z);
                }
                // currently the bullets being spawned are not parented to the parent's rotation
                else
                {
                    bullet1.transform.eulerAngles = new Vector3(0, 0, angle + minRotation);
                }
                // these resets are very important, lifetime and distance travelled
                bullet1.GetComponent<sineWaveInfo>().lifeTime = lifeTimeM;
                bullet1.GetComponent<sineWaveInfo>().distanceTravelled = 0;
                bullet1.GetComponent<sineWaveInfo>().amplitude = amplitude;

                //if(sineSpeedAcceleration == true)
                //{

                //}

                bullet1.GetComponent<sineWaveInfo>().speed = speed;
                bullet1.GetComponent<sineWaveInfo>().compression = compression;
                angle += increment;

                bullet1.SetActive(true);
               
            }
            
        }
        return;
    }

    // Should be a function that's called b/c if not, it will affect all bullets regardless of whether they are spawned
    public void ReverseBullets()
    {
        if (reverseBullets == true)
        {
            GameObject[] bulletsInScene = GameObject.FindGameObjectsWithTag("sineBullet1"); // rn the only prefab with a bullet tag
            foreach (GameObject bullet in bulletsInScene)
            {
                bullet.GetComponent<sineWaveInfo>().speed = -1 * bulletmSpeed;
            }
        }
        else
        {
            GameObject[] bulletsInScene = GameObject.FindGameObjectsWithTag("sineBullet1");
            foreach (GameObject bullet in bulletsInScene)
            {
                bullet.GetComponent<sineWaveInfo>().speed = bulletmSpeed;
            }
        }
    }
    public void DistributedRotations()
    {

    }

    
}
