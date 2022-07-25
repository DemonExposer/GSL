using TrieDictionary;

namespace Interpreter.Types; 

public class Class : Object {
	public TrieDictionary<Object> ClassProperties = new TrieDictionary<Object>();
	public string Name = null!;

	public virtual Object Instantiate(params Object[] args) {
		TrieDictionary<Object> propertiesCopy = new TrieDictionary<Object>();
		// TODO: This still copies references of values, which may pose a problem in the future, fix this
		ClassProperties.GetKeySet().ToList().ForEach(key => propertiesCopy.Insert(key, ClassProperties.Get(key)));
		return new Instance {ClassType = this, Properties = propertiesCopy};
	}

	public override string ToString() => "Class";

	public override string GetType() => "Class";
}