﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;

public class ColliderScript : MonoBehaviour
{
    public Collider2D MyCollider2D;
    bool CanDo = false;
    float pushpower = 0;
    float pushtime = 1;
    float pushdamage = 0;
    float currenttime;
    float maxtime = 1;

    void FixedUpdate()
    {
        if (!CanDo)
            return;
        currenttime += Time.fixedDeltaTime;
        if (currenttime >= maxtime)
            CanDo = false;
    }

    public void StartKick(float time)
    {
        currenttime = 0;
        maxtime = time;
        CanDo = true;
    }

    public void SetPower(float power, float time, float damage)
    {
        pushpower = power;
        pushtime = time;
        pushdamage = damage;
    }

    public void StopKick()
    {
        CanDo = false;
        GetComponent<RBScript>().PushEnd();
        GetComponent<StealthScript>().StealthEnd();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CanDo)
            return;
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        HPScript hp = collision.gameObject.GetComponent<HPScript>();
        if (hp != null && rb != null)
        {
            Fix64Vector2 explforce;
            Rigidbody2D selfrb = gameObject.GetComponent<Rigidbody2D>();
            explforce = (Fix64Vector2)rb.position - (Fix64Vector2)selfrb.position;
            collision.gameObject.GetComponent<RBScript>().GetPushed(explforce.normalized() * (Fix64)pushpower, pushtime);
            hp.GetHurt(pushdamage);
        }
        StopKick();
    }

    public void LSDSatAll()
    {
        MyCollider2D.enabled = false;
    }

    public void DSWLatAll()
    {
        if (!MyCollider2D.enabled)
            MyCollider2D.enabled = true;
        GetComponent<RBScript>().PushEnd();
    }
}
