namespace GrammarGrapher.syntax.actions
{
	public class ActionGoto
			: IInstruction
	{
		private readonly int instructionIndex;
		private readonly string precedingWhitespace;
		private readonly string gotoSpace;
		private readonly string targetName;
		private readonly string targetSpace;
		private readonly string invalid;

		private ActionGoto(
				int instructionIndex, 
				string precedingWhitespace, 
				string gotoSpace, 
				string targetName,
				string targetSpace, 
				string invalid)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingWhitespace;
			this.gotoSpace = gotoSpace;
			this.targetName = targetName;
			this.targetSpace = targetSpace;
			this.invalid = invalid;
		}

		public bool IsValid => instructionIndex > 0 && targetName.Length > 0 &&  invalid.Length == 0;

		public void Visit(IVisitor visitor)
		{
			Whitespace(visitor, precedingWhitespace);
			visitor.Goto();
			Whitespace(visitor, gotoSpace);
			if (targetName.Length > 0) visitor.GotoTargetName(targetName);
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
			var targetBeginIndex = fullString.NextNonWhite(index);
			var targetEndIndex = fullString.NextWhite(targetBeginIndex);
			var trailingSpace = fullString.NextNonWhite(targetEndIndex);

			return new ActionGoto(
					instructionIndex,
					precedingWhitespace,
					fullString.SubstringBetween(index, targetBeginIndex),
					fullString.SubstringBetween(targetBeginIndex, targetEndIndex),
					fullString.SubstringBetween(targetEndIndex, trailingSpace),
					fullString.SubstringBetween(trailingSpace, Utils.END));
		}
	}
}