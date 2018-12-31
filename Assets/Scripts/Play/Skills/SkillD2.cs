﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;

public class SkillD2 : MonoBehaviour
{
    public CooldownImage MyImageScript;
    //public float maxtime;
    public float bulletspeed = 5;
    public GameObject fireball;
    public float force = 15;
    public float damage = 10;
    private float currentcooldown;
    public float cooldowntime = 3;
    public bool skillavaliable;

    // Use this for initialization
    void Start()
    {
        currentcooldown = cooldowntime;
        fireball.GetComponent<BombExplode>().sender = gameObject;
        fireball.GetComponent<BoomerangScript>().senderRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FireD") && skillavaliable)
        {
            GetComponent<DoSkill>().singing = 0;
            gameObject.GetComponent<DoSkill>().Fire = Skill;
        }
    }

    private void FixedUpdate()
    {
        if (skillavaliable)
            return;
        if (currentcooldown >= cooldowntime)
        {
            skillavaliable = true;
        }
        else
        {
            currentcooldown += Time.fixedDeltaTime;
            MyImageScript.IconFillAmount = currentcooldown / cooldowntime;
        }
    }

    public void Skill(Fix64Vector2 actionplacef)
    {
        Vector2 actionplace = actionplacef.ToV2();
        GetComponent<DoSkill>().BeforeSkill();
        Vector2 singplace = transform.position;
        Vector2 skilldirection = actionplace - singplace;
        DoFire(singplace + 0.71f * skilldirection.normalized, skilldirection.normalized * bulletspeed);
        currentcooldown = 0;
        skillavaliable = false;
    }

    //[PunRPC]
    void DoFire(Vector2 fireplace, Vector2 speed2d)
    {
        GameObject bullet;
        //fireball.GetComponent<BombExplode>().sender = gameObject;
        bullet = Instantiate(fireball, fireplace, Quaternion.identity);
        //bullet.GetComponent<BombExplode>().bombpower = force;
        bullet.GetComponent<BombExplode>().bombdamage = damage;
        bullet.GetComponent<Rigidbody2D>().velocity = speed2d;
        //bullet.GetComponent<BombExplode>().maxtime = maxdistance / bulletspeed;
    }
}
