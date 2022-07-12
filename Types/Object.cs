namespace Interpreter.Types; 

/**
 * This class forces every other class that extends it to implement the ToString method
 */
public abstract class Object : LowObject {
	public abstract override string ToString();

	public abstract override string GetType();
}