using UnityEngine;

namespace GamePlay.Script
{
    public class ControlScript : MonoBehaviour
    {
        public static bool isPause;
        public bool isCount;
        public bool First;
        private float? previousRotate = null;
        public float allRotation;

        private void Start()
        {
            isPause = false;
            First = true;
        }

        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (isPause) return;
            var position = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
            var rotation = Mathf.Atan(position.y / position.x) * 180 / Mathf.PI;
            if (position.x < 0)
                rotation += 180;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            if (isCount)
                CountRotation();
        }


        private void CountRotation()
        {
            var rotation = transform.rotation.eulerAngles.z;

            if (First)
            {
                if (previousRotate == null)
                    previousRotate = rotation;
                First = false;
            }
            else
            {
                var delta = rotation - (float)previousRotate;
                if (delta > 180)
                    delta -= 360;
                if (delta < -180)
                    delta += 360;
                allRotation += delta;
                previousRotate = rotation;
            }
        }
    }
}