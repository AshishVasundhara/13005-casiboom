/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class ScoreAndEffect : MonoBehaviour
{
	public Transform[] scores;

	public Sprite[] effects;

	public SpriteRenderer sprEffect;

	public TextMesh[] txtScores;

	public Transform effect;

	public static ScoreAndEffect instance;

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
	}

	public void SetActive(int row, int typeBlockID, Vector3 po, int score)
	{
		txtScores[typeBlockID].text = score.ToString();
		if (row > 1)
		{
			int num = row - 2;
			effect.position = new Vector3(PosXLimit(num, po.x), po.y, po.z) + new Vector3(0f, 0.6f, 0f);
			sprEffect.sprite = effects[num];
			effect.gameObject.SetActive(value: true);
			effect.DOScale(1f, 1f).SetEase(Ease.OutBounce);
			DOVirtual.DelayedCall(1.2f, delegate
			{
				effect.localScale = Vector3.zero;
				effect.gameObject.SetActive(value: false);
			});
			scores[typeBlockID].gameObject.SetActive(value: true);
			scores[typeBlockID].position = new Vector3(PosXLimit(num, po.x), po.y, po.z);
			DOVirtual.DelayedCall(0.4f, delegate
			{
				scores[typeBlockID].DOScale(1f, 1f).SetEase(Ease.OutBounce);
			});
			DOVirtual.DelayedCall(1.6f, delegate
			{
				scores[typeBlockID].localScale = Vector3.zero;
				scores[typeBlockID].gameObject.SetActive(value: false);
			});
		}
		else
		{
			scores[typeBlockID].position = po + new Vector3(0f, 0.1f, 0f);
			scores[typeBlockID].gameObject.SetActive(value: true);
			scores[typeBlockID].DOScale(1f, 1f).SetEase(Ease.OutBounce);
			DOVirtual.DelayedCall(1.2f, delegate
			{
				scores[typeBlockID].localScale = Vector3.zero;
				scores[typeBlockID].gameObject.SetActive(value: false);
			});
		}
	}

	private float PosXLimit(int id, float PosX)
	{
		float num = 0f;
		float num2 = 0f;
		float result = PosX;
		switch (id)
		{
		case 0:
			num = -2.52f;
			num2 = 2.52f;
			break;
		case 1:
			num = -2.4f;
			num2 = 2.4f;
			break;
		default:
			num = -1.9f;
			num2 = 1.9f;
			break;
		}
		if (PosX < num)
		{
			result = num;
		}
		else if (PosX > num2)
		{
			result = num2;
		}
		return result;
	}
}
