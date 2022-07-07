namespace Interpreter.Tokens.Operators; 

public class AssignmentOperator : BinaryOperator {
	private IDictionary<string, object> vars;
	
	public AssignmentOperator() {
		Symbol = "=";
	}
	
	public void SetVars(IDictionary<string, object> vars) {
		this.vars = vars;
	}
	
	public override int Evaluate() {
		if (!vars.ContainsKey(((VariableToken) Left).Name))
			throw new KeyNotFoundException("Line " + Line + ": Trying to assign value to undeclared variable " + ((VariableToken) Left).Name);

		int res = Right.Evaluate();
		vars.Remove(((VariableToken) Left).Name);
		vars.Add(((VariableToken) Left).Name, res);
		
		return res;
	}
}