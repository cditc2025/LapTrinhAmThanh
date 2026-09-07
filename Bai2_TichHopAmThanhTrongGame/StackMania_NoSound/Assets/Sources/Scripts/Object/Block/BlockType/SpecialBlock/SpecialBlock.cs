using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
	public abstract class SpecialBlock : Block
	{
		protected Queue<int> numOfDividedBlocks = new Queue<int>();
		
		public Queue<int> NumOfDividedBlocks { get => numOfDividedBlocks; set => numOfDividedBlocks = value; }
	}
}
