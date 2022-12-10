using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialCustomizationBullets : MonoBehaviour
{

    [Header("CustomBulletResourceHolder")]
    public GameObject bulletSC;
    public GameObject bulletPoolSC;
    [Header("Spawning variables")]
    // accesses bullet data
    public int indexSC; 
    public bool randomSequenceOfPatterns;
    public bool willSpawnAuto;
    public SpecialBulletData[] customDataPatterns;

    public float timerSC;
    public float[] rotationsSC;

    [Header("Current Bullet Properties")]
    [SerializeField]
    private Vector2 currDirSD;
    [SerializeField]
    private float currSCspeed;
    GameObject specialBullet;

    void Start()
    {
        timerSC = GetBulletData().fireRateSC;
        rotationsSC = new float[GetBulletData().numScols];
    
    }

    // helper to access bullet data more easily
    public SpecialBulletData GetBulletData() // rn needs at least 1 bullet object inn the array
    {
        
        return customDataPatterns[indexSC];
    }
    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.G))
        //{
            
        //    indexSC++;
            
        //}
        // to avoid any potential error in adding indexSC to access the bullet data array
        if(indexSC >= customDataPatterns.Length)
        {
            indexSC = 0;
        }
        if(rotationsSC.Length != GetBulletData().numScols)
        {
            print("custom rotation size changed");
            rotationsSC = new float[GetBulletData().numScols];
        }

       
        if(timerSC <= 0f)
        {
            
            if(willSpawnAuto == true)
            {
                
                spawnCustomBullet();
                indexSC = (indexSC + 1) % customDataPatterns.Length;
            }
            else if(randomSequenceOfPatterns == true)
            {
                indexSC = Random.Range(0, customDataPatterns.Length);
            }
            timerSC = GetBulletData().fireRateSC;
        }
        timerSC -= Time.deltaTime;
    }


    public void DistributedRotationsSC()
    {
        float angle = 0;
        if(GetBulletData().numScols == 1)
        {
            rotationsSC[0] = GetBulletData().minAngle;
            return;
        }
        float angleInc = (GetBulletData().maxAngle - GetBulletData().minAngle) / (GetBulletData().numScols - 1);
        for (int i = 0; i < GetBulletData().numScols; ++i)
        {   
            rotationsSC[i] = angle + GetBulletData().minAngle; // adding min angle, testing it out
            angle += angleInc;
        }
    }

    public void RandomRotationsSC()
    {

        for (int i = 0; i < GetBulletData().numScols; ++i)
        {
            rotationsSC[i] = Random.Range(GetBulletData().minAngle, GetBulletData().maxAngle);
        }
    }

    public void spawnCustomBullet()
    {
        // temporary
        if (rotationsSC.Length != GetBulletData().numScols)
        {
            print("custom rotation size changed");
            rotationsSC = new float[GetBulletData().numScols];
        }
        /* handles rotations of each bullet */
        if (GetBulletData().hasRandomAngles == false)
        {
            DistributedRotationsSC();
        }
        else { RandomRotationsSC(); }

        // get all attributes of bullets before for loop iteration ,optimization
        currDirSD = GetBulletData().dirSC;
        currSCspeed = GetBulletData().bulletSpeedSC;
        bulletPoolSC = GameObject.FindGameObjectWithTag(GetBulletData().bulletPoolTag);
        // important that spawning/pooling is done within for loop
        for (int i = 0; i < GetBulletData().numScols; ++i)
        {
            specialBullet = bulletPoolSC.GetComponent<simpleBulletPool>().GetPooledObject();
            if (specialBullet != null)
            {
                //// set it at the origin, which is at the spawner's location and set angle of the bullet
                specialBullet.transform.position = this.transform.position;
                float angle = rotationsSC[i] + transform.eulerAngles.z; // rn it'll align with the parent(spawner's) rotation
                specialBullet.transform.rotation = Quaternion.Euler(0, 0, angle);


                //// manipulate values of regular bullet's attached script
                specialBullet.GetComponent<regularCustomBehavior>().direction = currDirSD;
                specialBullet.GetComponent<regularCustomBehavior>().blifeTime = GetBulletData().lifeTimeSC;

                if (GetBulletData().willAccelSC == true)
                {
                    specialBullet.GetComponent<regularCustomBehavior>().willAccel = true;
                    specialBullet.GetComponent<regularCustomBehavior>().speedCurve = GetBulletData().curveSC;
                }
                else { specialBullet.GetComponent<regularCustomBehavior>().bSpeed = currSCspeed; }

                specialBullet.SetActive(true);
            }
        }
    }

    // can be called by animator
    public void EnableSpawnAuto()
    {
        willSpawnAuto = true;
    }

    public void SpawnSpecificBullet(int index)
    {
        if(index >= customDataPatterns.Length) { return; }
        indexSC = index;
        spawnCustomBullet();
        return;
    }
}
