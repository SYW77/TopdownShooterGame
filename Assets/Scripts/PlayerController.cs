using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public GameObject bulletPrefab;

    public Material flashMaterial;
    public Material defaultMaterial;

    public AudioClip shotSound;
    public AudioClip hitSound;
    public AudioClip deadSound;

    Vector3 move;
    Dictionary<Collider2D, Coroutine> damageCoroutines = new Dictionary<Collider2D, Coroutine>();

    // Update is called once per frame
    void Update()
    {
        move = Vector3.zero;

        if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, -1, 0);
        }

        move = move.normalized;

        if (move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if(move.magnitude > 0)
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Stop");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(shotSound);

        Vector3 worldPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        worldPosition -= (transform.position + new Vector3(0, -0.5f, 0));

        GameObject newBullet = GetComponent<ObjectPool>().Get();
        if(newBullet!=null){
            newBullet.transform.position = transform.position + new Vector3(0, -0.5f);
            newBullet.GetComponent<Bullet>().Direction = worldPosition;
        }
        
    }

    public void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Character enemyCharacter = collision.gameObject.GetComponent<Character>();

            if (enemyCharacter != null && enemyCharacter.isAttackable) // ?? 특정 Enemy가 공격 가능할 때만 피격
            {
                if (!damageCoroutines.ContainsKey(collision.collider))
                {
                    Coroutine damageRoutine = StartCoroutine(DamageOverTime(collision.collider));
                    damageCoroutines[collision.collider] = damageRoutine;
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (damageCoroutines.ContainsKey(collision.collider))
            {
                StopCoroutine(damageCoroutines[collision.collider]);
                damageCoroutines.Remove(collision.collider);
            }
        }
    }

    private IEnumerator DamageOverTime(Collider2D enemy)
    {
        while (true)
        {
            if (GetComponent<Character>().Hit(1))
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                Flash();
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(deadSound);
                Die();
                yield break; // 플레이어가 죽으면 코루틴 종료
            }
            yield return new WaitForSeconds(1.0f); // 1초마다 데미지
        }
    }

    void Flash()
    {
        GetComponent<SpriteRenderer>().material = flashMaterial;
        Invoke("AfterFlash", 0.5f);
    }

    void AfterFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void Die()
    {
        // 점수 저장
        FindObjectOfType<GameManager>().SaveFinalScore();

        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 0.875f);
    }

    void AfterDying()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
