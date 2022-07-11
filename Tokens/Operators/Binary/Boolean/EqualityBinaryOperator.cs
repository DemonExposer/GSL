using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class EqualityBinaryOperator : BooleanOperator {
	public EqualityBinaryOperator() {
		Symbol = "==";
	}
	
	public override Object Evaluate() {
		throw new NotImplementedException();
	}
}