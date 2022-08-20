using System.Text;

namespace Interpreter.Types;

public class Dictionary : Object {
	private static List<Type> parents = new List<Type>(new [] {typeof(Object)});
	
	public override string GetType() => "Dictionary";

	public override string ToString() => ToString(0);
	
	public string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			sb.Append('\t');
		string indentStr = sb.ToString();
		
		sb = new StringBuilder("{\n");
		foreach (string s in Properties.GetKeySet())
			sb.Append(indentStr).Append($"\t{s}: {(Properties[s] is Dictionary dict ? dict.ToString(indent + 1) : Properties[s].ToString())}\n");
		sb.Append(indentStr).Append('}');

		return sb.ToString();
	}
}
