using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Controller : MonoBehaviour
{
    bool LightEnabled = true;
    [HideInInspector] public bool ShowtapePlaying = false;
    [HideInInspector] public bool TapeLoaded = false;

    bool DoorOpened = false;

    public GameObject emissiveObject; // Assign the material you want to control in the Unity Editor

    public GameObject leftReel;
    public GameObject rightReel;
    public GameObject lefttape;
    public GameObject righttape;

    public AudioClip doorOpen;
    public AudioClip doorClose;

    AudioSource audioSource;

    SC_AV_Management avManagement;
    UI_PlayRecord uiPlayRecord;
    Animator animator;

    public HingeJoint hingeJoint;

    // Start is called before the first frame update
    void Start()
    {
        avManagement = GameObject.Find("Show Selector").GetComponentInChildren<SC_AV_Management>();
        uiPlayRecord = GameObject.Find("Show Selector").GetComponentInChildren<UI_PlayRecord>();
        animator = GetComponent<Animator>();
        leftReel.SetActive(false);
        rightReel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadDeck()
    {
        if (TapeLoaded == true)
        {
            leftReel.SetActive(false);
            rightReel.SetActive(false);
            lefttape.SetActive(false);
            righttape.SetActive(false);
            TapeLoaded = false;
            uiPlayRecord.Stop();
        }
        else
        {
            uiPlayRecord.manager.Load();
            leftReel.SetActive(true);
            lefttape.SetActive(true);
            righttape.SetActive(true);
            rightReel.SetActive(true);
            TapeLoaded = true;
        }
    }

    public void TogglePlayback()
    {
        if (ShowtapePlaying == false)
        {
            animator.speed = 1f;
            avManagement.Resume();
            ShowtapePlaying = true;
        }
        else
        {
            animator.speed = 0f;
            avManagement.Pause();
            ShowtapePlaying = false;
        }
    }

    public void RewindTape()
    {
        avManagement.FFSong(-1);
        animator.speed = avManagement.manager.referenceSpeaker.pitch;
    }

    public void FastForwardTape()
    {
        avManagement.FFSong(1);
        animator.speed = avManagement.manager.referenceSpeaker.pitch;
    }

    public void EnableLight()
    {
        if (LightEnabled == true)
        {
            float emissiveIntensity = 0;
            Color emissiveColor = new Color(78, 27, 3);
            emissiveObject.GetComponent<Renderer>().materials[6].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
            LightEnabled = false;
        }
        else
        {
            float emissiveIntensity = 0.01f;
            Color emissiveColor = new Color(78, 27, 3);
            emissiveObject.GetComponent<Renderer>().materials[6].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
            LightEnabled = true;
        }
    }

    public void OpenCloseDoor()
    {
        if (DoorOpened == true)
        {
            audioSource.clip = doorClose;
            audioSource.PlayOneShot(doorClose);
            var motor = hingeJoint.motor;
            motor.force = 100;
            motor.targetVelocity = -80;
            motor.freeSpin = false;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;
            DoorOpened = false;
        }
        else
        {
            audioSource.clip = doorOpen;
            audioSource.PlayOneShot(doorOpen);
            var motor = hingeJoint.motor;
            motor.force = 100;
            motor.targetVelocity = 80;
            motor.freeSpin = false;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;
            DoorOpened = true;
        }
    }
}
