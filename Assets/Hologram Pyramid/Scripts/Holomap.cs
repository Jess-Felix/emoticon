using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Holomap : MonoBehaviour
{
   /* Equal to enum Emotion
    private const int ANGRY = 0;
    private const int HAPPY = 1;
    private const int SAD = 2;
    private const int CALM = 3;
    private const int LOAD = 4;
    */

    enum Emotion
    {
        Load = 0,
        Happy = 1,
        Calm = 2,
        Angry = 3,
        Sad = 4

    }

    public float minPlaytime = 2.5f;
    public float maxPlaytime = 10.0f;
        
    public VideoPlayer videoPlayer;
    public VideoClip loopLoading, fadeOutLoading, fadeInLoading, loopSad, fadeOutSad, fadeInSad, loopHappy, fadeOutHappy,
        fadeInHappy, loopCalm, fadeOutCalm, fadeInCalm, loopAngry, fadeOutAngry, fadeInAngry;
    
    int currentEmotion;
    public int nextEmotion;
   
    private float currentTime;
    private bool isTransitioning;
    public bool startNewEmotionAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        currentTime = 0;
        isTransitioning = false;
        startNewEmotionAnim = false;
        
        currentEmotion = (int) Emotion.Load;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (startNewEmotionAnim)
        {
            updateNewTransition(nextEmotion);
            startNewEmotionAnim = false;
            
        }

        if (isTransitioning)
        {
            updateNewTransition(nextEmotion);
        }

        if (currentTime > maxPlaytime && currentEmotion != (int)Emotion.Load)
        {
            UpdateCurrentEmotion((int)Emotion.Load);
        }
        
      //  Debug.Log(currentTime + " / " + maxPlaytime);
    }

    public void UpdateCurrentEmotion(int newEmotion)
    {
        if (!startNewEmotionAnim && !isTransitioning &&
            currentTime > minPlaytime &&
            newEmotion != currentEmotion) // TODO: do not accept new emotion if == to current
        { 
            nextEmotion = newEmotion;
            startNewEmotionAnim = true;
        }
    }
    
    
    private void updateNewTransition(int nextEmotion)
    {
        isTransitioning = true;

        switch (currentEmotion)
        {
            case (int) Emotion.Load:
                // Stop condition. If isTransitioning and current == next emotion => We are playing the fade in animation
                auxUpdateNewTransition(nextEmotion, loopLoading, fadeOutLoading);

                break;
            case (int) Emotion.Angry:
                auxUpdateNewTransition(nextEmotion, loopAngry, fadeOutAngry);

                break;
            
            case (int) Emotion.Sad:
                auxUpdateNewTransition(nextEmotion, loopSad, fadeOutSad);
                break;
            
            case (int) Emotion.Happy:
                auxUpdateNewTransition(nextEmotion, loopHappy, fadeOutHappy);

                break;
            
            case (int) Emotion.Calm:
                auxUpdateNewTransition(nextEmotion, loopCalm, fadeOutCalm);
                break;
        }

    }

    private void auxUpdateNewTransition(int nextEmotion, VideoClip loopClip, VideoClip fadeOutClip)
    {
        if (currentEmotion == nextEmotion && !videoPlayer.isPlaying && videoPlayer.isPrepared
        ) // to check if turns tot !isPlaying at the end
        {
            videoPlayer.isLooping = true;

            videoPlayer.clip = loopClip;
            videoPlayer.Play();

            isTransitioning = false;
        }

        // First state of the animation
        else if (videoPlayer.clip == loopClip )
        {
            currentTime = 0;
            videoPlayer.isLooping = false;
            videoPlayer.clip = fadeOutClip;
            videoPlayer.Play();
        }
        // Second state: starts the fade in, update the current emotion to next emotion
        else if (videoPlayer.clip == fadeOutClip && !videoPlayer.isPlaying && videoPlayer.isPrepared
        ) // to check if turns tot !isPlaying at the end
        {
            // Call method to update fade in next emotion 
            videoPlayer.isLooping = false;

            updateFadeIn(nextEmotion);
        }
    }

    private void updateFadeIn(int nextEmotion)
    {
        currentEmotion = nextEmotion;
        switch (nextEmotion)
        {
            case (int) Emotion.Load:
                videoPlayer.clip = fadeInLoading;
                videoPlayer.Play();
            break;
            
            case (int) Emotion.Sad:
                videoPlayer.clip = fadeInSad;
                videoPlayer.Play();
                break;
            
            case (int) Emotion.Happy:
                videoPlayer.clip = fadeInHappy;
                videoPlayer.Play();
                break;
            
            case (int) Emotion.Calm:
                videoPlayer.clip = fadeInCalm;
                videoPlayer.Play();
                break;
            
            case (int) Emotion.Angry:
                videoPlayer.clip = fadeInAngry;
                videoPlayer.Play();
                break;
        }
    }
    
}
