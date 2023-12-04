using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantToWater : MonoBehaviour, IPlantToWater
{
    public float delayForSoundAfterWatered = 2;
    public AudioClip soundToPlayAfterWatered = null;
    private AudioSource audioSource;

    private bool _isWilted = true;
    private Renderer _plantRenderer;
    [SerializeField] private Animator _plantAnimator;
    [SerializeField] private Color _wiltedColour = new Color(70f, 70f, 70f, 1f);
    [SerializeField] private Color _bloomColour = Color.white;
    [SerializeField] private Material _wiltedMaterial;
    [SerializeField] private Material _bloomMaterial;
    private float _bloomingSeconds = 4f;
    private float _targetPoint = 0f;


    public bool isWilted
    {
        get => _isWilted;
    }

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _plantRenderer = GetComponent<Renderer>();
        if (transform.GetComponent<Animator>())
        {
            _plantAnimator = transform.GetComponent<Animator>();
        }
        /*Outline _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
        _outline.OutlineColor = Color.yellow;
        _outline.OutlineWidth = 5f;
        RemoveHighlight();*/
    }

    void Update()
    {
        if (_isWilted == false && (_plantRenderer.material.color != _bloomColour))
        {
            MaterialTransition();
        }
    }

    public void Bloom()
    {
        // play bloom animation
        if (_isWilted)
        {
            Debug.Log("Bloom!");
            if (_plantAnimator)
            {
                _plantAnimator.SetTrigger("Bloom");
            }
            _isWilted = false;
            Invoke("playSoundAfterWatered", delayForSoundAfterWatered);
            _plantRenderer.material = _wiltedMaterial;
            //_plantRenderer.material.SetColor("_Color", _wiltedColour);
        }
    }

    private void playSoundAfterWatered()
    {
        if (soundToPlayAfterWatered != null)
        {
            audioSource.clip = soundToPlayAfterWatered;
            audioSource.Play();
        }
    }

    public void Highlight()
    {
        if (_isWilted)
        {
            Outline outline = GetComponent<Outline>();
            if (outline != null)
            {
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 5f;
                outline.enabled = true;
            }
            else
            {
                outline = gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 5f;
            }
        }
    }

    public void RemoveHighlight()
    {
        Outline outline = GetComponent<Outline>();
        if (outline != null && outline.enabled)
        {
            outline.OutlineColor = Color.yellow;
            outline.enabled = false;
        }
    }

    private void ColourTransition()
    {
        _targetPoint += Time.deltaTime / _bloomingSeconds;
        _plantRenderer.material.color = Color.Lerp(_wiltedColour, _bloomColour, _targetPoint);

    }

    private void MaterialTransition()
    {
        _targetPoint += Time.deltaTime / _bloomingSeconds;
        _plantRenderer.material.Lerp(_wiltedMaterial, _bloomMaterial, _targetPoint);
    }
}
