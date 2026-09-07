using System;
using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	[Serializable]
	public class DataColorBlock
	{
		public Material colorBlock;
	}
	[Serializable]
	public class DataColorShooter
	{
		public Material colorShooter;
		public Material colorTopShooter;
	}
	public class ColorManager : Singleton<ColorManager>
	{
		[Header("Block")]
		[SerializeField] private List<DataColorBlock> _matColorBlockList;
		[Header("Shooter")]
		[SerializeField] private List<DataColorShooter> _matColorShooterList;

		[SerializeField] private Material _matColorSecretShooter;

        public List<DataColorShooter> MatColorShooterList { get => _matColorShooterList; set => _matColorShooterList = value; }
        public List<DataColorBlock> MatColorBlockList { get => _matColorBlockList; set => _matColorBlockList = value; }
        public Material MatColorSecretShooter { get => _matColorSecretShooter; set => _matColorSecretShooter = value; }
    }
}
