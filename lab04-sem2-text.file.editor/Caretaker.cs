using System.Collections.Generic;

namespace lab04_sem2_text.file.editor
{
  public class Caretaker
  {
    public int MinimumMementoCountForUndo;

    public Stack<object> _mementos;
    public Stack<object> _redoStack;

    public Caretaker()
    {
      MinimumMementoCountForUndo = 2;
      _mementos = new Stack<object>();
      _redoStack = new Stack<object>();
    }

    public void SaveState(IOriginator originator)
    {
      object currentState;
      currentState = originator.GetMemento();

      _mementos.Push(currentState);
      _redoStack.Clear();
    }

    public void Undo(IOriginator originator)
    {
      if (_mementos.Count >= MinimumMementoCountForUndo) {
        object currentState;
        object previousState;

        currentState = _mementos.Pop();
        _redoStack.Push(currentState);

        previousState = _mementos.Peek();
        originator.SetMemento(previousState);
      }
    }

    public void Redo(IOriginator originator)
    {
      if (_redoStack.Count > 0) {
        object stateToRedo;
        stateToRedo = _redoStack.Pop();

        _mementos.Push(stateToRedo);
        originator.SetMemento(stateToRedo);
      }
    }

    public bool CanUndo()
    {
      bool canUndo;

      if (_mementos.Count >= MinimumMementoCountForUndo) {
        canUndo = true;
      }
      else {
        canUndo = false;
      }

      return canUndo;
    }

    public bool CanRedo()
    {
      bool canRedo;

      if (_redoStack.Count > 0) {
        canRedo = true;
      }
      else {
        canRedo = false;
      }

      return canRedo;
    }

    public void Clear()
    {
      _mementos.Clear();
      _redoStack.Clear();
    }
  }
}
