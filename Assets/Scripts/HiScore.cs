/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class HiScore : MonoBehaviour
{
	public Text txtStt;

	public Text txtName;

	public Text txtScore;

	public void setName(string playerName)
	{
		txtName.text = playerName;
	}

	public void setScore(int score)
	{
		txtScore.text = string.Empty + score;
	}

	public void setStt(int stt)
	{
		if (txtStt != null)
		{
			txtStt.text = "#" + stt;
		}
	}

	public void setAvt(int idAvt)
	{
	}
}
