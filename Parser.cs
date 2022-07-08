using System.Data;
using System.Text.RegularExpressions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Arithmetic;

namespace Interpreter; 

public class Parser {
	public static Token Parse(CheckedString[] line, int i, int depth) {
		Token t;
		Type tokenType;
		// Check if string is a keyword/operator
		if (Program.bindings.TryGetValue(line[i].Str, out tokenType)) {
			t = (Token) Activator.CreateInstance(tokenType); // Instantiate the corresponding class
			
			// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
			if (t is ArithmeticOperator) {
				line[i].IsDone = true;

				if (line[i - 1].Str == ")") {
					int numBrackets = 1;
					int j;
					for (j = i-2; numBrackets > 0; j--) {
						if (line[j].Str == "(")
							numBrackets--;
						else if (line[j].Str == ")")
							numBrackets++;
					}
					
					if (j <= 0 || line[j].IsDone)
						((ArithmeticOperator) t).Left = Parse(line, i - 1, depth + 1);
					else
						((ArithmeticOperator) t).Left = Parse(line, j, depth + 1);
				} else if (i - 2 < 0 || line[i-2].IsDone || line[i-1].Str == ")") {
					((ArithmeticOperator) t).Left = Parse(line, i - 1, depth + 1);
				} else {
					((ArithmeticOperator) t).Left = Parse(line, i - 2, depth + 1);
				}
				
				if (line[i + 1].Str == "(") {
					int numBrackets = 1;
					int j;
					for (j = i+2; numBrackets > 0; j++) {
						if (line[j].Str == "(")
							numBrackets++;
						else if (line[j].Str == ")")
							numBrackets--;
					}
					
					if (j >= line.Length || line[j].IsDone)
						((ArithmeticOperator) t).Right = Parse(line, i + 1, depth + 1);
					else
						((ArithmeticOperator) t).Right = Parse(line, j, depth + 1);
				} else if (i + 2 >= line.Length || line[i+2].IsDone || line[i+1].Str == "(") {
					((ArithmeticOperator) t).Right = Parse(line, i + 1, depth + 1);
				} else {
					((ArithmeticOperator) t).Right = Parse(line, i + 2, depth + 1);
				}
			} else if (t is DeclarationOperator) {
				((DeclarationOperator) t).SetVars(Program.vars);
				((DeclarationOperator) t).Left = Parse(line, i + 1, depth+1);
				((DeclarationOperator) t).Right = Parse(line, i + 3, depth+1); // Skip =
			} else if (t is AssignmentOperator) {
				((AssignmentOperator) t).SetVars(Program.vars);
				((AssignmentOperator) t).Left = Parse(line, i - 1, depth+1);
				((AssignmentOperator) t).Right = Parse(line, i + 1, depth+1);
			} else if (t is ParenthesesOperator) {
				int highestPriorityNum = -1;
				int index = -1;
				CheckedString[] subLine = {};
				if (line[i].Str == "(") {
					int numBrackets = 1;
					for (int j = i+1; numBrackets > 0; j++) {
						if (line[j].Str == ")")
							numBrackets--;
						else if (line[j].Str == "(")
							numBrackets++;

						if (numBrackets == 0)
							break;
						
						if (line[j].Str == "(") {
							while (numBrackets > 1) {
								subLine = subLine.Append(line[j]).ToArray();
								j++;
								if (line[j].Str == "(")
									numBrackets++;
								else if (line[j].Str == ")")
									numBrackets--;
							}
						}
						
						subLine = subLine.Append(line[j]).ToArray();
						
						int priority;
						if (Program.priorities.TryGetValue(line[j].Str, out priority)) {
							if (priority >= highestPriorityNum) {
								highestPriorityNum = priority;
								index = j;
							}
						}
					}
				} else {
					int numBrackets = 1;
					for (int j = i-1; numBrackets > 0; j--) {
						if (line[j].Str == ")")
							numBrackets++;
						else if (line[j].Str == "(")
							numBrackets--;

						if (numBrackets == 0)
							break;
						
						if (line[j].Str == ")") {
							while (numBrackets > 1) {
								subLine = subLine.Prepend(line[j]).ToArray();
								j++;
								if (line[j].Str == "(")
									numBrackets--;
								else if (line[j].Str == ")")
									numBrackets++;
							}
						}
						
						subLine = subLine.Prepend(line[j]).ToArray();
						
						int priority;
						if (Program.priorities.TryGetValue(line[j].Str, out priority)) {
							if (priority > highestPriorityNum) {
								highestPriorityNum = priority;
								index = 2*i - j; // Invert j with respect to i, so that later in the Parse call, it will result in the right number
							}
						}
					}
				}
				
				// Parse the data in the brackets, where index is the index of the head of the tree in line, so index-i
				// (i being the starting point of subLine in line) will be the index in subLine plus one
				((ParenthesesOperator) t).Child = Parse(subLine, index-i-1, depth + 1);
			}
		} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
			VariableToken vt = new VariableToken(Program.vars);
			vt.Name = line[i].Str;
			t = vt;

			if (i < line.Length-1 && line[i + 1].Str == "(") {
				line[i + 1].IsDone = true;
				// TODO: parse everything in the function call, and mark the second bracket as done
			}
		} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
			NumberToken nt = new NumberToken();
			nt.Num = Int32.Parse(line[i].Str);
			t = nt;
		} else {
			throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;
		
		return t;
	}
}