using System;
using System.Collections.Generic;

namespace GrammarGrapher.syntax
{
	public enum Position
	{
		First,
		Normal,
		Last,
		Only,
	}

	public static class Utils
	{
		public static void Iterate<T>(this IList<T> items, Action<T, Position> consume)
		{
			if (items.Count == 0) return;
			if (items.Count == 1)
			{
				consume(items[0], Position.Only);
				return;
			}

			consume(items[0], Position.First);

			for (var index = 1; index < items.Count - 1; index++)
			{
				consume(items[index], Position.Normal);
			}

			consume(items[items.Count - 1], Position.Last);
		}

		public static void Iterate<T, TContext>(this IList<T> items, TContext context, Action<TContext, T, Position> consume)
		{
			if (items.Count == 0) return;
			if (items.Count == 1)
			{
				consume(context, items[0], Position.Only);
				return;
			}

			consume(context, items[0], Position.First);

			for (var index = 1; index < items.Count - 1; index++)
			{
				consume(context, items[index], Position.Normal);
			}

			consume(context, items[items.Count - 1], Position.Last);
		}

		public const int END = -1;
		public static string SubstringBetween(this string str, int begin, int end)
		{
			if (end == -1)
			{
				end = str.Length;
			}

			if (begin == end)
			{
				return string.Empty;
			}

			if (end > str.Length)
			{
				end = str.Length;
			}

			return str.Substring(begin, end - begin);
		}

		public static int NextWhite(this string str, int startIndex)
		{
			var i = startIndex;
			while (i < str.Length && !char.IsWhiteSpace(str[i]))
				++i;

			return i;
		}

		public static int NextNonWhite(this string str, int startIndex)
		{
			var i = startIndex;
			while (i < str.Length && char.IsWhiteSpace(str[i]))
				++i;

			return i;
		}
	}
}