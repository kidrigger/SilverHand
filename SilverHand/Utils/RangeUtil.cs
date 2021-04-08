#region HEADER
// =============================================
//  SilverHand: RangeUtil.cs
//  Copyright (c) 2020-2021 Anish Bhobe
// =============================================
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace SilverHand.Utils
{
	public static class RangeUtil
	{
		private struct RangeEnumerator : IEnumerator<int>
		{
			private readonly int start;
			private readonly int end;

			public RangeEnumerator(Range range)
			{
				start = range.Start.Value-1;
				end = range.End.Value;
				Current = start;
			}

			public bool MoveNext()
			{
				Current++;
				return Current < end;
			}

			public void Reset()
			{
				Current = start;
			}

			object IEnumerator.Current => Current;

			public int Current { get; private set; }
			public void Dispose() { }
		}

		public static IEnumerator<int> GetEnumerator(this Range range)
		{
			return new RangeEnumerator(range);
		}
	}
}