using Interpreter.Types.Util;
using Object = Interpreter.Types.Object;
using Boolean = Interpreter.Types.Comparable.Boolean;

namespace Interpreter.Tokens.Statements; 

public class WhileLoop : Statement {
	public WhileLoop() {
		Symbol = "while";
	}

	public override Object Evaluate() {
		while (((Boolean) Left.Evaluate()).Bool)
			Right.Evaluate();

		return null!;
	}
}