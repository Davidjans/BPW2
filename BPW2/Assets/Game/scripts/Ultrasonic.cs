using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;
using System.Threading;


public class Ultrasonic : MonoBehaviour
{
    //get the serial port
    private Thread thread1;


    // Use this for initialization
    void Start()
    {
        //zoek voor de juiste COM (check in arduino code welke het is en pas de COM aan naar de gene die dr staat)
        ThreadWork.m_SerialPort = new SerialPort("COM6", 9600);
        //arduino openen
        

        thread1 = new Thread(ThreadWork.DoWork);
        thread1.Start();

        //StartCoroutine(ReadAsync());
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        thread1.Abort();
    }
}

public class ThreadWork
{
    public static bool interrupted;
    public static SerialPort m_SerialPort;

    public static void DoWork()
    {
        m_SerialPort.Open();
        m_SerialPort.DiscardInBuffer();
        while (true)
        {
            String read = m_SerialPort.ReadLine();
            Debug.LogError(read + " string value distance");
            float value = float.Parse(read);
            Debug.LogError(value + " float value distance");
            //if (read == "true")
            //{
            //    interrupted = true;
            //    Debug.Log("1");
            //}
            //else if (read == "false")
            //{
            //    interrupted = false;
            //    Thread.Sleep(5);
            //}
            //Debug.LogError(Serial.println(sonar.ping_cm()));
        }
    }
}