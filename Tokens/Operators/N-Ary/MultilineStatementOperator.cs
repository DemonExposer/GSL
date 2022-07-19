using Interpreter.Tokens.Statements.Unary;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class MultilineStatementOperator : NAryOperator {
	public MultilineStatementOperator() {
		Symbol = "{}";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		foreach (Token t in Children) {
			if (t is ReturnStatement)
				return t.Evaluate(vars);
			
			t.Evaluate(vars);
		}

		return null!;
	}
}