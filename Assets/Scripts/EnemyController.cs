using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Spawning,
        Moving,
        Dying
    }

    public float speed = 2;

    GameObject target;
    State state;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        state = State.Moving;
    }

    private void FixedUpdate()
    {
        if (state == State.Moving)
        {
            Vector2 direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);

            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            if (direction.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            float d = collision.gameObject.GetComponent<Bullet>().damage;

            if (GetComponent<Character>().Hit(d))
            {

            }
            else
            {
                //Á×¾úÀ» ¶§
                Die();
            }
        }
    }

    void Die()
    {
        state = State.Dying;
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 1.4f);
    }

    void AfterDying()
    {
        Destroy(gameObject);
    }
}
