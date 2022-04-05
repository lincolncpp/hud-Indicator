using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator {


    [System.Serializable]
    public class Style {
        public Texture texture;
        public float space = 5;
        public Texture arrow;
        public Color color = Color.green;
        public float width = 32;
        public float height = 32;
    }

    public class IndicatorPoint : MonoBehaviour {

        // Fix this shit
        public static float FADE_DISTANCE = 5f;
        public static float SCREEN_SPACE = 8f;


        [SerializeField] private List<IndicatorRenderer> renderers;

        public Style style;

        private List<IndicatorStructure> structures = new List<IndicatorStructure>();


        void Start() {
            if(renderers.Count == 0) {
                throw new UnassignedReferenceException("The indicator point must have at least one renderer assigned.");
            }

            foreach(IndicatorRenderer renderer in renderers) {
                IndicatorStructure structure;

                structure = renderer.CreateIndicator(this);
                structures.Add(structure);
            }
        }

        private void OnEnable() {
            foreach(IndicatorRenderer renderer in renderers) {
                renderer.EnableIndicator(this);
            }
        }

        private void OnDisable() {
            foreach(IndicatorRenderer renderer in renderers) {
                renderer.DisableIndicator(this);
            }
        }

        private void OnDestroy() {
            foreach(IndicatorRenderer renderer in renderers) {
                renderer.DestroyIndicator(this);
            }
        }

        void Update() {
            
            Vector3 vec = Camera.main.WorldToScreenPoint(transform.position);
            vec = renderers[0].GetComponent<RectTransform>().InverseTransformPoint(vec);

			structures[0].rect.anchoredPosition = new Vector2(vec.x, vec.y);


            // Get width and height from this \/
            print(renderers[0].GetComponent<RectTransform>().rect);

            return;







            bool offscreen = false;

            float scale = 1f / renderers[0].transform.lossyScale.x;


            float w = Screen.width * scale;
            float h = Screen.height * scale;

            Vector3 point = Camera.main.WorldToScreenPoint(transform.position) * scale;

            point.x -= w / 2f;
            point.y -= h / 2f;

            w -= 2f * SCREEN_SPACE + style.width + 2f * style.space + 2f * Mathf.Max(style.arrow.width, style.arrow.height);
            h -= 2f * SCREEN_SPACE + style.height + 2f * style.space + 2f * Mathf.Max(style.arrow.width, style.arrow.height);

            // On-Screen indicator
            if(point.z >= 0 && point.x >= -w / 2f && point.x <= w / 2f && point.y >= -h / 2f && point.y <= h / 2f) {

                // Update indicators on renderer
                foreach(IndicatorStructure structure in structures) {
                    structure.arrow.gameObject.SetActive(false);
                    structure.rect.anchoredPosition = new Vector2(point.x, point.y);
                }

            }
            // Off-Screen indicator
            else {
                offscreen = true;

                if(point.z < 0)
                    point *= -1;

                float a = 2f * point.x / w;
                float b = 2f * point.y / h;
                Vector2 pos;

                // The indicator lies on left or right corner
                if(Mathf.Abs(a) > Mathf.Abs(b)) {

                    // Right corner
                    if(a > 0) {
                        float y = w / 2f * point.y / point.x;
                        pos = new Vector2(w / 2f, y);
                    }
                    // Left corner
                    else {
                        float y = -w / 2f * point.y / point.x;
                        pos = new Vector2(-w / 2f, y);
                    }
                }
                // The indicator lies on top or bottom corner
                else {
                    // Top corner
                    if(b > 0) {
                        float x = h / 2f * point.x / point.y;
                        pos = new Vector2(x, h / 2);
                    }
                    // Bottom corner
                    else {
                        float x = -h / 2f * point.x / point.y;
                        pos = new Vector2(x, -h / 2);
                    }
                }

                // Update arrow rotation
                Vector2 dir = new Vector2(point.x, point.y);

                // Update indicators on renderer
                foreach(IndicatorStructure structure in structures) {
                    structure.arrow.gameObject.SetActive(true);
                    structure.arrow.rect.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir));
                    structure.rect.anchoredPosition = pos;
                }
            }
        }
    }


}