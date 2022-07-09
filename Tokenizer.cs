using System.Data;
using System.Text.RegularExpressions;
using Interpreter;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;

namespace DefaultNamespace; 

public class Tokenizer {
	public static Token[] Tokenize(CheckedString[] line) {
		Token[] res = new Token[line.Length];
		
		for (int i = 0; i < line.Length; i++) {
			if (line[i].Str == "-") { // Minus is unary when it is the first token or if the previous token is an operator
				if (i == 0 || Program.bindings.ContainsKey(line[i - 1].Str)) {
					res[i] = new MinusUnaryOperator();
					continue;
				}
			}

			Type tokenType;
			if (Program.bindings.TryGetValue(line[i].Str, out tokenType)) { // Check if string is a keyword/operator
				res[i] = (Token) Activator.CreateInstance(tokenType); // Instantiate the corresponding class
			} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
				VariableToken vt = new VariableToken(Program.vars);
				vt.Name = line[i].Str;
				res[i] = vt;

				if (i < line.Length-1 && line[i + 1].Str == "(") {
					line[i + 1].IsDone = true;
					// TODO: parse everything in the function call, and mark the second bracket as done
				}
			} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
				NumberToken nt = new NumberToken();
				nt.Num = Int32.Parse(line[i].Str);
				res[i] = nt;
			} else {
				throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
			}
		}

		return res;
	}
}