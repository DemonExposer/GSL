namespace Interpreter.Types; 

public class Instance : Object {
	private static List<Object> parents = new List<Object>();
	
	public override string ToString() => "Instance";

	public override string GetType() => ClassType.Name;
}