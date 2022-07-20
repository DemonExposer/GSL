namespace Interpreter.Types.Comparable; 

public class Integer : Comparable {
	public int Int;

	public Integer(int i) {
		Int = i;
	}
	
	public override string ToString() => Int.ToString();

	public override string GetType() => "Integer";

	public override Boolean Equals(Comparable c) {
		if (c is not Integer i)
			throw new IncomparableException("trying to compare Integer with non-Integer");

		return new Boolean(Int == i.Int);
	}
}