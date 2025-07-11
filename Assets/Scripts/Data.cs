/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using Utilities;

public class Data
{
	public static bool isPauseGame;

	public static bool isGameOver;

	public static bool isBuyIAP;

	public static bool isPlayEffect;

	public static int idPopup;

	public static int isSound;

	public static int isMusic;

	public static float DISTANCE_BLOCK;

	public static float B_LEFT = -2.94f;

	public static float B_BOT = -2.15f;

	public static int MATRIX_N;

	public static float SCALE_BOT;

	public const float SCALE_MOVE = 1.1f;

	public static string KEY_GAME_DATA = "BlockGudianMapData0808";

	public static string KEY_BEST_SCORE = "BEST";

	public const string KEY_NAME_PLAYER = "Name";

	public const string GAME_MODE = "Normal";

	public const string CHILD_ROOT = "get_send_score";

	public static string NAME_GAME = "Block_hoptac_jewelcu";

	public const string KEY_SOUND = "BlockGudianblock3d_key_sound";

	public const string KEY_MUSIC = "BlockGudianblock3d_key_music";

	public static float[] POS_BLOCK_BOT = new float[3]
	{
		-2.2f,
		0f,
		2.2f
	};

	public const string LINK_SERVER_SCORE = "http://dotmobstudio.com";

	public const string LINK_POLICY = "https://www.google.com";

	public static int idTypeGame;

	public static string namePlayer;

	public const float DISTANCE_BLOCK_ROOT = 0.85f;

	public const float DISTANCE_EFFECT = 0.1f;

	public static int countView = 0;

	public static bool isHaveRocketFly;

	public static int countHelp = 0;

	public const string KEY_HELP = "BlockGudianblock_gudian_help";

	public static float scaleBlockTypeGame = 1f;

	public static bool isTipBoomb;

	public const string KEY_TIP_BOOMB = "BlockGudianblock_tip_boomb";

	public static bool isTipTime;

	public const string KEY_TIP_TIME = "BlockGudianblock_tip_time";

	public static bool isShowInterstitial = true;

	public static bool isViewVideo = false;

	public static bool isAllowShowVideo = true;

	public static Vector3 MatrixToPos(int i, int j)
	{
		return new Vector3((float)j * DISTANCE_BLOCK + B_LEFT, B_BOT + DISTANCE_BLOCK * (float)i, 0f);
	}

	public static BPoint PosToMatrix(Vector3 pos)
	{
		int x = Mathf.RoundToInt((pos.y - B_BOT) / DISTANCE_BLOCK);
		int y = Mathf.RoundToInt((pos.x - B_LEFT) / DISTANCE_BLOCK);
		return new BPoint(x, y, 1);
	}

	public static void Init()
	{
		if (idTypeGame == 8)
		{
			MATRIX_N = 8;
			B_LEFT = -2.94f;
			B_BOT = -2.15f;
			DISTANCE_BLOCK = 0.84f;
			KEY_GAME_DATA = "MapData0808";
			KEY_BEST_SCORE = "BEST0808";
			NAME_GAME = "BlockJewel2DGudian";
			SCALE_BOT = 0.4f;
			scaleBlockTypeGame = 1f;
		}
		else if (idTypeGame == 10)
		{
			MATRIX_N = 10;
			B_LEFT = -3.01f;
			B_BOT = -2.23f;
			DISTANCE_BLOCK = 0.67f;
			KEY_GAME_DATA = "MapData";
			KEY_BEST_SCORE = "BEST";
			NAME_GAME = "Block_hoptac_jewelcu";
			SCALE_BOT = 0.5f;
			scaleBlockTypeGame = 0.796f;
		}
		else if (idTypeGame == 9)
		{
			MATRIX_N = 8;
			B_LEFT = -2.94f;
			B_BOT = -2.15f;
			DISTANCE_BLOCK = 0.84f;
			KEY_GAME_DATA = "MapData0808R";
			KEY_BEST_SCORE = "BEST0808R";
			NAME_GAME = "BlockJewel2DRGudian";
			SCALE_BOT = 0.4f;
			scaleBlockTypeGame = 1f;
		}
		else
		{
			MATRIX_N = 8;
			B_LEFT = -2.94f;
			B_BOT = -2.15f;
			DISTANCE_BLOCK = 0.84f;
			KEY_GAME_DATA = "MapData0808B";
			KEY_BEST_SCORE = "BEST0808B";
			NAME_GAME = "BlockJewel2DBGudian";
			SCALE_BOT = 0.4f;
			scaleBlockTypeGame = 1f;
		}
	}

	public static void InitStatic()
	{
		isShowInterstitial = true;
		isViewVideo = false;
		isPauseGame = false;
		isGameOver = false;
		isHaveRocketFly = false;
		isAllowShowVideo = true;
		namePlayer = UnityEngine.PlayerPrefs.GetString("Name", string.Empty);
		if (namePlayer.Equals(string.Empty))
		{
			namePlayer = "Player" + Random.Range(100, 2000);
			UnityEngine.PlayerPrefs.SetString("Name", namePlayer);
			UnityEngine.PlayerPrefs.Save();
		}
		isSound = Utilities.PlayerPrefs.GetInt("BlockGudianblock3d_key_sound", 1);
		isMusic = Utilities.PlayerPrefs.GetInt("BlockGudianblock3d_key_music", 1);
		countHelp = Utilities.PlayerPrefs.GetInt("BlockGudianblock_gudian_help", 0);
		isTipBoomb = Utilities.PlayerPrefs.GetBool("BlockGudianblock_tip_boomb", defaultValue: false);
		isTipTime = Utilities.PlayerPrefs.GetBool("BlockGudianblock_tip_time", defaultValue: false);
	}
}
