using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

	public class IndicatorOnScreen : Indicator {
		public enum IndicatorTextType { NONE, DISTANCE, TEXT };

		public IndicatorTextType textType = IndicatorTextType.DISTANCE;
		public string text = string.Empty;
		public IndicatorIconStyle style;

		public override void CreateIndicatorCanvas(IndicatorRenderer renderer) {
			IndicatorCanvasOnScreen indicatorCanvasOnScreen = new IndicatorCanvasOnScreen();
			indicatorCanvasOnScreen.Create(this, renderer);

			indicatorsCanvas.Add(renderer, indicatorCanvasOnScreen);
		}
	}

}