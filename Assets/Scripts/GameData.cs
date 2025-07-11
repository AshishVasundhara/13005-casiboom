/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	[Serializable]
	public class CellData
	{
		public int i;

		public int j;

		public int idSprite;

		public int status;

		public int count;

		public int addTime;

		public CellData(int i, int j, int idSprite, int status, int count, int addTime)
		{
			this.i = i;
			this.j = j;
			this.idSprite = idSprite;
			this.status = status;
			this.count = count;
			this.addTime = addTime;
		}
	}

	[Serializable]
	public class BlockData
	{
		public int id;

		public int idPosInBot;

		public int idTypeJewel;

		public BlockData(int id, int idPosInBot, int idTypeJewel)
		{
			this.id = id;
			this.idPosInBot = idPosInBot;
			this.idTypeJewel = idTypeJewel;
		}
	}

	public int score;

	public int countStep;

	public float countTimeLeft;

	public List<CellData> cells;

	public List<BlockData> blocks;
}
