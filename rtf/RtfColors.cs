using System.Text;

namespace GrammarGrapher.rtf
{
	public static class RtfColors
	{
		public const int BLACK = 0;
		// Black is the default table entry

		public const int WHITE = 1;
		private const string WHITE_STR = "\\red255\\green255\\blue255;";

		public const int GRAY = 2;
		private const string GRAY_STR = "\\red132\\green132\\blue132;";

		public const int RED = 3;
		private const string RED_STR = "\\red255\\green0\\blue0;";

		public const int PURPLE = 4;
		private const string PURPLE_STR = "\\red204\\green0\\blue204;";

		public const int BLUE = 5;
		private const string BLUE_STR = "\\red0\\green128\\blue255;";

		public const int ORANGE = 6;
		private const string ORANGE_STR = "\\red255\\green128\\blue0;";

		public const int YELLOW = 7;
		private const string YELLOW_STR = "\\red255\\green255\\blue0;";

		public const int GREEN = 8;
		private const string GREEN_STR = "\\red0\\green153\\blue0;";

		public const int DARK_RED = 9;
		private const string DARK_RED_STR = "\\red150\\green10\\blue10;";




		public const int FOREGROUND = WHITE;

		public static void Visit(StringBuilder rtf)
		{
			rtf.Append("{\\colortbl;");
			rtf.Append(WHITE_STR);
			rtf.Append(GRAY_STR);
			rtf.Append(RED_STR);
			rtf.Append(PURPLE_STR);
			rtf.Append(BLUE_STR);
			rtf.Append(ORANGE_STR);
			rtf.Append(YELLOW_STR);
			rtf.Append(GREEN_STR);
			rtf.Append(DARK_RED_STR);
			rtf.Append("}");
		}
	}
}