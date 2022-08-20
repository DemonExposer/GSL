namespace Interpreter.Types.Comparable; 

public abstract class Comparable : Object {
	private static List<Type> parents = new List<Type>(new [] {typeof(Object)});
	
	public abstract Boolean Equals(Comparable c);
}