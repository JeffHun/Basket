using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountorFinder : WebCamera
{
    [SerializeField] FlipMode imgFlip;
    [SerializeField] float threshold = 96.4f;
    [SerializeField] bool isShowProcessingImg = true;
    [SerializeField] float curveAccuracy = 10f;
    [SerializeField] float minArea = 5000f;
    [SerializeField] PolygonCollider2D polygonCollider;

    Mat img;
    Mat processImg = new Mat();
    Point[][] contours;
    HierarchyIndex[] hierarchy;
    Vector2[] vectorList;

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        img = OpenCvSharp.Unity.TextureToMat(input);

        Cv2.Flip(img, img, imgFlip);
        Cv2.CvtColor(img, processImg, ColorConversionCodes.BGR2GRAY);
        Cv2.Threshold(processImg, processImg, threshold, 255, ThresholdTypes.BinaryInv);
        Cv2.FindContours(processImg, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

        polygonCollider.pathCount = 0;
        foreach (Point[] contour in contours)
        {
            Point[] points = Cv2.ApproxPolyDP(contour, curveAccuracy, true);
            var area = Cv2.ContourArea(contour);

            if (area > minArea)
            {
                DrawContour(processImg, new Scalar(127, 127, 127), 2, points);

                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount -1, ToVector2(points));
            }
        }

        if (output == null)
            output = OpenCvSharp.Unity.MatToTexture(isShowProcessingImg ? processImg : img);
        else
            OpenCvSharp.Unity.MatToTexture(isShowProcessingImg ? processImg : img, output);

        return true;
    }

    Vector2[] ToVector2(Point[] points)
    {
        vectorList = new Vector2[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            vectorList[i] = new Vector2(points[i].X, points[i].Y);
        }
        return vectorList;
    }

    private void DrawContour(Mat img, Scalar color, int thickness, Point[] points)
    {
        for(int i = 1; i < points.Length; i++)
        {
            Cv2.Line(img, points[i - 1], points[i], color, thickness);
        }
        Cv2.Line(img, points[points.Length - 1], points[0], color, thickness);
    }
}
