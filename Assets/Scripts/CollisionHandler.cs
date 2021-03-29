using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip successAudioClip;
    [SerializeField] AudioClip collisionAudioClip;

    int currentSceneIndex;
    int maxSceneIndex;
    float levelLoadDelay = 1.5f;
    bool isTransitioning = false;

    void OnCollisionEnter(Collision collision)
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        
        if(isTransitioning) { return; }  //simpler way than wapping whole case statement within if statement

            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Friendly contact - ignore");
                    break;
                case "Finish":
                    Debug.Log("Congrats you reached the landing pad");
                    if (!isTransitioning)
                        StartSuccessSequence();
                    break;
                case "Fuel":
                    Debug.Log("You picked up fuel!");
                    break;
                default:
                    if (!isTransitioning)
                        ObstacleCollision();
                    break;
            }

        //Debug.Log("Arrrghgh - you crashed into a: " + collision.gameObject.tag);

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.PlayOneShot(successAudioClip);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
        //ReloadLevel();
        //GetComponent<Movement>().enabled = true;
    }

    private void LoadNextLevel()
    {
        Debug.Log("current Scene Index: " + currentSceneIndex);
        Debug.Log("max Scene Index: " + maxSceneIndex);
        
        if (currentSceneIndex == maxSceneIndex)
            SceneManager.LoadScene(0);
        else  
            SceneManager.LoadScene(++currentSceneIndex);
        //SceneManager.LoadScene(++currentSceneIndex);
    }

    private void ObstacleCollision()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.PlayOneShot(collisionAudioClip);
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Collided with obstacle or enemy - back to the start with you");

        //Invoke("StartCrashSequence", 1f);
        StartCrashSequence();

    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
        //ReloadLevel();
        //GetComponent<Movement>().enabled = true;
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

}
