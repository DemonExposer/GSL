using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class OrBinaryOperator : BooleanOperator {
	public OrBinaryOperator() {
		Symbol = "||/or";
	}
	
	public override Object Evaluate() {
		return new Types.Comparable.Boolean(((Types.Comparable.Boolean) Left.Evaluate()).Bool || ((Types.Comparable.Boolean) Right.Evaluate()).Bool);
	}
}