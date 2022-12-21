using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuStage : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Youmu General Variables")]
    public YoumuInfo Youmu_;
    public GameObject ReimuStage2;
    public AudioClip YoumuStageTheme;

    [Header("Phase 1 Variables")]
    public sineSpawner[] waveMotion;
    public regularSpawnInfo mainPhase1;
    public sineSpawner sprinkler;
    public bool Phase1YoumuFinished;

    [Header("Phase 2 Variables")]
    public bool Phase2YoumuFinished;
    public regularSpawnInfo[] Sides;
    public specialCustomizationBullets burst;
    public GameObject seedShooter;
    public regularSpawnInfo seedSpawner;

    [Header("Phase 3 Variables")]
    public bool Phase3YoumuFinished;
    public GameObject flowerSpawnerLocation;
    public GameObject spiraler;
    public regularSpawnInfo flowerTheme;
    public AnimationCurve[] flowerSpeeds;
    public AnimationCurve[] flowerCoreSpeeds;
    public simpleBulletPool flowerCorePool;
    public simpleBulletPool flowerPetalPool;
    public SpriteRenderer coreBounds;
    public int[] petalCounts;
    public Sprite[] petalColors;
    public Sprite[] flowerCoreColors;

    private Coroutine youmuBattle;

    void Start()
    {
        Youmu_.YoumucanBeDamagedByReimu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(youmuBattle);
            // FlowerBloom(5, flowerSpeeds[0], coreBounds.bounds);
           // print("it started");
           // youmuBattle = StartCoroutine(YoumuPhase3(petalCounts));
        }
        if(Youmu_.currentYoumuHp <= 0 && Phase1YoumuFinished == false)
        {
            foreach (sineSpawner p1spawner in waveMotion)
            {
                p1spawner.autoSpawn = false;
            }
            GameManager.instance.DisableAllBullets();
            StopCoroutine(youmuBattle);
            Youmu_.currentYoumuHp = .1f;
            Phase1YoumuFinished = true;
            Youmu_.YoumucanBeDamagedByReimu = false;
            // start talking
            GameManager.instance.StartSpecificConvo(1);
        }
        else if (Youmu_.currentYoumuHp <= 0 && Phase2YoumuFinished == false && Phase1YoumuFinished == true) // finish phase 2
        {
            print("finished phase 2");
            foreach(regularSpawnInfo side in Sides)
            {
                side.gameObject.SetActive(false);
            }
            GameManager.instance.DisableAllBullets();
            StopCoroutine(youmuBattle);
            Youmu_.currentYoumuHp = .1f;
            Phase2YoumuFinished = true;
            Youmu_.YoumucanBeDamagedByReimu = false;
            // move on to phase 3
            youmuBattle = StartCoroutine(YoumuPhase3(petalCounts));
        }

        else if (Youmu_.currentYoumuHp <= 0 && Phase3YoumuFinished == false && Phase2YoumuFinished == true && Phase1YoumuFinished == true) // finish phase 2
        {

            GameManager.instance.DisableAllBullets();
            StopCoroutine(youmuBattle);

            Phase3YoumuFinished = true;
            this.gameObject.SetActive(false);


            // show victory screen, blur everything, show score and msg about more for later
        }
    }


    public void StartYoumuPhase1()
    {
        // play audio
        GameManager.instance.GetComponent<AudioSource>().clip = YoumuStageTheme; // redundant
        GameManager.instance.GetComponent<AudioSource>().Play();
        GameManager.instance.GetComponent<AudioSource>().loop = true; 
        youmuBattle = StartCoroutine(YoumuPhase1());
    }
    public void StartYoumuPhase2()
    {
        youmuBattle = StartCoroutine(YoumuPhase2());
    }
    public IEnumerator YoumuPhase1()
    {
        Youmu_.YoumucanBeDamagedByReimu = true;

        float angleInc = 45f;
        foreach(sineSpawner p1spawner in waveMotion)
        {
            p1spawner.autoSpawn = true;
        }

        while(true)
        {
            // clockwise
            for(int i = 1; i < 4; ++i)
            {
                sprinkler.transform.rotation = Quaternion.Euler(0, 0, 360 - angleInc * i);
                sprinkler.SpawnRegularBullets(.2f, 2, 2.5f);
                yield return new WaitForSeconds(.2f);
            }

            yield return new WaitForSeconds(2.5f);
            // counter clockwise
            for (int i = 1; i < 4; ++i)
            {
                sprinkler.transform.rotation = Quaternion.Euler(0, 0, 135 + angleInc * i);
                sprinkler.SpawnRegularBullets(.2f, 2, 2.5f);
                yield return new WaitForSeconds(.2f);
            }

            yield return new WaitForSeconds(2f);
            yield return null;
        }
    }

    // Shoot Flowers that pause then shoot out their petals in a sine pattern
    public IEnumerator YoumuPhase2()
    {
        Youmu_.YoumucanBeDamagedByReimu = false;
        yield return StartCoroutine(Youmu_.YNewMaxHp(800.0f));
        yield return new WaitForSeconds(.5f);
        Youmu_.YoumucanBeDamagedByReimu = true;

        foreach(regularSpawnInfo side in Sides)
        {
            side.spawnerRotating = true;
        }

        while (true)
        {

            // spawn 10 then rest spawn 20 then rest
            for(int i = 0; i < 8; ++i)
            {
                Sides[0].spawnBullet();
                Sides[1].spawnBullet();
                yield return new WaitForSeconds(.25f);
            }
            StartCoroutine(Youmu_.YMoveToLocation(new Vector2(-6.2f, 3f), 1.75f));
        

            for(int i = 0; i < 15; ++i)
            {
                TrackReimu(ReimuStage2.transform, seedShooter);
                seedSpawner.spawnBullet();
                yield return new WaitForSeconds(.1f);
            }
            yield return new WaitForSeconds(.7f);
            StartCoroutine(Youmu_.YMoveToLocation(new Vector2(6.2f, 3f), 1.75f));
            for (int i = 0; i < 15; ++i)
            {
                TrackReimu(ReimuStage2.transform, seedShooter);
                seedSpawner.spawnBullet();
                yield return new WaitForSeconds(.1f);
            }

            yield return new WaitForSeconds(.3f);
            TrackReimu(ReimuStage2.transform, burst.gameObject);
            // burst
            for (int i = 0; i < 4; ++i)
            {
               
                burst.SpawnSpecificBullet(i);
                yield return new WaitForSeconds(burst.GetBulletData().fireRateSC);
            }
            yield return new WaitForSeconds(2.5f);


            yield return null;
        }

    }

    public IEnumerator YoumuPhase3(int[] petals)
    {
        Youmu_.YoumucanBeDamagedByReimu = false;
        yield return StartCoroutine(Youmu_.YNewMaxHp(1000f));
        yield return new WaitForSeconds(.5f);
        Youmu_.YoumucanBeDamagedByReimu = true;

        while (true)
        {

            List<GameObject> flowers = FlowerBloom(5, 3, flowerSpeeds[0],flowerCoreSpeeds[0], coreBounds.bounds, petalColors[1], flowerCoreColors[0]);
            
            yield return new WaitForSeconds(1f);

            // inner ring
            for(int i = 0; i < flowers.Count; ++i)
            {
                int childCount = flowers[i].transform.childCount;
                for (int k = 0; k < childCount; ++k)
                {
                    flowers[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().willAccel = true;
                   // flowers[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().speedCurve = flowerSpeeds[0];
                    flowers[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().timeAcceleration = 0;
                    yield return new WaitForSeconds(.01f);
                
                    //  flowers[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().bSpeed = 1.0f;

                }
                flowers[i].transform.DetachChildren();
            }
            yield return new WaitForSeconds(1.5f);
            List<GameObject> flowers2 = FlowerBloom(6, 5, flowerSpeeds[0], flowerCoreSpeeds[1], coreBounds.bounds, petalColors[0], flowerCoreColors[1]);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < flowers2.Count; ++i)
            {
                int childCount = flowers2[i].transform.childCount;
                for (int k = 0; k < childCount; ++k)
                {
                    flowers2[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().willAccel = true;
                  
                    flowers2[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().timeAcceleration = 0;
                    yield return new WaitForSeconds(.01f);

                    //  flowers[i].transform.GetChild(k).GetComponent<regularCustomBehavior>().bSpeed = 1.0f;

                }
                flowers2[i].transform.DetachChildren();
            }

            yield return new WaitForSeconds(2f);

            // spiral hunger bullets




            yield return StartCoroutine(Youmu_.YMoveToLocation( GenerateRandomPositionYoumu(), 1.5f ));
            yield return null;
        }


       


        
    }

    private void TrackReimu(Transform reimu, GameObject tracker)
    {
        Vector2 dir = reimu.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tracker.transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public List<GameObject> FlowerBloom(int numCores,int petals, AnimationCurve speedPetals,AnimationCurve speedCore , Bounds core, Sprite petalColor, Sprite coreColor)
    {
        // store cores

        // get pooled petals
        List<GameObject> cores = new List<GameObject>();

        for (int i = 0; i < numCores; ++i)
        {
            float angle = (i * 360f) / numCores;
            GameObject flowerCore = flowerCorePool.GetComponent<simpleBulletPool>().GetPooledObject();

            flowerCore.transform.position = this.transform.position;
            flowerCore.transform.rotation = Quaternion.Euler(0, 0, angle);

            flowerCore.GetComponent<regularCustomBehavior>().direction = flowerTheme.dirSpawn;
            flowerCore.GetComponent<regularCustomBehavior>().blifeTime = flowerTheme.lifeTimeR;

            flowerCore.GetComponent<regularCustomBehavior>().willAccel = true;
            flowerCore.GetComponent<regularCustomBehavior>().timeAcceleration = 0;
            flowerCore.GetComponent<regularCustomBehavior>().speedCurve = speedCore;


            flowerCore.GetComponent<SpriteRenderer>().sprite = coreColor;
            flowerCore.SetActive(true);
            // instantiate petals around
            for(int j = 0; j < petals; ++j)
            {
                float angleToCore = (j * Mathf.PI * 2f) / petals;

                float x = Mathf.Cos(angleToCore)  *  (core.extents.x + .1f); // multiplied by radius
                float y = Mathf.Sin(angleToCore) * (core.extents.y + .1f);
              

                GameObject flowerPetal = flowerPetalPool.GetComponent<simpleBulletPool>().GetPooledObject();
                flowerPetal.transform.position = flowerCore.transform.position + new Vector3(x, y, 0);
                flowerPetal.transform.rotation = Quaternion.Euler(0, 0, angleToCore * Mathf.Rad2Deg + transform.eulerAngles.z); // rotation possible
                flowerPetal.transform.SetParent(flowerCore.transform);

                flowerPetal.GetComponent<regularCustomBehavior>().bSpeed = 0.0f;
                flowerPetal.GetComponent<regularCustomBehavior>().blifeTime = flowerTheme.lifeTimeR;
                flowerPetal.GetComponent<regularCustomBehavior>().direction = flowerTheme.dirSpawn;
                flowerPetal.GetComponent<regularCustomBehavior>().timeAcceleration = 0;
                flowerPetal.GetComponent<regularCustomBehavior>().willAccel = false;
                flowerPetal.GetComponent<regularCustomBehavior>().speedCurve = flowerSpeeds[0];


                flowerPetal.GetComponent<SpriteRenderer>().sprite = petalColor;
                flowerPetal.SetActive(true);
            }


            cores.Add(flowerCore);

        }

        
        return cores;
    }


    public Vector2 GenerateRandomPositionYoumu()
    {

        float newX = Mathf.Clamp(transform.position.x + Random.Range(-2f, 2f), -6f, 6f);
        float newY = Mathf.Clamp(transform.position.y + Random.Range(-2f, 2f), 1.25f, 2.95f);
        Vector2 newPosition = new Vector2(newX, newY);

        return newPosition;
    }
}
