# Contractor Simulator — Test Infrastructure

Automated tests live under `Assets/Tests/`. Gameplay code under `Assets/_Project/` is compiled into `ContractorSimulator.Runtime` and referenced by test assemblies.

## Layout

| Folder | Assembly | Purpose |
|--------|----------|---------|
| `Shared/` | `ContractorSimulator.Tests.Shared` | Helpers, constants, builders, fakes (no test cases) |
| `EditMode/` | `ContractorSimulator.Tests.EditMode` | Fast unit tests (no Play Mode) |
| `PlayMode/` | `ContractorSimulator.Tests.PlayMode` | Integration tests (MonoBehaviour, physics, scenes) |
| `PlayMode/Networking/` | (same) | Multiplayer / Netcode tests (host + client flows) |
| `PlayMode/Scenes/` | (same) | Scene load smoke and vertical-slice checks |
| `Performance/` | `ContractorSimulator.Tests.Performance` | Benchmarks and regression budgets |

## Running tests

### Unity Editor

1. **Window → General → Test Runner** (or **Window → Analysis → Test Runner**).
2. **EditMode** tab — unit tests (milliseconds).
3. **PlayMode** tab — integration tests (seconds; enters Play Mode).
4. **Performance** tab — after adding performance test cases.

Filter by category: `Unit`, `Integration`, `Networking`, `Scene`, `Performance`, `Slow`.

### "No tests to show"

If the Test Runner is empty:

1. Check the **Console** for compile errors (red). Tests will not appear until all test assemblies compile.
2. Confirm **EditMode** vs **PlayMode** tab matches the tests you expect.
3. Clear any **search/filter** in the Test Runner window.
4. After pulling changes, let Unity reimport (watch the progress bar finish).
5. Common fixes: `StarterAssets.asmdef` exists, `ContractorSimulator.Runtime` references Netcode/InputSystem/TMP/StarterAssets, manifest uses `com.unity.test-framework.performance` (not `com.unity.performance-testing`).

### Command line (CI / local)

From repo root, with [Unity Test Runner](https://docs.unity3d.com/Packages/com.unity.test-framework@1.6/manual/reference-command-line.html):

```bash
Unity -batchmode -quit -projectPath ConstructionSimulator \
  -runTests -testPlatform editmode -testResults TestResults/editmode-results.xml

Unity -batchmode -quit -projectPath ConstructionSimulator \
  -runTests -testPlatform playmode -testResults TestResults/playmode-results.xml
```

GitHub Actions workflow: `.github/workflows/unity-tests.yml` (requires Unity license secrets — see workflow comments).

## Conventions

- **EditMode**: pure logic, serialization, path helpers, data validation. No `yield`, no `MonoBehaviour` lifecycle unless using test doubles.
- **PlayMode**: `UnityTest` + `IEnumerator`, scene loads, interaction, save I/O with real `Application.persistentDataPath`.
- **Networking**: use Multiplayer Play Mode or programmatic host/client when Netcode scenes are in build settings; mark slow tests `[Category("Slow")]`.
- **Isolation**: use `TestFileUtility` for temp save paths under `persistentDataPath/Tests/` — never overwrite `save_data.json` in tests.
- **Naming**: `{TypeUnderTest}Tests.cs`, method `MethodName_ExpectedBehavior_WhenCondition`.

## Scene tests

Scenes under test must be listed in **File → Build Profiles / Build Settings**. Constants: `TestSceneNames`. Scene tests call `Assert.Ignore` when a scene is not in the build list.

## Coverage

Enable **Code Coverage** package in Test Runner (EditMode) or batch:

```bash
Unity -batchmode -quit -projectPath ConstructionSimulator \
  -runTests -testPlatform editmode \
  -enableCodeCoverage -coverageResultsPath CodeCoverage/
```

## Adding tests for new systems

1. Implement gameplay code in `Assets/_Project/` (Runtime assembly).
2. Add EditMode tests for logic and data first.
3. Add PlayMode tests when behavior needs Unity runtime or Netcode.
4. Add a category attribute: `[Category(TestCategories.Unit)]`.
