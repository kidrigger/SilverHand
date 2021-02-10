using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverHand.Core
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class AccessorAttribute : Attribute
	{
		public IEnumerable<string> AccessorNames { get; init; }

		public AccessorAttribute(params string[] names) => AccessorNames = from name in (names ?? Enumerable.Empty<string>()) select name.ToLower();
	}
}
