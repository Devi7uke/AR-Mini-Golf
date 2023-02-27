using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public TapToPlaceIndicator tapToPlaceIndicator;
    public BallController[] holes;
    public TextMeshProUGUI strokesText;
    
    void Start(){
        tapToPlaceIndicator = GameObject.FindGameObjectWithTag("AR").GetComponent<TapToPlaceIndicator>();
    }
    public void UpdateStrokeNumber(int num){
        strokesText.SetText("Strokes: " + num);
    }
    public void ResetHole(){
        holes = tapToPlaceIndicator.placedObject.GetComponentsInChildren<BallController>();
        holes[0].ResetHole();
    }
    public void QuitMap(){
        tapToPlaceIndicator.ChangePosition();
    } 

    public void NextHole(int current){
        if(current < 2){
            tapToPlaceIndicator.level = current + 1;
        }else{
            tapToPlaceIndicator.level = 0;
        }
        QuitMap();
    }
}