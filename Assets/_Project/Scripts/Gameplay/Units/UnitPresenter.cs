using Zenject;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitPresenter : IInitializable
    {
        [Inject] private UnitService _unitService;
        [Inject] private UnitView _unitView;

        public void Initialize()
        {
            _unitService.OnChanged += UpdateView;
            UpdateView();
        }

        private void UpdateView() => _unitView.SetAmount(_unitService.Count);
    }
}
