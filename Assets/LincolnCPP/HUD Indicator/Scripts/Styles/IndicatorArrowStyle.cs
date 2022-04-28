using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

    [System.Serializable]
    public class IndicatorArrowStyle {
        public Texture texture = null;
        public Color color = Color.red;
        public float margin = 0f;
        public float width = 16f;
        public float height = 16f;
    }
}