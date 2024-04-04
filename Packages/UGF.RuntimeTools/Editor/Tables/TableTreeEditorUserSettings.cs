using UGF.CustomSettings.Editor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUserSettings
    {
        public static CustomSettingsEditorPackage<TableTreeEditorUserSettingsData> Settings { get; } = new CustomSettingsEditorPackage<TableTreeEditorUserSettingsData>
        (
            "UGF.RuntimeTools",
            nameof(TableTreeEditorUserSettings),
            CustomSettingsEditorUtility.DEFAULT_PACKAGE_EXTERNAL_USER_FOLDER
        );
    }
}
