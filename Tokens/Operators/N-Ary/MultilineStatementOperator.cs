using Interpreter.Tokens.Statements.Binary;
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
			if (t is ReturnStatement) // TODO: add flag to check if this is part of a function
				return t.Evaluate(vars);

			if (t is BinaryStatement) {
				Object obj = t.Evaluate(vars);
				if (obj != null!)
					return obj;
			}
			
			t.Evaluate(vars);
		}

		return null!;
	}
}