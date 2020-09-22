namespace GrammarGrapher.syntax
{
	public enum Consumption
	{
		Eat,
		Bounce,
	}

	public interface IVisitor
	{
		void EmptyLine(EmptyLine emptyLine);

		void BeginLine(ILine line, Position position);
		void BeginInstruction(IInstruction instruction, Position position);

		void Whitespace(string whitespace);
		void Block(string identifier);

		void On();
		void CharDescription(string charDesc);
		void Consumption(Consumption consumption);

		void Goto();
		void GotoTargetName(string targetName);

		void Flow();
		void FlowContentType(string contentType);

		void Error();
		void ErrorDescription(string errorText);

		void Eof();
		void EndOfChunk();

		void InvalidText(string invalid);

		void EndInstruction(IInstruction instruction, Position position);
		void EndLine(ILine line, Position position);

		void Finish();
	}
}