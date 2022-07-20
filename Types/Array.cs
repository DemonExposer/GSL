using System.Text;

namespace Interpreter.Types; 

public class Array : Object {
	public List<Object> Arr;

	public Array(IEnumerable<Object> arr) {
		Arr = arr.ToList();
	}
	
	public override string ToString() {
		if (Arr.Count == 0)
			return "[]";
		
		StringBuilder sb = new StringBuilder("[");
		Arr.ForEach(o => sb.Append(o.ToString()).Append(", "));
		sb.Append("\u0008\u0008]");

		return sb.ToString();
	}

	public override string GetType() => "Array";
}