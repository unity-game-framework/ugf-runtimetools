using System;
using System.Collections.Generic;
using UGF.CustomSettings.Editor;
using UGF.EditorTools.Runtime.IMGUI.Types;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeSettings
    {
        public static CustomSettingsEditorPackage<TableTreeSettingsData> Settings { get; } = new CustomSettingsEditorPackage<TableTreeSettingsData>
        (
            "UGF.RuntimeTools",
            nameof(TableTreeSettings),
            CustomSettingsEditorUtility.DEFAULT_PACKAGE_EXTERNAL_USER_FOLDER
        );

        public static int ClipboardEntriesCount { get { return Settings.GetData().Clipboard.Entries.Count; } }
        public static int ClipboardChildrenCount { get { return Settings.GetData().Clipboard.Children.Count; } }

        public static void Save()
        {
            Settings.SaveSettings();
        }

        public static bool HasState(Type type)
        {
            return TryGetState(type, out _);
        }

        public static bool TryStateRead(Type type, TableTreeViewState state)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (TryGetState(type, out TableTreeViewState targetState))
            {
                string text = EditorJsonUtility.ToJson(targetState);

                EditorJsonUtility.FromJsonOverwrite(text, state);
                return true;
            }

            return false;
        }

        public static void StateWrite(Type type, TableTreeViewState state)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (TryGetState(type, out TableTreeViewState targetState))
            {
                string text = EditorJsonUtility.ToJson(state);

                EditorJsonUtility.FromJsonOverwrite(text, targetState);
            }
            else
            {
                TableTreeSettingsData data = Settings.GetData();

                var stateData = new TableTreeSettingsData.StateData
                {
                    Type = new TypeReference(type.AssemblyQualifiedName),
                    State = new TableTreeViewState()
                };

                string text = EditorJsonUtility.ToJson(state);

                EditorJsonUtility.FromJsonOverwrite(text, stateData.State);

                data.States.Add(stateData);
            }
        }

        public static bool TryGetState(Type type, out TableTreeViewState state)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TableTreeSettingsData data = Settings.GetData();

            for (int i = 0; i < data.States.Count; i++)
            {
                TableTreeSettingsData.StateData stateData = data.States[i];

                if (stateData.Type.TryGet(out Type assetType) && assetType == type)
                {
                    state = stateData.State;
                    return true;
                }
            }

            state = default;
            return false;
        }

        public static bool TryStateReset(Type type, TableTreeOptions options)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (options == null) throw new ArgumentNullException(nameof(options));

            TableTreeSettingsData data = Settings.GetData();

            for (int i = 0; i < data.States.Count; i++)
            {
                TableTreeSettingsData.StateData stateData = data.States[i];

                if (stateData.Type.TryGet(out Type assetType) && assetType == type)
                {
                    stateData.State = TableTreeEditorUtility.CreateState(options);
                    return true;
                }
            }

            return false;
        }

        public static void StateClearAll()
        {
            TableTreeSettingsData data = Settings.GetData();

            data.States.Clear();
        }

        public static bool ClipboardTryMatch(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TableTreeSettingsData data = Settings.GetData();

            return data.Clipboard.Type.HasValue && data.Clipboard.Type.TryGet(out Type value) && value == type;
        }

        public static bool ClipboardHasAny()
        {
            TableTreeSettingsData data = Settings.GetData();

            return data.Clipboard.Entries.Count > 0 || data.Clipboard.Children.Count > 0;
        }

        public static void ClipboardCopyType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TableTreeSettingsData data = Settings.GetData();

            TypeReference reference = data.Clipboard.Type;

            reference.Set(type);

            data.Clipboard.Type = reference;
        }

        public static bool TryClipboardCopyEntries(IReadOnlyList<TableTreeViewItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            return OnTryClipboardCopy(items, Settings.GetData().Clipboard.Entries);
        }

        public static bool TryClipboardCopyChildren(IReadOnlyList<TableTreeViewItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            return OnTryClipboardCopy(items, Settings.GetData().Clipboard.Children);
        }

        public static void ClipboardClear()
        {
            TableTreeSettingsData data = Settings.GetData();

            data.Clipboard.Type.Clear();
            data.Clipboard.Entries.Clear();
            data.Clipboard.Children.Clear();
        }

        private static bool OnTryClipboardCopy(IReadOnlyList<TableTreeViewItem> items, ICollection<object> values)
        {
            for (int i = 0; i < items.Count; i++)
            {
                TableTreeViewItem item = items[i];

                object value;

                try
                {
                    value = item.SerializedProperty.boxedValue;
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Table entry can not be copied.\n{exception}");
                    return false;
                }

                values.Add(value);
            }

            return true;
        }
    }
}
