using System;
using GrammarGrapher.syntax.actions;
using GrammarGrapher.syntax.elements;
using GrammarGrapher.syntax.triggers;

namespace GrammarGrapher.syntax
{
	public static class Instruction
	{
		public static IInstruction Parse(string str, int index)
		{
			var beginIndex = str.NextNonWhite(0);

			var precedingWhitespace = str.SubstringBetween(0, beginIndex);

			var endIndex = str.NextWhite(beginIndex);

			if (endIndex == beginIndex)
			{
				// this case should have been prevented by the line parser
				throw new Exception("unexpected whitespace line");
			}

			var identifier = str.SubstringBetween(beginIndex, endIndex);

			return FindParser(identifier)(index, identifier, precedingWhitespace, str, endIndex);
		}

		public static IInstruction Empty(string whitespace)
		{
			return new EmptyInstruction(whitespace);
		}

		private static Func<int, string, string, string, int, IInstruction> FindParser(string identifier)
		{
			switch (identifier.ToLower())
			{
				case "on": return TriggerOn.Parse;
				case "eof": return TriggerEof.Parse;
				case "eoc": return TriggerEoc.Parse;
					
				case "goto": return ActionGoto.Parse;
				case "flow": return ActionFlow.Parse;
				case "error": return ActionError.Parse;

				default:
					return Block.Parse;
			}
		}
	}
}
