using UnityEngine;
using UnityEngine.UI;

public class Scr_Slider : MonoBehaviour
{
    private void Start()
    {
        Text valueText = this.transform.GetChild(0).GetComponent<Text>();

        valueText.text = "" + GetComponent<Slider>().value;
    }

    public void SetSliderValue()
    {
        Text valueText = this.transform.GetChild(0).GetComponent<Text>();

        valueText.text = "" + GetComponent<Slider>().value;
    }
}
