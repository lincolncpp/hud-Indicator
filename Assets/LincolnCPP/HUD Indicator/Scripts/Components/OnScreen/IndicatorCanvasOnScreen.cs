using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LincolnCpp.HUDIndicator {
	public class IndicatorCanvasOnScreen : IndicatorCanvas {

        private IndicatorOnScreen indicatorOnScreen;

        // Icon variables
        private RawImage rawImage;
        private RectTransform rectTransform;
        private IndicatorIconStyle style;

		public override void Create(Indicator indicator, IndicatorRenderer renderer) {
			base.Create(indicator, renderer);

            indicatorOnScreen = indicator as IndicatorOnScreen;

            // Get indicator style
            style = indicatorOnScreen.style;

            // Create game object
            gameObject = new GameObject($"IndicatorOnScreen:{indicator.gameObject.name}");
            gameObject.transform.SetParent(renderer.transform);

            // Setup rect transform
            rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(style.width, style.height);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Create icon image from texture
            rawImage = gameObject.AddComponent<RawImage>();
            rawImage.texture = style.texture;
            rawImage.color = style.color;
        }

		public override void Update() {
            if(!active) return;

            if(IsVisible()) {
                UpdatePosition();
            }
			else {
                if(gameObject.activeSelf) {
                    gameObject.SetActive(false);
				}
            }
        }

        private void UpdatePosition() {
            Rect rendererRect = renderer.GetRect();
            Vector3 pos = renderer.rectTransform.InverseTransformPoint(Camera.main.WorldToScreenPoint(indicator.gameObject.transform.position));

            rendererRect.x += style.width / 2f;
            rendererRect.y += style.height/ 2f;
            rendererRect.width -= style.width;
            rendererRect.height -= style.height;

            // On screen
            if (pos.z >= 0 && pos.x >= rendererRect.x && pos.x <= rendererRect.x + rendererRect.width && pos.y >= rendererRect.y && pos.y <= rendererRect.y + rendererRect.height) {
                gameObject.SetActive(true);
                
                rectTransform.position = renderer.rectTransform.TransformPoint(new Vector3(pos.x, pos.y, 0));            
            }
            else {
                gameObject.SetActive(false);
            }

        }
    }
}
