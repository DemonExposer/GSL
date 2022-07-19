using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class EqualityBinaryOperator : BooleanOperator {
	public EqualityBinaryOperator() {
		Symbol = "==";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Object leftObj = Left.Evaluate(vars), rightObj = Right.Evaluate(vars);
		if (leftObj is not Comparable c1)
			throw new IncomparableException("trying to compare incomparable type " + leftObj.GetType());

		if (rightObj is not Comparable c2)
			throw new IncomparableException("trying to compare incomparable type " + rightObj.GetType());

		return c1.Equals(c2);
	}
}