namespace Interpreter.Types.Comparable.Numbers; 

public class Integer : Number {
	public int Int { get => (int) Num.Num; set => Num.Num = value; }
	private static List<Type> parents = new List<Type>(new [] {typeof(Number)});

	public Integer(int i) {
		Int = i;
	}

	public override string GetType() => "Integer";
}