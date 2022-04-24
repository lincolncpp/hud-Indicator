using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator.teste {


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

        private List<Indicator> indicators = new List<Indicator>();


        void Start() {
            if(renderers.Count == 0) {
                throw new UnassignedReferenceException("The indicator point must have at least one renderer assigned.");
            }

            foreach(IndicatorRenderer renderer in renderers) {
                Indicator structure;

                structure = renderer.CreateIndicator(this);
                indicators.Add(structure);
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
            foreach(Indicator indicator in indicators) {
                Rect rendererRect = indicator.renderer.rect.rect;
                Vector3 pos = indicator.renderer.rect.InverseTransformPoint(Camera.main.WorldToScreenPoint(transform.position));

                rendererRect.width -= style.width;
                rendererRect.height -= style.height;

                // On-Screen
                if (pos.z >= 0 && pos.x >= rendererRect.x && pos.x <= rendererRect.x + rendererRect.width && pos.y >= rendererRect.y && pos.y <= rendererRect.y + rendererRect.height) {
                    // Update indicators on renderer
                    indicator.arrow.gameObject.SetActive(false);
                }
                // Off-Screen
				else {
                    if(pos.z < 0) {
                        pos *= -1;
                    }

                    float a = pos.x / rendererRect.width;
                    float b = pos.y / rendererRect.height;

                    // The indicator lies on left or right corner
                    if(Mathf.Abs(a) > Mathf.Abs(b)) {

                        // Right corner
                        if(a > 0) {
                            float y = rendererRect.width/2f * pos.y / pos.x;
                            pos = new Vector2(rendererRect.width/2f, y);
                        }
                        // Left corner
                        else {
                            float y = -rendererRect.width/2f * pos.y / pos.x;
                            pos = new Vector2(-rendererRect.width/2f, y);
                        }
                    }
                    // The indicator lies on top or bottom corner
                    else {
                        // Top corner
                        if(b > 0) {
                            float x = rendererRect.height / 2f * pos.x / pos.y;
							pos = new Vector2(x, rendererRect.height / 2f);
                        }
                        // Bottom corner
                        else {
                            float x = -rendererRect.height / 2f * pos.x / pos.y;
							pos = new Vector2(x, -rendererRect.height / 2f);
                        }
                    }
                }
                
                indicator.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			}
            

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
                foreach(Indicator structure in indicators) {
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
                foreach(Indicator structure in indicators) {
                    structure.arrow.gameObject.SetActive(true);
                    structure.arrow.rect.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir));
                    structure.rect.anchoredPosition = pos;
                }
            }
        }
    }


}