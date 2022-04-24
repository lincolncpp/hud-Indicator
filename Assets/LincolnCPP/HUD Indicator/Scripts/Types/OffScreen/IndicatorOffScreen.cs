using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LincolnCpp.HUDIndicator {

	[System.Serializable]
	public class IndicatorOffScreen : Indicator {

		public enum RotationMode { NONE, AIM_TO_CORNER, AIM_TO_TARGET };

		public bool showArrow = true;
		public IndicatorArrowStyle arrowStyle;

		public RotationMode rotationMode = RotationMode.NONE;

		public override void CreateIndicatorCanvas(IndicatorRenderer renderer) {
			throw new System.NotImplementedException();
		}
	}
}