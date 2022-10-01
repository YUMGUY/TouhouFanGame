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
    public bool spawnReverse;

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
                heartBullet.transform.rotation =this.transform.rotation; // parents rotation affects bullet's starting rotation
                heartBullet.transform.position = transform.position + new Vector3( xPos,yPos);

                heartBullet.GetComponent<regularCustomBehavior>().direction = new Vector2(xPos,yPos).normalized; // either make this dirSpawn or a new Vector
                heartBullet.GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed + new Vector2(xPos, yPos).magnitude; // works, forms shape constantly

                heartBullet.GetComponent<regularCustomBehavior>().blifeTime = lifeTimeR;

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

        // wait 1 second before all heart bullets pause
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            mBullets[i].GetComponent<regularCustomBehavior>().bSpeed = 0;

            //Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            //Vector2 dir = player.position - mBullets[i].transform.position;              // this code tracks the player's position once
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
          //  mBullets[i].transform.rotation = Quaternion.Euler(0, 0, angle);                // will have to change angle with knive sprites, +90 if 1 = x, -90 if  1 = y vel
          //  mBullets[i].GetComponent<regularCustomBehavior>().direction = Vector2.right;
            // offset is needed if the sprite isn't pointed right
                                                                                                   /*two different behaviors when direction is being controlled
                                                                                                    * and when the angle is being controlled */
            //mBullets[i].GetComponent<regularCustomBehavior>().direction = dir.normalized;
        }

        // there are many ways for the heart bullets to behave towards the player, will ask for suggestions

        // temporary, for now instead of using animation, just set speed back to original speed

        for(int i = 0; i < numberOfCustomBullets; ++i)
        {
            yield return new WaitForSeconds(.1f);
            Transform player = GameObject.FindGameObjectWithTag("Player").transform; // continuosly tracks Reimu
            Vector2 dir = player.position - mBullets[i].transform.position;
            mBullets[i].GetComponent<regularCustomBehavior>().direction = dir.normalized;
            mBullets[i].GetComponent<regularCustomBehavior>().bSpeed = spawnSpeed;
        }

        print("coroutine MaidsLove done");
        yield return null;
    }

    // surrounds the player with layers of knives with a noticeable gap
    public IEnumerator SurroundTimeStop(float gapMin, float gapMax, float gapOffset)
    {
        yield return null;
    }
}
