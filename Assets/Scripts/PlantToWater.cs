using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantToWater : MonoBehaviour, IPlantToWater
{
    private bool _isWilted = true;
    private Renderer _plantRenderer;
    [SerializeField] private Animator _plantAnimator;
    [SerializeField] private Color _wiltedColor = new Color(145f, 123f, 26f, 1f);
    [SerializeField] private Color _bloomColor = new Color(89f, 181f, 56f, 1f);
    [SerializeField] private float _bloomingSeconds = 2f;
    private float _targetPoint = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        _plantRenderer = GetComponent<Renderer>();
        _plantRenderer.material.SetColor("_Color", _wiltedColor);
        if (transform.GetComponent<Animator>())
        {
            _plantAnimator = transform.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (_isWilted == false && _plantRenderer.material.color != _bloomColor)
        {
            ColourTransition();
        }
    }

    public void Bloom()
    {
        // play bloom animation
        if (_isWilted)
        {
            Debug.Log("Bloom!");
            _plantAnimator.SetTrigger("Bloom");
           
            _isWilted = false;
        }
    }

    private void ColourTransition()
    {
        _targetPoint += Time.deltaTime / _bloomingSeconds;
        _plantRenderer.material.color = Color.Lerp(_wiltedColor, _bloomColor, _targetPoint);
    }
}