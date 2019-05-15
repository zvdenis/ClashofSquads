using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    /// <summary>
    /// Фоновая музыка для главного мееню
    /// </summary>
    public AudioSource BackgroundMusic;
    
    public Slider MusicVolumeSlider;
    /// <summary>
    /// Запускает режими игры для двух игроков на одном устройстве
    /// </summary>
    public void GoToTwoPlayerMode()
    {
        PrepareGame();
        GameInfo.CurrentGameType = GameInfo.GameType.TwoPlayerOneDevice;
        SceneManager.LoadScene("2PlayerScene");
    }

    private void Start()
    {
        MusicVolumeSlider.value = GameInfo.MusicVolume;
    }

    /// <summary>
    /// Включает режим игры для двух игроков
    /// </summary>
    public void GoToOnePlayerMode()
    {
        PrepareGame();
        GameInfo.CurrentGameType = GameInfo.GameType.OnePlayerOneDevice;
        SceneManager.LoadScene("2PlayerScene");
    }

    /// <summary>
    /// Выхоидт из игры
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Общая длля всех типов игры подготовка
    /// </summary>
    private void PrepareGame()
    {
        GameInfo.MusicVolume = MusicVolumeSlider.value;
    }
    
    /// <summary>
    /// Изменение громкости звука
    /// </summary>
    public void ChangeVolume()
    {
        GameInfo.MusicVolume = MusicVolumeSlider.value;
        BackgroundMusic.volume = GameInfo.MusicVolume * GameInfo.VolumeCoefficient;

    }
    
}
