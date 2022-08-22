using Interpreter.Types.Comparable.Numbers.Util;

namespace Interpreter.Types.Comparable.Numbers;

public class Number : Comparable {
	public NumberType Num = new NumberType();
	
	public override string ToString() => Num.Num.ToString()!;

	public override string GetType() => "Number";

	public override Boolean Equals(Comparable c) {
		if (c is not Number n)
			throw new IncomparableException("trying to compare Integer with non-Integer");

		return new Boolean(Num == n.Num);
	}

	public static Object GetProperInstance(NumberType nt) {
		switch (nt.Num) {
			case int i:
				return new Integer(i);
			case double d:
				return new Double(d);
		}

		throw new Exception("incorrect type");
	}
}