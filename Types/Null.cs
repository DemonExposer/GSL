namespace Interpreter.Types; 

public class Null : Object {
	private static List<Type> parents = new List<Type>(new [] {typeof(Object)});

	public override string ToString() => "null";

	public override string GetType() => "Null";
}