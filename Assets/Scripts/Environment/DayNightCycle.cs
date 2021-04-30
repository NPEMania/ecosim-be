using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour {

    public Light directionalLight;
    public LightingPreset preset;
    public float TimeOfDay {
        get {return timeOfDay;}
        set {}
    }
    [SerializeField] [Range(0, 24)] private float timeOfDay;

    void Start() {
        
    }

    private void Update() {
        if (Application.isPlaying) {
            timeOfDay = (timeOfDay + Time.deltaTime) % 24;
            UpdateLighting(timeOfDay / 24);
        }
    }

    private void UpdateLighting(float timePercent) {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        if (directionalLight != null) {
            directionalLight.color = preset.directionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(timePercent * 360 - 90, 170, 0));
        }
    }
}
