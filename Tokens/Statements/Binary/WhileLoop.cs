using Object = Interpreter.Types.Object;
using Boolean = Interpreter.Types.Comparable.Boolean;

namespace Interpreter.Tokens.Statements.Binary; 

public class WhileLoop : BinaryStatement {
	public WhileLoop() {
		Symbol = "while";
	}

	public override Object Evaluate() {
		while (((Boolean) Left.Evaluate()).Bool)
			Right.Evaluate();

		return null!;
	}
}