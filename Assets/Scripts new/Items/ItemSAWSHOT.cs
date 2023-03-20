using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSAWSHOT : MonoBehaviour
{
    public int instances = 1;
    int timer = 0;
    public Vector3 bulletOffset = new Vector3(0, 0, 0);
    public GameObject guyLatchedTo;
    public bool dogma = false;
    public bool canDoTheThing = false;
    public bool isAProc = false;

    public GameObject SawShotVisual;

    GameObject master;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("got saw: " + gameObject.name.ToString());
        master = gameObject.GetComponent<DealDamage>().master;
        if (gameObject.GetComponent<DealDamage>().isBulletClone || gameObject.GetComponent<checkAllLazerPositions>() != null)
        {
            DetermineShotRolls();
        }
    }

    public void DetermineShotRolls()
    {
        canDoTheThing = true;
        float procMoment = 100f - 20 * gameObject.GetComponent<DealDamage>().procCoeff;
        float pringle = Random.Range(0f, 100f);
        //Debug.Log(pringle.ToString());
        bool isCrit = false;
        isAProc = false;
        if (pringle > procMoment)
        {
            if (gameObject.GetComponent<checkAllLazerPositions>() == null && gameObject.GetComponent<isMelee>() == null)
            {
                gameObject.GetComponent<MeshFilter>().mesh = master.GetComponent<EntityReferencerGuy>().saw;
                gameObject.AddComponent<ItemPIERCING>();
                gameObject.GetComponent<DealDamage>().onlyDamageOnce = false;
                if (gameObject.GetComponent<ItemBOUNCY>() != null)
                {
                    Destroy(GetComponent<ItemBOUNCY>());
                }
            }

            isAProc = true;
        }
    }

    void RollOnHit(GameObject enemo)
    {
        if (isAProc)
        {
            if (enemo.tag == "Player" || enemo.tag == "Hostile")
            {
                GameObject Poop = Instantiate(master.GetComponent<EntityReferencerGuy>().sawVisual, new Vector3(-9999, 9999), Quaternion.Euler(0, 0, 0));
                Poop.GetComponent<ItemHolder>().itemsHeld = gameObject.GetComponent<ItemHolder>().itemsHeld;
                Poop.GetComponent<DealDamage>().overwriteDamageCalc = true;
                Poop.GetComponent<DealDamage>().damageAmt = gameObject.GetComponent<DealDamage>().finalDamageStat / 5;
                Poop.GetComponent<SawRotation>().instances = instances;

                if (gameObject.GetComponent<checkAllLazerPositions>() != null)
                {
                    Poop.transform.localScale = 0.5f * transform.localScale;
                }
                else
                {
                    Poop.transform.localScale = transform.localScale;
                }
                
                Poop.GetComponent<SawRotation>().guyLatchedTo = enemo;
                Poop.GetComponent<SawRotation>().advanceTimer = true;
                Poop.GetComponent<SawRotation>().bulletOffset = 0.5f * (transform.position - (Poop.GetComponent<SawRotation>().guyLatchedTo).transform.position).normalized;
                Poop.AddComponent<SawShotCreep>();

                if (gameObject.GetComponent<DealDamage>().isBulletClone)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    RollOnHits(col.gameObject);
    //}

    void Undo()
    {
        Destroy(this);
    }
}
