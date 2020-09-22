namespace GrammarGrapher.syntax.triggers
{
	public class TriggerEoc
			: IInstruction
	{
		private readonly int instructionIndex;
		private readonly string precedingWhitespace;
		private readonly string trailingSpace;
		private readonly string invalid;

		private TriggerEoc(
				int instructionIndex,
				string precedingWhitespace,
				string trailingSpace,
				string invalid)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingWhitespace;
			this.trailingSpace = trailingSpace;
			this.invalid = invalid;
		}

		public bool IsValid => instructionIndex == 0 && invalid.Length == 0;

		public void Visit(IVisitor visitor)
		{
			Whitespace(visitor, precedingWhitespace);
			visitor.EndOfChunk();
			Whitespace(visitor, trailingSpace);
			if (invalid.Length > 0) visitor.InvalidText(invalid);

		}

		private static void Whitespace(IVisitor visitor, string white)
		{
			if (white.Length > 0) visitor.Whitespace(white);
		}

		public static IInstruction Parse(
				int instructionIndex,
				string _,
				string precedingWhitespace,
				string fullString,
				int index)
		{
			var trailingSpace = fullString.NextNonWhite(index);

			return new TriggerEoc(
					instructionIndex,
					precedingWhitespace,
					fullString.SubstringBetween(index, trailingSpace),
					fullString.SubstringBetween(trailingSpace, Utils.END));
		}
	}
}