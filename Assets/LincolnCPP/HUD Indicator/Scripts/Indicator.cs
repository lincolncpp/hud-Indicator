using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LincolnCpp.HUDIndicator {

    [System.Serializable]
    public abstract class Indicator : MonoBehaviour {

        public bool visible = true;
        [SerializeField] private List<IndicatorRenderer> renderers;
        
        protected Dictionary<IndicatorRenderer, IndicatorCanvas> indicatorsCanvas = new Dictionary<IndicatorRenderer, IndicatorCanvas>();

		private void Start() {
            foreach(IndicatorRenderer renderer in renderers) {
                CreateIndicatorCanvas(renderer);
            }
        }

		private void Update() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.Update();
            }
        }

        private void OnEnable() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.OnEnable();
            }
        }

        private void OnDisable() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                element.Value.OnDisable();
            }
        }

		private void OnDestroy() {
            foreach(KeyValuePair<IndicatorRenderer, IndicatorCanvas> element in indicatorsCanvas) {
                DestroyIndicatorCanvas(element.Key);
            }
		}

        protected abstract void CreateIndicatorCanvas(IndicatorRenderer renderer);

        private void DestroyIndicatorCanvas(IndicatorRenderer renderer) {
            if(indicatorsCanvas.ContainsKey(renderer)) {
                indicatorsCanvas[renderer].Destroy();
			}
		}
    }
}
