namespace Interpreter.Types.Comparable;

public class IncomparableException : Exception {
	public IncomparableException(string s) : base(s) { }

	public IncomparableException() { }
}