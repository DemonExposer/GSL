using TrieDictionary;

namespace Interpreter.Types; 

public class Class {
	public TrieDictionary<Object> Properties = new TrieDictionary<Object>();
	public string Name;

	public Class(string name) {
		Name = name;
	}
}