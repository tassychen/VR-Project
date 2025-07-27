//using UnityEditor;
//using UnityEngine;
//using System.Collections;
//using YAProgressBar;

//[CustomEditor(typeof(CircularCurve))]
//public class CircularCurveInspector : Editor
//{
//    private void OnValidate()
//    {
        
//    }

//    private void OnInspectorGUI()
//    {
//        Debug.Log("OnValidate");
//        //The 'Editor' class has a 'target' variable which is set to the object to be drawn when 'OnSceneGUI' is called
//        CircularCurve circle = target as CircularCurve;

//        //To move rotate and csale our line we are to convert local space points of line into world space points
//        Transform handleTransform = circle.transform;
//        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
//        Vector3 p0Tangent;
//        Quaternion q0, q1, qCenter;
//        Vector3 p0 = handleTransform.TransformPoint(circle.GetPoint(0, out q0, out p0Tangent));
//        Vector3 p1 = handleTransform.TransformPoint(circle.GetPoint(1, out q1));
//        Vector3 pCenter = handleTransform.TransformPoint(circle.GetPoint(0.5f, out qCenter));

//        float curveAngle = circle.GetFullAngle();

//        Handles.color = Color.white;
//        Vector3 curvePlaneNormal = Vector3.up;
//        if (circle.CurvePlane == BaseCurve.PlaneAxes.XY)
//        {
//            curvePlaneNormal = circle.transform.forward;
//        }
//        else if (circle.CurvePlane == BaseCurve.PlaneAxes.XZ)
//        {
//            curvePlaneNormal = circle.transform.up;
//            curveAngle = curveAngle;
//        }
//        else if (circle.CurvePlane == BaseCurve.PlaneAxes.ZY)
//        {
//            curvePlaneNormal = circle.transform.right;
//        }
//        Handles.DrawWireArc(circle.transform.position, curvePlaneNormal, p0 - circle.transform.position, curveAngle, circle.Radius);// p0, p1);

//        //To show position handles for our two points and drag points in the scene view
//        //EditorGUI.BeginChangeCheck();
//        //p0 = Handles.DoPositionHandle(p0, Quaternion.FromToRotation(p0 - circle.transform.position, circle.transform.right));
//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    Undo.RecordObject(circle, "Move Point"); //To make it possible to Undo drag operations
//        //    EditorUtility.SetDirty(circle); //To tell Unity that changes were made
//        //    //circle.p0 = handleTransform.InverseTransformPoint(p0);
//        //}

//        //EditorGUI.BeginChangeCheck();
//        //p1 = Handles.DoPositionHandle(p1, handleRotation);
//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    Undo.RecordObject(circle, "Move Point"); //To make it possible to Undo drag operations
//        //    EditorUtility.SetDirty(circle); //To tell Unity that changes were made
//        //    //circle.p1 = handleTransform.InverseTransformPoint(p1);
//        //}
//        //H//andles.ArrowCap(1, circle.transform.position, circle.transform.rotation, circle.Radius);
//        //circle.Radius = Handles.RadiusHandle(circle.transform.rotation, circle.transform.position, circle.Radius);

//        //bool Button(Vector3 position, Quaternion direction, float size, float pickSize, DrawCapFunction capFunc);
//        //Vector3 v3 = Handles.ScaleHandle(circle.transform.localScale, circle.transform.position, circle.transform.rotation, circle.Radius);
//        //
//        pCenter = Handles.FreeMoveHandle(pCenter, Quaternion.FromToRotation(Vector3.right, pCenter -circle.transform.position), HandleUtility.GetHandleSize(pCenter) / 8f, Vector3.zero, Handles.CubeCap);
//        circle.Radius = (pCenter - circle.transform.position).magnitude;

//        float halfSize = HandleUtility.GetHandleSize(p0);
//        circle.StartAngle = Handles.ScaleValueHandle(circle.StartAngle, p0, 
//            Quaternion.FromToRotation(Vector3.forward, -p0Tangent),
//            halfSize * 2, Handles.ConeCap, 0.1f);// + halfSize * (-p0Tangent)

//        Handles.color = Color.red;
//        int sign = circle.FlipNormal ? 1 : -1;
//        if (Handles.Button(pCenter + sign * (pCenter - circle.transform.position).normalized * HandleUtility.GetHandleSize(pCenter) / 4f,
//            Quaternion.FromToRotation(Vector3.forward, sign * pCenter - sign * circle.transform.position),
//            HandleUtility.GetHandleSize(pCenter) / 4f,
//            HandleUtility.GetHandleSize(pCenter) / 5f,
//            Handles.ConeCap))
//        {
//            circle.FlipNormal = !circle.FlipNormal;
//        }
//        //circle.OnValidate();
//        //Debug.Log(circleinside);

//        //HandleUtility.ClosestPointToArc();
//        //public static void CubeCap(int controlID, Vector3 position, Quaternion rotation, float size);
//    }
//}
