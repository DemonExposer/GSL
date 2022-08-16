using System.Text;

namespace Interpreter.Types;

public class Dictionary : Object {
	public override string GetType() => "Dictionary";

	public override string ToString() { // TODO: Make this indent nested dictionaries correctly
		StringBuilder sb = new StringBuilder("{\n");
		foreach (string s in Properties.GetKeySet())
			sb.Append($"\t{s}: {Properties[s].ToString()}\n");
		sb.Append('}');

		return sb.ToString();
	}
}
