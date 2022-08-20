namespace Interpreter.Types; 

public class Instance : Object {
	private static List<Type> parents = new List<Type>(new [] {typeof(Object)});
	
	public override string ToString() => "Instance";

	public override string GetType() => ClassType.Name;
}