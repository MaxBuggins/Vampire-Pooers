using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSOY : MonoBehaviour
{
    public int instances = 1;
    public float initialDamage;
    public float bonusDamage;


    // Start is called before the first frame update
    void Start()
    {
        //bonusDamage = 0;
        SetBonus();
        //GetDamVal();
    }

    void SetBonus()
    {
        if (gameObject.GetComponent<Attack>() != null)
        {
            gameObject.GetComponent<Attack>().PlayerShootAudio = EntityReferencerGuy.Instance.soyShotAudio;
            gameObject.GetComponent<Attack>().fireTimerDIV = 1 + 2 * instances;
        }
        gameObject.GetComponent<DealDamage>().massCoeff = 1 / (2 * instances);
    }
    //void GetDamVal()
    //{
    //    initialDamage = gameObject.GetComponent<DealDamage>().damageBase;
    //    float funnyDamage = 4 * instances;
    //    gameObject.GetComponent<DealDamage>().finalDamageMult /= funnyDamage;
    //    bonusDamage = funnyDamage;
    //    gameObject.GetComponent<DealDamage>().massCoeff /= 4 * instances;
    //    if (gameObject.GetComponent<Attack>() != null)
    //    {
    //        gameObject.GetComponent<Attack>().fireTimerDIV = 1 + 4 * instances;
    //    }
    //}

    void GetDamageMods()
    {
        gameObject.GetComponent<DealDamage>().damageToPassToVictim /= 2 * instances;
    }

    void IncreaseInstances(string name)
    {
        if (name == this.GetType().ToString())
        {
            //Invoke(nameof(ResetVal), 0.005f);
            instances++;
            SetBonus();
            //Invoke(nameof(GetDamVal), 0.005f);
        }
    }

    void ResetVal()
    {
        if (gameObject.GetComponent<Attack>() != null)
        {
            gameObject.GetComponent<Attack>().fireTimerDIV = 1;
        }
        gameObject.GetComponent<DealDamage>().massCoeff = 1;
        //gameObject.GetComponent<DealDamage>().damageBase *= bonusDamage;
    }

    public void Undo()
    {
        //ResetVal();
        gameObject.GetComponent<Attack>().PlayerShootAudio = EntityReferencerGuy.Instance.normieShotAudio;
        Destroy(this);
    }

    //public bool runStart = true;

    //void Awake()
    //{
    //    gameObject.GetComponent<DealDamage>().finalDamageDIV += 4;
    //    gameObject.GetComponent<DealDamage>().massCoeff /= 4;
    //}

    //void Start()
    //{
    //    if (gameObject.GetComponent<Attack>() != null)
    //    {
    //        gameObject.GetComponent<Attack>().fireTimerDIV += 4;
    //        gameObject.GetComponent<Attack>().PlayerShootAudio = EntityReferencerGuy.Instance.soyShotAudio;
    //    }
    //}

    //public void Undo()
    //{
    //    gameObject.GetComponent<DealDamage>().finalDamageDIV -= 4;
    //    gameObject.GetComponent<DealDamage>().massCoeff *= 4;
    //    if (gameObject.GetComponent<Attack>() != null)
    //    {
    //        gameObject.GetComponent<Attack>().fireTimerDIV -= 4;
    //        gameObject.GetComponent<Attack>().PlayerShootAudio = EntityReferencerGuy.Instance.normieShotAudio;
    //    }

    //    Destroy(this);

    //    Debug.Log("Foodland Sale on NOW!!!!!!!!");
    //}
}
