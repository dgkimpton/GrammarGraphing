using System.Diagnostics;
using System.Text;
using GrammarGrapher.syntax;

namespace GrammarGrapher.graphing
{
	public class DotString : IVisitor
	{
		private readonly StringBuilder dot;
		private int currentInstructionIndex;
		private string currentBlockId;

		public DotString()
		{
			dot = new StringBuilder();
			dot.AppendLine("digraph G {");

			dot.AppendLine("  graph[");
			dot.AppendLine("    bgcolor=gray, fontname = Arial, fontcolor = white, fontsize = 8,");
			dot.AppendLine("    splines = compound, nodesep=0.5");
			dot.AppendLine("  ];");
			dot.AppendLine("");
			dot.AppendLine("  node[");
			dot.AppendLine("    fillcolor=gray70, shape=box, fontname=Arial, fontcolor=black, fontsize=8,");
			dot.AppendLine("    style=\"rounded,filled\", margin=\"0.1, 0.05\", width=0, height=0, penwidth=0");
			dot.AppendLine("  ];");
			dot.AppendLine("");
			dot.AppendLine("  edge[");
			dot.AppendLine("    fontname=\"Helvetica bold\", color=cyan, fontcolor=red, fontsize=10,");
			dot.AppendLine("    margin=10, labeldistance = 30");
			dot.AppendLine("  ];");
			dot.AppendLine("");

			currentInstructionIndex = 0;
		}

		public void BeginLine(ILine line, Position position)
		{
			currentDescription = null;
			currentRule = null;
			currentInstructionId = null;
		}

		public void BeginInstruction(IInstruction instruction, Position position)
		{
		}

		public void Whitespace(string whitespace)
		{
		}

		public void Block(string identifier)
		{
			dot.AppendLine($"{identifier}  [ fillcolor=gray30, fontcolor=yellow]");
			currentBlockId = identifier;
		}

		private string currentRule;
		public void On()
		{
			currentRule = "on";
		}

		private string currentDescription;
		public void CharDescription(string charDesc)
		{
			currentDescription = charDesc;
		}

		private string currentInstructionId;
		public void Consumption(Consumption consumption)
		{
			if (currentRule == "on" && currentDescription != null && currentBlockId != null)
			{
				var sourceId = currentBlockId;
				var targetId = $"__TChar{currentInstructionIndex++}";

				dot.Append($"{targetId} ");
				dot.Append($"[label=\"{currentDescription}\", fontcolor={(consumption == syntax.Consumption.Eat ? "crimson" : "black")}];");

				dot.AppendLine($"{sourceId} -> {targetId};");
				currentInstructionId = targetId;
			}
		}

		public void Goto()
		{
			currentRule = "goto";
		}

		public void GotoTargetName(string targetName)
		{
			if (currentRule == "goto" && currentInstructionId != null)
			{
				var sourceId = currentInstructionId;
				var targetId = targetName;
				dot.AppendLine($"{sourceId} -> {targetId};");
			}
		}

		public void Flow()
		{
			currentRule = "flow";
		}

		public void FlowContentType(string contentType)
		{
			if (currentRule == "flow" && currentInstructionId != null)
			{
				var sourceId = currentInstructionId;
				var targetId = $"__Flow{currentInstructionIndex++}";

				dot.Append($"{targetId} ");
				dot.Append($"[label=\"{contentType}\", fillcolor=\"forestgreen\", fontcolor=white];");

				dot.AppendLine($"{sourceId} -> {targetId};");

				currentInstructionId = targetId;
			}
		}

		public void Error()
		{
			if (currentRule == "error" && currentInstructionId != null)
			{
				var sourceId = currentInstructionId;
				var targetId = $"__Error{currentInstructionIndex++}";

				dot.Append($"{targetId} ");
				dot.Append($"[label=\"error\", fillcolor=\"gray30\", fontcolor=orange];");

				dot.AppendLine($"{sourceId} -> {targetId};");

				currentInstructionId = targetId;
			}
		}

		public void ErrorDescription(string errorText)
		{
			// no where to put this, so drop it for now
		}

		public void Eof()
		{
			if (currentBlockId != null)
			{
				var sourceId = currentBlockId;
				var targetId = $"__Eof{currentInstructionIndex++}";

				dot.Append($"{targetId} ");
				dot.Append($"[label=\"eof\", fontcolor=purple];");

				dot.AppendLine($"{sourceId} -> {targetId};");
				currentInstructionId = targetId;
			}
		}

		public void EndOfChunk()
		{
			if (currentBlockId != null)
			{
				var sourceId = currentBlockId;
				var targetId = $"__Eoc{currentInstructionIndex++}";

				dot.Append($"{targetId} ");
				dot.Append($"[label=\"eoc\", fontcolor=purple];");

				dot.AppendLine($"{sourceId} -> {targetId};");
				currentInstructionId = targetId;
			}
		}

		public void EndInstruction(IInstruction instruction, Position position)
		{
		}

		public void EndLine(ILine line, Position position)
		{
		}

		public void InvalidText(string invalid)
		{

		}

		public void EmptyLine(EmptyLine emptyLine)
		{

		}

		public void Finish()
		{
			dot.AppendLine("}");
		}

		public override string ToString()
		{
			return dot.ToString();
		}
	}
}