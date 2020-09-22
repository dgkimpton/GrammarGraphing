namespace GrammarGrapher.syntax.elements
{
	public class Block
			: IInstruction
	{
		private readonly int instructionIndex;
		private readonly string identifier;
		private readonly string precedingWhitespace;
		private readonly string trailingWhitespace;
		private readonly string followingInvalid;

		private Block(
				int instructionIndex,
				string identifier,
				string precedingWhitespace,
				string trailingWhitespace)
		{
			this.instructionIndex = instructionIndex;
			this.identifier = identifier;
			this.precedingWhitespace = precedingWhitespace;
			this.trailingWhitespace = trailingWhitespace;
			this.followingInvalid = string.Empty;
		}

		private Block(
				int instructionIndex,
				string identifier,
				string precedingWhitespace,
				string trailingWhitespace,
				string following)
		{
			this.instructionIndex = instructionIndex;
			this.identifier = identifier;
			this.precedingWhitespace = precedingWhitespace;
			this.trailingWhitespace = trailingWhitespace;
			this.followingInvalid = following;
		}

		public bool IsValid => followingInvalid.Length == 0 && instructionIndex == 0;

		public void Visit(IVisitor visitor)
		{
			visitor.Whitespace(precedingWhitespace);
			visitor.Block(identifier);
			visitor.Whitespace(trailingWhitespace);

			if (!IsValid)
			{
				visitor.InvalidText(followingInvalid);
			}
		}

		public static IInstruction Parse(
				int instructionIndex,
				string identifier,
				string precedingWhitespace,
				string fullString,
				int index)
		{
			var endSpaceIndex = fullString.NextNonWhite(index);

			if (endSpaceIndex == fullString.Length)
			{
				return new Block(instructionIndex,
				                 identifier, 
				                 precedingWhitespace, 
				                 fullString.SubstringBetween(index, Utils.END));

			}

			return new Block(instructionIndex, 
			                 identifier,
			                 precedingWhitespace,
			                 fullString.SubstringBetween(index, endSpaceIndex),
			                 fullString.SubstringBetween(endSpaceIndex, Utils.END));
		}
	}
}