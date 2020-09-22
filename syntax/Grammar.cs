using System.Collections.Generic;

namespace GrammarGrapher.syntax
{
	public class Grammar
	{
		public static Grammar Parse(string[] inputLines)
		{
			var lineNumber = 0;
			var lines = new List<ILine>();
			var isValid = true;

			foreach (var lineString in inputLines)
			{
				var line = Line.Parse(lineString, lineNumber);
				isValid = isValid && line.IsValid;
				lines.Add(line);
				++lineNumber;
			}

			return new Grammar(lines, isValid);
		}

		private readonly List<ILine> lines;
		public bool IsValid { get; }
		
		private Grammar(List<ILine> list, in bool isValid)
		{
			lines = list;
			IsValid = isValid;
		}

		public void Visit(IVisitor visitor)
		{
			lines.Iterate(visitor,
			              (theVisitor, item, position) =>
			              {
				              theVisitor.BeginLine(item, position);
				              item.Visit(theVisitor);
				              theVisitor.EndLine(item, position);
			              });
			visitor.Finish();
		}
	}
}