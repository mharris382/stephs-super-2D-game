using UnityEngine;

namespace DefaultNamespace
{
    public class DebugMe : MonoBehaviour
    {
        public string printLine = "Debug";

        public void Print()
        {
            Debug.Log(printLine);
        }
    }
}