using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    public GameObject promptImageBorder;
    public GameObject promptImage;
    private bool promptIsMaxmimzed = false;
    private Vector2 borderMinSize = new Vector2(310f, 160f);
    private Vector2 borderMinPos = new Vector2(191f, -157f);
    private Vector2 borderMaxSize = new Vector2(1845, 965);
    private Vector2 borderMaxPos = new Vector2(958, -561);
    private Vector2 promptSize = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(promptImageBorder.GetComponent<RectTransform>().rect);
        Debug.Log(promptImageBorder.GetComponent<RectTransform>().position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            AdjustPromptSize();
        }
        if (Input.GetKeyDown("n")) 
        {
            AdjustPromptOpacity();
        }
    }

    private void AdjustPromptOpacity()
    {
        Color newColor = promptImageBorder.GetComponent<Color>();
        newColor.a = 0.4f;
        // promptImageBorder.GetComponent<Color>() = newColor;
        throw new NotImplementedException();
    }

    private void AdjustPromptSize()
    {
        if (promptIsMaxmimzed)
        {
            promptImageBorder.GetComponent<RectTransform>().sizeDelta = borderMinSize;
            promptImageBorder.GetComponent<RectTransform>().anchoredPosition = borderMinPos;
            promptSize[0] = borderMinSize[0] - 10;
            promptSize[1] = borderMinSize[1] - 10;
            promptIsMaxmimzed = false;
        }
        else 
        {
            promptImageBorder.GetComponent<RectTransform>().sizeDelta = borderMaxSize;
            promptImageBorder.GetComponent<RectTransform>().anchoredPosition = borderMaxPos;
            promptSize[0] = borderMaxSize[0] - 10;
            promptSize[1] = borderMaxSize[1] - 10;
            promptIsMaxmimzed = true;
        }
        promptImage.GetComponent<RectTransform>().sizeDelta = new Vector2(promptSize[0], promptSize[1]);
    }
}
