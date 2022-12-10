using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoumuInfo : MonoBehaviour
{
    [Header("Health Management")]
    public Image YoumuHPBar;
    public float currentYoumuHp;
    public float maxYoumuHp;
    public bool YoumucanBeDamagedByReimu;
    public ReimuInfo reimuConnectiontoYoumu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        YCalculateHp();

    }

    private void YCalculateHp()
    {
        YoumuHPBar.fillAmount = currentYoumuHp / maxYoumuHp;
    }

    public IEnumerator YNewMaxHp(float newMax)
    {

        maxYoumuHp = newMax;
        float startingHp = currentYoumuHp;
        float time1 = 0;
        float duration = 2.0f;
        while (time1 <= duration)
        {
            currentYoumuHp = Mathf.Lerp(startingHp, maxYoumuHp, time1 / duration);
            time1 += Time.deltaTime;
            yield return null;
        }

        print("refilled hp");

        yield return null;
    }

    public IEnumerator YMoveToLocation(Vector2 newLocation, float duration)
    {
       // movingInBattle = true;
        float time1_ = 0f;
        Vector3 startingPosition = transform.position;
        while (time1_ < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, newLocation, time1_ / duration);
            time1_ += Time.deltaTime;
            yield return null;
        }

      //  movingInBattle = false;
        print("finished moving");
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("reimu_Bullet") && YoumucanBeDamagedByReimu)
        {
            GameManager.instance.IncreaseHunger(2);
            currentYoumuHp -= reimuConnectiontoYoumu.reimuDmg;
            collision.gameObject.SetActive(false);
        }
    }

    
}
