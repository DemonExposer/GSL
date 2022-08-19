using Interpreter.Types.Comparable.Numbers.Util;

namespace Interpreter.Types.Comparable.Numbers;

public class Number : Comparable {
	public NumberType Num = new NumberType();

	public Number(NumberType nt) {
		Num = nt;
	}
	
	protected Number() { }
	
	public override string ToString() => Num.Num.ToString()!;

	public override string GetType() => "Number";

	public override Boolean Equals(Comparable c) {
		if (c is not Number n)
			throw new IncomparableException("trying to compare Integer with non-Integer");

		return new Boolean(Num == n.Num);
	}
}