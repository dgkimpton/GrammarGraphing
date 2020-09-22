using System;
using System.Collections.Generic;

namespace GrammarGrapher.syntax
{
	public interface ILine
	{
		bool IsValid { get; }
		void Visit(IVisitor theVisitor);
	}
	public class Line : ILine
	{
		private const char INSTRUCTION_DELIMITER = ',';
		private static readonly char[] instructionDelimiter = {INSTRUCTION_DELIMITER};

		public static ILine Parse(string str, int lineNumber)
		{
			if (str.Length == 0)
			{
				return new EmptyLine(lineNumber);
			}

			var instructionStrings = str.Split(instructionDelimiter, StringSplitOptions.None);
			
			var isValid = true;
			var instructions = new List<IInstruction>();

			for (var index = 0; index < instructionStrings.Length; index++)
			{
				var instructionStr = instructionStrings[index];
				if (string.IsNullOrWhiteSpace(instructionStr))
				{
					instructions.Add(Instruction.Empty(instructionStr));
				}
				else
				{
					var instruction = Instruction.Parse(instructionStr, index);
					isValid = isValid && instruction.IsValid;
					instructions.Add(instruction);
				}
			}

			return new Line(lineNumber, instructions, isValid);
		}

		private readonly List<IInstruction> instructions;

		private Line(int lineNumber, List<IInstruction> instructions, in bool isValid)
		{
			IsValid = isValid;
			LineNumber = lineNumber;
			this.instructions = instructions;
		}

		public void Visit(IVisitor visitor)
		{
			instructions.Iterate(visitor,
			                     (theVisitor, instruction, position) =>
			                     {
				                     theVisitor.BeginInstruction(instruction, position);
									 instruction.Visit(theVisitor);
									 theVisitor.EndInstruction(instruction, position);
								 });
		}

		public bool IsValid { get; }
		public int LineNumber { get; }
	}

	public class EmptyLine : ILine
	{
		private readonly int lineNumber;

		public EmptyLine(int lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		public bool IsValid => true;

		public void Visit(IVisitor visitor)
		{
			visitor.EmptyLine(this);
		}
	}
}