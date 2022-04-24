using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

    [System.Serializable]
    public class IndicatorArrowStyle {
        [HideInInspector] public Indicator indicator;
        public Texture texture = null;
        public Color color = Color.red;
        public float margin = 0f;
    }
}