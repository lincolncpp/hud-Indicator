using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

	[System.Serializable]
	public class IndicatorOffScreen : Indicator {

		public IndicatorIconStyle style;
		public bool showArrow = true;
		public IndicatorArrowStyle arrowStyle;

		protected override void CreateIndicatorCanvas(IndicatorRenderer renderer) {
			IndicatorCanvasOffScreen indicatorCanvasOffScreen = new IndicatorCanvasOffScreen();
			indicatorCanvasOffScreen.Create(this, renderer);

			indicatorsCanvas.Add(renderer, indicatorCanvasOffScreen);
		}
	}
}