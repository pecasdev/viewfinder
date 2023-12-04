using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCamSwitch : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Animator camera1Animator;
    public Animator camera2Animator;

    public float camera1AnimationLength; // Duration of Camera 1's animation
    public float camera2AnimationLength; // Duration of Camera 2's animation
    public string nextScene = "level1_new"; // Name of the next scene to load

    void Start()
    {
        // Start with Camera 1
        camera1.enabled = true;
        camera2.enabled = false;

        // Start Camera 1's animation
        camera1Animator.SetTrigger("StartAnimation");

        // Start the sequence
        StartCoroutine(Sequence());
    }

    public void StartCamera2Animation()
    {
        camera2Animator.SetTrigger("StartAnimation");

        // Start the coroutine to switch cameras after a delay
        StartCoroutine(SwitchCamerasAfterDelay(1.3f));
    }

    IEnumerator SwitchCamerasAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Switch cameras
        camera1.enabled = false;
        camera2.enabled = true;
    }

    IEnumerator Sequence()
    {
        // Wait for Camera 1's animation
        yield return new WaitForSeconds(camera1AnimationLength);

        // The StartCamera2Animation is expected to be triggered by an animation event

        // Wait for Camera 2's animation
        yield return new WaitForSeconds(camera2AnimationLength);

        // Load the next scene
        SceneManager.LoadScene(nextScene);
    }
}
