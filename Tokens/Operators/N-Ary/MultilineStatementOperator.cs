using Interpreter.Tokens.Statements.Binary;
using Interpreter.Tokens.Statements.Unary;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class MultilineStatementOperator : NAryOperator {
	public bool IsPartOfFunction = false;
	
	public MultilineStatementOperator() {
		Symbol = "{}";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		foreach (Token t in Children) {
			if (t is ReturnStatement) {
				if (!IsPartOfFunction)
					throw new FormatException("Line " + t.Line + ": return statement not allowed outside of function");
				
				return t.Evaluate(vars);
			}

			if (t is BinaryStatement bs) {
				((MultilineStatementOperator) bs.Right).IsPartOfFunction = IsPartOfFunction;
				Object obj = t.Evaluate(vars);
				if (obj != null!)
					return obj;
			} else {
				t.Evaluate(vars);
			}
		}

		return null!;
	}
}