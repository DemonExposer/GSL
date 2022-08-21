using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Util;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class InheritsStatement : UnaryStatement {
	public InheritsStatement() {
		Symbol = "inherits";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Child is ParenthesesOperator po ? po.Evaluate(vars) : new ArgumentArray(new[] {Child.Evaluate(vars)});
}