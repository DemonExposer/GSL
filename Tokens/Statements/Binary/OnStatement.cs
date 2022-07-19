using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Binary; 

public class OnStatement : BinaryStatement {
	public OnStatement() {
		Symbol = "on";
	}

	public override Object Evaluate() => ((Boolean) Left.Evaluate()).Bool ? Right.Evaluate() : null!;
}