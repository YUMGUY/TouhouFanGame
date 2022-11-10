using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuyaStage : MonoBehaviour
{


    [Header("bullet spawners and Time Variables")]
    public GameObject Sakuya_;
    public GameObject[] spawners;
    public GameObject pattern2Spawner;
    public GameObject pattern2Helper;
    public Animator wineBottle1;
    public Animator wineBottle2;
    public string knifeTag;

    [Header("Sakuya Info")]
    public int sakuyaHp;
    public Animator sakuyaController;
    public GameObject ReimuTarget;
    private Vector2 targetPosition_;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // testing
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MaidLovePattern();
        }

        // testing wine anim and spit roast
        if (Input.GetKeyDown(KeyCode.T))
        {
            sakuyaController.SetTrigger("spitRoast");
            wineBottle1.SetBool("fling", true);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {

            wineBottle1.SetTrigger("rightExist");
        }
    }

    // ////////////////////////////////////////////////////// FIRST PATTERN " COMPLIMENTS TO THE CHEF "  //////////////////
    public void MaidLovePattern()
    {
        sakuyaController.SetTrigger("heartPattern");
    }

    // control flow of the pattern
    public void SetHeartBool(int number)
    {
        switch(number)
        {
            case 1:
                sakuyaController.SetBool("heart1", false);
                sakuyaController.SetBool("heart2", true);
                sakuyaController.SetBool("heart3", false);
                break;
            case 2:
                sakuyaController.SetBool("heart1", false);
                sakuyaController.SetBool("heart2", false);
                sakuyaController.SetBool("heart3", true);
                break;
            case 3:
                sakuyaController.SetBool("heart1", true);
                sakuyaController.SetBool("heart2", false);
                sakuyaController.SetBool("heart3", false);
                break;
        }
    }

    public IEnumerator ChefsCompliment()
    {
        float time = .1f;
        for(int i = 0; i < 3; ++i)
        {
            foreach (GameObject spawner in spawners)
            {
                StartCoroutine(spawner.GetComponent<regularSpawnInfo>().MaidsLove());
            }
            
            yield return new WaitForSeconds(time);
            time += .025f;
        }
        
        yield return null;
    }
    //////////////////////////////////////////////////////////////////////////////////


    /* ///////////////////////////// 2ND PATTERN " SPIT ROAST " //////////////////////////////
     * 
     * Includes TIME STOP, needs pooling and better ideas
     * 
    */
    public void SpitRoast()
    {
        StartCoroutine(SurroundTimeStop(0, 0, .1f));
    }

    public IEnumerator SurroundTimeStop(float gapMin, float gapMax, float gapOffset)
    {

        Color orig = ReimuTarget.GetComponent<SpriteRenderer>().color; // for debugging purposes

        ReimuTarget.GetComponent<playerMovement>().playerCanMove = false;
        ReimuTarget.GetComponent<SpriteRenderer>().color = Color.blue;
        print("Sakuya used time stop");

        /*
         effects of time stop, make everything grey and all bullets stop
         
         */

        GameObject[] encircleCollection = new GameObject[40]; // arbitrary number, can be changed to whatever as long as it is even, and the # of iterations match
        GameObject prefabBullet = pattern2Spawner.GetComponent<regularSpawnInfo>().bulletR;
        for (int k = 0; k < 4; ++k)
        {
            for (int i = 0; i < 10; ++i)
            {
                
                GameObject bullet = Instantiate(prefabBullet);

                // optimizable
                float angle = (i * 2f * Mathf.PI) / 10;

                float x = Mathf.Cos(angle + (k * gapOffset)) * (2 + (k * gapOffset)); // multiplied by radius
                float y = Mathf.Sin(angle + (k * gapOffset)) * (2 + (k * gapOffset));

                targetPosition_ = ReimuTarget.transform.position + new Vector3(x, y);
                bullet.transform.position = targetPosition_;
                bullet.GetComponent<regularCustomBehavior>().blifeTime = pattern2Spawner.GetComponent<regularSpawnInfo>().lifeTimeR;
                bullet.GetComponent<regularCustomBehavior>().bSpeed = 0f;
                // new way of rotating bullet towards the player, instead of setting velocity to -1
                Vector2 dir = ReimuTarget.transform.position - bullet.transform.position;
                bullet.GetComponent<regularCustomBehavior>().direction = Vector2.right;
                float angle2 = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle2);

                encircleCollection[(k * 10) + i] = bullet;
            }

            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);

        print("Time has resumed");
        ReimuTarget.GetComponent<playerMovement>().playerCanMove = true;
        ReimuTarget.GetComponent<SpriteRenderer>().color = orig;

        // unfreeze bullets layer by layer
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                encircleCollection[(i * 10) + j].GetComponent<regularCustomBehavior>().willAccel = true; // make it accelerate later on
                encircleCollection[(i * 10) + j].GetComponent<regularCustomBehavior>().speedCurve = pattern2Spawner.GetComponent<regularSpawnInfo>().accelerationCurveR;
            }
            yield return new WaitForSeconds(.5f);
        }

       // StartCoroutine(CirclingBullets());

        yield return null;
    }
    public IEnumerator CirclingBullets()
    {
        // spawn circling bullets
        GameObject circleBullet = pattern2Helper.GetComponent<regularSpawnInfo>().bulletR;
        GameObject[] circles = new GameObject[10];

        for (int i = 0; i < circles.Length; ++i)
        {
            GameObject circlingB = Instantiate(circleBullet, pattern2Helper.transform);
            circlingB.transform.position = pattern2Helper.transform.position;
            float angle = (i * 2f * Mathf.PI * Mathf.Rad2Deg) / circles.Length;
            circlingB.transform.rotation = Quaternion.Euler(0, 0, angle);
           // circlingB.transform.localScale = new Vector3(1, 1, 0) ;
            circlingB.GetComponent<regularCustomBehavior>().direction = Vector2.right;
            circlingB.GetComponent<regularCustomBehavior>().blifeTime = pattern2Helper.GetComponent<regularSpawnInfo>().lifeTimeR;
            circlingB.GetComponent<regularCustomBehavior>().willAccel = pattern2Helper.GetComponent<regularSpawnInfo>().accelBullets;
            circlingB.GetComponent<regularCustomBehavior>().speedCurve = pattern2Helper.GetComponent<regularSpawnInfo>().accelerationCurveR;
            circles[i] = circlingB;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < circles.Length; ++i)
        {
            circles[i].GetComponent<regularCustomBehavior>().bSpeed = 0;
            circles[i].GetComponent<regularCustomBehavior>().timeAcceleration = 0;
            circles[i].GetComponent<regularCustomBehavior>().willAccel = false;
        }

        yield return new WaitForSeconds(.3f);
        for (int i = 0; i < circles.Length; ++i)
        {   
            circles[i].GetComponent<regularCustomBehavior>().willAccel = true;
            pattern2Helper.GetComponent<regularSpawnInfo>().spawnBullet();
           
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////
    
  
    /* ////////////////////////// 3RD PATTERN " POP THE CHAMPAGNE " /////////////////////////////
         
     */



    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        //account for reimu bullet
        /*if(other.CompareTag()) {
        
        }
        */
    }

}
