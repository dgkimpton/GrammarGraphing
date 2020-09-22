namespace GrammarGrapher.syntax.actions
{
	public class ActionError
			: IInstruction
	{
		private readonly int instructionIndex;
		private readonly string precedingWhitespace;
		private readonly string gotoSpace;
		private readonly string errorText;

		private ActionError(
				int instructionIndex,
				string precedingWhitespace,
				string gotoSpace,
				string errorText)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingWhitespace;
			this.gotoSpace = gotoSpace;
			this.errorText = errorText;
		}

		public bool IsValid => instructionIndex > 0 && errorText.Length > 0;

		public void Visit(IVisitor visitor)
		{
			Whitespace(visitor, precedingWhitespace);
			visitor.Error();
			Whitespace(visitor, gotoSpace);
			if (errorText.Length > 0) visitor.ErrorDescription(errorText);

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
			var errorTextBeginIndex = fullString.NextNonWhite(index);

			return new ActionError(
					instructionIndex,
					precedingWhitespace,
					fullString.SubstringBetween(index, errorTextBeginIndex),
					fullString.SubstringBetween(errorTextBeginIndex, Utils.END));
		}
	}
}