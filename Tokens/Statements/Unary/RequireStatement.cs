using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class RequireStatement : UnaryStatement {
	public RequireStatement() {
		Symbol = "require";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		string fileName = Child.Str + ".gsl";
		return null!;
	}
}