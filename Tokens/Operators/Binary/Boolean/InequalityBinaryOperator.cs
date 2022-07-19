using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class InequalityBinaryOperator : BooleanOperator {
	public InequalityBinaryOperator() {
		Symbol = "!=";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Object leftObj = Left.Evaluate(vars), rightObj = Right.Evaluate(vars);
		if (leftObj is not Comparable c1)
			throw new IncomparableException("trying to compare incomparable type " + leftObj.GetType());

		if (rightObj is not Comparable c2)
			throw new IncomparableException("trying to compare incomparable type " + rightObj.GetType());

		Types.Comparable.Boolean res = c1.Equals(c2);
		res.Bool = !res.Bool;
		return res;
	}
}