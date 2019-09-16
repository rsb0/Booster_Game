using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidbody;
    AudioSource audioSource;
    float rcsThrust = 400f;
    float mainThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Rotate() {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.A)) {
            rigidbody.freezeRotation = true; // take manual control of rocket
            transform.Rotate(Vector3.forward * rotationThisFrame);
            rigidbody.freezeRotation = false; // resume physics control of rotation
        }
        else if(Input.GetKey(KeyCode.D)) {
            rigidbody.freezeRotation = true; // take manual control of rocket
            transform.Rotate(- Vector3.forward * rotationThisFrame);
            rigidbody.freezeRotation = false; // resume physics control of rotation
        }
        
    }

    private void Thrust() {
        if(Input.GetKey(KeyCode.Space)) {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);

            if(audioSource.volume != 1.0f){
                audioSource.volume = 1.0f;
            }
            if(! audioSource.isPlaying){
                audioSource.Play();
            }
        } else {
            float startVolume = audioSource.volume;
            if(audioSource.isPlaying) {
                if(audioSource.volume > 0) { 
                    audioSource.volume -= startVolume * (Time.deltaTime / 0.5f);
                } else {
                    audioSource.Stop();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Friendly":
                //Do nothing
                break;

            case "Finish":
                print("Hit Finish");
                SceneManager.LoadScene(1);
                break;

            default:
                print("Dead");
                SceneManager.LoadScene(0);
                break;
        }
    }
}
