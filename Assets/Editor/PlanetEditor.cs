using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;
    
    private Editor _shapeEditor;
    private Editor _colorEditor;
    
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                this._planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            this._planet.GeneratePlanet();
        }

        this.DrawSettingsEditor(this._planet.shapeSettings, this._planet.OnShapeSettingsUpdated, ref this._planet.isShapeSettingsFoldout, ref this._shapeEditor);
        this.DrawSettingsEditor(this._planet.colorSettings, this._planet.OnColorSettingsUpdated, ref this._planet.isColorSettingsFoldout, ref this._colorEditor);
    }

    void DrawSettingsEditor(Object settings, 
        System.Action onSettingsUpdated, 
        ref bool isFoldout,
        ref Editor editor
        )
    {
        if (settings == null) return;
        isFoldout = EditorGUILayout.InspectorTitlebar(isFoldout, settings);
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            if (!isFoldout) return;
            // Only create editor if the system has to
            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();
            if (check.changed && onSettingsUpdated!=null)
            {
                onSettingsUpdated();
            }
        }
    }

    private void OnEnable()
    {
        this._planet = (Planet)this.target;
        
    }
}
