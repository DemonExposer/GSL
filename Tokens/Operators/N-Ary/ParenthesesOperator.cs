using Interpreter.Types.Util;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class ParenthesesOperator : NAryOperator {
	public ParenthesesOperator() {
		Symbol = "()";
	}

	public override Object Evaluate() => Children.Length == 1 ? Children[0].Evaluate() : new ArgumentArray(Children.Select(child => child.Evaluate()).ToArray());
}