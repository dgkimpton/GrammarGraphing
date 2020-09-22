using System;
using System.ComponentModel;
using System.Windows.Forms;
using GrammarGrapher.graphing;
using GrammarGrapher.rtf;
using GrammarGrapher.syntax;

namespace GrammarGrapher.ui
{
	public partial class Form1 : Form
	{
		private readonly SaveOnExit lifebelt;

		private readonly HighlightableRichText syntax;
		private readonly Debounce debounce;
		private readonly SyntaxHighlight highlighter;
		private readonly GraphGenerator grapher;

		public Form1()
		{
			InitializeComponent();

			lifebelt = new SaveOnExit();
			syntax = new HighlightableRichText(input);
			debounce = new Debounce();
			highlighter = new SyntaxHighlight();
			grapher = new GraphGenerator();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != (Keys.Control | Keys.Enter))
				return base.ProcessCmdKey(ref msg, keyData);

			GenerateGraph();
			return true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			syntax.Text = lifebelt.Load();

			syntax.TextChanged += debounce.Reset;
			debounce.Triggered += RunSyntaxHighlighting;
			highlighter.ValidityChanged += SetUpdateEnabled;
			update.Click += (o, a) => GenerateGraph();

			RunSyntaxHighlighting();
			GenerateGraph();
		}

		private void SetUpdateEnabled()
		{
			update.Enabled = highlighter.IsValid;
		}

		private void GenerateGraph()
		{
			lifebelt.Save(syntax.Text);
			var ast = Grammar.Parse(syntax.Lines);

			if (ast.IsValid)
			{
				SuspendLayout();
				output.Image = null;
				output.Image = grapher.Run(ast);
				ResumeLayout();
			}
		}

		private void RunSyntaxHighlighting()
		{
			SuspendLayout();
			syntax.Update(highlighter.Run(Grammar.Parse(syntax.Lines)));
			ResumeLayout();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			lifebelt.Save(syntax.Text);
			base.OnClosing(e);
		}
	}
}
