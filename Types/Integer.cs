namespace Interpreter.Types; 

public class Integer : Object {
	public int Int;

	public Integer(int i) {
		Int = i;
	}
	
	public override string ToString() {
		return Int.ToString();
	}
}