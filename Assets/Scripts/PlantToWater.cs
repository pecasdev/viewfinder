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
    [SerializeField] private Color _wiltedColor = new Color(145f, 123f, 26f, 1f);
    [SerializeField] private Color _bloomColor = new Color(89f, 181f, 56f, 1f);
    [SerializeField] private float _bloomingSeconds = 2f;
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
        _plantRenderer.material.SetColor("_Color", _wiltedColor);
        if (transform.GetComponent<Animator>())
        {
            _plantAnimator = transform.GetComponent<Animator>();
        }
        Outline _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.yellow;
        _outline.OutlineWidth = 5f;
        RemoveHighlight();
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
            Invoke("playSoundAfterWatered", delayForSoundAfterWatered);
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
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 5f;
                outline.enabled = true;
            }
            else
            {
                outline = gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
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
        _plantRenderer.material.color = Color.Lerp(_wiltedColor, _bloomColor, _targetPoint);
    }
}
