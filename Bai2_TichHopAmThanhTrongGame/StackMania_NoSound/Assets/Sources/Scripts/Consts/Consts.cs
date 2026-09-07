using UnityEngine;

namespace KienChi
{
	public static class Consts
	{
		public const string FILE_PREFIX = "Level";
		public const string FILE_SUFFIX = ".bat";
		public const int DIFFICULT_NORMAL = 0;
		public const int DIFFICULT_DIFFICULT = 1;
		public const int DIFFICULT_BRUTAL = 2;
		#region LAYER
		public const string LAYER_DEFAULT = "Default";
		public const string LAYER_SHOOTER = "Shooter";
		public const string LAYER_BLOCK = "Block";
		public const string LAYER_SLOT_ADS = "SlotAds";
		#endregion

		#region BLOCKS
		public const int SIMPLE_BLOCK = 0;
		public const int TOWER_BLOCK = 1;
		public const int GOLD_BLOCK = 10;
		public const float BLOCK_X_GAP = 1;
		public const float BLOCK_Y_GAP = 1;
		public const float BLOCK_Z_GAP = 1;
		public const float BLOCK_SIZE = 1;
		public const int MAX_BLOCKS_PER_ROW = 10;
		public const int MIN_BLOCKS_PER_COL = 12;
		#endregion

		#region SHOOTER
		public const int SIMPLE_SHOOTER = 0;
		public const int DOU_SIMPLE_SHOOTER = 1;
		public const int LINK_SHOOTER = 2;
		public const int SECRET_SHOOTER = 3;
		public const int MAX_SHOOTING_BLOCKS = 5;
		public const int GAIN_GOLD_DESTROY_BLOCK = 1;
		public const float SHOOTER_SIZE = 1.5f;
		public const float SHOOTER_X_GAP = 1.5f;
		public const float SHOOTER_Y_GAP = 2.2f;
		public const string IDLE_ANIM = "Idle";
		public const string MOVING_ANIM = "Moving";
		public const string SHOOTING_ANIM = "Shooting";
		#endregion

		#region SLOTS
		public const float SLOTS_X_GAP = 1.5f;
		public const float SLOTS_Y_GAP = 2.2f;
		public const int MIN_SLOTS_PER_ROW = 6;
		public const int MAX_SLOTS_PER_ROW = 7;
		#endregion
	}
}
