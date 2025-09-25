using UnityEngine;
using UnityEngine.UI;

public class ShuffleButton : GridAbstract
{
    public Button shuffleButton;
    protected override void Start()
    {
        shuffleButton.onClick.AddListener(OnShuffleButtonClick);
    }
    public void OnShuffleButtonClick()
    {
        this.ctrl.blockHandler.Shuffle();
    }
}
