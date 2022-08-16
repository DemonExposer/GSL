namespace Interpreter.Types.Util;
using String = Comparable.String;

public class DictionaryEntry : Object {
	public String Key;
	public Object Value;

	public DictionaryEntry(String key, Object value) {
		Key = key;
		Value = value;
	}

	public override string GetType() => "DictionaryEntry";

	public override string ToString() => $"\"{Key}\": {Value.ToString()}";
}

