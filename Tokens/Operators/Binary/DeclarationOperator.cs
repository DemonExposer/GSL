using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	private List<TrieDictionary<Object>> vars = null!;
	
	public DeclarationOperator() {
		Symbol = "decl";
	}

	public void SetVars(List<TrieDictionary<Object>> vars) {
		this.vars = vars;
	}
	
	public override Object Evaluate() {
		try {
			vars[^1].Get(((VariableToken) Left).Name);
			throw new InvalidOperationException(((VariableToken) Left).Name + " is already defined");
		} catch (KeyNotFoundException) { }
		
		Object res = new Integer(0); // TODO: Make this default to null instead of 0
		vars[^1][((VariableToken) Left).Name] = res;

		if (Right is AssignmentOperator)
			return Right.Evaluate();
		
		return res;
	}
}