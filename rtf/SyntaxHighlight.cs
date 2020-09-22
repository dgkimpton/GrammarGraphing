using System;
using GrammarGrapher.syntax;

namespace GrammarGrapher.rtf
{
	public class SyntaxHighlight
    {
	    public event Action ValidityChanged;
	    public bool IsValid { get; private set; }

	    public SyntaxHighlight()
	    {
		    IsValid = true;
	    }
		
	    public string Run(Grammar grammar)
	    {
		    var astValid = grammar.IsValid;
		    if (astValid != IsValid)
		    {
			    IsValid = astValid;
				ValidityChanged?.Invoke();
		    }

			var rtf = new RtfBuilder();
			grammar.Visit(rtf);

		    return rtf.ToString();
	    }
    }
}

