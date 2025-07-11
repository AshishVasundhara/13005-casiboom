/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using Spine;
//using Gley.MobileAds;
using Spine.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class Manager : MonoBehaviour
{
	public static Manager instance;

	public Camera cameraMain;

	private List<Block> blocks;

	private int score;

	private int bestScore;

	public SpriteRenderer spriteScore;

	public SpriteRenderer spriteEffect;

	public SpriteRenderer spriteEnd;

	private bool isSaveMe;

	private bool isHighScore;

	private int countStep;

	public Image blackOut;

	public Block[] objectPrefabs;

	private bool isBlockTouched;

	private Block blockTouched;

	public LayerMask layerMask;

	public Transform board;

	public Transform bot;

	public GameObject boomLayer;

	public Transform guid_hand;

	private Sequence mySequence;

	public SkeletonGraphic mSkeletonGraphicTop;

	public SkeletonGraphic mSkeletonGraphicBot;

	private int countRocket;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		if (Data.idTypeGame == 11)
		{
			boomLayer.SetActive(value: true);
		}
		blocks = new List<Block>();
		score = 0;
		isSaveMe = false;
		isHighScore = false;
		//AdMobController.instance.ShowBanner();
		PlaySkeletonGraphic(isPlay: true);
		DOVirtual.DelayedCall(EffectIn() + 0.5f + 0.1f * (float)(Data.MATRIX_N - 1), delegate
		{
			LoadScene();
		});
	}

	public void PlaySkeletonGraphic(bool isPlay)
	{
		if (isPlay)
		{
			mSkeletonGraphicTop.freeze = false;
			mSkeletonGraphicBot.freeze = false;
			mSkeletonGraphicBot.AnimationState.SetAnimation(0, "animation", loop: false);
			mSkeletonGraphicBot.AnimationState.Complete += BotAnimationState_Complete;
			mSkeletonGraphicTop.AnimationState.SetAnimation(0, "animation", loop: false);
			mSkeletonGraphicTop.AnimationState.Complete += TopAnimationState_Complete;
		}
		else
		{
			mSkeletonGraphicTop.freeze = true;
			mSkeletonGraphicBot.freeze = true;
		}
	}

	private void TopAnimationState_Complete(TrackEntry trackEntry)
	{
		mSkeletonGraphicTop.freeze = true;
	}

	private void BotAnimationState_Complete(TrackEntry trackEntry)
	{
		mSkeletonGraphicBot.freeze = true;
	}

	private float EffectIn()
	{
		blackOut.DOColor(Color.clear, 0.5f).OnComplete(delegate
		{
			SoundManager.PlaySound("Down", 0.7f);
			board.DOMoveY(1.1f, 0.5f).SetEase(Ease.OutBack);
			bot.DOMoveY(-4.04f, 0.5f).SetEase(Ease.OutBack);
			blackOut.gameObject.SetActive(value: false);
		});
		return 1f;
	}

	private void SpawBlock(int countMax = 3)
	{
		for (int i = 0; i < countMax; i++)
		{
			int num = 0;
			num = ((score >= 3000) ? ((3000 >= score || score >= 5000) ? UnityEngine.Random.Range(0, 14) : UnityEngine.Random.Range(0, 13)) : UnityEngine.Random.Range(0, 12));
			int idTypeJewel = UnityEngine.Random.Range(0, 6);
			blocks.Add(CreateBlock(num, i, idTypeJewel));
		}
	}

	private Block CreateBlock(int id, int idPosInBot, int idTypeJewel)
	{
		Block block = UnityEngine.Object.Instantiate(objectPrefabs[id]);
		block.transform.parent = bot;
		block.SetTransform(Data.POS_BLOCK_BOT[idPosInBot] + 7f, Vector3.one * Data.SCALE_BOT);
		block.idPosInBot = idPosInBot;
		block.id = id;
		block.idSprite = idTypeJewel;
		block.SetSprites();
		return block;
	}

	public void CheckAndCreateBlocks(Block block, int countSpaw = 1)
	{
		RemoveBlock(block);
		if (Data.countHelp < 4)
		{
			ReadFile();
			SetGuilHand(is_active: true);
			BlocksIn();
		}
		else if (blocks.Count <= 0)
		{
			SpawBlock(countSpaw);
			BlocksIn();
		}
		if (!Data.isHaveRocketFly)
		{
			CheckAbilitiContinue();
		}
	}

	public void RemoveBlock(Block block)
	{
		if (blocks.Contains(block))
		{
			blocks.Remove(block);
		}
	}

	public void SetScore(int score)
	{
		this.score += score;
		GamePlayUI.instance.SetScore(this.score);
		if (this.score > bestScore)
		{
			isHighScore = true;
			bestScore = this.score;
			GamePlayUI.instance.SetBest(bestScore);
		}
	}

	public void CheckAbilitiContinue()
	{
		int num = 0;
		int index = 0;
		for (int i = 0; i < blocks.Count; i++)
		{
			if (CheckAbilitiBlock(blocks[i]))
			{
				index = i;
				num++;
			}
		}
		if (num <= 0)
		{
			if (score >= 0)
			{
				if (!isSaveMe) // && API.IsRewardedVideoAvailable())
				{
					Utilities.PlayerPrefs.SetInt(Data.KEY_BEST_SCORE, bestScore);
					Utilities.PlayerPrefs.DeleteKey(Data.KEY_GAME_DATA);
					Utilities.PlayerPrefs.Flush();
					Data.isGameOver = true;
					isSaveMe = true;
					//SaveMe.instance.Show();
					SetTxtEnd(isActive: true, isNoMove: true);
					GameOverFunction();
				}
				else
				{
					//SetTxtEnd(isActive: true, isNoMove: true);
					//GameOverFunction();
				}
				return;
			}
			SpawBlock();
			for (int j = 0; j < blocks.Count; j++)
			{
				if (CheckAbilitiBlock(blocks[j]))
				{
					index = j;
					num++;
				}
			}
		}
		else if (num == 1)
		{
			SetUnique(blocks[index]);
		}
	}

	public bool CheckAbilitiBlock(Block block)
	{
		bool flag = false;
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (!Cells.instance.IsActiveCell(i, j))
				{
					flag = block.CheckAbilitiInMatrixBlock(i, j);
					if (flag)
					{
						break;
					}
				}
			}
			block.SetColors(flag);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	public void SaveGameData()
	{
		GameData gameData = new GameData();
		gameData.score = score;
		gameData.cells = new List<GameData.CellData>();
		gameData.cells = Cells.instance.GetCellsData();
		gameData.countStep = countStep;
		if (Data.idTypeGame == 9)
		{
			gameData.countTimeLeft = TimeCount.instance.countTimeLeft;
		}
		else
		{
			gameData.countTimeLeft = 0f;
		}
		gameData.blocks = new List<GameData.BlockData>();
		for (int i = 0; i < blocks.Count; i++)
		{
			gameData.blocks.Add(blocks[i].GetBlockData());
		}
		string value = JsonUtility.ToJson(gameData, prettyPrint: true);
		Utilities.PlayerPrefs.SetString(Data.KEY_GAME_DATA, value);
		Utilities.PlayerPrefs.Flush();
	}

	private void LoadGameData()
	{
		bestScore = Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, 0);
		GamePlayUI.instance.SetBest(bestScore);
		string @string = Utilities.PlayerPrefs.GetString(Data.KEY_GAME_DATA, string.Empty);
		if (!@string.Equals(string.Empty))
		{
			GameData gameData = JsonUtility.FromJson<GameData>(@string);
			SetScore(gameData.score);
			for (int i = 0; i < gameData.cells.Count; i++)
			{
				Cells.instance.SetActiveCell(isActive: true, gameData.cells[i].idSprite, gameData.cells[i].status, gameData.cells[i].i, gameData.cells[i].j);
				if (Data.idTypeGame == 11 && gameData.cells[i].status == 3)
				{
					Cells.instance.CreateBomb(gameData.cells[i].i, gameData.cells[i].j, gameData.cells[i].count);
				}
				if (gameData.cells[i].status == 4)
				{
					Cells.instance.CreateRocket(gameData.cells[i].i, gameData.cells[i].j);
				}
				if (Data.idTypeGame == 9 && gameData.cells[i].status == 5)
				{
					Cells.instance.CreateAddTime(gameData.cells[i].i, gameData.cells[i].j, gameData.cells[i].addTime);
				}
			}
			for (int j = 0; j < gameData.blocks.Count; j++)
			{
				blocks.Add(CreateBlock(gameData.blocks[j].id, gameData.blocks[j].idPosInBot, gameData.blocks[j].idTypeJewel));
			}
			if (Data.idTypeGame == 9)
			{
				TimeCount.instance.SetActive(isActive: true, gameData.countTimeLeft);
			}
			if (Data.idTypeGame == 11)
			{
				countStep = gameData.countStep;
			}
		}
		else
		{
			SpawBlock();
			if (Data.idTypeGame == 9)
			{
				TimeCount.instance.SetActive(isActive: true);
			}
		}
		CheckAbilitiContinue();
	}

	private void LoadScene()
	{
		if (Data.countHelp < 4)
		{
			ReadFile();
			SetGuilHand(is_active: true);
		}
		else
		{
			LoadGameData();
		}
		Data.isPauseGame = false;
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		if ((Data.idTypeGame == 9 && !Data.isTipTime) || (Data.idTypeGame == 11 && !Data.isTipBoomb))
		{
			TipController.instance.SetActive(isActive: true);
		}
		BlocksIn();
	}

	private void ResetGame(bool isNow)
	{
		ResetBlock();
		SpawBlock();
		score = 0;
		countStep = 0;
		bestScore = Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, 0);
		GamePlayUI.instance.ResetScore();
		if (!isNow)
		{
			DOVirtual.DelayedCall(1f, delegate
			{
				DOTween.KillAll(complete: true);
				Data.isPlayEffect = false;
				Data.idPopup = 0;
				CheckAbilitiContinue();
				Cells.instance.ResetEffect();
				DOVirtual.DelayedCall(0.1f * (float)(Data.MATRIX_N - 1), delegate
				{
					Cells.instance.EffectIn();
					BlocksIn();
				});
				if (Data.idTypeGame == 9)
				{
					TimeCount.instance.SetActive(isActive: true);
				}
			});
			return;
		}
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		CheckAbilitiContinue();
		Cells.instance.ResetEffect();
		DOVirtual.DelayedCall(0.1f * (float)(Data.MATRIX_N - 1), delegate
		{
			Cells.instance.EffectIn();
			BlocksIn();
		});
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.SetActive(isActive: true);
		}
	}

	private void ResetStart()
	{
		score = 0;
		isSaveMe = false;
		isHighScore = false;
		bestScore = Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, 0);
		GamePlayUI.instance.ResetScore();
		DOVirtual.DelayedCall(EffectIn() + 0.5f, delegate
		{
			Cells.instance.EffectIn();
			SpawBlock();
			CheckAbilitiContinue();
			BlocksIn();
		});
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.SetActive(isActive: true);
		}
	}

	public void BlocksIn()
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			blocks[i].transform.DOMoveX(Data.POS_BLOCK_BOT[blocks[i].idPosInBot], 0.5f);
		}
	}

	public void BlocksSort()
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			blocks[i].transform.DOMoveX(Data.POS_BLOCK_BOT[i], 0.5f);
			blocks[i].idPosInBot = i;
		}
	}

	private void ResetBlock()
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			UnityEngine.Object.Destroy(blocks[i].gameObject);
		}
		blocks.Clear();
	}

    private Vector3 _lastMousePosition = Vector3.zero;
    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            switch (Data.idPopup)
            {
                case 1:
                    Setting.instance.ResumeButton();
                    break;
                case 2:
                    GameOver.instance.ReSetButton();
                    break;
                case 3:
                    LeaderBoard.instance.Hide();
                    break;
                case 4:
                    LeaderBoard.instance.SetActiveNoti(isActive: false);
                    break;
                case 5:
                    LeaderBoard.instance.SetActiveRename(isActive: false);
                    break;
                case 7:
                    SaveMe.instance.Hide();
                    SetTxtEnd(isActive: true, isNoMove: true);
                    GameOverFunction();
                    break;
                case 10:
                    HighScore.instance.SetActive(isActive: false);
                    break;
                case 11:
                    TipController.instance.SetActive(isActive: false);
                    break;
                default:
                    GamePlayUI.instance.SettingClick();
                    break;
                case 6:
                case 8:
                case 9:
                    break;
            }
        }
        //if (UnityEngine.Input.touchCount <= 0)
        //{
        //    isBlockTouched = false;
        //    return;
        //}
        Touch touch;
        if (Application.isEditor)
        {
            touch = new Touch();
            if (Input.GetMouseButtonDown(0))
            {
                touch.position = Input.mousePosition;
                touch.deltaTime = Time.deltaTime;
                touch.deltaPosition = Input.mousePosition - _lastMousePosition;
                touch.phase = TouchPhase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                touch.position = Input.mousePosition;
                touch.deltaTime = Time.deltaTime;
                touch.deltaPosition = Input.mousePosition - _lastMousePosition;
                touch.phase = touch.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touch.position = Input.mousePosition;
                touch.deltaTime = Time.deltaTime;
                touch.deltaPosition = Input.mousePosition - _lastMousePosition;
                touch.phase = TouchPhase.Ended;
            }
        }
        else
        {
            touch = UnityEngine.Input.GetTouch(0);
        }

        Vector3 position = touch.position;
        Vector2 vector = cameraMain.ScreenToWorldPoint(position);
        if (touch.phase == TouchPhase.Began && !Data.isPauseGame)
        {
            //Vector2 vector2 = base.transform.position;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, vector - vector2, Vector2.Distance(vector, vector2), layerMask);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerMask);
            if (raycastHit2D.collider != null)
            {
                if (raycastHit2D.transform.gameObject.tag.Contains("ButtonSetting"))
                {
                    Vector3 localScale = raycastHit2D.transform.localScale;
                    if (localScale.y == 1f)
                    {
                        SoundManager.PlaySound("btn");
                        raycastHit2D.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
                        GamePlayUI.instance.SettingClick();
                    }
                }
                else
                {
                    isBlockTouched = true;
                    blockTouched = raycastHit2D.transform.gameObject.GetComponent<Block>();
                    blockTouched.Down(vector);
                }
            }
        }
        if (isBlockTouched && touch.phase == TouchPhase.Moved)
        {
            blockTouched.Drag(vector);
        }
        if (isBlockTouched && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            blockTouched.Up();
            isBlockTouched = false;
            blockTouched = null;
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            SaveFile("help_start_3");
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            GameOver.instance.SetActive(isActive: true, isHigh: true);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            TipController.instance.SetActive(isActive: true);
        }
    }

	public void ResumeButton()
	{
		Data.isPlayEffect = true;
		Data.isPauseGame = false;
		Data.isPlayEffect = false;
		Data.idPopup = 0;
	}

	public void RePlayButton()
	{
		Data.isPlayEffect = true;
		DOTween.KillAll(complete: true);
		Cells.instance.ResetCells();
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		Data.isPauseGame = false;
		Data.isGameOver = false;
		countStep = 0;
		ResetStart();
	}

	public void ResetButton()
	{
		Data.isPlayEffect = true;
		Data.isPauseGame = false;
		Data.isGameOver = false;
		Utilities.PlayerPrefs.DeleteKey(Data.KEY_GAME_DATA);
		Utilities.PlayerPrefs.Flush();
		ResetGame(!Cells.instance.EatCells());
		Cells.instance.ResetUnique();
	}

	public void GameOverFunction()
	{
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.PauseTime(isPause: true);
		}
		SoundManager.PlaySound("over");
		Data.isPauseGame = true;
		Data.isGameOver = true;
		GameOver.instance.ResetScore();
		Utilities.PlayerPrefs.SetInt(Data.KEY_BEST_SCORE, bestScore);
		Utilities.PlayerPrefs.DeleteKey(Data.KEY_GAME_DATA);
		Utilities.PlayerPrefs.Flush();
		Cells.instance.GameOverEffect();
		PoolsRocket.instance.PoolObject();
		PoolCrack.instance.PoolObject();
		PoolsBomb.instance.PoolObject();
		PoolAddTime.instance.PoolObject();
		PoolsCellShadown.instance.PoolObject();
		SoundManager.PlaySound("Up");
		DOVirtual.DelayedCall(2.5f, delegate
		{
			SetTxtEnd(isActive: false, isNoMove: false);
			if (board != null)
			{
				board.DOMoveY(12f, 0.5f).SetEase(Ease.InBack);
			}
			if (bot != null)
			{
				bot.DOMoveY(-8f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
				{
					ResetBlock();
					PoolsBlockEnd.instance.PoolObject();
					Data.idPopup = 2;
					GameOver.instance.SetActive(isActive: true, isHighScore);
					EffectScore();
				});
			}
		});
	}

	private void EffectScore()
	{
		GameOver.instance.SetScore(score);
		GameOver.instance.SetBest(bestScore);
	}

	public void SaveMeEat()
	{
		int xRandom = UnityEngine.Random.Range(0, Data.MATRIX_N - 5);
		int yRandom = UnityEngine.Random.Range(0, Data.MATRIX_N - 5);
		Cells.instance.EatCellsSaveMe(xRandom, yRandom);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			if (!Data.isGameOver)
			{
				SaveGameData();
			}
		}
		else if (Data.isViewVideo)
		{
			Data.isViewVideo = false;
		}
		else if (Data.isAllowShowVideo)
		{
			if (UnityEngine.Random.Range(0, 1000) % 100 < 50)
			{
				//AdMobController.instance.ShowInterstitial();
			}
		}
		else
		{
			Data.isAllowShowVideo = true;
		}
	}

	public void SynchronizedBestScore(int bestScore)
	{
		this.bestScore = bestScore;
		GamePlayUI.instance.SetBest(this.bestScore);
	}

	private int GetIDRotate(int idCurrent)
	{
		int num = 0;
		if (idCurrent < 15)
		{
			return idCurrent + 5;
		}
		if (15 <= idCurrent && idCurrent < 20)
		{
			return idCurrent - 15;
		}
		if (20 <= idCurrent && idCurrent < 24)
		{
			return idCurrent + 4;
		}
		if (24 <= idCurrent && idCurrent < 28)
		{
			return idCurrent - 4;
		}
		return idCurrent;
	}

	private Block RotateBlockGhost(Block block)
	{
		int iDRotate = GetIDRotate(block.id);
		if (iDRotate != block.id)
		{
			return CreateBlock(iDRotate, block.idPosInBot, block.idSprite);
		}
		return block;
	}

	private int GetDeltaId(int id)
	{
		int num = 0;
		if (id < 20)
		{
			return 4;
		}
		if (20 <= id && id < 28)
		{
			return 2;
		}
		return 1;
	}

	private void SetUnique(Block block)
	{
		int num = 0;
		int iFirstWorldMatrix = 0;
		int jFirstWorldMatrix = 0;
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (!Cells.instance.IsActiveCell(i, j))
				{
					if (block.CheckAbilitiInMatrixBlock(i, j))
					{
						iFirstWorldMatrix = i;
						jFirstWorldMatrix = j;
						num++;
					}
					if (num > 1)
					{
						break;
					}
				}
			}
			if (num > 1)
			{
				break;
			}
		}
		if (num == 1)
		{
			Cells.instance.SetUnique(block.GetMatrixWorld(iFirstWorldMatrix, jFirstWorldMatrix));
		}
	}

	public void CheckBomb()
	{
		if (Data.idTypeGame == 11 && Cells.instance.CheckExplose())
		{
			SoundManager.PlaySound("boomBreak", 1f);
			SetTxtEnd(isActive: true, isNoMove: false);
			Data.isPauseGame = true;
			DOVirtual.DelayedCall(1f, delegate
			{
				GameOverFunction();
			});
		}
	}

	public void CreateBomb()
	{
		if (Data.idTypeGame == 11)
		{
			countStep++;
			if (countStep >= 6 && Cells.instance.SetActiveBomb())
			{
				SoundManager.PlaySound("boomActive", 1f);
				countStep = 0;
			}
		}
	}

	public void CreateRocket()
	{
		if (UnityEngine.Random.Range(1, 1000) % 100 < 5)
		{
			int activeRocket = UnityEngine.Random.Range(1, 5);
			countRocket++;
			Cells.instance.SetActiveRocket(activeRocket);
		}
	}

	public void CreateAddTime()
	{
		if (Data.idTypeGame == 9 && UnityEngine.Random.Range(1, 1000) % 100 < 20)
		{
			int activeAddTime = UnityEngine.Random.Range(1, 5);
			countRocket++;
			Cells.instance.SetActiveAddTime(activeAddTime);
		}
	}

	private void SaveFile(string name)
	{
		GameData gameData = new GameData();
		gameData.score = score;
		gameData.cells = new List<GameData.CellData>();
		gameData.cells = Cells.instance.GetCellsData();
		gameData.countStep = countStep;
		gameData.blocks = new List<GameData.BlockData>();
		for (int i = 0; i < blocks.Count; i++)
		{
			gameData.blocks.Add(blocks[i].GetBlockData());
		}
		string contents = JsonUtility.ToJson(gameData, prettyPrint: true);
		string path = Path.Combine(Application.dataPath, name + ".json");
		File.WriteAllText(path, contents);
	}

	public void ReadFile()
	{
		string path = "Help/help_start_" + Data.countHelp;
		string text = Resources.Load<TextAsset>(path).text;
		GameData gameData = JsonUtility.FromJson<GameData>(text);
		SetScore(gameData.score);
		for (int i = 0; i < gameData.cells.Count; i++)
		{
			Cells.instance.SetActiveCell(isActive: true, gameData.cells[i].idSprite, gameData.cells[i].status, gameData.cells[i].i, gameData.cells[i].j);
			if (Data.idTypeGame == 11 && gameData.cells[i].status == 3)
			{
				Cells.instance.CreateBomb(gameData.cells[i].i, gameData.cells[i].j, gameData.cells[i].count);
			}
			if (gameData.cells[i].status == 4)
			{
				Cells.instance.CreateRocket(gameData.cells[i].i, gameData.cells[i].j);
			}
		}
		for (int j = 0; j < gameData.blocks.Count; j++)
		{
			blocks.Add(CreateBlock(gameData.blocks[j].id, gameData.blocks[j].idPosInBot, gameData.blocks[j].idTypeJewel));
		}
		CheckAbilitiContinue();
	}

	public void SetGuilHand(bool is_active)
	{
		Vector3[] array = new Vector3[4]
		{
			new Vector3(0f, -4.49f, -3.3f),
			new Vector3(0f, -4.58f, -3f),
			new Vector3(0f, -4.44f, -3f),
			new Vector3(0f, -4.44f, -3f)
		};
		Vector3[] array2 = new Vector3[4]
		{
			new Vector3(0f, 0.61f, -3.3f),
			new Vector3(0.38f, 0.18f, -3f),
			new Vector3(0f, 0.18f, -3f),
			new Vector3(0f, 0.18f, -3f)
		};
		if (is_active)
		{
			guid_hand.position = array[Data.countHelp];
			mySequence = DOTween.Sequence();
			mySequence.Append(guid_hand.DOScale(1f, 0.3f).SetDelay(0.2f)).Append(guid_hand.DOMove(array2[Data.countHelp], 1f).SetDelay(0.3f)).Append(guid_hand.DOScale(1.2f, 0.3f).SetDelay(0.5f))
				.Append(guid_hand.DOMove(array2[Data.countHelp], 0.7f))
				.SetEase(Ease.Linear)
				.SetLoops(-1);
		}
		else
		{
			mySequence.Kill();
		}
		guid_hand.gameObject.SetActive(is_active);
	}

	public void SetTxtEnd(bool isActive, bool isNoMove)
	{
		if (isActive)
		{
			spriteEnd.gameObject.SetActive(value: true);
			if (isNoMove)
			{
				spriteEnd.sprite = MyResources.instance.endsName[0];
			}
			else if (Data.idTypeGame == 8 || Data.idTypeGame == 10)
			{
				spriteEnd.sprite = MyResources.instance.endsName[0];
			}
			else if (Data.idTypeGame == 9)
			{
				spriteEnd.sprite = MyResources.instance.endsName[1];
			}
			else
			{
				spriteEnd.sprite = MyResources.instance.endsName[2];
			}
			DOVirtual.DelayedCall(0.5f, delegate
			{
				spriteEnd.transform.DOScale(2f, 0.2f).SetLoops(2, LoopType.Yoyo);
			});
		}
		else
		{
			spriteEnd.transform.DOScale(2f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				spriteEnd.gameObject.SetActive(value: false);
			});
		}
	}
}
