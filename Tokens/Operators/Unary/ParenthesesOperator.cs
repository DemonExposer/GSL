using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class ParenthesesOperator : UnaryOperator {
	public ParenthesesOperator() {
		Symbol = "()";
	}
	
	public override Object Evaluate() {
		return Child.Evaluate();
	}
}