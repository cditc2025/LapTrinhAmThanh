using System;
using UnityEngine;

namespace KienChi
{
	[Serializable]
	public class Size2D
	{
		public int x;
		public int y;
	}
	[Serializable]
	public class ValueShooterLink
	{
		public int index;
		public int order;
	}
	[Serializable]
	public class ValueGoldBlock
	{
		public int numOfGoldBlocks;
	}
	[Serializable]
	public class ValueBlock
	{
		public int color;
		public int typeBlock;
		public int numOfBlocks;
		public ValueGoldBlock valueGoldBlock;
		public Size2D size;
	}
	[Serializable]
	public class BlockData
	{
		public Size2D size;
		public uint totalBlocks;
		public ValueBlock[] valueBlocks;
	}
	[Serializable]
	public class ValueShooter
	{
		public int color;
		public int typeShooter;
		public int numOfBullet;
		public ValueShooterLink valueShooterLink;
	}
	[Serializable]
	public class ShooterData
	{
		public uint totalShooters;
		public Size2D size;
		public ValueShooter[] valueShooters;
	}
	[Serializable]
	public class SlotData
	{
		public uint totalSlot;
	}
	[Serializable]
	public class Level
	{
		public int levelNum;
		public int difficulty;
	}
	[Serializable]
	public class DataLevel
	{
		public Level level;
		public BlockData block;
		public ShooterData shooter;
		public SlotData slot;
	}
}
