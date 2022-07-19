namespace Interpreter.Types.Util; 

public class ArgumentArray : Object {
	public Object[] Arr;
	
	public ArgumentArray(Object[] arr) {
		Arr = arr;
	}
	
	public override string ToString() => "FunctionArgumentArray";

	public override string GetType() => "FunctionArgumentArray";
}