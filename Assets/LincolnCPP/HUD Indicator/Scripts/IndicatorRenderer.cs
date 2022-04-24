using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

    public class IndicatorRenderer : MonoBehaviour {
        public bool visible = true;
        public Color canvasColor = new Color(1, 0, 0, .5f);

        [HideInInspector] public RectTransform rectTransform;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        public Rect GetRect() {
            return rectTransform.rect;
		}

        private void OnDrawGizmosSelected() {
            rectTransform = GetComponent<RectTransform>();

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, canvasColor);
            tex.Apply();

            Vector3 pos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.x, rectTransform.rect.y, 0));
            Graphics.DrawTexture(new Rect(pos.x, pos.y, rectTransform.rect.width * rectTransform.lossyScale.x, rectTransform.rect.height * rectTransform.lossyScale.y), tex);

            DestroyImmediate(tex);
        }
    }
}
