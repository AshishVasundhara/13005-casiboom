/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
public class BPoint
{
	public int x
	{
		get;
		set;
	}

	public int y
	{
		get;
		set;
	}

	public int status
	{
		get;
		set;
	}

	public BPoint()
	{
	}

	public BPoint(int x, int y, int status)
	{
		this.x = x;
		this.y = y;
		this.status = status;
	}
}
