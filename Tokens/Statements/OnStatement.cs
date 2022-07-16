using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements; 

public class OnStatement : Statement {
	public OnStatement() {
		Symbol = "on";
	}

	public override Object Evaluate() => ((Boolean) Left.Evaluate()).Bool ? Right.Evaluate() : null!;
}