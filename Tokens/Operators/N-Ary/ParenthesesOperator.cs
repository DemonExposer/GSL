using Interpreter.Types.Util;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class ParenthesesOperator : NAryOperator {
	public ParenthesesOperator() {
		Symbol = "()";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Children.Length == 1 ? Children[0].Evaluate(vars) : new ArgumentArray(Children.Select(child => child.Evaluate(vars)).ToArray());
}