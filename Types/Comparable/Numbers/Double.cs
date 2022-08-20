namespace Interpreter.Types.Comparable.Numbers; 

public class Double : Number {
	public double D { get => (double) Num.Num; set => Num.Num = value; }

	public Double(double d) {
		D = d;
	}

	public override string GetType() => "Double";
}