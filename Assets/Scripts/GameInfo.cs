using System.Collections;
using System.Collections.Generic;
using a;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{

	/// <summary>
	/// Полоска здоровья синего игрока
	/// </summary>
	public Slider BlueHPSlider;
	/// <summary>
	/// Полоска здоровья синего игрока
	/// </summary>
	public Slider RedHPSlider;
	
	/// <summary>
	/// Фоновая музыка
	/// </summary>
	public  AudioSource BackgroundMusic;
	
	/// <summary>
	/// Объект синего игрока
	/// </summary>
	public static PlayerInfo BluePlayer;
	/// <summary>
	/// Объект красного игрока
	/// </summary>
	public static PlayerInfo RedPlayer;

	/// <summary>
	/// Текст для золота синего игрока
	/// </summary>
	public Text BluePlayerGoldText;
	/// <summary>
	/// Текст для золота красного игрока
	/// </summary>
	public Text RedPlayerGoldText;

	/// <summary>
	/// Начальное количество золота игрокоа
	/// </summary>
	public float PlayerStartGold = 25;

	/// <summary>
	/// ссылка на текущую игру
	/// </summary>
	public static GameInfo gameInfo;
	
	/// <summary>
	/// Модель лучника
	/// </summary>
	public GameObject archerDull;
	/// <summary>
	/// Модель копейщика
	/// </summary>
	public GameObject spearmanDull;
	/// <summary>
	/// Модель катапульты
	/// </summary>
	public GameObject catapultDull;
	
	/// <summary>
	/// Статическая модель лучника
	/// </summary>
	public static GameObject ArcherDull;
	/// <summary>
	/// Статическая модель копейщика
	/// </summary>
	public static GameObject SpearmanDull;
	/// <summary>
	/// Статическая модель катапульты
	/// </summary>
	public static GameObject CatapultDull;

	/// <summary>
	/// Коэффициент звука в игре
	/// </summary>
	public const float VolumeCoefficient = 1f / 8f;
	
	/// <summary>
	/// Время начала игры
	/// </summary>
	public static float StartTime;
	/// <summary>
	/// Время игровой сессии
	/// </summary>
	public static float GameSessionTime = 0;

	/// <summary>
	/// Громкость, установленная игроком
	/// </summary>
	public static float MusicVolume = 1f;

	/// <summary>
	/// Текст окончания игры
	/// </summary>
	public Text GameoverText;

	/// <summary>
	/// Победитель игры
	/// </summary>
	public static string GameWinner = "Blue";
	
	/// <summary>
	/// Законченна ли игры
	/// </summary>
	public static bool IsFinished = false;
	/// <summary>
	/// Тип игры
	/// </summary>
	public enum GameType
	{
		TwoPlayerOneDevice,
		OnePlayerOneDevice,
		TwoPlayerTwoDevice
	}

	/// <summary>
	/// Тип текущей игры
	/// </summary>
	public static  GameType CurrentGameType;

	
	
	/// <summary>
	/// Обновляет игформацию о золоте и здоровье игроков
	/// </summary>
	public void UpdateUI()
	{
		BluePlayerGoldText.text = "" + Mathf.Floor(BluePlayer.Resources.AmountOfGold);
		RedPlayerGoldText.text = "" + Mathf.Floor(RedPlayer.Resources.AmountOfGold);

		BlueHPSlider.value = BluePlayer.PlayerHP;
		RedHPSlider.value = RedPlayer.PlayerHP;
	}
	
	

	private void Awake()
	{
		BackgroundMusic.volume = MusicVolume * VolumeCoefficient;
		
		gameInfo = this;
		
		RedPlayer = new PlayerInfo();
		BluePlayer = new PlayerInfo();
		RedPlayer.Resources =new PlayerResources();
		BluePlayer.PlayerColor = "Blue";
		BluePlayer.Resources =new PlayerResources();
		BluePlayer.Resources.GoldChanged += UpdateUI;
		BluePlayer.Resources.AmountOfGold = PlayerStartGold;
		BluePlayer.IsBaseBlocked = false;
		
		RedPlayer.PlayerColor = "Red";
		RedPlayer.Resources.GoldChanged += UpdateUI;
		RedPlayer.Resources.AmountOfGold = PlayerStartGold;
		RedPlayer.IsBaseBlocked = false;


		if (CurrentGameType == GameType.OnePlayerOneDevice)
		{
			RedPlayerGoldText.transform.parent.transform.Translate(10000,0,0);
		}
	}

	// Use this for initialization
	void Start ()
	{
		TerrainScript.EnableSpawning = true;
		IsFinished = false;
		StartTime = Time.time;
		
		ArcherDull = archerDull;
		CatapultDull = catapultDull;
		SpearmanDull = spearmanDull;
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameoverText.text = GameWinner + " win!";
	}

	
	/// <summary>
	/// Завершает игру и подводит итог
	/// </summary>
	public static void FinishGame()
	{
		if (IsFinished) return;

		IsFinished = true;
		
		GameSessionTime = Time.time - StartTime;
		TerrainScript.EnableSpawning = false;
		
		
		UIcontrollerScript.HidePanel(UIcontrollerScript.DownPanel);
		UIcontrollerScript.ShowPanel(UIcontrollerScript.EndGamePanel);
		
		Debug.Log("Finishing the game");
	}

	/// <summary>
	/// Возвращает в стартовое меню
	/// </summary>
	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
