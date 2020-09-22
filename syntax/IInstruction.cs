namespace GrammarGrapher.syntax
{
	public interface IInstruction
	{
		bool IsValid { get; }
		void Visit(IVisitor visitor);
	}
}