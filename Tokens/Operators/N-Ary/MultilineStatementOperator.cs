using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class MultilineStatementOperator : NAryOperator {
	public MultilineStatementOperator() {
		Symbol = "{}";
	}

	public override Object Evaluate() {
		foreach (Token t in Children)
			t.Evaluate();

		return null!;
	}
}