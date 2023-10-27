using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUIController : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _controlUIAnimator;
    [SerializeField] private GameObject _triggerText;
    [SerializeField] private GameObject _buttonText;
    [SerializeField] private float _waitTime = 7.0f;



    void Start()
    {
        _controlUIAnimator = GetComponent<Animator>();
        StartCoroutine(PlayControls());
    }


    IEnumerator PlayControls()
    {
        Debug.Log("Play");
        yield return new WaitForSeconds(1.0f);
        DisplayTriggerText();
        yield return new WaitForSeconds(_waitTime);
        StartCoroutine(HideControlsUI());
        yield return new WaitForSeconds(2.0f);
        DisplayButtonText();
        yield return new WaitForSeconds(_waitTime);
        StartCoroutine(HideControlsUI());
    }


    public void DisplayTriggerText()
    {
        Debug.Log("Trigger");
        _triggerText.SetActive(true);
        _controlUIAnimator.Play("ControlsFadeIn");
    }

    public void DisplayButtonText()
    {
        Debug.Log("Button");
        _buttonText.SetActive(true);
        _controlUIAnimator.Play("ControlsFadeIn");
    }

    public IEnumerator HideControlsUI()
    {
        Debug.Log("Hide");
        _controlUIAnimator.Play("ControlsFadeOut");
        yield return new WaitForSeconds(1.0f);
        _triggerText.SetActive(false);
        _buttonText.SetActive(false);
    }




}
