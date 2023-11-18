using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringRenderer : MonoBehaviour
{
    [Header("Render Positions")]
    [SerializeField] private Transform start;
    [SerializeField] private Transform middle;
    [SerializeField] private Transform end;

    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        if(Application.isEditor && !Application.isPlaying) {
            UpdatePositions();
        }
    }

    private void OnEnable() {
        Application.onBeforeRender += UpdatePositions;
    }

    private void OnDisable() {
        Application.onBeforeRender -= UpdatePositions;
    }

    private void UpdatePositions() {
        lineRenderer.SetPositions(new Vector3[] { start.position, middle.position, end.position });
    }
}
