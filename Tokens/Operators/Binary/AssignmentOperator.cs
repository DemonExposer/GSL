using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class AssignmentOperator : BinaryOperator {
	private TrieDictionary<Object> vars;
	
	public AssignmentOperator() {
		Symbol = "=";
	}
	
	public void SetVars(TrieDictionary<Object> vars) {
		this.vars = vars;
	}
	
	public override Object Evaluate() {
		try {
			vars.Get(((VariableToken) Left).Name);
		} catch (KeyNotFoundException) {
			throw new KeyNotFoundException("Line " + Line + ": Trying to assign value to undeclared variable " + ((VariableToken) Left).Name);
		}

		Object res = Right.Evaluate();
		vars.Remove(((VariableToken) Left).Name);
		vars[((VariableToken) Left).Name] = res;
		
		return res;
	}
}