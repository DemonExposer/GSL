namespace Interpreter.Types.Comparable.Numbers; 

public class Integer : Number {
	public int Int { get => (int) Num.Num; set => Num.Num = value; }

	public Integer(int i) {
		Int = i;
	}

	public override string GetType() => "Integer";
}