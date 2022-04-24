using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

    public abstract class IndicatorCanvas {
        public Indicator indicator { private set; get; }
        public IndicatorRenderer renderer { private set; get; }

        public bool active { private set; get; }

        public virtual void Create(Indicator indicator, IndicatorRenderer renderer) {
            this.indicator = indicator;
            this.renderer = renderer;

            active = true;
		}

        public abstract void Update();

        public virtual void OnEnable() {
            active = true;
        }
        public virtual void OnDisable(){
            active = false;   
        }

        public virtual void Destroy() {}

        public bool IsVisible() {
            return indicator.visible && renderer.visible;
        }
    }
}