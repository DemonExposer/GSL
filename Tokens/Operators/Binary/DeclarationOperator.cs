using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	private TrieDictionary<Object> vars = null!;
	
	public DeclarationOperator() {
		Symbol = "decl";
	}

	public void SetVars(TrieDictionary<Object> vars) {
		this.vars = vars;
	}
	
	public override Object Evaluate() {
		Object res = new Integer(0); // TODO: Make this default to null instead of 0
		vars[((VariableToken) Left).Name] = res;

		if (Right is AssignmentOperator)
			return Right.Evaluate();
		
		return res;
	}
}