﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;

public class SkillY1 : MonoBehaviour
{
    public CooldownImage MyImageScript;
    public GameObject MyLineObj;
    public GameObject MyLine;
    public float maxdistance = 10;
    private float currentcooldown;
    public float cooldowntime = 3;
    public bool skillavaliable;
    public float speed;
    public float damage;
    public float maxtime;

    // Use this for initialization
    void Start ()
    {
        currentcooldown = cooldowntime;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("FireY") && skillavaliable)
        {
            GetComponent<DoSkill>().singing = 0;
            if (MyLine != null)
                MyLine.GetComponent<DestroyScript>().Destroyself();
            MyLine = Instantiate(MyLineObj, gameObject.transform.position, Quaternion.identity);
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
        Vector2 singplace = transform.position;
        Vector2 skilldirection = actionplace - singplace;
        float realdistance = Mathf.Min(skilldirection.magnitude, maxdistance);
        if (realdistance <= 0.6)
        {
            return;
        }   //半径小于自身半径时不施法
        Vector2 realplace = singplace + skilldirection.normalized * realdistance;
        GetComponent<DoSkill>().BeforeSkill();
        currentcooldown = 0;
        skillavaliable = false;
        if (Physics2D.OverlapPoint(realplace))
        {
            Collider2D hit = Physics2D.OverlapPoint(realplace);
            if (hit.GetComponent<HPScript>() != null)
            {
                //MyLine.GetComponent<BlueLineScript>().DoMyJob(hit.gameObject.GetPhotonView().photonView.viewID, gameObject.GetPhotonView().viewID, Vector2.zero, speed, damage, maxtime);
                return;
            }
        }
        //MyLine.GetComponent<BlueLineScript>().DoMyJob(0, gameObject.GetPhotonView().viewID, realplace, speed, damage, maxtime);
    }
}
