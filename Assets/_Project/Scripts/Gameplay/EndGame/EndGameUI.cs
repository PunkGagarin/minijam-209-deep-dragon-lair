using _Project.Scripts.Audio.Domain;
using _Project.Scripts.Infrastructure.GameStates;
using _Project.Scripts.Infrastructure.GameStates.States;
using _Project.Scripts.Utils;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace _Project.Scripts.Gameplay.EndGame
{
    public class EndGameUI : ContentUi
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        [Inject] private GameStateMachine _stateMachine;
        [Inject] private AudioService _audio;

        private void Awake()
        {
            Hide();

            _restartButton.onClick.AddListener(OnRestart);
            _menuButton.onClick.AddListener(OnMenu);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestart);
            _menuButton.onClick.RemoveListener(OnMenu);
        }

        private void OnRestart()
        {
            _audio.PlaySound(Sounds.buttonClick);
            _stateMachine.Enter<LoadGameplayState>();
            _audio.PlayMusic(Sounds.bgm.ToString());
        }

        private void OnMenu()
        {
            _audio.PlaySound(Sounds.buttonClick);
            _stateMachine.Enter<MainMenuState>();
            _audio.PlayMusic(Sounds.bgm.ToString());
        }
    }
}
