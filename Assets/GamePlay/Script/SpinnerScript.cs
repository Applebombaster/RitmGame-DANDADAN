using UnityEngine;

namespace GamePlay.Script
{
    public class SpinnerScript : MonoBehaviour
    {
        public Rigidbody2D myRigidbody2D;
        private float speed = 6;
        private float timeLive;
        private float time;
        private float scale = 0;
        private float positionScale;
        private float targetScale = 1;
        public float timeALive;
        public LogicScript logic;

        void Start()
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            timeLive = timeALive * 2;
            positionScale = Date.RadiusCircle / 2;
            transform.localScale = new Vector3(scale, scale, scale);
            var rotation = GameObject.FindGameObjectWithTag("Center").transform.rotation.eulerAngles.z;
            var rotate = Random.Range(-90, 90);
            var rot = (rotation + rotate) / 180 * Mathf.PI;
            myRigidbody2D.velocity = new Vector2(Mathf.Cos(rot) * speed,
                Mathf.Sin(rot) * speed);
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > timeALive)
            {
                GameObject.FindGameObjectWithTag("Center").transform.GetChild(0).gameObject.GetComponent<ShieldScript>()
                    .FinishSpinner();
                Destroy(GameObject.FindGameObjectWithTag("Anim").gameObject);
                Destroy(gameObject);
            }

            if (timeALive == 0 && time > timeLive)
            {
                logic.ShowMissEffect();
                Destroy(GameObject.FindGameObjectWithTag("Anim").gameObject);
                Destroy(gameObject);
            }

            UpdateScale();
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