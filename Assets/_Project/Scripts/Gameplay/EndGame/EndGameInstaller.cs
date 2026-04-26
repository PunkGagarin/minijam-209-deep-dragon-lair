using UnityEngine;

using Zenject;

using _Project.Scripts.Gameplay.Stages;

namespace _Project.Scripts.Gameplay.EndGame
{
    public class EndGameInstaller : MonoInstaller
    {
        [SerializeField] private EndGameUI _ui;
        [SerializeField] private EndGameCameraReveal _cameraReveal;
        [SerializeField] private FinalStageSequence _finalStageSequence;
        [SerializeField] private StageProgressionConfig _stageProgressionConfig;

        public override void InstallBindings()
        {
            Container.Bind<EndGameUI>()
                .FromInstance(_ui).AsSingle();
            Container.QueueForInject(_ui);

            Container.Bind<EndGameCameraReveal>()
                .FromInstance(_cameraReveal).AsSingle();
            Container.QueueForInject(_cameraReveal);

            Container.Bind<FinalStageSequence>()
                .FromInstance(_finalStageSequence).AsSingle();
            Container.QueueForInject(_finalStageSequence);

            Container.Bind<StageProgressionConfig>()
                .FromInstance(_stageProgressionConfig).AsSingle();

            Container.BindInterfacesAndSelfTo<StageProgressionController>()
                .AsSingle().NonLazy();
        }
    }
}
