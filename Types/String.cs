namespace Interpreter.Types; 

public class String : Object {
	public string Str = null!;

	public String(string s) {
		Str = s;
	}

	public override string ToString() => Str;

	public override string GetType() => "String";
}