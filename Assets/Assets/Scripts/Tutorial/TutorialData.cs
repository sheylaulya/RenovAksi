using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tutorial/Tutorial Data")]
public class TutorialData : ScriptableObject
{
    public string tutorialName;
    public List<TutorialStepData> steps;
}