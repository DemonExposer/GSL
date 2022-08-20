namespace Interpreter.Types.Util; 

public class ArgumentArray : Object {
	public Object[] Arr;
	private static List<Type> parents = new List<Type>(new [] {typeof(Object)});
	
	public ArgumentArray(Object[] arr) {
		Arr = arr;
	}
	
	public override string ToString() => "FunctionArgumentArray";

	public override string GetType() => "FunctionArgumentArray";
}