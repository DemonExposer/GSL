using TrieDictionary;
using Object = Interpreter.Types.Object;
using Boolean = Interpreter.Types.Comparable.Boolean;

namespace Interpreter.Tokens.Statements.Binary; 

public class WhileLoop : BinaryStatement {
	public WhileLoop() {
		Symbol = "while";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		List<TrieDictionary<Object>> properVars = new List<TrieDictionary<Object>>(vars);
		properVars.Add(new TrieDictionary<Object>());
		
		while (((Boolean) Left.Evaluate(properVars)).Bool)
			Right.Evaluate(properVars);

		return null!;
	}
}