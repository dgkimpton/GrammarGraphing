namespace GrammarGrapher.syntax.triggers
{
	public class TriggerOn
			: IInstruction
	{
		private readonly int instructionIndex;

		private readonly string precedingWhitespace;
		// on string is implicit
		private readonly string onWhitespace;
		private readonly string charDesc;
		private readonly string descWhitespace;
		private readonly Consumption? consumption;
		private readonly string trailingWhitespace;
		private readonly string followingInvalid;

		private TriggerOn(
				int instructionIndex,
				string precedingSpace,
				string onSpace,
				string charDesc,
				string charSpace,
				Consumption? consumption, 
				string trailingSpace)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingSpace;
			this.onWhitespace = onSpace;
			this.charDesc = charDesc;
			this.descWhitespace = charSpace;
			this.consumption = consumption;
			this.trailingWhitespace = trailingSpace;
			this.followingInvalid = string.Empty;
		}

		private TriggerOn(
				int instructionIndex,
				string precedingSpace,
				string onSpace,
				string charDesc,
				string charSpace,
				Consumption? consumption,
				string trailingSpace,
				string followingInvalid)
		{
			this.instructionIndex = instructionIndex;
			this.precedingWhitespace = precedingSpace;
			this.onWhitespace = onSpace;
			this.charDesc = charDesc;
			this.descWhitespace = charSpace;
			this.consumption = consumption;
			this.trailingWhitespace = trailingSpace;
			this.followingInvalid = followingInvalid;
		}

		public bool IsValid => charDesc.Length > 0 && consumption != null && instructionIndex == 0;

		public void Visit(IVisitor visitor)
		{
			if (precedingWhitespace.Length > 0) visitor.Whitespace(precedingWhitespace);

			visitor.On();

			if (onWhitespace.Length > 0) visitor.Whitespace(onWhitespace);

			if (charDesc.Length > 0)
			{
				visitor.CharDescription(charDesc);

				if (descWhitespace.Length > 0) visitor.Whitespace(descWhitespace);

				if (consumption != null)
				{
					visitor.Consumption(consumption.Value);

					if (trailingWhitespace.Length > 0) visitor.Whitespace(trailingWhitespace);
				}
			}

			if (followingInvalid.Length > 0) visitor.InvalidText(followingInvalid);
		}

		public static IInstruction Parse(
				int instructionIndex,
				string _,
				string precedingWhitespace,
				string fullString,
				int index)
		{
			var descBeginIndex = fullString.NextNonWhite(index);
			var onSpace = fullString.SubstringBetween(index, descBeginIndex);

			var descEndIndex = fullString.NextWhite(descBeginIndex);
			var charDesc = fullString.SubstringBetween(descBeginIndex, descEndIndex);

			var consumptionBeginIndex = fullString.NextNonWhite(descEndIndex);
			var charSpace = fullString.SubstringBetween(descEndIndex, consumptionBeginIndex);

			var consumptionEndIndex = fullString.NextWhite(consumptionBeginIndex);
			var consumptionStr = fullString.SubstringBetween(consumptionBeginIndex, consumptionEndIndex);

			var consumption = Translate(consumptionStr);

			if (consumption == null)
			{
				return new TriggerOn(instructionIndex,
				                     precedingWhitespace,
				                     onSpace,
				                     charDesc,
				                     charSpace,
				                     null,
				                     string.Empty,
				                     fullString.SubstringBetween(consumptionBeginIndex, Utils.END));
			}
			else
			{
				var expectedEndIndex = fullString.NextNonWhite(consumptionEndIndex);
				var trailing = fullString.SubstringBetween(consumptionEndIndex, expectedEndIndex);

				return expectedEndIndex < fullString.Length
						       ? new TriggerOn(instructionIndex,
						                       precedingWhitespace,
						                       onSpace,
						                       charDesc,
						                       charSpace,
						                       consumption,
						                       trailing,
						                       fullString.SubstringBetween(expectedEndIndex, Utils.END))
						       : new TriggerOn(instructionIndex,
						                       precedingWhitespace,
						                       onSpace,
						                       charDesc,
						                       charSpace,
						                       consumption,
						                       trailing);
			}

		}

		private static Consumption? Translate(string consumptionStr)
		{
			switch (consumptionStr.ToLower())
			{
				case "eat": return Consumption.Eat;
				case "bounce": return Consumption.Bounce;
				default: return null;
			}
		}
	}
}