using Interpreter.Types;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	private IDictionary<string, Object> vars;
	
	public DeclarationOperator() {
		Symbol = "decl";
	}

	public void SetVars(IDictionary<string, Object> vars) {
		this.vars = vars;
	}
	
	public override Object Evaluate() {
		Object res = Right.Evaluate();
		vars.Add(((VariableToken) Left).Name, res);
		return res;
	}
}