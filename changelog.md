# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.10.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.10.0) - 2022-07-26  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/15?closed=1)  
    

### Added

- Add table asset ([#58](https://github.com/unity-game-framework/ugf-runtimetools/issues/58))  
    - Add `Table<T>` class as serializable data stored by key.
    - Add `TableAsset` class to define asset with `Table<T>` data.
    - Add `TableDrawer` class to draw table controls in inspector.
    - Add `TableEntryDropdownAttribute` attribute class to mark fields to draw value selection from any table in project.

## [2.9.2](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.9.2) - 2022-07-12  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/14?closed=1)  
    

### Fixed

- Fix missing tryget by type methods for generic provider interface ([#56](https://github.com/unity-game-framework/ugf-runtimetools/issues/56))  
    - Update dependencies: `com.ugf.editortools` to `2.8.0` version.
    - Add `IProvider<T, T>.TryGet()` interface missing methods used to get item by the type.

## [2.9.1](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.9.1) - 2022-07-05  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/13?closed=1)  
    

### Fixed

- Fix provider tryget has missing method overload ([#54](https://github.com/unity-game-framework/ugf-runtimetools/issues/54))  
    - Fix `Provider<T, T>` class has missing `TryGet()` method overloads.

## [2.9.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.9.0) - 2022-07-05  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/12?closed=1)  
    

### Added

- Add component containers ([#50](https://github.com/unity-game-framework/ugf-runtimetools/issues/50))  
    - Update dependencies: add `com.ugf.editortools` of `2.5.0` version.
    - Add `Container` class as implementation of `IContainer` interface.
    - Add `ContainerComponent` class as implementation of `IContainer` interface for gameobject.
- Add scene get component extensions ([#49](https://github.com/unity-game-framework/ugf-runtimetools/issues/49))  
    - Add `SceneExtensions.GetComponent()` methods to get component by specified type from gameobjects in scene.
- Add provider get value by type ([#48](https://github.com/unity-game-framework/ugf-runtimetools/issues/48))  
    - Add `IProvider.TryGet()` and `Get()` methods using specified type of the value.
    - Deprecate `ProviderEntryNotFoundByIdException` exception class.

## [2.8.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.8.0) - 2022-06-06  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/11?closed=1)  
    

### Added

- Add nullable reference container ([#25](https://github.com/unity-game-framework/ugf-runtimetools/issues/25))  
    - Add `ObjectReference<T>` structure to store nullable references with validation.

## [2.7.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.7.0) - 2022-05-07  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/10?closed=1)  
    

### Added

- Add wait async with generic argument ([#45](https://github.com/unity-game-framework/ugf-runtimetools/issues/45))  
    - Update package _Unity_ version to `2021.3`.
    - Add `TaskExtensions.WaitAsync<T>()` method overload.

## [2.6.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.6.0) - 2022-02-20  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/9?closed=1)  
    

### Added

- Add task result ([#43](https://github.com/unity-game-framework/ugf-runtimetools/issues/43))  
    - Add `TaskResult<T>` structure to hold task result which can be empty to support async try get pattern.

## [2.5.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.5.0) - 2021-12-29  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/8?closed=1)  
    

### Added

- Add context value scope ([#42](https://github.com/unity-game-framework/ugf-runtimetools/pull/42))  
    - Update package _Unity_ version to `2021.2`.
    - Add `ContextValueScope` structure to define scope when value available in context.

## [2.4.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.4.0) - 2021-11-23  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/7?closed=1)  
    

### Added

- Add ObjectRelativesProvider ([#39](https://github.com/unity-game-framework/ugf-runtimetools/pull/39))  
    - Add `ObjectRelativesProvider<T>` class to manage relative connection between multiple objects.

## [2.3.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.3.0) - 2021-08-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/6?closed=1)  
    

### Added

- Add extension method for task waiting with coroutines and async operations ([#33](https://github.com/unity-game-framework/ugf-runtimetools/pull/33))  
    - Update package _Unity_ version up to `2020.3`.
    - Add `TaskExtensions.WaitCoroutine()` extension method for `Task` class, used to wait for task inside of coroutine.
    - Add `TaskExtensions.WaitAsync()` extension method for `AsyncOperation` class, used to wait for async operation inside of async/await context.
    - Add `TaskExtensions.WaitAsync()` extension method for `ResourceRequest` class, used to wait for request inside of async/await context.

### Removed

- Remove empty RuntimeUtility class ([#34](https://github.com/unity-game-framework/ugf-runtimetools/pull/34))  
    - Remove `RuntimeUtility` class, because it was empty.

## [2.2.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.2.0) - 2021-07-22  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/5?closed=1)  
    

### Added

- Add text encoding utilities ([#29](https://github.com/unity-game-framework/ugf-runtimetools/pull/29))  
    - Add `EncodingUtility` to get default encoding using `EncodingType` enumeration.

## [2.1.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.1.0) - 2021-07-12  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/4?closed=1)  
    

### Added

- Add utility to get application storage paths ([#27](https://github.com/unity-game-framework/ugf-runtimetools/pull/27))  
    - Add `StorageUtility.GetPath()` to get path depending on `StoragePathType` enumerable.

## [2.0.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.0.0) - 2021-01-23  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/3?closed=1)  
    

### Changed

- Rework provider instance ([#20](https://github.com/unity-game-framework/ugf-runtimetools/pull/20))  
    - Change `ProviderInstance` static class to store multiple providers using `IProvider<Type, IProvider>` provider.

## [1.2.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/1.2.0) - 2021-01-21  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/2?closed=1)  
    

### Added

- Add provider static instance ([#17](https://github.com/unity-game-framework/ugf-runtimetools/pull/17))  
    - Add `ProviderInstance<TProvider>` static generic class to store global providers.

## [1.1.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/1.1.0) - 2021-01-18  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/1?closed=1)  
    

### Added

- Add generic context class ([#14](https://github.com/unity-game-framework/ugf-runtimetools/pull/14))  
    - Add `IContext` interface and `Context` class as default implementation.
- Add Provider class ([#13](https://github.com/unity-game-framework/ugf-runtimetools/pull/13))  
    - Add `IProvider` and `IProvider<TId, TEntry>` interfaces and `Provider<TId, TEntry>` as default implementation.

## [1.0.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/1.0.0) - 2020-10-10  

### Release Notes

- No release notes.


