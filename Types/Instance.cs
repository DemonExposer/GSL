namespace Interpreter.Types; 

public class Instance : Object {
	public List<Object> Parents = new List<Object>();
	
	public override string ToString() => "Instance";

	public override string GetType() => ClassType.Name;
}