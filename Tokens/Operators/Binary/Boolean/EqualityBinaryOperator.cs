using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class EqualityBinaryOperator : BooleanOperator {
	public EqualityBinaryOperator() {
		Symbol = "==";
	}
	
	public override Object Evaluate() {
		Object leftObj = Left.Evaluate(), rightObj = Right.Evaluate();
		if (leftObj is not Comparable c1)
			throw new IncomparableException("trying to compare incomparable type " + leftObj.GetType());

		if (rightObj is not Comparable c2)
			throw new IncomparableException("trying to compare incomparable type " + rightObj.GetType());

		return c1.Equals(c2);
	}
}