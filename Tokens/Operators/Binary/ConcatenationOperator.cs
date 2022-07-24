using TrieDictionary;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Tokens.Operators.Binary; 

public class ConcatenationOperator : BinaryOperator {
	public ConcatenationOperator() {
		Symbol = ":";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new String($@"{Left.Evaluate(vars).ToString()}{Right.Evaluate(vars).ToString()}");
}