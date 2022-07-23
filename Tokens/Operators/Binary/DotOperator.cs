using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary; 

public class DotOperator : BinaryOperator {
	public DotOperator() {
		Symbol = ".";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Left.Evaluate(vars).Properties[Right.Str];
}