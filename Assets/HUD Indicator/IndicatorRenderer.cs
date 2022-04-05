using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUDIndicator {
    
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

    public class IndicatorStructure {
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

        private Dictionary<int, IndicatorStructure> indicators = new Dictionary<int, IndicatorStructure>();

        void Start(){
        
        }

        void Update(){
        
        }

		public IndicatorStructure CreateIndicator(IndicatorPoint point) {
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

            if(indicators[hash] != null) {
                indicators[hash].gameObject.SetActive(false);
            }
	    }

        private IndicatorStructure CreateIndicatorStructure(IndicatorPoint point) {
            IndicatorStructure structure        = new IndicatorStructure();

            // Create game object
            structure.gameObject                = new GameObject($"Indicator:{point.gameObject.name}");
            structure.gameObject.transform.SetParent(transform);

            // Create raw image
            structure.rawImage                  = structure.gameObject.AddComponent<RawImage>();
            structure.rawImage.texture          = point.style.texture;
            structure.rawImage.color            = point.style.color;

            // Create arrow game object
            structure.arrow.gameObject          = new GameObject($"Indicator:Arrow:{point.gameObject.name}");
            structure.arrow.gameObject.transform.SetParent(structure.gameObject.transform);

            // Create arrow raw image
            structure.arrow.rawImage            = structure.arrow.gameObject.AddComponent<RawImage>();
            structure.arrow.rawImage.texture    = point.style.arrow;
            structure.arrow.rawImage.color      = point.style.color;

            // Setup rect transform
            structure.rect                      = structure.gameObject.GetComponent<RectTransform>();
            structure.rect.localScale           = Vector3.one;
            structure.rect.sizeDelta            = new Vector2(point.style.width, point.style.height);

            // Setup arrow rect transform
            structure.arrow.rect                = structure.arrow.gameObject.GetComponent<RectTransform>();
            structure.arrow.rect.localScale     = Vector3.one;
            structure.arrow.rect.sizeDelta      = new Vector2(point.style.arrow.width, point.style.arrow.height);
            structure.arrow.rect.pivot          = new Vector2(0.5f, 1 + (point.style.height / 2f + point.style.space) / point.style.arrow.height);

            return structure;
        }
    }

}