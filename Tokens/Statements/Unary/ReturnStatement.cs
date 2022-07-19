using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class ReturnStatement : UnaryStatement {
	public ReturnStatement() {
		Symbol = "return";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Child.Evaluate(vars);
}