using Interpreter.Types;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class MinusUnaryOperator : UnaryOperator {
	public MinusUnaryOperator() {
		Symbol = "-";
	}
	
	public override Object Evaluate() {
		return new Integer(-((Integer) Child.Evaluate()).Int);
	}
}