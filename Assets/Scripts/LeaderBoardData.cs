/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections.Generic;

[Serializable]
public class LeaderBoardData
{
	[Serializable]
	public class Player
	{
		public string player_name;

		public int score;

		public int avt;

		public int rank;

		public int current_position;

		public Player(string player_name, int score, int avt, int rank, int current_position)
		{
			this.player_name = player_name;
			this.score = score;
			this.avt = avt;
			this.rank = rank;
			this.current_position = current_position;
		}
	}

	public int score;

	public int current_position;

	public int avt;

	public List<Player> pre_player;

	public List<Player> around_player;
}
