using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUDIndicator.teste {
    
    public class Arrow {
        public GameObject gameObject;
        public RawImage rawImage;
        public RectTransform rect;

        public void Destroy() {
            if(gameObject) {
                GameObject.Destroy(gameObject);
			}
        }
    }

    public class Indicator {
        public IndicatorRenderer renderer;
        public GameObject gameObject;
        public RawImage rawImage;
        public RectTransform rect;

        public Arrow arrow = new Arrow();

        public void Destroy() {
            arrow.Destroy();

            if(gameObject) {
                GameObject.Destroy(gameObject);
			}
		}
	}

    public class IndicatorRenderer : MonoBehaviour{

        [SerializeField] private Color canvasColor = new Color(1, 0, 0, .3f);
        
        [HideInInspector] public RectTransform rect;

        private Dictionary<int, Indicator> indicators = new Dictionary<int, Indicator>();

        void Start(){
            rect = GetComponent<RectTransform>();
        }

        void Update(){
        
        }

		public Indicator CreateIndicator(IndicatorPoint point) {
            int hash = point.GetHashCode();

            if(!indicators.ContainsKey(hash)) {
                indicators.Add(hash, CreateIndicatorStructure(point));
		    }

            return indicators[hash];
	    }

        public void DestroyIndicator(IndicatorPoint point) {
            int hash = point.GetHashCode();

            if(indicators.ContainsKey(hash)) {
                indicators[hash].Destroy();

                indicators.Remove(hash);
            }
	    }

        public void EnableIndicator(IndicatorPoint point) {
            int hash = point.GetHashCode();

            if(indicators.ContainsKey(hash)) {
                indicators[hash].gameObject.SetActive(true);
		    }
	    }

        public void DisableIndicator(IndicatorPoint point) {
            int hash = point.GetHashCode();

            if(indicators[hash] != null && indicators[hash].gameObject != null) {
                indicators[hash].gameObject.SetActive(false);
            }
	    }

        private Indicator CreateIndicatorStructure(IndicatorPoint point) {
            Indicator indicator                 = new Indicator();
            indicator.renderer                  = this;

            // Create game object
            indicator.gameObject                = new GameObject($"Indicator:{point.gameObject.name}");
            indicator.gameObject.transform.SetParent(transform);

            // Create raw image
            indicator.rawImage                  = indicator.gameObject.AddComponent<RawImage>();
            indicator.rawImage.texture          = point.style.texture;
            indicator.rawImage.color            = point.style.color;

            // Create arrow game object
            indicator.arrow.gameObject          = new GameObject($"Indicator:Arrow:{point.gameObject.name}");
            indicator.arrow.gameObject.transform.SetParent(indicator.gameObject.transform);

            // Create arrow raw image
            indicator.arrow.rawImage            = indicator.arrow.gameObject.AddComponent<RawImage>();
            indicator.arrow.rawImage.texture    = point.style.arrow;
            indicator.arrow.rawImage.color      = point.style.color;

            // Setup rect transform
            indicator.rect                      = indicator.gameObject.GetComponent<RectTransform>();
            indicator.rect.localScale           = Vector3.one;
            indicator.rect.sizeDelta            = new Vector2(point.style.width, point.style.height);

            // Setup arrow rect transform
            indicator.arrow.rect                = indicator.arrow.gameObject.GetComponent<RectTransform>();
            indicator.arrow.rect.localScale     = Vector3.one;
            indicator.arrow.rect.sizeDelta      = new Vector2(point.style.arrow.width, point.style.arrow.height);
            indicator.arrow.rect.pivot          = new Vector2(0.5f, 1 + (point.style.height / 2f + point.style.space) / point.style.arrow.height);

            return indicator;
        }


		private void OnDrawGizmosSelected() {
            rect = GetComponent<RectTransform>();

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, canvasColor);
            tex.Apply();

            Vector3 pos = rect.TransformPoint(new Vector3(rect.rect.x, rect.rect.y, 0));
            Graphics.DrawTexture(new Rect(pos.x, pos.y, rect.rect.width * rect.lossyScale.x, rect.rect.height * rect.lossyScale.y), tex);

            DestroyImmediate(tex);
        }

    }

}