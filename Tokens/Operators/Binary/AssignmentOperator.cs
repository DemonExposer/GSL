using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary; 

public class AssignmentOperator : BinaryOperator {
	private IDictionary<string, Object> vars;
	
	public AssignmentOperator() {
		Symbol = "=";
	}
	
	public void SetVars(IDictionary<string, Object> vars) {
		this.vars = vars;
	}
	
	public override Object Evaluate() {
		if (!vars.ContainsKey(((VariableToken) Left).Name))
			throw new KeyNotFoundException("Line " + Line + ": Trying to assign value to undeclared variable " + ((VariableToken) Left).Name);

		Object res = Right.Evaluate();
		vars.Remove(((VariableToken) Left).Name);
		vars.Add(((VariableToken) Left).Name, res);
		
		return res;
	}
}