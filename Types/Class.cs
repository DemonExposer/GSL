using TrieDictionary;

namespace Interpreter.Types; 

public class Class : Object {
	public TrieDictionary<Object> ClassProperties = new TrieDictionary<Object>();
	public string Name = null!;
	private List<Class> parents = new List<Class>();

	public virtual Object Instantiate(params Object[] args) {
		TrieDictionary<Object> propertiesCopy = new TrieDictionary<Object>();
		// TODO: This still copies references of values, which may pose a problem in the future, fix this
		ClassProperties.GetKeySet().ToList().ForEach(key => propertiesCopy.Insert(key, ClassProperties[key]));
		return new Instance {ClassType = this, Properties = propertiesCopy};
	}

	public override string ToString() => "Class";

	public override string GetType() => "Class";

	public void AddParent(Class c) {
		parents.Add(c);
		c.ClassProperties.GetKeySet().ToList().ForEach(key => ClassProperties.Insert(key, c.ClassProperties[key]));
	}
}