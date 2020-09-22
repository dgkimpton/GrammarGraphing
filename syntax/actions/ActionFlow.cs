namespace GrammarGrapher.syntax.actions
{
	public class ActionFlow
			: IInstruction
	{
		private readonly int instructionIndex;
		private readonly string precedingWhitespace;
		private readonly string gotoSpace;
		private readonly string contentType;
		private readonly string targetSpace;
		private readonly string invalid;

		private ActionFlow(
				int instructionIndex,
				string precedingWhitespace,
				string gotoSpace,
				string contentType,
				string targetSpace,
				string invalid)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingWhitespace;
			this.gotoSpace = gotoSpace;
			this.contentType = contentType;
			this.targetSpace = targetSpace;
			this.invalid = invalid;
		}

		public bool IsValid => instructionIndex > 0 && contentType.Length > 0 && invalid.Length == 0;

		public void Visit(IVisitor visitor)
		{
			Whitespace(visitor, precedingWhitespace);
			visitor.Flow();
			Whitespace(visitor, gotoSpace);
			if (contentType.Length > 0) visitor.FlowContentType(contentType);
			Whitespace(visitor, targetSpace);
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
			var contentBeginIndex = fullString.NextNonWhite(index);
			var contentEndIndex = fullString.NextWhite(contentBeginIndex);
			var trailingSpace = fullString.NextNonWhite(contentEndIndex);

			return new ActionFlow(
					instructionIndex,
					precedingWhitespace,
					fullString.SubstringBetween(index, contentBeginIndex),
					fullString.SubstringBetween(contentBeginIndex, contentEndIndex),
					fullString.SubstringBetween(contentEndIndex, trailingSpace),
					fullString.SubstringBetween(trailingSpace, Utils.END));
		}
	}
}