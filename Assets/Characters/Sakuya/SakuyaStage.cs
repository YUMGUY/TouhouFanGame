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
    public string knifeTag;

    [Header("Sakuya Info")]
    public int sakuyaHp;
    public Animator sakuyaController;
    
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

        if (Input.GetKeyDown(KeyCode.T))
        {
           //StartCoroutine(pattern2Spawner.GetComponent<regularSpawnInfo>().SurroundTimeStop(0, 0, .1f));
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
     * Includes TIME STOP 
     * 
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
