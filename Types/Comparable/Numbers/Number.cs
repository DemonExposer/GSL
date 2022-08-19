namespace Interpreter.Types.Comparable.Numbers;

// Imagine having a Number type like in Java. We all know who is responsible...
public class NumberType {
	private object num = null!;
	public object Num {
		get => num;
		set {
			if (value is not int and not double)
				throw new Exception("Tried to assign invalid type to Number");

			num = value;
		}
	}

	public NumberType(object o) {
		if (o is not int and not double)
			throw new Exception("Tried to assign invalid type to Number");
		
		num = o;
	}

	public NumberType() {
		
	}

	public static NumberType operator +(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return new NumberType(i + j);
					case double k:
						return new NumberType(i + k);
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return new NumberType(d + j);
					case double k:
						return new NumberType(d + k);
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}
	
	public static NumberType operator -(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return new NumberType(i - j);
					case double k:
						return new NumberType(i - k);
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return new NumberType(d - j);
					case double k:
						return new NumberType(d - k);
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}
	
	public static NumberType operator *(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return new NumberType(i * j);
					case double k:
						return new NumberType(i * k);
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return new NumberType(d * j);
					case double k:
						return new NumberType(d * k);
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}
	
	public static NumberType operator /(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return new NumberType(i / j);
					case double k:
						return new NumberType(i / k);
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return new NumberType(d / j);
					case double k:
						return new NumberType(d / k);
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}
	
	public static bool operator ==(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return i == j;
					case double k:
						return i == k;
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return d == j;
					case double k:
						return d == k;
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}

	public static bool operator !=(NumberType a, NumberType b) => !(a == b);
	
	public static bool operator >(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return i > j;
					case double k:
						return i > k;
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return d > j;
					case double k:
						return d > k;
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}

	public static bool operator <(NumberType a, NumberType b) => !(a > b) && a != b;

	public static bool operator >=(NumberType a, NumberType b) => a > b || a == b;
	
	public static bool operator <=(NumberType a, NumberType b) => a < b || a == b;

	public static NumberType operator ^(NumberType a, NumberType b) {
		switch (a.num) {
			case int i:
				switch (b.num) {
					case int j:
						return new NumberType((int) Math.Pow(i, j));
					case double k:
						return new NumberType(Math.Pow(i, k));
				}
				break;
			case double d:
				switch (b.num) {
					case int j:
						return new NumberType(Math.Pow(d, j));
					case double k:
						return new NumberType(Math.Pow(d, k));
				}
				break;
		}

		throw new Exception("Tried to compare incomparable types");
	}

	public static NumberType operator -(NumberType a) {
		switch (a.num) {
			case int i:
				return new NumberType(-i);
			case double d:
				return new NumberType(-d);
		}
		
		throw new Exception("Tried to compare incomparable types");
	}
}

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