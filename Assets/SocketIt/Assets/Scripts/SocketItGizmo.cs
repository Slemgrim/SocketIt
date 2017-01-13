using UnityEngine;
using UnityEditor;

namespace SocketIt
{
    public class SocketItGizmo
    {
        private static Color ConnectionColor = Color.green;
        private static Color CompositionColor = Color.red;

        public static void DrawConnection(Connection connection)
        {
            Color color = ConnectionColor;

            Handles.color = color;

            if (connection.Connector != null && connection.Connectee != null)
            {
                Vector3 startPoint = connection.Connectee.transform.position;
                Vector3 endPoint = connection.Connector.transform.position;

                Vector3 startTangent = connection.Connectee.transform.position + connection.Connectee.transform.forward;
                Vector3 endTangent = connection.Connector.transform.position + connection.Connector.transform.forward;

                float distance = Vector3.Distance(startPoint, endPoint);

                if (distance > 0.1) {

                    Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, color, null, 5);

                    Bezier bezier = new Bezier(startPoint, endPoint, startTangent, endTangent);

                    DrawArrow(bezier.GetPointAtTime(0.5f), bezier.GetPointAtTime(0.51f));

                    if(distance > 0.5)
                    {
                        DrawArrow(bezier.GetPointAtTime(0.2f), bezier.GetPointAtTime(0.21f));
                        DrawArrow(bezier.GetPointAtTime(0.8f), bezier.GetPointAtTime(0.81f));
                    } 
                    
                }
            }

            Handles.DrawWireDisc(connection.Connector.transform.position, Camera.current.transform.forward, .1f);
            Handles.DrawWireDisc(connection.Connector.transform.position, Camera.current.transform.forward, .08f);
            Handles.DrawSolidDisc(connection.Connectee.transform.position, Camera.current.transform.forward, .1f);
        }

        public static void ConnectModules(GameObject startObject, GameObject endObject)
        {
            Color color = CompositionColor;

            Handles.color = color;

            Vector3 startPoint = startObject.transform.position;
            Vector3 endPoint = endObject.transform.position;

            float distance = Vector3.Distance(startPoint, endPoint);

            if (distance > 0.1)
            {
                Handles.DrawBezier(startPoint, endPoint, startPoint, endPoint, color, null, 5);               
            }
            
            Handles.DrawSolidDisc(startObject.transform.position, Camera.current.transform.forward, .1f);
            Handles.DrawSolidDisc(endObject.transform.position, Camera.current.transform.forward, .1f);
        }

        public static void DrawSocketAngle(Socket socket, float angle)
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

        public static void DrawComposition(Composition composition)
        {
			if (composition.Origin == null) {
				return;
			}

            Vector3 labelPosition = composition.Origin.transform.position + Vector3.up * 2 + Vector3.right * 2;

            Handles.Label(labelPosition, composition.name);

            Vector3 startPoint = composition.Origin.transform.position;
            Handles.color = Color.white;
            Handles.DrawLine(labelPosition, startPoint);

            foreach (Connection connection in composition.Connections)
            {
                ConnectModules(connection.Connector.Module.gameObject, connection.Connectee.Module.gameObject);
                DrawConnection(connection);
            }
        }
    }
}

