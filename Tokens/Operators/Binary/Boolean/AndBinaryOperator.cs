using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class AndBinaryOperator : BooleanOperator {
	public AndBinaryOperator() {
		Symbol = "&&/and";
	}
	
	public override Object Evaluate() {
		return new Types.Comparable.Boolean(((Types.Comparable.Boolean) Left.Evaluate()).Bool && ((Types.Comparable.Boolean) Right.Evaluate()).Bool);
	}
}