using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Types.Util;

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

