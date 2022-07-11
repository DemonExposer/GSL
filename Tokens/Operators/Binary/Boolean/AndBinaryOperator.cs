using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class AndBinaryOperator : BooleanOperator {
	public AndBinaryOperator() {
		Symbol = "&&/and";
	}
	
	public override Object Evaluate() {
		return new Types.Boolean(((Types.Boolean) Left.Evaluate()).Bool && ((Types.Boolean) Right.Evaluate()).Bool);
	}
}