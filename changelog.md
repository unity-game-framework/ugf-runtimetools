# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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


