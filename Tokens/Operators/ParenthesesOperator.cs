namespace Interpreter.Tokens.Operators; 

public class ParenthesesOperator : UnaryOperator {
	public ParenthesesOperator() {
		Symbol = "()";
	}
	
	public override int Evaluate() {
		return Child.Evaluate();
	}
}