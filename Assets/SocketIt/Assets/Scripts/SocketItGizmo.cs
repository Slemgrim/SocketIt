using UnityEngine;
using UnityEditor;

namespace SocketIt
{
    public class SocketItGizmo
    {
        private static Color ConnectionColor = Color.green;
        private static Color NodeColor = Color.cyan;

        public static void DrawConnection(Connection connection)
        {
            bool isNode = false;

            Color color = ConnectionColor;
            if(connection.SocketA.Module.GetComponent<NodeModule>() != null && connection.SocketB.Module.GetComponent<NodeModule>() != null)
            {
                color = NodeColor;
                isNode = true;
            }

            Handles.color = color;

            if (connection.SocketA != null && connection.SocketB != null)
            {
                Vector3 startPoint = connection.SocketA.transform.position;
                Vector3 endPoint = connection.SocketB.transform.position;

                Vector3 startTangent = connection.SocketA.transform.position + connection.SocketA.transform.forward;
                Vector3 endTangent = connection.SocketB.transform.position + connection.SocketB.transform.forward;

                float distance = Vector3.Distance(startPoint, endPoint);

                if (distance > 0.1) {

                    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, color, null, 5);

                    Bezier bezier = new Bezier(startPoint, endPoint, startTangent, endTangent);

                    if (isNode)
                    {
                        DrawArrow(bezier.GetPointAtTime(0.5f), bezier.GetPointAtTime(0.51f));

                        if(distance > 0.5)
                        {
                            DrawArrow(bezier.GetPointAtTime(0.2f), bezier.GetPointAtTime(0.21f));
                            DrawArrow(bezier.GetPointAtTime(0.8f), bezier.GetPointAtTime(0.81f));
                        } 
                    }
                }
            }

            Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .1f);
            Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .08f);
            

            if(connection.Initiator == connection.SocketA)
            {
                Handles.DrawSolidDisc(connection.SocketB.transform.position, Vector3.forward, .05f);
            }
            else
            {
                Handles.DrawSolidDisc(connection.SocketA.transform.position, Vector3.forward, .05f);
            }


        }

        public static void DrawSocketAngle(SnapSocket socket, float angle)
        {
            if(angle == 180)
            {
                return;
            }
            Color color = Color.green;
            color.a = 0.05f;
            Handles.color = color;

            Handles.DrawSolidArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, angle, 0.5f);
            Handles.DrawSolidArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, -angle, 0.5f);
            color.a = 1f;
            Handles.color = color;

            Handles.DrawWireArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, angle, 0.5f);
            Handles.DrawWireArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, -angle, 0.5f);
        }

        private static void DrawArrow(Vector3 position, Vector3 target)
        {
            Handles.ConeCap(0, position, Quaternion.LookRotation(target - position), .2f);
        }

        public static void DrawConstruct(Construct construct)
        {
            Vector3 labelPosition = construct.Center.transform.position + Vector3.up * 2 + Vector3.right * 2;

            Handles.Label(labelPosition, construct.name);

            Vector3 startPoint = construct.Center.transform.position;
            Handles.color = Color.white;
            Handles.DrawLine(labelPosition, startPoint);

            foreach (Module module in construct.Modules)
            {
                Vector3 endPoint = module.transform.position;
                Handles.DrawLine(startPoint, endPoint);
            }
        }
    }
}

