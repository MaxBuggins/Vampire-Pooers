using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDMGADDPT5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<DealDamage>().damageBase += 25f;
    }
}
