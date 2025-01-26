using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MaxHP = 3;
    float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    public void Initialize()
    {
        HP = MaxHP;
    }

    /*
     *��������� true�� �����Ѵ�.
     */
    public bool Hit(float damage)
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
        }

        return HP > 0;
    }
}
