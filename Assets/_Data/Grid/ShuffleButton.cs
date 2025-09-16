using UnityEngine;
using UnityEngine.UI;

public class ShuffleButton : GridAbstract
{
    public Button shuffleButton;
    protected override void Start()
    {
        shuffleButton.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        Debug.LogWarning("Button clicked!");
        this.ctrl.blockHandler.Shuffle();
    }
}
