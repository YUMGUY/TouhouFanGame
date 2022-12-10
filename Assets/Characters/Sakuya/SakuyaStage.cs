using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SakuyaStage : MonoBehaviour
{

    [Header("Stage Flags")]
    public bool phase1;
    public bool phase2;
    public bool phase3;
    public SakuyaInfo sakuyaInfoSource;
    public AudioClip BattleStageMusic;

    [Header("bullet spawners and Time Variables")]
    public GameObject Sakuya_;
    public SakuyaInfo Sakuya_Info;
    public specialCustomizationBullets personalSakuyaSpawner;
    public specialCustomizationBullets wineSpawner1;
    public specialCustomizationBullets wineSpawner2;

    // phase 3
    public regularSpawnInfo extraDiff;

    public GameObject[] spawners;
    public GameObject pattern2Spawner;
    public GameObject pattern2Helper;
    public Animator wineBottle1;
    public Animator wineBottle2;
    public AudioClip TimeStopIndicator;
    public AudioSource SakuyaSoundFX;
    public string knifeTag;

    [Header("Sakuya Bullet & Animation Variables")]
    public regularSpawnInfo sakuyaMainSpawnerController;
    public specialCustomizationBullets sakuyaMain_SC;
    public Animator sakuyaController;
    public GameObject ReimuTarget;
    private Vector2 targetPosition_;

    [Header("Battle Flags")]
    private Coroutine battle;
    private Coroutine maidPattern;
    private Coroutine movePhase;
   // private Coroutine spitRoastCoroutine;
    
    [SerializeField]
    private bool startBattle = false;
    
    //private int phaseCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // testing
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    print("stopped phase 1, can now start phase 2 safely");
        //    StopCoroutine(battle);
        //    StopCoroutine(maidPattern);
        //    StopCoroutine(movePhase);
        //}

        //// testing wine anim and spit roast
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    print("Fling wine bottles");
        //  //  sakuyaController.SetTrigger("spitRoast");
            
        //    wineBottle1.SetBool("fling", true);
        //    wineBottle2.SetBool("fling", true);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    // rn manually call shots
        //    wineBottle1.SetTrigger("rightExist");
        //    AnimationClip[] clips = wineBottle1.runtimeAnimatorController.animationClips;
        //    foreach(AnimationClip clip in clips)
        //    {
        //        print(clip.length + ": seconds");
        //    }
        //    wineBottle2.SetTrigger("leftExist");
        //    wineBottle1.SetBool("fling", false);
        //    wineBottle2.SetBool("fling", false);
        //}

        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    GameManager.instance.StartSpecificConvo(2);
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    FreezeAllBullets();
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    UnfreezeAllBullets();
        //}

        // scuffed way to stop the 1st phase and start phase 2
        if (sakuyaInfoSource.currentSakuyaHp <= 0.0f && phase1 == true)
        {
            print("stopped phase 1");
            StopCoroutine(battle);
            phase1 = false;
            phase2 = true;
            Sakuya_Info.canBeDamagedByReimu = false; // stops incoming dmg????
            GameManager.instance.DisableAllBullets();
            sakuyaInfoSource.currentSakuyaHp = .01f;
            GameManager.instance.StartSpecificConvo(1);
         //   StartCoroutine(StartPhase2Coroutine()); // replace this by calling Gamemanager conversation
          
        }
        // 3rd phase started after stopping phase 2
        else if (sakuyaInfoSource.currentSakuyaHp <= 0.0f && phase2 == true)
        {
            print("stopped phase 2, started phase 3");
            Sakuya_Info.canBeDamagedByReimu = false;
            StopCoroutine(battle); // necessary as it could keep on running
            sakuyaInfoSource.currentSakuyaHp = .01f; // scuffed way to ensure next stage is executed only once and doesn't start one and end stage immediately
            phase2 = false;
            phase3 = true;
            StopAllCoroutines();
            GameManager.instance.DisableAllBullets();
            battle = StartCoroutine(SakuyaBattlePhase3());

        }
        else if(sakuyaInfoSource.currentSakuyaHp <= 0.0f && phase3 == true && phase2 == false && phase1 == false)
        {
            print("you defeated Sakuya");
            phase3 = false;
            GameManager.instance.DisableAllBullets();
            StopCoroutine(battle);

            GameManager.instance.StartSpecificConvo(2);

            // destroy wine bottles
            // trigger death animation

            // GameManager kills Sakuya
        }



    }

    // ////////////////////////////////////////////////////// FIRST PATTERN " COMPLIMENTS TO THE CHEF "  //////////////////
    public void MaidLovePattern() // get rid of, no more animation controller for 1st phase, only coroutine
    {
       // sakuyaController.SetTrigger("heartPattern");
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


    /* ///////////////////////////// 2ND PATTERN " SPIT ROAST " //////////////////////////////
     * 
     * Includes TIME STOP, needs pooling and better ideas
     * 
    */
    public void SpitRoast()
    {
        StartCoroutine(SurroundTimeStop(0, 0, .1f)); // store this into coroutine that can be stopped
    }

    public IEnumerator SurroundTimeStop(float gapMin, float gapMax, float gapOffset)
    {
        // play sound effect, yield for time of sound effect
        SakuyaSoundFX.PlayOneShot(TimeStopIndicator);
        yield return new WaitForSeconds(1.1f);

        Color orig = ReimuTarget.GetComponent<SpriteRenderer>().color; // for debugging purposes
        ReimuTarget.GetComponent<Animator>().SetTrigger("Frozen");
        ReimuTarget.GetComponent<ReimuInfo>().canShoot = false;
        ReimuTarget.GetComponent<playerMovement>().playerCanMove = false;
        ReimuTarget.GetComponent<SpriteRenderer>().color = Color.blue;
        print("Sakuya used time stop");
        FreezeAllBullets();

        /*
         effects of time stop, make everything grey/color different and all bullets stop
         
         */

        GameObject[] encircleCollection = new GameObject[30]; // arbitrary number, can be changed to whatever as long as it is even, and the # of iterations match
        GameObject prefabBullet = pattern2Spawner.GetComponent<regularSpawnInfo>().bulletR;
        for (int k = 0; k < 3; ++k)
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

            yield return new WaitForSeconds(.25f);
        }

        yield return new WaitForSeconds(1f);

        UnfreezeAllBullets();
        print("Time has resumed");
        ReimuTarget.GetComponent<playerMovement>().playerCanMove = true;
        ReimuTarget.GetComponent<SpriteRenderer>().color = orig;
        ReimuTarget.GetComponent<ReimuInfo>().canShoot = true;

        // unfreeze bullets layer by layer
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                encircleCollection[(i * 10) + j].GetComponent<regularCustomBehavior>().willAccel = true; // make it accelerate later on
                encircleCollection[(i * 10) + j].GetComponent<regularCustomBehavior>().speedCurve = pattern2Spawner.GetComponent<regularSpawnInfo>().accelerationCurveR;
            }
            yield return new WaitForSeconds(.5f);
        }
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
            //print("accelerated");
            circles[i].GetComponent<regularCustomBehavior>().willAccel = true;
            pattern2Helper.GetComponent<regularSpawnInfo>().spawnBullet();
            yield return new WaitForSeconds(.1f);
           
        }
    }

    // move these 2 functions into game manager
    public void FreezeAllBullets()
    {

        GameObject[] allKnives = GameObject.FindGameObjectsWithTag("knife");
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("regularBullet");
        regularCustomBehavior[] bulletsInScene = FindObjectsOfType<regularCustomBehavior>();
        ReimuBulletInfo[] reimuBullets = FindObjectsOfType<ReimuBulletInfo>();
    
        foreach (GameObject knife in allKnives)
        {
            knife.GetComponent<regularCustomBehavior>().bulletCanMove = false;
        }
        foreach (GameObject bullet in allBullets)
        {
            bullet.GetComponent<regularCustomBehavior>().bulletCanMove = false;
        }
        foreach (regularCustomBehavior bullet in bulletsInScene)
        {
            bullet.bulletCanMove = false;
        }
        foreach (ReimuBulletInfo bullet in reimuBullets)
        {
            bullet.RbulletSpeed = 0;
            bullet.timeStopped = true;
        }
    }
    public void UnfreezeAllBullets()
    {

        GameObject[] allKnives = GameObject.FindGameObjectsWithTag("knife");
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("regularBullet");
        ReimuBulletInfo[] reimuBullets = FindObjectsOfType<ReimuBulletInfo>();

        regularCustomBehavior[] bulletsInScene = FindObjectsOfType<regularCustomBehavior>();
        print(allKnives.Length + "is the length of all knive arrays");
        print(allBullets.Length + "is the length of all bullet arrays");
        foreach (GameObject knife in allKnives)
        {
            knife.GetComponent<regularCustomBehavior>().bulletCanMove = true;
        }
        foreach (GameObject bullet in allBullets)
        {
            bullet.GetComponent<regularCustomBehavior>().bulletCanMove = true;
        }

        foreach(regularCustomBehavior bullet in bulletsInScene)
        {
            bullet.bulletCanMove = true;
        }
        foreach (ReimuBulletInfo bullet in reimuBullets)
        {
            bullet.RbulletSpeed = 10;
            bullet.timeStopped = false;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////


    /* ////////////////////////// 3RD PATTERN " POP THE CHAMPAGNE " /////////////////////////////
         
     */



    //

    /////////////// CODE FOR MOVING SAKUYA //////////////
    


    ////////////////// CODE FOR CONTROLLING FLOW OF BATTLE////////////////////////////////////////////////////////////////
   public IEnumerator SakuyaBattle() // PHASE 1
    {
        print("started battle: so hungry");
        while(phase1 == true)
        {

            // MaidLovePattern();

            // substitute for animation triggering
            maidPattern = StartCoroutine(sakuyaMainSpawnerController.MaidsLove());

            // put some pattern here
            for(int i = 0; i < 3; ++i)
            {
                sakuyaMain_SC.indexSC = i;
                yield return new WaitForSeconds(sakuyaMain_SC.GetBulletData().fireRateSC);
                sakuyaMain_SC.spawnCustomBullet();
            }
            yield return (maidPattern);
            // move to function
            float randNumX = Random.Range(-2, 3);
            float randNumY = Random.Range(-1, 3);
            Vector3 newPosition = Sakuya_.transform.position + new Vector3(randNumX, randNumY, 0);
            if (Sakuya_.transform.position.y > 3f && randNumY > 0)
            {
                randNumY = Random.Range(-1.5f,-.05f);
            }
            if(Sakuya_.transform.position.x > 7 || Sakuya_.transform.position.x < -7 || Sakuya_.transform.position.y > 3 || Sakuya_.transform.position.y < 1.75)
            {
                newPosition = new Vector3(2, 2, 0); // temporary default positions
            }
            movePhase = StartCoroutine(sakuyaInfoSource.MoveToLocation(newPosition));
            
            yield return new WaitForSeconds(3f);
            yield return null;
        }
        print("finished phase 1");
        // call 
 
    }

    // interconnected with StartPhase2
    public void StartPhase1()
    {   
        if(startBattle == false) // start once for Sakuya
        {
            print("started phase 1");
            battle = StartCoroutine(SakuyaBattle());
            GameManager.instance.GetComponent<AudioSource>().clip = BattleStageMusic;
            GameManager.instance.GetComponent<AudioSource>().Play();
            GameManager.instance.GetComponent<AudioSource>().loop = true;
            Sakuya_Info.canBeDamagedByReimu = true;
            startBattle = true;
            
        }
    }

    public IEnumerator StartPhase2Coroutine()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(sakuyaInfoSource.MoveToLocation(new Vector2(0, 2f)));
        print("stage 2 started");
        sakuyaInfoSource.canBeDamagedByReimu = false;
        yield return (StartCoroutine(sakuyaInfoSource.NewMaxHp(800f)));
        yield return new WaitForSeconds(.5f);
        sakuyaInfoSource.canBeDamagedByReimu = true;
        battle = StartCoroutine(SakuyaBattlePhase2());
        yield return null;

    }

    public IEnumerator SakuyaBattlePhase2()
    {
        print("phase 2 started");

        // add a wait for 1.25 second after convo ends
        while(phase2 == true)
        {
            sakuyaController.SetTrigger("spitRoast");
            yield return null;


            yield return new WaitForSeconds(sakuyaController.GetCurrentAnimatorStateInfo(0).length + 5f);

         
            personalSakuyaSpawner.indexSC = 5;
            personalSakuyaSpawner.spawnCustomBullet();
            personalSakuyaSpawner.indexSC = 6;
            personalSakuyaSpawner.spawnCustomBullet();
            personalSakuyaSpawner.indexSC = 7;
            personalSakuyaSpawner.spawnCustomBullet();
            yield return new WaitForSeconds(2f);
            
            
        }
        print("phase 2 stopped");
        yield return null;
    }

    public IEnumerator SakuyaBattlePhase3()
    {
        print("stage 3 started");
        yield return (StartCoroutine(sakuyaInfoSource.NewMaxHp(900f)));
        sakuyaInfoSource.canBeDamagedByReimu = true;
        yield return new WaitForSeconds(1f); // act as buffer
        // show wine bottles , yield  accounting wine bottle animation length, and then shoot
        // rn manually call shots
        //  int random = 0;

        while(true)
        {
            // currently 1 second long
            wineBottle1.SetTrigger("rightExist");
            wineBottle2.SetTrigger("leftExist");
            wineBottle1.SetBool("fling", false);
            wineBottle2.SetBool("fling", false);


            yield return new WaitForSeconds(.9f);
            print("now shoot");

            // also set spiraling
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    //random = Random.Range(0, 2); // 0 for no random angles, 1 for random angles;
                    // if (random == 0) {
                    wineSpawner1.SpawnSpecificBullet(0);
                    wineSpawner2.SpawnSpecificBullet(0);
                    //}
                    //else { 
                    //    wineSpawner1.SpawnSpecificBullet(2);
                    //    wineSpawner2.SpawnSpecificBullet(2);
                    //}
                    wineSpawner1.indexSC = 1;
                    wineSpawner2.indexSC = 1;
                    yield return new WaitForSeconds(wineSpawner1.GetBulletData().fireRateSC);
                    wineSpawner1.spawnCustomBullet();
                    wineSpawner2.spawnCustomBullet();

                }

                yield return new WaitForSeconds(1f);
            }

            // yield seconds
            yield return new WaitForSeconds(.5f);
            Vector2 newPos = generateRandomPosition();
            movePhase = StartCoroutine(sakuyaInfoSource.MoveToLocation(newPos));
            wineBottle1.SetBool("fling", true);
            wineBottle2.SetBool("fling", true);
            // fling wine bottles, 2.997 seconds long
            pattern2Helper.GetComponent<regularSpawnInfo>().spawnBullet();
            pattern2Helper.GetComponent<regularSpawnInfo>().spawnBullet();
            yield return new WaitForSeconds(1f);
            extraDiff.spawnBullet();
            //print("reset");
            yield return new WaitForSeconds(2.8f); // 4 - 2.997
            yield return null;
        }
    }


    public Vector2 generateRandomPosition()
    {

        float newX = Mathf.Clamp(transform.position.x + Random.Range(-2f, 2f), -6f, 6f);
        float newY = Mathf.Clamp(transform.position.y + Random.Range(-2f, 2f), 1.25f, 2.95f);
        Vector2 newPosition = new Vector2(newX, newY);

        return newPosition;
    }

}
