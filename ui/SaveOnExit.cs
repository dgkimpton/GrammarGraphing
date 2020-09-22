using System.IO;

namespace GrammarGrapher.ui
{
	public class SaveOnExit
	{
		private readonly string saveFile;

		public SaveOnExit()
		{
			saveFile = Path.Combine(Path.GetTempPath(), "SyntaxHighlighted.SaveOnExit.txt");
		}

		public void Save(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				File.WriteAllText(saveFile, text);
			}
		}

		public string Load()
		{
			return File.Exists(saveFile)
					       ? string.Join('\n', File.ReadAllLines(saveFile))
					       : string.Empty;
		}
	}
}