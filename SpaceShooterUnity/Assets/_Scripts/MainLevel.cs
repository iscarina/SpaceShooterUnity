using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLevel : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    //Volume
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
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
