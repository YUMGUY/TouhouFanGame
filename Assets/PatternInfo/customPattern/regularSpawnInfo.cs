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
    public int newNumCol; // this controls numCol since I don't want to make a new array constantly 

    public float spawnSpeed;
    public float lifeTimeR;
    public Vector2 dirSpawn;
    public float spawnRateR;
    public bool willSpawnAuto;



    [SerializeField]
    private float spawnRateTimerR;
    


    [Header("Rotation Variables")]
    public bool randomAngles;
    public float minRotationR;
    public float maxRotationR;
    public float[] rotationsR;


    [Header("Custom Variables")]
    public GameObject Reimu;
    public bool makeHeart;
    public bool spawnReverse;
    public bool accelBullets;
    public int numberOfCustomBullets;
    public AnimationCurve accelerationCurveR;
    private Vector2 targetPosition;



    // circumvent resizing array constantly by checking once, could do a void function that gets called within each attack pattern ********NOTE*************

    void Start()
    {
        rotationsR = new float[numCol];
        
    }
    // Update is called once per frame
    void Update()
    {
        // resize rotation array when needed
        if (newNumCol != numCol)
        {
            Debug.Log("Changed rotations array size within if statement");
            numCol = newNumCol;
            rotationsR = new float[numCol];
        }
        //if(Input.GetKeyDown(KeyCode.O))
        //{
        //    newNumCol++;
        //}

      
        // rn it'll spawn automatically
        spawnRateTimerR -= Time.deltaTime;
        if(willSpawnAuto == true)
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
                StartCoroutine(MaidsLove());
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
                rBullet.GetComponent<regularCustomBehavior>().blifeTime = lifeTimeR;

                if(accelBullets == true)
                {
                    rBullet.GetComponent<regularCustomBehavior>().willAccel = true;
                    rBullet.GetComponent<regularCustomBehavior>().speedCurve = accelerationCurveR;
                }
                else { rBullet.GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed; }

                rBullet.SetActive(true);
            }
        }
    }

    // have to add a check for resizing the array
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
   GameObject[] spawnHeart()
    {
        print("spawned heart bullets"); // for testing
        GameObject[] heartCollection = new GameObject[numberOfCustomBullets];
        float t = 0;
        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            
            GameObject heartBullet = bulletPoolR.GetComponent<simpleBulletPool>().GetPooledObject();
            t = (i * 2f * Mathf.PI)/numberOfCustomBullets;
            float xPos = .25f * Mathf.Pow(Mathf.Sin(t), 3);
            float yPos = .5f * .03125f * (13 * Mathf.Cos(t) - 5 * Mathf.Cos(2 * t) - 2 * Mathf.Cos(3 * t) - Mathf.Cos(4 * t));
            if (heartBullet != null)
            {
                // reset variables
                float angle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;
                heartBullet.transform.rotation = Quaternion.Euler(0, 0, angle); // parents rotation affects bullet's starting rotation
                heartBullet.transform.position = transform.position + new Vector3( xPos,yPos);

                heartBullet.GetComponent<regularCustomBehavior>().direction = Vector2.right; // either make this dirSpawn or a new Vector
                heartBullet.GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed + new Vector2(xPos, yPos).magnitude; // works, forms shape constantly

                heartBullet.GetComponent<regularCustomBehavior>().blifeTime = lifeTimeR;
                //reset acceleration variables
                heartBullet.GetComponent<regularCustomBehavior>().timeAcceleration = 0f;
                heartBullet.GetComponent<regularCustomBehavior>().willAccel = false;

                // populate the Bullets being spawned array
                heartCollection[i] = heartBullet;
                heartBullet.SetActive(true);  
            }
           
        }
        if(spawnReverse)
        {
            for(int i = 0; i < heartCollection.Length/2; ++i)
            {
                GameObject temp = heartCollection[i];
                heartCollection[i] = heartCollection[heartCollection.Length - i - 1];
                heartCollection[heartCollection.Length - i - 1] = temp;
            }
        }

        return heartCollection; 
    }

    // set plans for preventing bad allocation of rotation = new everytime new pattern is chosen

    public IEnumerator MaidsLove()
    {
        GameObject[] mBullets = new GameObject[numberOfCustomBullets];
        mBullets = spawnHeart();

        // wait 2 seconds before all heart bullets pause
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            mBullets[i].GetComponent<regularCustomBehavior>().bSpeed = 0;
            mBullets[i].GetComponent<regularCustomBehavior>().direction = Vector2.right;
        }

        // there are many ways for the heart bullets to behave towards the player, will ask for suggestions, rn kinda looks pretty
        
        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            yield return new WaitForSeconds(.01f);
            // continuosly tracks Reimu
            Vector2 dir = Reimu.transform.position - mBullets[i].transform.position;              // this code tracks the player's position once
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            mBullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);

            // custom accelerate the bullets
            mBullets[i].GetComponent<regularCustomBehavior>().willAccel = accelBullets;
            mBullets[i].GetComponent<regularCustomBehavior>().speedCurve = accelerationCurveR;

        }

        yield return null;
    }

    
}
