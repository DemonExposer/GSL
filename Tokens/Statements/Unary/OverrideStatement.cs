using Interpreter.Tokens.Statements.Binary;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class OverrideStatement : UnaryStatement {
	public OverrideStatement() {
		Symbol = "override";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		if (Child is not FunctionStatement fs)
			throw new FormatException("override statement is only compatible with functions");

		fs.IsOverride = true;
		return fs.Evaluate(vars);
	}
}