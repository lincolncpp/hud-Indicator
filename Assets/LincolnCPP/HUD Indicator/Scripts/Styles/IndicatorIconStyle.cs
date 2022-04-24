using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

    [System.Serializable]
    public class IndicatorIconStyle {
        public enum IconSource { TEXTURE, PREFAB };

        public IconSource iconSource = IconSource.TEXTURE;
        public Texture texture = null;
        public GameObject prefab = null;
        public Color color = Color.red;
        public float width = 32f;
        public float height = 32f;
    }
}