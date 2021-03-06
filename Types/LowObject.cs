using TrieDictionary;

namespace Interpreter.Types; 

public class LowObject {
	public Class ClassType = null!;
	public TrieDictionary<Object> Properties = new TrieDictionary<Object>();

	public new virtual string ToString() => "Object";

	public new virtual string GetType() => "Object";
}