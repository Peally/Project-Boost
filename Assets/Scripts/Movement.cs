using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float engineThrust = 1000f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;
    
    Rigidbody rb;
    AudioSource rocketAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rocketAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //Freezes the physics based rotation for rigidbody (for collisions etc)
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //Unfreezing rotation so that physics engine can take over
    }

    private void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(!rocketAudioSource.isPlaying)
            {
                rocketAudioSource.PlayOneShot(mainEngine);
            }
            rb.AddRelativeForce(Vector3.up * engineThrust * Time.deltaTime);
        }
        else
        {
            rocketAudioSource.Stop();
        }

    }

   
}
