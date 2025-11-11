using UnityEngine;

namespace GamePlay.Script
{
    public class NoteScript : MonoBehaviour
    {
        public Rigidbody2D myRigidbody2D;
        private float speed = 6;
        private float timeLive = 2;
        private float time;
        private float scale = 0;
        private float positionScale;
        private float targetScale = 1;
        private LogicScript logic;

        void Start()
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            positionScale = Date.RadiusCircle / 2;
            transform.localScale = new Vector3(scale, scale, scale);
            var rotation = GameObject.FindGameObjectWithTag("Center").transform.rotation.eulerAngles.z;
            var rotate = Random.Range(-60, 60);
            var rot = (rotation + rotate) / 180 * Mathf.PI;
            myRigidbody2D.velocity = new Vector2(Mathf.Cos(rot) * speed,
                Mathf.Sin(rot) * speed);
        }

        private void Update()
        {
            time += Time.deltaTime;
            UpdateScale();
            if (time > timeLive)
            {
                logic.ShowMissEffect();
                Destroy(gameObject);
            }
        }

        private void UpdateScale()
        {
            if (scale < targetScale)
            {
                scale = transform.position.magnitude / positionScale;
                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }
}