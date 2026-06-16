using UnityEngine;

public class RestartOnEnable : MonoBehaviour
{
    public SelectionDifficulte selectionDifficulte;
    public NoteScroller noteScroller;
    void OnEnable()
    {
        selectionDifficulte.ChoisirHard();
        noteScroller.changeBool();
    }
}
