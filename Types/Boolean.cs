namespace Interpreter.Types; 

public class Boolean : Object {
	public bool Bool;

	public Boolean(bool b) {
		Bool = b;
	}

	public override string ToString() {
		return Bool.ToString();
	}
}