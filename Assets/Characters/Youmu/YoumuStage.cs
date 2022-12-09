using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuStage : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Youmu General Variables")]
    public YoumuInfo Youmu_;

    [Header("Phase 1 Variables")]
    public sineSpawner[] waveMotion;
    public regularSpawnInfo mainPhase1;
    public sineSpawner sprinkler;
    public bool Phase1YoumuFinished;

    [Header("Phase 2 Variables")]
    public bool Phase2YoumuFinished;
    public regularSpawnInfo[] Sides;

    [Header("Phase 3 Variables")]
    public bool Phase3YoumuFinished;

    private Coroutine youmuBattle;

    void Start()
    {
        Youmu_.YoumucanBeDamagedByReimu = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Youmu_.currentYoumuHp <= 0 && Phase1YoumuFinished == false)
        {
            foreach (sineSpawner p1spawner in waveMotion)
            {
                p1spawner.autoSpawn = false;
            }
            GameManager.instance.DisableAllBullets();
            StopCoroutine(youmuBattle);
            Phase1YoumuFinished = true;
            Youmu_.YoumucanBeDamagedByReimu = false;
            // start talking
            GameManager.instance.StartSpecificConvo(1);
        }
        if (Youmu_.currentYoumuHp <= 0 && Phase2YoumuFinished == false) // finish phase 2
        {
            foreach (sineSpawner p1spawner in waveMotion)
            {
                p1spawner.autoSpawn = false;
            }
            GameManager.instance.DisableAllBullets();
            StopCoroutine(youmuBattle);

            Phase2YoumuFinished = true;
            Youmu_.YoumucanBeDamagedByReimu = false;
            // move on to phase 3
        }
    }


    public void StartYoumuPhase1()
    {
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
        yield return new WaitForSeconds(.2f);
        Youmu_.YoumucanBeDamagedByReimu = true;

        foreach(regularSpawnInfo side in Sides)
        {
            side.spawnerRotating = true;
        }

        while (true)
        {
            yield return null;
        }

    }

    public IEnumerator YoumuPhase3(int[] petalNumbers)
    {



        yield return null;
    }
}
