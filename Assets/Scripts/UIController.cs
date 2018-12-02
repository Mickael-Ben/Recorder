using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;
public class UIController : MonoBehaviour
{
    private AndroidUtils androidUtils;
    public GameObject blackPanel;
    private System.Timers.Timer timerForScreen = new System.Timers.Timer(1000);
    public GameObject whitePanel;
    private int Counter = 0;
    private int initialCounter = 0;

    [SerializeField] private GameObject startRecordbtn, stopRecordBtn;
    private void Start()
    {
        if (!AndroidUtils.IsPermitted(AndroidPermission.ACCESS_FINE_LOCATION))//test request permission
            AndroidUtils.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION);
        androidUtils = FindObjectOfType<AndroidUtils>();

        blackPanel.gameObject.SetActive(false);
        whitePanel.gameObject.SetActive(false);
        timerForScreen.Elapsed += Ticks;
        timerForScreen.AutoReset = true;
        timerForScreen.Enabled = true;
    }

    private void Ticks(object source, ElapsedEventArgs e)
    {
        Debug.Log("Tick! Counter " + Counter);
        Counter += 1;
    }

    public void Update()
    {
        if (Counter > initialCounter + 2)
        {
            Debug.Log("PANEL CANCELLED, Counter : " + Counter + "initial counter : " + initialCounter);
            blackPanel.gameObject.SetActive(false);
            whitePanel.gameObject.SetActive(false);
        }
    }

    public void OnClickStartRecord()
    {
        initialCounter = Counter;
        Debug.Log("initialCounter " + (initialCounter.ToString()));
        blackPanel.gameObject.SetActive(true);
        whitePanel.gameObject.SetActive(true);
        startRecordbtn.SetActive(false);
        stopRecordBtn.SetActive(true);
        androidUtils.PrepareRecorder();
        StartCoroutine(DelayCallRecord());

    }
    private IEnumerator DelayCallRecord()
    {
        yield return new WaitForSeconds(0.1f);
        androidUtils.StartRecording();
    }
    public void OnClickStopRecord()
    {
        startRecordbtn.SetActive(true);
        stopRecordBtn.SetActive(false);
        androidUtils.StopRecording();
        StartCoroutine(DelaySaveVideo());
    }
    private IEnumerator DelaySaveVideo()
    {
        yield return new WaitForSeconds(1);
        androidUtils.SaveVideoToGallery();
    }
    public void OnClickOpenGallery()
    {
        AndroidUtils.OpenGallery();
    }
}
