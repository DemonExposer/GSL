using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Unary;

namespace Interpreter; 

public class Parser {
	private static Token ArithmeticParse(Token[] line, int i, int depth, bool isRightBound) {
		if (isRightBound ? line[i + 1].Str == "(" : line[i - 1].Str == ")") {
			// Move to first BinaryOperator in sight. If that does not exist or is already done, move to closest highest level token
			// which is i+1 if rightbound (e.g. cur + b makes i+1 == "+") and k+1 if leftbound (e.g. -a + cur makes j+1 == "-")
			int j = i + (isRightBound ? 1 : -1);
			int numBrackets = 0;
			while (numBrackets != 0 || (isRightBound ? j < line.Length : j >= 0) && line[j] is not BinaryOperator) {
				if (line[j].Str == "(")
					numBrackets++;
				else if (line[j].Str == ")")
					numBrackets--;
			
				j += isRightBound ? 1 : -1;
			}

			/*
			 * Of course check for bounds and check whether this is the time when the brackets should be parsed
			 * If there is a BinaryOperator on the other side of the brackets,
			 * they should only be parsed when that operator is above the current operator in the parse tree.
			 * Or in other words, the other operator should be done
			 */
			if ((isRightBound ? j >= line.Length : j < 0) || line[j].IsDone)
				if (!isRightBound)
					return Parse(line, j + 1, depth + 1);
				else
					return Parse(line, i + 1, depth + 1);
			
			// BinaryOperator does exist and is not done, parse it
			return Parse(line, j, depth + 1);
		}

		/*
		 * Why are the next variables not named j and numBrackets? Because of some super super super super weird bug_,
		 * which makes the compiler think that those variables are already defined in the scope, when clearly, they aren't.
		 * It is because of this COMPILE TIME BUG_, that runtime is blocked, even though the code would just work fine.
		 * Also, you can't say bug_ normally or JetBrains Rider will think something of it
		 */ 
		
		// Move to first BinaryOperator in sight. If that does not exist or is already done, move to closest highest level token
		// which is i+1 if rightbound (e.g. cur + b makes i+1 == "+") and k+1 if leftbound (e.g. -a + cur makes k+1 == "-")
		int k = i + (isRightBound ? 1 : -1);
		int bracketNum = 0;
		while (bracketNum != 0 || (isRightBound ? k < line.Length : k >= 0) && line[k] is not BinaryOperator) {
			if (line[k].Str == "(")
				bracketNum++;
			else if (line[k].Str == ")")
				bracketNum--;
			
			k += isRightBound ? 1 : -1;
		}

		// BinaryOperator does not exist or is already done
		if ((isRightBound ? k >= line.Length : k < 0) || line[k].IsDone)
			if (!isRightBound)
				return Parse(line, k + 1, depth + 1);
			else
				return Parse(line, i + 1, depth + 1);
		
		// BinaryOperator does exist and is not done, parse it
		return Parse(line, k, depth + 1);
	}

	/**
	 * Removes parentheses and parses inside expression by identifying the top operator and calling Parse
	 */
	private static Token ParenthesesParse(Token[] line, int i, int depth, bool isRightBound) {
		int startIndex = -1;
		int highestPriorityNum = -1;
		int index = -1;
		Token[] subLine = {};
		int numBrackets = 1;
		for (int j = i + (isRightBound ? 1 : -1); numBrackets > 0; j += isRightBound ? 1 : -1) {
			// Go until paired bracket is found
			if (line[j].Str == ")")
				numBrackets += isRightBound ? -1 : 1;
			else if (line[j].Str == "(")
				numBrackets += isRightBound ? 1 : -1;

			if (numBrackets == 0)
				break;

			// If there is a nested set of brackets, add that entire set immediately, because otherwise something goes wrong
			if (isRightBound ? line[j].Str == "(" : line[j].Str == ")") {
				while (numBrackets > 1) {
					if (isRightBound && subLine.Length == 0)
						startIndex = j;
					subLine = isRightBound ? subLine.Append(line[j]).ToArray() : subLine.Prepend(line[j]).ToArray();
					j += isRightBound ? 1 : -1;
					if (line[j].Str == "(")
						numBrackets += isRightBound ? 1 : -1;
					else if (line[j].Str == ")")
						numBrackets += isRightBound ? -1 : 1;
				}
			}

			if (isRightBound) {
				if (subLine.Length == 0)
					startIndex = j;
				subLine = subLine.Append(line[j]).ToArray();
			} else {
				startIndex = j;
				subLine = subLine.Prepend(line[j]).ToArray();
			}

			// Get the index of the operator with the lowest priority (highest number) to make sure that gets parsed first
			int priority;
			if (line[j] is BinaryOperator && Program.priorities.TryGetValue(line[j].Str, out priority)) {
				if (isRightBound ? priority >= highestPriorityNum : priority > highestPriorityNum) {
					highestPriorityNum = priority;
					index = j;
				}
			}
		}
		
		if (index == -1) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Warning: Double brackets on line " + line[i].Line);
			Console.ResetColor();
			index = startIndex; // Make sure index-startIndex is 0
		}

		if (startIndex == -1) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error in bracket parsing");
			Console.ResetColor();
		}
		
		// Parse the data in the brackets, where index is the index of the head of the tree in line, so index-i
		// (i being the starting point of subLine in line) will be the index in subLine plus one
		return Parse(subLine, index - startIndex, depth + 1);
	}
	
	public static Token Parse(Token[] line, int i, int depth) {
		Token t = line[i];

		// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
		if (t is ArithmeticOperator arOp) {
			line[i].IsDone = true;
			
			arOp.Left = ArithmeticParse(line, i, depth, false);
			arOp.Right = ArithmeticParse(line, i, depth, true);
		} else if (t is DeclarationOperator decOp) { // TODO: get proper top operator
			decOp.SetVars(Program.vars);
			decOp.Left = Parse(line, i + 1, depth+1);
			decOp.Right = Parse(line, i + 3, depth+1); // Skip =
		} else if (t is AssignmentOperator assOp) { // TODO: get proper top operator
			assOp.SetVars(Program.vars);
			assOp.Left = Parse(line, i - 1, depth+1);
			assOp.Right = Parse(line, i + 1, depth+1);
		} else if (t is ParenthesesOperator parOp) {
			parOp.Child = ParenthesesParse(line, i, depth + 1, line[i].Str == "(");
		} else if (t is MinusUnaryOperator minUnOp) {
			minUnOp.Child = Parse(line, i + 1, depth + 1);
		} else if (t is VariableToken vt) { // TODO: make sure multiple arguments get parsed properly
			if (i + 1 < line.Length && line[i+1] is ParenthesesOperator)
				vt.Args = Parse(line, i + 1, depth + 1);
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;
		
		return t;
	}
}