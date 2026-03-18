using System.Collections.Generic;

namespace lab04_sem2_text.file.editor
{
  public class Caretaker
  {
    public int MinimumMementoCountForUndo;

    public Stack<object> Mementos;
    public Stack<object> RedoStack;

    public Caretaker()
    {
      MinimumMementoCountForUndo = 2;
      Mementos = new Stack<object>();
      RedoStack = new Stack<object>();
    }

    public void SaveState(IOriginator originator)
    {
      object currentState;
      currentState = originator.GetMemento();

      Mementos.Push(currentState);
      RedoStack.Clear();
    }

    public void Undo(IOriginator originator)
    {
      object currentState;
      object previousState;

      if (Mementos.Count >= MinimumMementoCountForUndo) {
        currentState = Mementos.Pop();
        RedoStack.Push(currentState);

        previousState = Mementos.Peek();
        originator.SetMemento(previousState);
      }
    }

    public void Redo(IOriginator originator)
    {
      object stateToRedo;

      if (RedoStack.Count > 0) {
        stateToRedo = RedoStack.Pop();

        Mementos.Push(stateToRedo);
        originator.SetMemento(stateToRedo);
      }
    }

    public bool CanUndo()
    {
      bool canUndo;

      if (Mementos.Count >= MinimumMementoCountForUndo) {
        canUndo = true;
      } else {
        canUndo = false;
      }

      return canUndo;
    }

    public bool CanRedo()
    {
      return Mementos.Count >= MinimumMementoCountForUndo;
    }

    public void Clear()
    {
      Mementos.Clear();
      RedoStack.Clear();
    }
  }
}
