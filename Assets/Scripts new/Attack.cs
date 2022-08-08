using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float shotSpeed = 5f;
    Vector2 newShotVector;
    Rigidbody2D bulletRB;
    float currentAngle;
    Vector2 vectorToTarget;
    int noExtraShots = 0;
    float shotAngleCoeff = 1;
    public float trueDamageValue;
    public GameObject Bullet;
    float fireTimerLength = 25;
    float fireTimer = 0f;
    public GameObject PlayerShootAudio;

    void Awake()
    {
        trueDamageValue = gameObject.GetComponent<DealDamage>().finalDamageStat;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (fireTimer < 0)
            {
                vectorToTarget = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y).normalized;
                for (int i = -1; i < noExtraShots; i++)
                {
                    GameObject newObject = Instantiate(Bullet, transform.position, transform.rotation);
                    newObject.transform.localScale = new Vector3(trueDamageValue * 0.0015f + .45f, trueDamageValue * 0.0015f + .45f, trueDamageValue * 0.0015f + .45f);
                    bulletRB = newObject.GetComponent<Rigidbody2D>();
                    currentAngle = 0.3f * shotAngleCoeff * (0.5f * noExtraShots - i - 1);
                    newShotVector = new Vector2(vectorToTarget.x * Mathf.Cos(currentAngle) - vectorToTarget.y * Mathf.Sin(currentAngle), vectorToTarget.x * Mathf.Sin(currentAngle) + vectorToTarget.y * Mathf.Cos(currentAngle));
                    bulletRB.velocity = newShotVector * shotSpeed;
                }
                fireTimer = fireTimerLength;
                Instantiate(PlayerShootAudio);
            }
        }
    }

    void FixedUpdate()
    {
        fireTimer -= 1;
    }
}
