using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class PlayMenuManager : MonoBehaviour
{
    public GameObject stopGraphic;
    public GameObject playGraphic;
    public GameObject loopGraphic;
    public GameObject loopSongGraphic;
    public GameObject finishButton;
    public UI_PlayRecord playUI;
    public TMP_Text TimeText;

    string EndTimeResult;

    public TMP_Text titleText;
    public Slider playSlider;
    int endtexttimer = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        if (playUI.manager.playMovements)
        {
            if (playUI.manager.referenceSpeaker.isPlaying)
            {
                if (stopGraphic.activeSelf)
                {
                    stopGraphic.SetActive(false);
                }
                if (!playGraphic.activeSelf)
                {
                    playGraphic.SetActive(true);
                }
            }
            else
            {
                if (!stopGraphic.activeSelf)
                {
                    stopGraphic.SetActive(true);
                }
                if (playGraphic.activeSelf)
                {
                    playGraphic.SetActive(false);
                }
            }
            switch (playUI.manager.songLoopSetting)
            {
                case UI_ShowtapeManager.LoopVers.noLoop:
                    if (!loopGraphic.activeSelf)
                    {
                        loopGraphic.SetActive(true);
                    }
                    if (loopSongGraphic.activeSelf)
                    {
                        loopSongGraphic.SetActive(false);
                    }
                    break;
                case UI_ShowtapeManager.LoopVers.loopSong:
                    if (loopGraphic.activeSelf)
                    {
                        loopGraphic.SetActive(false);
                    }
                    if (!loopSongGraphic.activeSelf)
                    {
                        loopSongGraphic.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

            int nowtimer = (int)Mathf.Floor(playUI.manager.referenceSpeaker.time);
            if (playUI.manager.useVideoAsReference)
            {
                playSlider.value = (float)playUI.manager.referenceVideo.time / ((float)playUI.manager.referenceVideo.length / (float)playUI.manager.referenceVideo.GetAudioChannelCount(0)) * 100;
            }
            else
            {
                playSlider.value = playUI.manager.referenceSpeaker.time / playUI.manager.speakerClip.length * 100;
            }

            TimeText.text = Mathf.Floor(nowtimer / 60).ToString("00") + ":" + (nowtimer % 60).ToString("00") + "/" + EndTimeResult;
        }

    }

    public void ClickSlider(int section)
    {
        if (playUI.manager.useVideoAsReference)
        {
            playUI.manager.referenceVideo.time = (section / 10.0f) * ((float)playUI.manager.referenceVideo.length / (float)playUI.manager.referenceVideo.GetAudioChannelCount(0));
        }
        else
        {
            playUI.manager.referenceSpeaker.time = (section / 10.0f) * playUI.manager.speakerClip.length;
        }

        playUI.manager.syncTvsAndSpeakers.Invoke();
    }

    public void TextUpdate(bool record)
    {
        if (!playUI.manager.recordMovements)
        {
            this.transform.localScale = Vector3.one;
            foreach (Transform child in transform)
            {
                Button3D check = child.GetComponent<Button3D>();
                if (check != null)
                {
                    check.enabled = true;
                }
                child.transform.localScale = Vector3.one;
            }
            finishButton.SetActive(false);
            if (playUI.manager.useVideoAsReference)
            {
                endtexttimer = (int)((float)playUI.manager.referenceVideo.length / (float)playUI.manager.referenceVideo.GetAudioChannelCount(0));
            }
            else
            {
                endtexttimer = (int)(playUI.manager.speakerClip.length);
            }
            EndTimeResult = Mathf.Floor(endtexttimer / 60.0f).ToString("00") + ":" + (endtexttimer % 60).ToString("00");

            //Title
            if (playUI.manager.useVideoAsReference)
            {
                titleText.text = "Youtube Video";
            }
            else
            {
                string[] combined = Path.GetFileName(playUI.manager.showtapeSegmentPaths[playUI.manager.currentShowtapeSegment]).Split(new string[] { " - " }, StringSplitOptions.None);

                if (combined.Length > 1)
                {
                    titleText.text = combined[0];
                }
                else
                {
                    titleText.text = combined[0].Substring(0, combined[0].Length - 5);
                }
            }

        }
        else
        {
            endtexttimer = (int)(playUI.manager.speakerClip.length);
            EndTimeResult = Mathf.Floor(endtexttimer / 60.0f).ToString("00") + ":" + (endtexttimer % 60).ToString("00");

            string[] combined = Path.GetFileName(playUI.manager.showtapeSegmentPaths[playUI.manager.currentShowtapeSegment]).Split(new string[] { " - " }, StringSplitOptions.None);

            if (combined.Length > 1)
            {
                titleText.text = combined[0];
            }
            else
            {
                titleText.text = combined[0].Substring(0, combined[0].Length - 5);
            }
            finishButton.SetActive(true);
        }
    }
    public void SwapIcon(bool pause)
    {
        if (!pause)
        {
            playGraphic.SetActive(true);
            stopGraphic.SetActive(false);
        }
        else
        {
            playGraphic.SetActive(false);
            stopGraphic.SetActive(true);
        }
    }
    public void SwapLoop(int ingoing)
    {
        if (ingoing == 0)
        {
            loopGraphic.SetActive(true);
            loopSongGraphic.SetActive(false);
        }
        else
        {
            loopGraphic.SetActive(false);
            loopSongGraphic.SetActive(true);
        }
    }
}