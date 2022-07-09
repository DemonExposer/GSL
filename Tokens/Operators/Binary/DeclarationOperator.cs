namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	private IDictionary<string, object> vars;
	
	public DeclarationOperator() {
		Symbol = "decl";
	}

	public void SetVars(IDictionary<string, object> vars) {
		this.vars = vars;
	}
	
	public override int Evaluate() {
		int res = Right.Evaluate();
		vars.Add(((VariableToken) Left).Name, res);
		return res;
	}
}