﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///using Photon;

public class DestroyScript : MonoBehaviour
{
    public bool breakable = false;
    public bool selfprotect = true;

    public void Destroyself()
    {
        //photonView.RPC("SDestroy", PhotonTargets.All);
    }

    //[PunRPC]
    public void SDestroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
