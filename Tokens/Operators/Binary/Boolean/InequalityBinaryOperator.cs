using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class InequalityBinaryOperator : BooleanOperator {
	public InequalityBinaryOperator() {
		Symbol = "!=";
	}

	public override Object Evaluate() {
		throw new NotImplementedException();
	}
}