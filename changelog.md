# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.0.0-preview.18](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.18) - 2024-08-08  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/43?closed=1)  
    

### Removed

- Remove table ([#153](https://github.com/unity-game-framework/ugf-runtimetools/issues/153))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview.10` version.
    - Remove _Table_ related code.

## [3.0.0-preview.17](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.17) - 2024-05-19  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/42?closed=1)  
    

### Added

- Add table dropdown entry asset type cache ([#150](https://github.com/unity-game-framework/ugf-runtimetools/issues/150))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview.9` version.
    - Add `TableEditorUtility.TryGetEntryNameFromCache()` method to get entry names for specific table type.
    - Change `TableEntryDropdownDrawer` to display entry names for target table type only.

## [3.0.0-preview.16](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.16) - 2024-05-09  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/41?closed=1)  
    

### Changed

- Change table tree options creation ([#148](https://github.com/unity-game-framework/ugf-runtimetools/issues/148))  
    - Add `TableTreeWindow.CreateOptions()` protected method to create default instance of `TableTreeOptions`.
    - Remove `TableTreeWindow.OnCreateDrawer()` and `SetTarget()` overload methods with `TableTreeOptions` as parameter.
    - Remove `TableTreeEditorUtility.ShowWindow()` overload methods with `TableTreeOptions` as parameter, options created by window itself.

## [3.0.0-preview.15](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.15) - 2024-04-22  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/40?closed=1)  
    

### Added

- Add table drawer with children support ([#144](https://github.com/unity-game-framework/ugf-runtimetools/issues/144))  
    - Add `TableChildrenDrawer` class as drawer for table with entries with children.

## [3.0.0-preview.14](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.14) - 2024-04-21  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/39?closed=1)  
    

### Fixed

- Fix table drawer missing has selected property ([#141](https://github.com/unity-game-framework/ugf-runtimetools/issues/141))  
    - Add `TableDrawer.HasSelected` property to check whether drawer has selected index.
- Fix table try get as non generic argument ([#140](https://github.com/unity-game-framework/ugf-runtimetools/issues/140))  
    - Fix `Table` class generic get methods to be defined with class generic argument instead of interface.

## [3.0.0-preview.13](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.13) - 2024-04-07  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/38?closed=1)  
    

### Changed

- Update dependencies ([#137](https://github.com/unity-game-framework/ugf-runtimetools/issues/137))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview.7` version.

## [3.0.0-preview.12](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.12) - 2024-04-07  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/37?closed=1)  
    

### Added

- Add table add and remove methods ([#133](https://github.com/unity-game-framework/ugf-runtimetools/issues/133))  
    - Add `ITable.Add()`, `ITable.Remove()` and related overloads to add or remove entries from table.
- Add table tree reload ([#132](https://github.com/unity-game-framework/ugf-runtimetools/issues/132))  
    - Add `TableTreeDrawer.Apply()` and `Reload()` methods to control data.
    - Add _Table Tree Reload_ toolbar button and context menu.
- Add table tree header tooltip ([#131](https://github.com/unity-game-framework/ugf-runtimetools/issues/131))  
    - Add _Table Tree Header_ tooltip display for each column with display and property name.

## [3.0.0-preview.11](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.11) - 2024-04-05  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/36?closed=1)  
    

### Fixed

- Fix table children add button with selection ([#128](https://github.com/unity-game-framework/ugf-runtimetools/issues/128))  
    - Add _Table Tree Drawer_ multiple selection duplicate and paste.
    - Fix _Table Tree Drawer_ add entry and child buttons.

## [3.0.0-preview.10](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.10) - 2024-04-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/35?closed=1)  
    

### Fixed

- Fix table tree window attribute type check ([#126](https://github.com/unity-game-framework/ugf-runtimetools/issues/126))  
    - Fix `TableTreeEditorUtility.GetWindowType()` method to property check for asset type.

## [3.0.0-preview.9](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.9) - 2024-04-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/34?closed=1)  
    

### Added

- Add table tree extensibility support ([#124](https://github.com/unity-game-framework/ugf-runtimetools/issues/124))  
    - Add _Table Tree State_ and _Clipboard_ public _APIs_.
    - Add `TableTreeWindowAttribute` class as attribute which can be used to define custom _Table Tree Window_ for specific table asset type.
    - Change `TableTreeDrawer` class to be more extensible.

## [3.0.0-preview.8](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.8) - 2024-04-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/33?closed=1)  
    

### Added

- Add table paste override ([#122](https://github.com/unity-game-framework/ugf-runtimetools/issues/122))  
    - Add _Table Tree_ context menu to paste values for all columns of the selected items.
- Add table entry dropdown value copy ([#118](https://github.com/unity-game-framework/ugf-runtimetools/issues/118))  
    - Add `TableEntryDropdownDrawer` dropdown reference value copy button.
- Add table words match search ([#117](https://github.com/unity-game-framework/ugf-runtimetools/issues/117))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview.6` version.
    - Change `TableTreeColumnSearcher` class to support search by multiple words.
- Add table tree multiple values change support ([#111](https://github.com/unity-game-framework/ugf-runtimetools/issues/111))  
    - Add _Table Tree_ copy and paste values for specific column.
    - Add _Table Tree_ context menu.

## [3.0.0-preview.7](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.7) - 2024-02-12  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/32?closed=1)  
    

### Fixed

- Fix table tree drawer duplicate entry ([#115](https://github.com/unity-game-framework/ugf-runtimetools/issues/115))  
    - Fix `TableTreeDrawer` class add and duplicate behavior.

## [3.0.0-preview.6](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.6) - 2024-02-12  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/31?closed=1)  
    

### Added

- Add table entry dropdown available assets menu ([#112](https://github.com/unity-game-framework/ugf-runtimetools/issues/112))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview.2` version.
    - Add `TableEntryDropdownDrawer` class as drawer for table entry dropdown.
    - Add `TableEntryDropdownDrawer` class table button to display available tables when no entry selected.
- Add table importer ([#90](https://github.com/unity-game-framework/ugf-runtimetools/issues/90))  
    - Add `TableAssetImporter` and `TableAssetImporter<T>` abstract classes as default asset importer implementation.
    - Add `TableAssetImporterEditor` class to implement table importer inspector editor.

## [3.0.0-preview.5](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.5) - 2024-01-14  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/30?closed=1)  
    

### Added

- Add table entry dropdown open table window button ([#109](https://github.com/unity-game-framework/ugf-runtimetools/issues/109))  
    - Add `TableTreeEditorUtility.ShowWindow()` method argument to focus specific item after window opened.
    - Add `TableTreeView.TryGetItemByEntryId()` method to find item by entry id.
    - Add `TableTreeView.TryFocusAtItem()` method to focus item by entry id.
    - Change `TableEntryDropdownAttribute` attribute drawing to display table selection dropdown which can be used to open specific table.

## [3.0.0-preview.4](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.4) - 2024-01-11  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/29?closed=1)  
    

### Added

- Add table tree drawer preference saving inside of user settings ([#96](https://github.com/unity-game-framework/ugf-runtimetools/issues/96))  
    - Update dependencies: add `com.ugf.customsettings` of `3.4.1` version.
    - Add _Table Tree Settings_ user preference file for state and clipboard data.
    - Add `TableTreeEditorUtility.CreateState()` method to create state from table tree options.
    - Add `TableTreeView.State` property to get access to the tree view state.

### Fixed

- Fix item deletion without children ([#104](https://github.com/unity-game-framework/ugf-runtimetools/issues/104))  
    - Fix `TableTreeView.GetChildrenParentSelection()` method to property check for entry type.

## [3.0.0-preview.3](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.3) - 2024-01-11  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/28?closed=1)  
    

### Added

- Add table entry name dynamic cache ([#98](https://github.com/unity-game-framework/ugf-runtimetools/issues/98))  
    - Add `TableEntryDropdownPropertyDrawer` class to display dropdown tooltip with all known entry tables and names.
    - Change `TableEntryCache` internal class to store multiple names per entry.

## [3.0.0-preview.2](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.2) - 2024-01-10  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/27?closed=1)  
    

### Added

- Add table tree children array size change ([#95](https://github.com/unity-game-framework/ugf-runtimetools/issues/95))  
    - Add `TableTreeDrawer.DrawRowCellValue` event which can be used override cell value drawing.
    - Add `TableTreeOptions.` property to setup row height.
    - Add `TableTreeViewItem.PropertyChildrenSize` property to access to the children array size property.
    - Change `TableTreeDrawer.OnDrawRowCellChildren()` method to allow array size editing.

### Removed

- Remove table tree locked ids ([#97](https://github.com/unity-game-framework/ugf-runtimetools/issues/97))  
    - Remove `TableDrawer.UnlockIds` and `TableTreeDrawer.UnlockIds` properties and updated GUI.

## [3.0.0-preview.1](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview.1) - 2024-01-10  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/26?closed=1)  
    

### Added

- Add table display as tree view ([#89](https://github.com/unity-game-framework/ugf-runtimetools/issues/89))  
    - Add `TableTreeDrawer` and related classes to draw table data as table tree view.

## [3.0.0-preview](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/3.0.0-preview) - 2023-11-18  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/25?closed=1)  
    

### Added

- Add table drawer override event for entry draw ([#84](https://github.com/unity-game-framework/ugf-runtimetools/issues/84))  
    - Update dependencies: `com.ugf.editortools` to `3.0.0-preview` version.
    - Update package _Unity_ version to `2023.2`.
    - Update package registry to _UPM Hub_.
    - Add `TableDrawer.Added`, `Removing`, `Selected`, `Deselecting` and `MenuOpening` events which can be used instead of virtual methods.
    - Add `TableDrawer.DrawingEntry`, `DrawingEntryHeader` and `DrawingEntryProperties` events which can be used instead of virtual methods to override drawing.

### Removed

- Remove deprecated code ([#87](https://github.com/unity-game-framework/ugf-runtimetools/issues/87))  
    - Remove _Code_ marked with obsolete attributes.
- Remove collection extensions ([#76](https://github.com/unity-game-framework/ugf-runtimetools/issues/76))  
    - Remove _Collections_ extension methods and related classes.

## [2.19.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.19.0) - 2023-06-17  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/24?closed=1)  
    

### Added

- Add table get by name methods ([#79](https://github.com/unity-game-framework/ugf-runtimetools/issues/79))  
    - Add `ITable.TryGetByName()` method and overloads used to get entry by name.

## [2.18.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.18.0) - 2022-11-24  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/23?closed=1)  
    

### Added

- Add optional generic structure ([#77](https://github.com/unity-game-framework/ugf-runtimetools/issues/77))  
    - Add `Optional<T>` readonly structure to define optional value.

## [2.17.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.17.0) - 2022-10-29  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/22?closed=1)  
    

### Added

- Add behaviour enable scope ([#74](https://github.com/unity-game-framework/ugf-runtimetools/issues/74))  
    - Add `BehaviourEnableScope` structure as scope to enable specified behaviour.

## [2.16.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.16.0) - 2022-09-12  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/21?closed=1)  
    

### Added

- Add validation labels ([#72](https://github.com/unity-game-framework/ugf-runtimetools/issues/72))  
    - Add `ValidateAttribute.Label` property to specify validation label.
    - Add `ValidateReport.HasAnyValid()` and `HasAnyInvalid()` methods to determine whether results contains any specific results.
    - Add `ValidateReport.GetByMemberResults()` and `TryGetByMemberResults()` methods to get results by the specified member name.
    - Add `ValidateMemberResult.Label` property to specify label of validation result.
    - Deprecate `ValidateUtility.Validate()`, `ValidateFields()` and `ValidateProperties()` method overloads with `all` argument, validation made for all members by default.

## [2.15.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.15.0) - 2022-08-07  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/20?closed=1)  
    

### Added

- Add table draw default header ([#70](https://github.com/unity-game-framework/ugf-runtimetools/issues/70))  
    - Add `TableDrawer.DrawEntryDefault()`, `DrawEntryPropertiesHeader()` and `DrawEntryProperties()` methods to draw default inspector for table entry.

## [2.14.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.14.0) - 2022-08-06  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/19?closed=1)  
    

### Added

- Add not null validate attribute ([#67](https://github.com/unity-game-framework/ugf-runtimetools/issues/67))  
    - Add `ValidateNotNullAttribute` attribute class used to validate whether member value is not null.
- Add validate attribute non nested option ([#66](https://github.com/unity-game-framework/ugf-runtimetools/issues/66))  
    - Add `ValidateAttribute.ValidateMembers` property used to determine whether to validate members of target.
    - Change all value validate attributes to don't use default validation of the members.

## [2.13.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.13.0) - 2022-08-05  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/18?closed=1)  
    

### Added

- Add validate no empty attribute ([#64](https://github.com/unity-game-framework/ugf-runtimetools/issues/64))  
    - Add `ValidateNotEmptyAttribute` attribute class used to validate whether value is not empty collection.

## [2.12.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.12.0) - 2022-08-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/17?closed=1)  
    

### Fixed

- Fix validate attribute arguments ([#62](https://github.com/unity-game-framework/ugf-runtimetools/issues/62))  
    - Add `ValidateNotDefaultAttribute` attribute class used to validate field or property whether its value is not default value.
    - Fix `ValidateMaxAttribute`, `ValidateMinAttribute` and `ValidateRangeAttribute` classes to accept values.

## [2.11.0](https://github.com/unity-game-framework/ugf-runtimetools/releases/tag/2.11.0) - 2022-08-04  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-runtimetools/milestone/16?closed=1)  
    

### Added

- Add data validation attributes ([#60](https://github.com/unity-game-framework/ugf-runtimetools/issues/60))  
    - Update dependencies: `com.ugf.editortools` to `2.11.0` version.
    - Add `ValidateAttribute` attribute class used to validate field or property whether it has value.
    - Add `ValidateMatchAttribute` attribute class used to validate field or property whether it match specified regular expression.
    - Add `ValidateMaxAttribute` attribute class used to validate field or property whether its value less or equal to specified value.
    - Add `ValidateMinAttribute` attribute class used to validate field or property whether its value greater or equal to specified value.
    - Add `ValidateNotAttribute` attribute class used to validate field or property whether it not equal to specified value.
    - Add `ValidateOneOfAttribute` attribute class used to validate field or property whether its value one of the specified values.
    - Add `ValidateRangeAttribute` attribute class used to validate field or property whether its value in range of the specified values.
    - Add `ValidateUtility` class with methods to validate fields, properties or entire specified object with report or exception.
    - Add `CollectionsUtility.GetCount()` method to get count from `IEnumerable` object

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


