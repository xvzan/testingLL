﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;

public class SkillY2 : MonoBehaviour
{
    public CooldownImage MyImageScript;
    public float maxdistance = 15;
    public float bulletspeed = 10;
    public GameObject DisBall;
    public float DisTime = 2;
    private float currentcooldown;
    public float cooldowntime = 3;
    public bool skillavaliable;

    // Use this for initialization
    void Start()
    {
        currentcooldown = cooldowntime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FireY") && skillavaliable)
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
        DoFire(singplace + 0.66f * skilldirection.normalized, skilldirection.normalized * bulletspeed);
        currentcooldown = 0;
        skillavaliable = false;
    }

    void DoFire(Vector2 fireplace, Vector2 speed2d)
    {
        GameObject bullet;
        //DisBall.GetComponent<BombExplode>().sender = gameObject;
        bullet = Instantiate(DisBall, fireplace, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = speed2d;
        bullet.GetComponent<BombExplode>().pushtime = DisTime;
        bullet.GetComponent<BombExplode>().maxtime = maxdistance / bulletspeed;
    }
}
