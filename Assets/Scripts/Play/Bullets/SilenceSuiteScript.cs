﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///using Photon;

public class SilenceSuiteScript : MonoBehaviour
{
    public float speed = 7;
    public GameObject SC;
    GameObject cA;
    GameObject cB;
    public LineRenderer LRR;
    public float turnangle = 15;
    public float maxtime = 1;
    float timepsd = 0;
    //public Rigidbody2D rA;
    //public Rigidbody2D rB;

	// Use this for initialization
	void Start ()
    {
        //rA = cA.GetComponent<Rigidbody2D>();
        //rB = cB.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void FixedUpdate()
    {
        //if (!photonView.isMine)
            //return;
        timepsd += Time.fixedDeltaTime;
        if (timepsd >= maxtime)
        {
            endwork();
            enabled = false;
        }
    }

    public void work(Vector2 workdir)
    {
        Vector2 sA = Quaternion.AngleAxis(turnangle, Vector3.forward) * workdir.normalized * speed;
        Vector2 sB = Quaternion.AngleAxis(turnangle, Vector3.back) * workdir.normalized * speed;
        maxtime = workdir.magnitude * 2 / (sA + sB).magnitude;
        /*
        cA = PhotonNetwork.Instantiate(SC.name, transform.position, Quaternion.identity, 0);
        cB = PhotonNetwork.Instantiate(SC.name, transform.position, Quaternion.identity, 0);
        cA.GetComponent<Rigidbody2D>().velocity = sA;
        cB.GetComponent<Rigidbody2D>().velocity = sB;
        */
        //enabled = true;
    }
    
    void endwork()
    {
        cA.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        cB.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //photonView.RPC("SSLine", PhotonTargets.All, cA.GetComponent<Rigidbody2D>().position, cB.GetComponent<Rigidbody2D>().position);
        Vector2 RayV2 = cB.GetComponent<Rigidbody2D>().position - cA.GetComponent<Rigidbody2D>().position;
        RaycastHit2D[] Allhit = Physics2D.RaycastAll(cA.GetComponent<Rigidbody2D>().position, RayV2);
        foreach (RaycastHit2D hit in Allhit)
        {
            if (hit.collider.GetComponent<LinktoUI>() == null)
                continue;
            hit.collider.GetComponent<LinktoUI>().ShutUp();
        }
        cA.GetComponent<DestroyScript>().Destroyself();
        cB.GetComponent<DestroyScript>().Destroyself();
    }

    //[PunRPC]
    void SSLine(Vector2 a, Vector2 b)
    {
        LRR.SetPosition(0, a);
        LRR.SetPosition(1, b);
        StartCoroutine(byebye());
    }

    IEnumerator byebye()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<DestroyScript>().Destroyself();
    }
}
