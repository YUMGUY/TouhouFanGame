using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularSpawnInfo : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Spawn Resource")]
    public GameObject bulletR;
    public GameObject bulletPoolR;


    [Header("spawn variables")]
    public int numCol;
    public float spawnSpeed;
    public float lifeTimeR;
    public Vector2 dirSpawn;
    public float spawnRateR;

    [SerializeField]
    private float spawnRateTimerR;
    


    [Header("Rotation Variables")]
    public bool randomAngles;
    public float minRotationR;
    public float maxRotationR;
    public float[] rotationsR;


    [Header("Custom Variables")]
    public bool makeHeart;
    public int numberOfCustomBullets;

    void Start()
    {
        rotationsR = new float[numCol];
    }

    // Update is called once per frame
    void Update()
    {
        // rn it'll spawn automatically
        spawnRateTimerR -= Time.deltaTime;
        if(makeHeart == false)
        {
            if(spawnRateTimerR <= 0)
            {
                spawnBullet();
                spawnRateTimerR = spawnRateR;
            }
        }

        else if(makeHeart == true)
        {
            if(Input.GetKeyDown(KeyCode.H))
            {
                print("spawnHeart");
                spawnHeart();
            }
        }
        
    }



    public void spawnBullet()
    {
        if(randomAngles == false)
        {
            DistributedRotationsR();
        }
        else { RandomRotations(); }
        for(int i = 0; i < numCol; ++i)
        {
            GameObject rBullet =bulletPoolR.GetComponent<simpleBulletPool>().GetPooledObject();

            if(rBullet != null )
            {
                // set it at the origin, which is at the spawner's location and set angle of the bullet
                rBullet.transform.position = this.transform.position;
                float angle = rotationsR[i] + transform.eulerAngles.z; // rn it'll align with the parent(spawner's) rotation
                rBullet.transform.rotation = Quaternion.Euler(0, 0, angle);


                // manipulate values of regular bullet's attached script
                rBullet.GetComponent<regularCustomBehavior>().direction = dirSpawn;
                rBullet.GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed;
                rBullet.GetComponent<regularCustomBehavior>().blifeTime = lifeTimeR;

                rBullet.SetActive(true);


            }
        }
    }

    public void DistributedRotationsR()
    {
        float angle = 0;
        float angleInc = (maxRotationR - minRotationR) / numCol;
        for(int i = 0; i < numCol; ++i)
        {
            rotationsR[i] = angle;
            angle += angleInc;
        }
    }

    public void RandomRotations()
    {
        
        for(int i = 0; i < numCol; ++i)
        {
            rotationsR[i] = Random.Range(minRotationR, maxRotationR);
        }
    }

    // can make the heart a prefab as well
    public void spawnHeart()
    {
        float t = 0;
        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            print("spawned heart bullets");
            GameObject heartBullet = bulletPoolR.GetComponent<simpleBulletPool>().GetPooledObject();
            t = (i * 2f * Mathf.PI)/numberOfCustomBullets;
            float xPos = 4 * Mathf.Pow(Mathf.Sin(t), 3);
            float yPos = .25f * (13 * Mathf.Cos(t) - 5 * Mathf.Cos(2 * t) - 2 * Mathf.Cos(3 * t) - Mathf.Cos(4 * t));
            if (heartBullet != null)
            {
                heartBullet.transform.position = transform.position + new Vector3(xPos, yPos);

                heartBullet.GetComponent<regularCustomBehavior>().direction = new Vector2(xPos,yPos).normalized; // either make this dirSpawn or a new Vector
                heartBullet.GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed;

                heartBullet.GetComponent<regularCustomBehavior>().blifeTime = lifeTimeR;
               
                heartBullet.SetActive(true);
            }
           
        }
       
        
        
        
    }

    // set plans for preventing bad allocation of rotation = new everytime new pattern is chosen
}
