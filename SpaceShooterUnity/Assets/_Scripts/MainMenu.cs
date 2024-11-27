using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;
    [SerializeField] private Button ButtonSound;

    private AudioSource[] audioSources;

    private bool isMuted = false;

    void Start()
    {

        audioSources = FindObjectsOfType<AudioSource>();

        ButtonSound.onClick.AddListener(Volume);

        // Inicializa el sprite según el estado actual
        UpdateVolumeSprite();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Volume()
    {
        isMuted = !isMuted;

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = isMuted; 
        }

        UpdateVolumeSprite();
    }

    private void UpdateVolumeSprite()
    {
        ButtonSound.image.sprite = isMuted ? muteSprite : unmuteSprite;
    }

}
