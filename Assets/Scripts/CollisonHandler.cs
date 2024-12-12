using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisonHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource audioSource;
    bool isControllable = true;
    
    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update() 
     {    
          RespondToDebugKeys();
     }
     void RespondToDebugKeys()
     {
          if (Keyboard.current.lKey.wasPressedThisFrame)
          {
                LoadNextLevel();
          }
          
     }
    private void OnCollisionEnter(Collision other) 
    {
        if (!isControllable)
        {
            return;
        }
        switch (other.gameObject.tag) 
        {
            case "Friendly" :
                Debug.Log("Everything is looking good");
                break; 

            case "Finish" :
                StartSuccesSequence();
                break;

            default : 
                StartCrashSequence();
                break;

        }
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextscene = currentScene + 1;
        if(nextscene == SceneManager.sceneCountInBuildSettings)
        {
            nextscene = 0;
        }
        SceneManager.LoadScene(nextscene);
    }
    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    void StartSuccesSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
}

