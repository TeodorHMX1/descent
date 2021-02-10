using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Constants
{
	//Scenes
	public const string SceneBattle = "Battle";
	public const string SceneMenu = "MainMenu";

	//Gun Types
	public const string Pistol = "Pistol";
	public const string ShotGun = "ShotGun";
	public const string AssaultRifle = "AssaultRifle";

	//Enemy Types
	public const string RedRobot = "RedRobot";
	public const string BlueRobot = "BlueRobot";
	public const string YellowRobot = "YellowRobot";

	//Pickup Types
	public const int PickUpPistolAmmo = 1;
	public const int PickUpAssaultRifleAmmo = 2;
	public const int PickUpShotgunAmmo = 3;
	public const int PickUpHealth = 4;
	public const int pickUpArmor = 5;

	//Misc
	public const string Game = "GameManager";
	public const float CameraDefaultZoom = 60f;

	public const string PlayPrefsSensivityX = "PlayPrefs_SensivityX";
	public const string PlayPrefsSensivityY = "PlayPrefs_SensivityY";
	public const string PlayPrefsSoundVolume = "PlayPrefs_SoundVolume";

	public const string SceneGameMenu = "GameMenuScreen";
	public const string SceneGameOver = "GameOverScreen";
	public const string SceneGameWon = "GameWonScreen";
	public const string SceneGame = "GameScreen";
	public const string SceneGameOptions = "OptionsScreen";
	public const string SceneGameInstructions = "InstructionsScreen";

	public static readonly int[] AllPickUpTypes =
	{
		pickUpArmor,
		PickUpAssaultRifleAmmo,
		PickUpHealth,
		PickUpPistolAmmo,
		PickUpShotgunAmmo
	};
}