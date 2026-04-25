# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Mini Jam #209 entry built with Unity 6.0.4 + URP. Game jam project — infrastructure complete, gameplay domain is the active development area.

## Build & Run

This is a Unity project — there is no CLI build command. Open in Unity 6.0.4 and press Play. Scenes must be loaded in order: `Bootstrap → MainMenu → Gameplay` (configured in Build Settings).

No automated tests exist. Manual validation is done by running the game in the Editor.

## Architecture

### Layer structure

```
Assets/_Project/Scripts/
├── Gameplay/         # Game features (one subfolder per feature)
│   ├── Dragon/       # Dragon entity and movement
│   ├── Units/        # Unit logic
│   └── Currencies/   # Currency system
├── Infrastructure/   # App lifecycle: GameRunner, StateMachine, States, SceneManagement
├── GameplayData/     # ScriptableObject repositories and base Definitions
├── Audio/            # Audio subsystem: Domain/, Data/, View/
├── Localization/     # EN/RU via XML
├── MainMenu/         # Main menu UI
└── Utils/            # Coroutine helper, Pause service, Editor tools
```

### Core patterns

**State Machine** controls game flow. States live in `Infrastructure/GameStates/States/`. Flow: `BootstrapState → MainMenuState → LoadGameplayState → GameplayState`.

Each state implements `IState, IGameState`. Register states in `ProjectInstaller`, transition with `_stateMachine.Enter<SomeState>()`.

**Zenject DI** wires everything. No `new SomeService()` — everything is bound in installers and injected via `[Inject]` field.

Three installer types:
- `MonoInstaller` — scene-bound, serialized fields for Unity references
- `ScriptableObjectInstaller` — asset-based config (e.g. `GlobalConfigInstaller`)
- Installers in `ProjectContext` apply project-wide; scene `GameObjectContext`/`SceneContext` are local

**MVP** used for UI: `Model` (data + PlayerPrefs), `Presenter` (`IInitializable`, logic), `View` (MonoBehaviour, UI only). See `Audio/` for the canonical example.

**Repositories** — inherit `Repository<T> : ScriptableObject` where `T : Definition` for any game data. Bind with `FromInstance()` in an installer.

**UniTask** for all async and time-based operations. Coroutines are **never** used — including `WaitForSeconds`, `WaitUntil`, and similar. Use `UniTask.Delay`, `UniTask.WaitUntil`, `async UniTaskVoid` instead. This applies to all code: MonoBehaviour components, services, states.

**Component-based approach** — вся игровая логика (не UI) строится на `MonoBehaviour`-компонентах. Одна ответственность = один компонент. Зависимости между игровыми компонентами через `[SerializeField]` или `GetComponent`, не через Zenject. Пример: `Gameplay/Dragon/` — `Dragon.cs` (сущность) и `Movement.cs` (движение) разделены.

### Adding new things

**New gameplay feature:**
1. Создать папку `Gameplay/FeatureName/`
2. Каждое игровое поведение — отдельный `MonoBehaviour` в этой папке
3. Если нужны данные (ScriptableObject) — создать папку `GameplayData/Definitions/FeatureName/`

**New service:**
1. Define interface in `Domain/`
2. Implement class
3. Bind in installer: `Container.BindInterfacesAndSelfTo<MyService>().AsSingle()`
4. Inject via `[Inject]`

**New game state:**
1. `public class MyState : IState, IGameState`
2. Implement `Enter()` and `Exit()`
3. Add `Container.Bind<IGameState>().To<MyState>().AsSingle()` in `ProjectInstaller`
4. Transition: `_stateMachine.Enter<MyState>()`

**New gameplay data:**
1. `public class MyDef : Definition { }` 
2. `public class MyRepo : Repository<MyDef> { }` with `[CreateAssetMenu]`
3. Bind in `RepositoryInstaller`

## Coding conventions

| Element | Convention |
|---|---|
| Private fields | `_camelCase` |
| Constants | `UPPER_SNAKE_CASE` |
| Interfaces | `IPascalCase` |
| Properties | `PascalCase`, `private set` |
| Namespaces | Mirror directory path |

- `[SerializeField] private` for inspector-exposed fields — never `public` fields
- `[Inject]` fields where possible over constructor
- Prefer `List<T>` over arrays (`T[]`) where possible — including `[SerializeField]` коллекции
- Usings grouped: System → UnityEngine → third-party → project

## Key dependencies

- **Zenject** — DI container (in `Plugins/`)
- **UniTask** — async/await (Cysharp)
- **DOTween** — tweening (loading curtain fade)
- **Cinemachine 2.10.7**, **Input System 1.19.0**, **URP 17.4.0**
