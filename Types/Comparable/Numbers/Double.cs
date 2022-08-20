namespace Interpreter.Types.Comparable.Numbers; 

public class Double : Number {
	public double D { get => (double) Num.Num; set => Num.Num = value; }
	private static List<Type> parents = new List<Type>(new [] {typeof(Number)});

	public Double(double d) {
		D = d;
	}

	public override string GetType() => "Double";
}