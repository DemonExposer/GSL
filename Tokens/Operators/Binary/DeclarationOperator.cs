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
		Object res = new Integer(0); // TODO: Make this default to null instead of 0
		vars.Add(((VariableToken) Left).Name, res);

		if (Right is AssignmentOperator)
			return Right.Evaluate();
		
		return res;
	}
}