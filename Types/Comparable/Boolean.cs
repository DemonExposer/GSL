namespace Interpreter.Types.Comparable; 

public class Boolean : Comparable {
	public bool Bool;

	public Boolean(bool b) {
		Bool = b;
	}

	public override string ToString() => Bool ? "true" : "false";

	public override string GetType() => "Boolean";

	public override Boolean Equals(Comparable c) {
		if (c is not Boolean b)
			throw new IncomparableException("trying to compare Boolean with non-Boolean");
		
		return new Boolean(Bool == b.Bool);
	}
}