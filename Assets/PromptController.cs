using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PromptController : MonoBehaviour
{
    public GameObject promptImageBorder;
    public GameObject promptImage;
    private bool promptIsMaxmimzed = false;
    private bool promptIsTransparent = false;
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
            TogglePromptSize();
        }
        if (Input.GetKeyDown("n")) 
        {
            TogglePromptOpacity();
        }
    }

    private void TogglePromptOpacity()
    {
        if (!promptIsMaxmimzed)
        {
            return;
        }

        if (promptIsTransparent)
        {
            Color borderCol = promptImageBorder.GetComponent<Image>().color;
            borderCol.a = 1;
            promptImageBorder.GetComponent<Image>().color = borderCol;
            Color promptCol = promptImage.GetComponent<RawImage>().color;
            promptCol.a = 1;
            promptImage.GetComponent<RawImage>().color = promptCol;
            promptIsTransparent = false;
        }
        else
        {
            Color borderCol = promptImageBorder.GetComponent<Image>().color;
            borderCol.a = 0.3f;
            promptImageBorder.GetComponent<Image>().color = borderCol;
            Color promptCol = promptImage.GetComponent<RawImage>().color;
            promptCol.a = 0.3f;
            promptImage.GetComponent<RawImage>().color = promptCol;
            promptIsTransparent = true;
        }
    }

    private void TogglePromptSize()
    {
        // Minimize the prompt
        if (promptIsMaxmimzed)
        {
            // Make prompt opaque when minimizing
            if (promptIsTransparent)
            {
                TogglePromptOpacity();
            }
            promptImageBorder.GetComponent<RectTransform>().sizeDelta = borderMinSize;
            promptImageBorder.GetComponent<RectTransform>().anchoredPosition = borderMinPos;
            promptSize[0] = borderMinSize[0] - 10;
            promptSize[1] = borderMinSize[1] - 10;
            promptIsMaxmimzed = false;
        }
        // Maximize the prompt
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
