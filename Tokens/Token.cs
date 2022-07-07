namespace Interpreter.Tokens;

public abstract class Token {
	public int Line;

	public abstract string ToString(int indent);
	public abstract int Evaluate();
	public abstract int Size();
}