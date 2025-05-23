using UnityEngine;
using CesiumForUnity;
using UnityEngine.Splines;
using System.Collections.Generic;
using System;

[ExecuteAlways]
public class CesiumDynamicCircleCrop : MonoBehaviour
{
    [Header("Circle Crop Settings")]
    public double centerLatitude = 59.3293;
    public double centerLongitude = 18.0686;
    public double centerHeight = 1.2;
    public double radiusMeters = 500;
    [Range(6, 64)]
    public int numberOfPoints = 24;

    private CesiumCartographicPolygon polygon;
    private SplineContainer splineContainer;

    void Awake()
    {
        polygon = GetComponent<CesiumCartographicPolygon>();
        splineContainer = GetComponent<SplineContainer>();
        if (polygon == null)
            Debug.LogError("No CesiumCartographicPolygon component found on this GameObject!");
        if (splineContainer == null)
            Debug.LogError("No SplineContainer component found on this GameObject!");
    }

    void OnValidate() { UpdateCircle(); }
    void Start() { UpdateCircle(); }

    public void UpdateCircle()
    {
        if (polygon == null || splineContainer == null)
            return;

        // Build the spline points (lat, lon, height)
        var knots = new List<BezierKnot>();

        double earthRadius = 6378137.0; // WGS84
        double latRad = centerLatitude * Math.PI / 180.0;

        for (int i = 0; i < numberOfPoints; i++)
        {
            double angle = 2.0 * Math.PI * i / numberOfPoints;
            double dx = radiusMeters * Math.Cos(angle);
            double dy = radiusMeters * Math.Sin(angle);

            double dLat = (dx / earthRadius) * (180.0 / Math.PI);
            double dLon = (dy / (earthRadius * Math.Cos(latRad))) * (180.0 / Math.PI);

            double lat = centerLatitude + dLat;
            double lon = centerLongitude + dLon;

            knots.Add(new BezierKnot(new Vector3((float)lat, (float)lon, (float)centerHeight)));
        }

        var spline = new Spline();
        foreach (var knot in knots)
            spline.Add(knot);
        spline.Closed = true;

        // Remove all splines in the container
        while (splineContainer.Splines.Count > 0)
        {
            splineContainer.RemoveSpline(splineContainer.Splines[0]);
        }

        // Add new spline
        splineContainer.AddSpline(spline);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(splineContainer);
        UnityEditor.EditorUtility.SetDirty(polygon);
#endif
    }


    public void SetCenter(double newLat, double newLon)
    {
        centerLatitude = newLat;
        centerLongitude = newLon;
        UpdateCircle();
    }

    public void SetRadius(double newRadiusMeters)
    {
        radiusMeters = newRadiusMeters;
        UpdateCircle();
    }
}
