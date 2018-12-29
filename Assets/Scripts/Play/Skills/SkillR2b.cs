﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///using Photon;

public class SkillR2b : MonoBehaviour
{
    public CooldownImage MyImageScript;
    private float currentcooldown;
    public float cooldowntime = 5;
    public bool skillavaliable;
    public float LDspeed = 15;
    bool ImLSDS = false;
    MoveScript MS;

    // Use this for initialization
    void Start ()
    {
        currentcooldown = cooldowntime;
        MS = GetComponent<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FireR") && skillavaliable)
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

    public void IdoDSWL()
    {
        if (ImLSDS)
        {
            ImLSDS = false;
            gameObject.GetComponent<StealthScript>().StealthEnd();
        }
    }

    public void Skill(Vector2 actionplace)
    {
        Vector2 singplace = transform.position;
        Vector2 skilldirection = actionplace - singplace;
        GetComponent<DoSkill>().BeforeSkill();
        MS.controllable = true;
        currentcooldown = 0;
        skillavaliable = false;
        ImLSDS = true;
        gameObject.GetComponent<RBScript>().GetPushed(skilldirection.normalized * LDspeed, Mathf.Infinity);
        gameObject.GetComponent<StealthScript>().StealthByTime(Mathf.Infinity, true);
    }
}
