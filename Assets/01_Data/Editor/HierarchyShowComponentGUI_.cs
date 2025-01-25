#nullable enable
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Hierarchy�E�B���h�E�ɃR���|�[�l���g�̃A�C�R����\������g���@�\
/// </summary>
/// <remarks>
/// <para>Unity2020.3.5f1�œ���m�F�B</para>
/// <para>
/// <list type="bullet">
/// <item><description>Transform�ȊO�̃R���|�[�l���g�̃A�C�R���\���B</description></item>
/// <item><description>�X�N���v�g�̃A�C�R���͕����t�^����Ă��Ă�1�̂ݕ\���B</description></item>
/// <item><description>�R���|�[�l���g�������ɂȂ��Ă���ꍇ�̓A�C�R���F���������ɂȂ��Ă���B</description></item>
/// <item><description>Hierarchy�E�B���h�E�ŉE�N���b�N�ŕ\������郁�j���[�u�R���|�[�l���g�A�C�R���\���ؑցv�̑I���ŕ\��/��\���̐ؑ։\�B</description></item>
/// </list>
/// </para>
/// </remarks>
public static class ComponentIconDrawerInHierarchy
{
    private const int IconSize = 16;

    private const string MenuPath = "GameObject/�R���|�[�l���g�A�C�R���\���ؑ�";

    private const string ScriptIconName = "cs Script Icon";

    private static readonly Color colorWhenDisabled = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private static Texture? scriptIcon;

    private static bool enabled = true;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        UpdateEnabled();

        /*
         * �r���g�C���A�C�R���̌Ăяo�����͈ȉ����Q�l�ɂ���
         * https://unitylist.com/p/5c3/Unity-editor-icons
         */
#pragma warning disable UNT0023 // Coalescing assignment on Unity objects
        scriptIcon ??= EditorGUIUtility.IconContent(ScriptIconName).image;
#pragma warning restore UNT0023
    }

    [MenuItem(MenuPath, false, 20)]
    private static void ToggleEnabled()
    {
        enabled = !enabled;
        UpdateEnabled();
    }

    private static void UpdateEnabled()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= DisplayIcons;
        if (enabled)
            EditorApplication.hierarchyWindowItemOnGUI += DisplayIcons;
    }

    private static void DisplayIcons(int instanceID, Rect selectionRect)
    {
        // instanceID���I�u�W�F�N�g�Q�Ƃɕϊ�
        if (!(EditorUtility.InstanceIDToObject(instanceID) is GameObject gameObject)) return;

        var pos = selectionRect;
        pos.x = pos.xMax - IconSize;
        pos.width = IconSize;
        pos.height = IconSize;

        // �I�u�W�F�N�g���������Ă���R���|�[�l���g�ꗗ���擾
        var components
            = gameObject
                .GetComponents<Component>()
                .Where(x => !(x is Transform || x is ParticleSystemRenderer))
                .Reverse()
                .ToList();

        var existsScriptIcon = false;
        foreach (var component in components)
        {
            Texture image = AssetPreview.GetMiniThumbnail(component);
            if (image == null) continue;

            // Script�̃A�C�R����1�̂ݕ\��
            if (image == scriptIcon)
            {
                if (existsScriptIcon) continue;
                existsScriptIcon = true;
            }

            // �A�C�R���`��
            DrawIcon(ref pos, image, component.IsEnabled() ? Color.white : colorWhenDisabled);
        }
    }

    private static void DrawIcon(ref Rect pos, Texture image, Color? color = null)
    {
        Color? defaultColor = null;
        if (color.HasValue)
        {
            defaultColor = GUI.color;
            GUI.color = color.Value;
        }

        GUI.DrawTexture(pos, image, ScaleMode.ScaleToFit);
        pos.x -= pos.width;

        if (defaultColor.HasValue)
            GUI.color = defaultColor.Value;
    }

    /// <summary>
    /// �R���|�[�l���g���L�����ǂ������m�F����g�����\�b�h
    /// </summary>
    /// <param name="this">�g���Ώ�</param>
    /// <returns>�R���|�[�l���g���L���ƂȂ��Ă��邩�ǂ���</returns>
    private static bool IsEnabled(this Component @this)
    {
        var property = @this.GetType().GetProperty("enabled", typeof(bool));
        return (bool)(property?.GetValue(@this, null) ?? true);
    }
}
