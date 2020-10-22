using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] bool rocketColissionsEnabled = true;

    [SerializeField] float rotationThrust = 250f;
    [SerializeField] float engineThrust = 200f;
    [SerializeField] float levelLoadDelay = 2.5f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip die;

    [SerializeField] ParticleSystem mainEngineParticlesRight;
    [SerializeField] ParticleSystem mainEngineParticlesLeft;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem dieParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugInput();
        }
    }

    private void RespondToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("you pressed the L key");
            print("loading next level...");
            StartWinSequence();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            print("colissions toggled");
            rocketColissionsEnabled = !rocketColissionsEnabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !rocketColissionsEnabled)
        {
            return;
        }

        switch(collision.gameObject.tag)
        {
            case "Safe":
                print("rocket hit something Safe!");
                //do nothing
                break;
            case "Finish":
                print("rocket hit the Finish!");
                StartWinSequence();
                break;
            default:
                print("rocket hit something Deadly (default)!");
                StartDieSequence();
                break;
        }
    }
    private void StartWinSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(win);
        StopEngineParticles();
        winParticles.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }
    private void StartDieSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(die);
        StopEngineParticles();
        dieParticles.Play();
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentSceneIndex += 1;
        SceneManager.LoadScene(currentSceneIndex % totalScenes);
        state = State.Alive;
    }

    //spacebar for thrust, play sound if thrusting
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
            PlayEngineSound();
            PlayEngineParticles();
        }
        else
        {
            audioSource.Stop();
            StopEngineParticles();
        }
    }

    private void PlayEngineParticles()
    {
        if (!mainEngineParticlesRight.isEmitting)
        {
            mainEngineParticlesRight.Play();
            mainEngineParticlesLeft.Play();
        }
    }
    private void StopEngineParticles()
    {
        mainEngineParticlesRight.Stop();
        mainEngineParticlesLeft.Stop();
    }

    private void PlayEngineSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void Thrust()
    {
        float engineThrustThisFrame = engineThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * engineThrustThisFrame);
    }

    //rotate either left or right
    private void RespondToRotateInput()
    {
        //pause physics
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rotationThrust * Time.deltaTime;

        //look for input and rotate
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        //resume physics
        rigidBody.freezeRotation = false;

    }
}
