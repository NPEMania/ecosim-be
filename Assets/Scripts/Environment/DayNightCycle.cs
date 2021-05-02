using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour {

    //60x s: 24h

    public Light directionalLight;
    public LightingPreset preset;
    public float TimeOfDay {
        get {return timeOfDay;}
        set {}
    }
    [SerializeField] [Range(0, 24)] private float timeOfDay;
    public float dayEquivalentInMinutes = 1f;
    [HideInInspector] public float dayRate;

    void Awake() {
        dayRate = 24f / (60f * dayEquivalentInMinutes);
    }

    private void Update() {
        if (Application.isPlaying) {
            timeOfDay = (timeOfDay + Time.deltaTime * dayRate) % 24f;
            UpdateLighting(timeOfDay / 24f);
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
