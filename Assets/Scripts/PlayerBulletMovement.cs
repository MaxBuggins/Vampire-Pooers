using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMovement : MonoBehaviour
{
    Vector2 vectorToEnemy;
    public GameObject Player;
    public GameObject ATGMissile;
    Vector2 closestEnemyPos;
    Vector2 mousePos;
    Vector2 bulletPos;
    Vector3 currentNearest;
    Vector2 newShotVector;
    Vector2 ShotVector;
    float ATGProc;
    int homingInstances = 0;
    int ATGInstances = 0;
    List<int> Sploinky = new List<int>();
    public float destroyDelay = 25; //in seconds
    int bounces;
    Vector2 enemyPos;
    int pierces;
    int splits;
    float speed;
    Rigidbody2D bulletRB;
    int homingCheckTimer;
    GameObject closest;

    public Rigidbody2D rb;

    void Start()
    {
        Sploinky = FindObjectOfType<Player_Movement>().itemsHeld;
        Player = GameObject.Find("Player");
        foreach (int item in Sploinky)
        {
            //Debug.Log(item.ToString());
            switch (item)
            {
                case (int)ITEMLIST.HOMING:
                    homingInstances++;
                    break;
                case (int)ITEMLIST.ATG:
                    ATGInstances++;
                    break;
            }
        }

        Invoke(nameof(DestorySelf), destroyDelay); //will invoke (run the function) in so many seconds
        bounces = Player.GetComponent<Player_Movement>().bounceInstances;
        speed = Player.GetComponent<Player_Movement>().shotSpeed;
        pierces = Player.GetComponent<Player_Movement>().pierceInstances;
        splits = Player.GetComponent<Player_Movement>().splitInstances;
    }

    void DestorySelf() //deeath
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (homingInstances >= 1)
        {
            homingCheckTimer--;
            if (homingCheckTimer <= 0)
            {
                homingCheckTimer = 6;
                GameObject[] gos;
                gos = GameObject.FindGameObjectsWithTag("Hostile");
                closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject go in gos)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
            }

            currentNearest = closest.transform.position;

            if ((transform.position - currentNearest).magnitude < 5*homingInstances)
            {
                closestEnemyPos.x = currentNearest.x;
                closestEnemyPos.y = currentNearest.y;
                bulletPos.x = gameObject.transform.position.x;
                bulletPos.y = gameObject.transform.position.y;
                vectorToEnemy = (closestEnemyPos - bulletPos).normalized;
                rb.velocity = 10f * (rb.velocity + vectorToEnemy.normalized).normalized;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Hostile")
        {
            if (ATGInstances > 0)
            {
                ATGProc = Random.Range(0, 10);
                if (ATGProc > (8 - 0.5 * ATGInstances))
                {
                    GameObject[] gos;
                    gos = GameObject.FindGameObjectsWithTag("Player");
                    GameObject closest = null;
                    float distance = Mathf.Infinity;
                    Vector3 position = transform.position;
                    foreach (GameObject go in gos)
                    {
                        Vector3 diff = go.transform.position - position;
                        float curDistance = diff.sqrMagnitude;
                        if (curDistance < distance)
                        {
                            closest = go;
                            distance = curDistance;
                            currentNearest = go.transform.position;
                        }
                    }
                    Instantiate(ATGMissile, currentNearest, new Quaternion(1, 0, 0, 0));
                }
            }
            if (splits > 0)
            {
                if (gameObject.tag == "PlayerBullet")
                {
                    bulletPos.x = gameObject.transform.position.x;
                    bulletPos.y = gameObject.transform.position.y;
                    enemyPos.x = col.transform.position.x;
                    enemyPos.y = col.transform.position.y;
                    ShotVector = speed * (bulletPos - enemyPos).normalized;
                    GameObject splitty = Instantiate(gameObject, transform.position, transform.rotation);
                    splitty.transform.localScale = 0.4f * transform.localScale;
                    splitty.tag = "playerBulletSplit";
                    bulletRB = splitty.GetComponent<Rigidbody2D>();
                    newShotVector = new Vector2(ShotVector.x * Mathf.Cos(-Mathf.PI / 2) - ShotVector.y * Mathf.Sin(-Mathf.PI / 2), ShotVector.x * Mathf.Sin(-Mathf.PI / 2) + ShotVector.y * Mathf.Cos(-Mathf.PI / 2));
                    bulletRB.velocity = new Vector2(newShotVector.x, newShotVector.y);
                    GameObject splitty2 = Instantiate(gameObject, transform.position, transform.rotation);
                    splitty2.transform.localScale = 0.4f * transform.localScale;
                    splitty2.tag = "playerBulletSplit";
                    bulletRB = splitty2.GetComponent<Rigidbody2D>();
                    newShotVector = new Vector2(ShotVector.x * Mathf.Cos(Mathf.PI / 2) - ShotVector.y * Mathf.Sin(Mathf.PI / 2), ShotVector.x * Mathf.Sin(Mathf.PI / 2) + ShotVector.y * Mathf.Cos(Mathf.PI / 2));
                    bulletRB.velocity = new Vector2(newShotVector.x, newShotVector.y);
                }
            }
        }

        if (col.gameObject.tag == "Wall")
        {
            bounces = 0;
        }

        if (bounces > 0)
        {
            bulletPos.x = gameObject.transform.position.x;
            bulletPos.y = gameObject.transform.position.y;
            enemyPos.x = col.transform.position.x;
            enemyPos.y = col.transform.position.y;
            rb.velocity = speed * (bulletPos - enemyPos).normalized;
            bounces--;
        }
        else
        {
            if (pierces > 0)
            {
                pierces--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
