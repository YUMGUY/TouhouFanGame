using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SakuyaInfo : MonoBehaviour
{
    [Header("Health Management")]
    public Image radialHP_Bar;
    public float currentSakuyaHp;
    public float maxSakuyaHp;
    public bool canBeDamagedByReimu;
    public SakuyaStage phaseController;

    [SerializeField]
    private bool movingInBattle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateHp();
        if(Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(NewMaxHp(1000.0f));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collision == reimu bullet, current -=
        if(collision != null)
        {
            if (collision.CompareTag("reimu_Bullet") && canBeDamagedByReimu == true)
            {
                currentSakuyaHp -= 5f;
                Destroy(collision.gameObject);
            }
        }
       
    }

    private void CalculateHp()
    {
        radialHP_Bar.fillAmount = currentSakuyaHp / maxSakuyaHp;
    }

    public IEnumerator NewMaxHp(float newMax)
    {

        maxSakuyaHp = newMax;
        float startingHp = currentSakuyaHp;
        float time1 = 0;
        float duration = 2.0f;
        while(time1 <= duration)
        {
           currentSakuyaHp = Mathf.Lerp(startingHp, maxSakuyaHp, time1 / duration);
            time1 += Time.deltaTime;
            yield return null;
        }
      
        print("refilled hp");
       
        yield return null;
    }

    public IEnumerator MoveToLocation(Vector2 newLocation)
    {
        movingInBattle = true;
        float time1_ = 0f;
        float duration = 1.5f;
        Vector3 startingPosition = transform.position;
       while(time1_ < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, newLocation, time1_ / duration);
            time1_ += Time.deltaTime;
            yield return null;
        }

        movingInBattle = false;
        print("finished moving");
        yield return null;
    }

   
}
