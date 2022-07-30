namespace Interpreter; 

public class DoublyLinkedList<T> {
	public DoublyLinkedList<T> Left = null!, Right = null!;
	public T? Value;

	public static DoublyLinkedList<T> FromArray(T[] arr) {
		DoublyLinkedList<T> init = new DoublyLinkedList<T>();
		init.Value = arr[0];

		DoublyLinkedList<T> iter = init;
		for (int i = 1; i < arr.Length; i++) {
			iter.Right = new DoublyLinkedList<T>();
			iter = iter.Right;
			iter.Value = arr[i];
		}

		return init;
	}
}