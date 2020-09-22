using System;
using System.Windows.Forms;

namespace GrammarGrapher.ui
{
	public class HighlightableRichText
	{
		private readonly RichTextBox textBox;

		public event Action TextChanged;

		public HighlightableRichText(RichTextBox textBox)
		{
			this.textBox = textBox;
			this.textBox.TextChanged += OnTextBoxOnTextChanged;
		}

		private void OnTextBoxOnTextChanged(object sender, EventArgs args)
		{
			this.TextChanged?.Invoke();
		}

		public void Update(string rtf)
		{
			var start = textBox.SelectionStart;

			textBox.Rtf = rtf;
			textBox.SelectionStart = start;
		}

		public string[] Lines => textBox.Lines;

		public string Text
		{
			get => textBox.Text;
			set => textBox.Text = value;
		}
	}
}