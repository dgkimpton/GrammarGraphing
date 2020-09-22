using System;
using System.Text;
using GrammarGrapher.syntax;

namespace GrammarGrapher.rtf
{
	public class RtfBuilder : IVisitor
	{
		private readonly StringBuilder rtf;
		private bool inError;

		public RtfBuilder()
		{
			rtf = new StringBuilder();
			OpenSection();
			Append("\\rtf1\\ansi");
			OpenSection();
			Append("\\fonttbl");
			OpenSection();
			Append("\\f0\\fnil\\fcharset0 Courier New;");
			CloseSection();
			CloseSection();

			RtfColors.Visit(rtf);

			OpenSection();
		}

		public void BeginLine(ILine line, Position position)
		{
			if (position == Position.Only || position == Position.Last)
				OpenSection();

			OpenSection();
		}

		public void BeginInstruction(IInstruction instruction, Position position)
		{
			OpenSection();

			if (!instruction.IsValid)
				BeginError();
		}

		public void Whitespace(string whitespace)
		{
			Append(whitespace);
		}

		public void Block(string identifier)
		{
			OpenSection();
			Bold();
			FontColor(RtfColors.FOREGROUND);
			Append(identifier);
			CloseSection();
		}

		public void On()
		{
			OpenSection();
			FontColor(RtfColors.GRAY);
			Append("on");
			CloseSection();
		}

		public void CharDescription(string charDesc)
		{
			OpenSection();
			Bold();
			FontColor(RtfColors.BLUE);
			Append(charDesc);
			CloseSection();
		}

		public void Consumption(Consumption consumption)
		{
			OpenSection();
			switch (consumption)
			{
				case syntax.Consumption.Eat:
					FontColor(RtfColors.RED);
					Append("eat");
					break;

				case syntax.Consumption.Bounce:
					FontColor(RtfColors.GRAY);
					Append("bounce");
					break;

				default: throw new ArgumentOutOfRangeException(nameof(consumption), consumption, null);
			}

			CloseSection();
		}

		public void Goto()
		{
			OpenSection();
			FontColor(RtfColors.GRAY);
			Append("goto");
			CloseSection();
		}

		public void GotoTargetName(string targetName)
		{
			OpenSection();
			Bold();
			FontColor(RtfColors.FOREGROUND);
			Append(targetName);
			CloseSection();
		}

		public void Flow()
		{
			OpenSection();
			FontColor(RtfColors.GRAY);
			Append("flow");
			CloseSection();
		}

		public void FlowContentType(string content)
		{
			OpenSection();
			FontColor(RtfColors.GREEN);
			Append(content);
			CloseSection();
		}

		public void Error()
		{
			OpenSection();
			FontColor(RtfColors.GRAY);
			Append("error");
			CloseSection();
		}

		public void ErrorDescription(string errorText)
		{
			OpenSection();
			Bold();
			FontColor(RtfColors.ORANGE);
			Append(errorText);
			CloseSection();
		}

		public void Eof()
		{
			OpenSection();
			FontColor(RtfColors.PURPLE);
			Append("eof");
			CloseSection();
		}

		public void EndOfChunk()
		{
			OpenSection();
			FontColor(RtfColors.PURPLE);
			Append("eoc");
			CloseSection();
		}

		public void InvalidText(string invalid)
		{
			BeginError();

			Append(invalid);
		}

		public void EmptyLine(EmptyLine emptyLine)
		{
			Append("\n");
		}

		public void Finish()
		{
			CloseSection();
		}

		public void EndInstruction(IInstruction instruction, Position position)
		{
			EndError();

			if (position != Position.Last && position != Position.Only)
			{
				OpenSection();
				FontColor(RtfColors.GRAY);
				Append(",");
				CloseSection();
			}

			CloseSection();
		}

		public void EndLine(ILine line, Position position)
		{
			if (position != Position.Last && position != Position.Only)
				EndParagraph();

			CloseSection();
		}

		private void EndParagraph()
		{
			Append("\\par");
		}

		private void OpenSection()
		{
			Append("{");
		}


		private void CloseSection()
		{
			Append("}");
		}

		private void Append(string str)
		{
			rtf.Append(str);
		}

		private void BeginError()
		{
			if (!inError)
			{
				OpenSection();
				BackgroundColor(RtfColors.DARK_RED);
				FontColor(RtfColors.BLACK);
				inError = true;
			}
		}

		private void EndError()
		{
			if (inError)
			{
				CloseSection();
				inError = false;
			}
		}

		private void Bold()
		{
			Append("\\b");
		}

		private void FontColor(int colorIndex)
		{
			Append("\\cf");
			Append(colorIndex.ToString());
			Append(" ");
		}

		private void BackgroundColor(int colorIndex)
		{
			Append("\\highlight");
			Append(colorIndex.ToString());
		}


		public override string ToString()
		{
			return rtf.ToString();
		}
	}
}