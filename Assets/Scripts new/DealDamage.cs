using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public float finalDamageStat;
    public float procCoeff;
    public float damageBase;
    [System.NonSerialized] public float damageBonus = 1; // For things like converter and the bonus funny level up.
    public float damageMult;
    public float finalDamageMult = 1;
    public float critProb = 0.05f;
    public float critMult = 2;
    public GameObject owner;
    public GameObject master;
    public bool canDealDamage = true;

    public float massCoeff = 2;

    public bool overwriteDamageCalc;
    public bool onlyDamageOnce = true;

    public int tickInterval = 1;
    public int timer = 0;
    public float damageAmt;

    public float iFrameFac = 1;

    public float damageToPresent;

    public int damageType;
    public List<int> procChainIndexes = new List<int>();
    public float procChanceBonus = 1;

    public float damageToPassToVictim;

    void Start()
    {
        master = EntityReferencerGuy.Instance.master;

        if (gameObject.tag == "Hostile" || gameObject.tag == "Player")
        {
            owner = gameObject;
        }

        CalcDamage();
    }

    public void DetermineShotRolls()
    {
        procChainIndexes.Clear();
    }

    public void CalcDamage()
    {
        if (!overwriteDamageCalc)
        {
            //damageAmt = damageBase;// * damageMult * finalDamageMult / finalDamageDIV;
            damageAmt = damageBase * finalDamageMult;
        }
    }

    //void Update()
    //{
    //    CalcDamage();

    //    damageToPresent = damageBase * damageMult * finalDamageMult / finalDamageDIV;
    //}

    //For applying any on-hit effects - sends the RollOnHit message, which is picked up by any on-hit effects THIS object has, which then apply the effect or whatever to col.gameobject.
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.GetComponent<HPDamageDie>() != null && procCoeff != 0 && finalDamageStat != 0 && gameObject.tag != col.gameObject.tag && col.gameObject.tag != "Wall" && col.gameObject.GetComponent<HPDamageDie>().iFrames < 0)
    //    {
    //        SendRollOnHits(col.gameObject);
    //    }
    //}

    //void OnTriggerStay2D(Collider2D col)
    //{
    //    Debug.Log("erm globule");
    //    //if (col.gameObject.GetComponent<HPDamageDie>() != null && procCoeff != 0 && finalDamageStat != 0 && gameObject.tag != col.gameObject.tag && col.gameObject.tag != "Wall" && col.gameObject.GetComponent<HPDamageDie>().iFrames < 0)
    //    //{
    //    SendRollOnHits(col.gameObject);
    //    //}
    //}

    //public void TriggerTheOnHits(GameObject whoToEffect)
    //{
    //    SendRollOnHits(whoToEffect);
    //}

    void GetDamageMods()
    {
        //no (throws error if this method isn't here :) )
    }

    public float GetDamageAmount()
    {
        if (!overwriteDamageCalc)
        {
            damageToPassToVictim = damageBase * finalDamageMult;
            SendMessage("GetDamageMods");
        }
        else
        {
            damageToPassToVictim = finalDamageStat;
        }
        return damageToPassToVictim;
    }

    public void CalculateDamage(GameObject victim) // When an object is hurt, it calls this method on the responsible object.
    {
        if (canDealDamage)
        {
            float critChance = 100f * critProb;
            int numCrits = ChanceRoll(critChance, gameObject, -5);
            float critMult = 1;
            bool isCrit = false;

            if (!overwriteDamageCalc)
            {
                for (int i = 0; i < numCrits; i++)
                {
                    critMult *= 2;
                    isCrit = true;
                }

                damageToPassToVictim = damageBase * finalDamageMult * critMult;
            }
            else
            {
                damageToPassToVictim = finalDamageStat;
            }

            SendMessage("GetDamageMods");

            if (gameObject.GetComponent<explosionBONUSSCRIPTWOW>() != null)
            {
                Vector3 vecFromCtr = (transform.position - victim.transform.position) / (2.65f * victim.transform.localScale.x);
                float fracFromCtr = Mathf.Clamp(1 - new Vector3(vecFromCtr.x, vecFromCtr.y, 0).magnitude * 0.75f, 0, 1);
                Debug.Log("fracFromCtr: " + fracFromCtr.ToString());
                damageToPassToVictim *= fracFromCtr;
            }

            victim.GetComponent<HPDamageDie>().Hurty(damageToPassToVictim, isCrit, true, iFrameFac, damageType, false, gameObject);
        }
    }

    public void SendRollOnHits(GameObject victim)
    {
        if (procCoeff > 0)
        {
            owner.SendMessage("RollOnHit", new GameObject[] { victim, gameObject });
            gameObject.SendMessage("RollOnHit", new GameObject[] { victim, gameObject });
        }
    }

    public void RollOnHit(GameObject[] gameObjects)
    {
        //erm nothing, literally just here to get the sendmessages to shut the fuck up about no recievers!
    }

    public int ChanceRoll(float value, GameObject source, int procItemIndex) // Items that are rolling for a chance-based thing use this. 
    {
        if (gameObject.tag == "Hostile" || gameObject.tag == "Player") // The player and shit shouldn't have proc indexes anyway
        {
            procChainIndexes.Clear();
        }

        bool scriptIsUsed = false; // This stores whether the current script doing the roll has already been used in the proc chain.
        List<int> procIndexes = source.GetComponent<DealDamage>().procChainIndexes;

        // Checks if the item index calling the chance roll has already been used in this proc chain.
        for (int i = 0; i < procIndexes.Count; i++)
        {
            if (procIndexes[i] == procItemIndex)
            {
                scriptIsUsed = true;
            }
        }

        int numberOfProcs = 0;

        if (!scriptIsUsed)
        {
            float percentChance = value * source.GetComponent<DealDamage>().procCoeff * procChanceBonus;
            //Debug.Log("erm percentage chance do be: " + percentChance.ToString() + ", value: " + value.ToString());

            for (int i = 0; i < 20; i++)
            {
                float procMoment = 100f - percentChance;
                float pringle = Random.Range(0f, 100f);

                if (pringle > procMoment)
                {
                    numberOfProcs++;
                }

                if (percentChance > 100f)
                {
                    percentChance -= 100f;
                }
                else
                {
                    break;
                }
            }
        }

        return numberOfProcs;
    }

    //public void RollOnHit(GameObject[] objects)
    //{
    //    GameObject victim = objects[0];
    //    GameObject source = objects[1];
    //    Debug.Log(victim.ToString() + " / " + source.ToString());
    //}
}