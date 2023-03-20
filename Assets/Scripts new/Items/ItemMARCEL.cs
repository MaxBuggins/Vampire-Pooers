using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMARCEL : MonoBehaviour
{
    public int instances = 1;
    GameObject marcelInstance;
    int timer = 2200;

    // Start is called before the first frame update
    void Start()
    {
        GameObject master = gameObject.GetComponent<DealDamage>().master;
        marcelInstance = master.GetComponent<EntityReferencerGuy>().marcelageloo;
    }

    void FixedUpdate()
    {
        if (timer > ((30 / instances) + 15) * 50)
        {
            GameObject marceller = Instantiate(marcelInstance, transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Quaternion.Euler(0, 0, 0));
            marceller.transform.localScale *= 1 + 0.5f * instances;
            marceller.GetComponent<marcelFunny>().owner = gameObject;
            timer = 0;
        }

        timer++;
    }
}
