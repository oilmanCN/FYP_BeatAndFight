//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Button Controller 1P")]
	public class ButtonController1P : MonoBehaviour
	{
		#region Fields
		
		[Tooltip("The lane to which this button will send signals.")]
		public PaceLaneController1P paceLaneController1P;

		[Tooltip("The Image component for the button graphic.  The texture will be tinted using the lane's color.")]
		public Image imageCom;

		[Tooltip("The Text component for the button.  The text used is specified in the Lane Controller.")]
		public Text textCom;
		
		#endregion
		#region Methods
		
		void Start()
		{
			// Adjust the color to match that of the target Lane.
			Color buttonColor = paceLaneController1P.color;
			buttonColor.a = imageCom.color.a;
			imageCom.color = buttonColor;

			// Change our button setup depending on whether we're using Touch Input or not.  For brevity,
			//  only iOS and Android are currently handled.
#if !(UNITY_IOS || UNITY_ANDROID)
			textCom.text = paceLaneController1P.keyboardButtonPositive.ToString();
            textCom.text = paceLaneController1P.keyboardButtonNegetive.ToString();

            // Adjust the brightness of the text for readability.  We get the "brightness" of the
            //  color with grayscale and then invert it to get a feasible brightness.
            textCom.color = Color.Lerp(Color.black, Color.white, 1f - buttonColor.grayscale);
#else
			// No text to show.  Simply disable the Text component.
			textCom.enabled = false;
#endif
		}
		
		#endregion
	}
}
