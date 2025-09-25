using UnityEngine;
using UnityEngine.UI;

public class ResetButton : GridAbstract
{
    public Button resetButton;
    protected override void Start()
    {
        resetButton.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        this.ctrl.blockHandler.ResetButtonClicked();
    }
}
