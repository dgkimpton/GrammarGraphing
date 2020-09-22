namespace GrammarGrapher.syntax
{
	public class EmptyInstruction
			: IInstruction
	{
		private readonly string whitespace;

		public EmptyInstruction(string whitespace)
		{
			this.whitespace = whitespace;
		}

		public bool IsValid => true;

		public void Visit(IVisitor visitor)
		{
			visitor.BeginInstruction(this, Position.Only);
			visitor.Whitespace(whitespace);
			visitor.EndInstruction(this, Position.Only);
		}
	}
}