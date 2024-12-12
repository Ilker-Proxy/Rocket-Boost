
using UnityEngine;
using   UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
   [SerializeField] InputAction thrust;
   [SerializeField] InputAction rotation;
   [SerializeField] float thrustStrength = 10f;
   [SerializeField] float rotationStrength = 10f;
   [SerializeField] AudioClip mainEngineSFX;
   [SerializeField] ParticleSystem mainEngineParticles;
   [SerializeField] ParticleSystem leftThrustParticles;
   [SerializeField] ParticleSystem rightThrustParticles;
   Rigidbody rb;
   AudioSource audioSource;

   private void Start()
   {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
   }
     
   private void OnEnable() 
   {
        thrust.Enable();
        rotation.Enable();
   }
   private void FixedUpdate()
   {
        ProcessThrust();
        ProcessRotation();
   }
   private void ProcessThrust()
   {
    if(thrust.IsPressed())
        {
          StartThrusting();
        }
     else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ProcessRotation()
   {
    float rotationInput = rotation.ReadValue<float>();
    if(rotationInput < 0)
        {
            RotateRight();
        }
        else if(rotationInput > 0)
        {
          RotateLeft();
        }
        else
        {
          StopRotating();
        }

    }

    private void StopRotating()
    {
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrustParticles.isPlaying)
        {
          rightThrustParticles.Stop();
          leftThrustParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
   {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
   }
   void StartThrusting()
   {
     rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
     if(!audioSource.isPlaying)
     {
          audioSource.PlayOneShot(mainEngineSFX);
     }
     if (!mainEngineParticles.isPlaying)
     {
          mainEngineParticles.Play();
     }
   }
}

