using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuyaStage : MonoBehaviour
{


    [Header("bullet spawners and Time Variables")]
    public GameObject Sakuya_;
    public GameObject[] spawners;
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
    }

    // ////////////////////////////////////////////////////// FIRST PATTERN " COMPLIMENTS TO THE CHEF "  //////////////////
    public void MaidLovePattern()
    {
        sakuyaController.SetTrigger("heartPattern");
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
