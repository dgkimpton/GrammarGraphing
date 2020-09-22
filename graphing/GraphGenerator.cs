using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using GrammarGrapher.syntax;

namespace GrammarGrapher.graphing
{
	public class GraphGenerator
	{
		private readonly string tempPath;
		private int nextFileIndex;

		public GraphGenerator()
		{
			tempPath = Path.GetTempPath();
		}

		public Image Run(Grammar grammar)
		{
			var dot = new DotString();
			grammar.Visit(dot);

			var filePart = MakeFilename();

			var dotFile = Path.Combine(tempPath, filePart + ".gv");
			File.Delete(dotFile);
			File.WriteAllText(dotFile, dot.ToString());

			var filename = Path.Combine(tempPath, filePart + ".png");
			File.Delete(filename);

			Generate(dotFile, filename);

			return Image.FromFile(filename);
		}

		private string MakeFilename()
		{
			++nextFileIndex;
			return "DotGraphGenerator-flow-" + nextFileIndex;
		}

		private static void Generate(string dotFile, string outFile)
		{
			using var dotExe = new Process
			                   {
					                   StartInfo =
					                   {
							                   FileName = "dot.exe",
							                   Arguments = $"-Tpng {dotFile} -o{outFile}",
							                   WindowStyle = ProcessWindowStyle.Hidden,
							                   CreateNoWindow = true,
							                   RedirectStandardOutput = true,
											   RedirectStandardError = true,
							                   UseShellExecute = false,
									   }
			                   };

			try
			{

				dotExe.Start();
				Console.WriteLine(dotExe.StandardOutput.ReadToEnd());
				Console.WriteLine(dotExe.StandardError.ReadToEnd());
				dotExe.WaitForExit(3000);

				if (dotExe.HasExited == false)
				{
					if (dotExe.Responding)
						dotExe.CloseMainWindow();
					else
						dotExe.Kill();

					dotExe.WaitForExit(500);
				}


				if (dotExe.ExitCode != 0)
					throw new Exception($"exit code {dotExe.ExitCode}");
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred trying to convert .dot to .png : {ex.Message}");
			}
		}
	}
}