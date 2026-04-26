using System;
using System.Collections.Generic;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Stages
{
    [CreateAssetMenu(fileName = "StageProgressionConfig", menuName = "Game Resources/Configs/StageProgressionConfig")]
    public class StageProgressionConfig : ScriptableObject
    {
        [field: SerializeField] public List<StageDefinition> Stages { get; private set; } = new();
    }

    [Serializable]
    public class StageDefinition
    {
        [field: SerializeField] public int RequiredGold { get; private set; } = 1000;
        [field: SerializeField] public bool IsFinalStage { get; private set; }
        [field: SerializeField] public StageCameraSettings CameraSettings { get; private set; } = new();
    }

    [Serializable]
    public class StageCameraSettings
    {
        [field: SerializeField] public float TargetOrthographicSize { get; private set; } = 5f;
        [field: SerializeField] public float Duration { get; private set; } = 1.5f;
        [field: SerializeField] public AnimationCurve Easing { get; private set; } = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}
