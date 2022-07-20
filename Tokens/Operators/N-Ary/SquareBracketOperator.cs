using TrieDictionary;
using Array = Interpreter.Types.Array;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class SquareBracketOperator : NAryOperator {
	public Array Arr = null!;
	
	public SquareBracketOperator() {
		Symbol = "[]";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Arr = new Array(Children.Select(child => child.Evaluate(vars)));
		return Arr;
	}
}