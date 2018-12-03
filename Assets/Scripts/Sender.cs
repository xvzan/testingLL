﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Sender : MonoBehaviour
{
    public InputField TextToSend;
    public Text TextReceived;
    public HostToggle theHT;
    public ClientToggle theCT;
    public Image SignalLight;
    public byte[] buffer;
    public int sz;
    public int rcsz;
    public byte error;
    public bool started = false;
    public bool isServer;
    public GameObject SendButton;
    public HostTopology HTo;
    public static int HSID;
    public static int CNID;
    public int CHANID;
    byte[] rcbuffer = new byte[1024];
    int rcbfsz = 1024;

    private void Start()
    {
        ResetSelf();
    }

    public void ResetSelf()
    {
        started = false;
        SendButton.SetActive(false);
        SignalLight.color = Color.white;
    }

    public void StartSelf()
    {
        started = true;
        SignalLight.color = Color.yellow;
    }

    public void ClickSendButton()
    {
        buffer = System.Text.Encoding.Unicode.GetBytes(TextToSend.text);
        sz = buffer.Length;
        if (sz != 0)
        {
            NetworkTransport.Send(HSID, CNID, CHANID, buffer, sz, out error);
            TextReceived.text = System.Text.Encoding.Unicode.GetString(buffer);
            TextToSend.text = null;
        }
    }

    private void FixedUpdate()
    {
        if (!started)
            return;
        SignalControl();
    }

    public void SignalControl()
    {
        int RecChanID;
        NetworkEventType recData = NetworkTransport.Receive(out HSID, out CNID, out RecChanID, rcbuffer, rcbfsz, out rcsz, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.BroadcastEvent:
                break;
            case NetworkEventType.ConnectEvent:
                SignalLight.color = Color.green;
                SendButton.SetActive(true);
                break;
            case NetworkEventType.DataEvent:
                if (RecChanID == CHANID)
                    PrintReceived();
                else
                    DeSerializeReceived();
                break;
            case NetworkEventType.DisconnectEvent:
                SignalLight.color = Color.red;
                SendButton.SetActive(false);
                break;
        }
    }

    private void PrintReceived()
    {
        TextReceived.text = System.Text.Encoding.Unicode.GetString(rcbuffer);
        rcbuffer = new byte[1024];
    }

    void DeSerializeReceived()
    {
        NetWriter.bRC = rcbuffer;
        NetWriter.Eat();
        rcbuffer = new byte[1024];
    }
}

public class ClickData
{
    public MButton blr;
    public float xPos;
    public float yPos;

    public void setdata(MButton b,float x,float y)
    {
        blr = b;
        xPos = x;
        yPos = y;
    }
}

[Serializable]
public class Data2S
{
    public uint frameNum;
    public List<ClickData> clickDatas;
}

public enum MButton { left, right };
