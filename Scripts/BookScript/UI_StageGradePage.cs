using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageGradePage : MonoBehaviour
{
    [Header("LEFT PAGE")]
    public Image gradeImage;
    public Image gradeImageOutline;

    // Start is called before the first frame update
    private void OnEnable()
    {
        UpdateImageValues();
    }
    public void UpdateImageValues()
    {
        StageManager.Instance.sumStageScoreGrade();
        int index = 12;

        switch (StageManager.Instance.stageCurrGrade)
        {
            case "SSS": index = 0;  break;
            case "SS":  index = 2;  break;
            case "S":   index = 4;  break;
            case "A":   index = 6;  break;
            case "B":   index = 8;  break;
            case "C":   index = 10; break;
            case "D":   index = 12; break;
        }

        gradeImage.sprite = StageManager.Instance.gradeImage[index];
        gradeImageOutline.sprite = StageManager.Instance.gradeImage[index+1];
    }
}
