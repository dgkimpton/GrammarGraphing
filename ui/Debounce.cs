using System;
using System.Windows.Forms;

namespace GrammarGrapher.ui
{
	public class Debounce
	{
		private bool isRunningTrigger;
		private readonly Timer parseTimer = new Timer();

		public event Action Triggered;

		public Debounce()
		{
			parseTimer.Interval = 50;
			parseTimer.Tick += Tick;
		}

		public void Reset()
		{
			parseTimer.Stop();
			if (!isRunningTrigger)
				parseTimer.Start();
		}

		private void Tick(object sender, EventArgs e)
		{
			isRunningTrigger = true;
			parseTimer.Stop();
			Triggered?.Invoke();
			isRunningTrigger = false;
		}
	}
}