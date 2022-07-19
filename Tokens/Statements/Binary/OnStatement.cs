using TrieDictionary;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Binary; 

public class OnStatement : BinaryStatement {
	public OnStatement() {
		Symbol = "on";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		List<TrieDictionary<Object>> properVars = new List<TrieDictionary<Object>>(vars);
		properVars.Add(new TrieDictionary<Object>());
		
		return ((Boolean) Left.Evaluate(properVars)).Bool ? Right.Evaluate(properVars) : null!;
	}
}