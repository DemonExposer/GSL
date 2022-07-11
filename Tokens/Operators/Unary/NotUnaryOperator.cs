using Boolean = Interpreter.Types.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class NotUnaryOperator : UnaryOperator {
	public NotUnaryOperator() {
		Symbol = "!";
	}
	
	public override Object Evaluate() {
		return new Boolean(!((Boolean) Child.Evaluate()).Bool);
	}
}