//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;

namespace SonicBloom.Koreo.Demos
{
    [AddComponentMenu("Koreographer/Demos/Rhythm Game/Pace Note Object 1P")]
    public class PaceNoteObject1P : MonoBehaviour
    {
        #region Fields

        [Tooltip("The visual to use for this Note Object.")]
        public SpriteRenderer visuals;

        // If active, the KoreographyEvent that this Note Object wraps.  Contains the relevant timing information.
        KoreographyEvent trackedEvent;

        // If active, the Lane Controller1P that this Note Object is contained by.
        PaceLaneController1P paceLaneController1P;

        // If active, the Rhythm Game Controller1P that controls the game this Note Object is found within.
        PaceController1P paceController1P;

        #endregion
        #region Static Methods

        // Unclamped Lerp.  Same as Vector3.Lerp without the [0.0-1.0] clamping.
        static Vector3 Lerp(Vector3 from, Vector3 to, float t)
        {
            return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
        }

        #endregion
        #region Methods

        // Prepares the Note Object for use.
        public void Initialize(KoreographyEvent evt, Color color, PaceLaneController1P laneCont, PaceController1P gameCont)
        {
            trackedEvent = evt;
            visuals.color = color;
            paceLaneController1P = laneCont;
            paceController1P = gameCont;

            UpdatePosition();
        }

        // Resets the Note Object to its default state.
        void Reset()
        {
            trackedEvent = null;
            paceLaneController1P = null;
            paceController1P = null;
        }

        void Update()
        {
            UpdateHeight();

            UpdatePosition();

            if (transform.position.x >= paceLaneController1P.DespawnX)
            {
                paceController1P.ReturnNoteObjectToPool(this);
                Reset();
            }
        }

        // Updates the height of the Note Object.  This is relative to the speed at which the notes fall and 
        //  the specified Hit Window range.
        void UpdateHeight()
        {
            float baseUnitHeight = visuals.sprite.rect.width / visuals.sprite.pixelsPerUnit;
            float targetUnitHeight = paceController1P.WindowSizeInUnits * 2f; // Double it for before/after.

            Vector3 scale = transform.localScale;
            scale.x = targetUnitHeight / baseUnitHeight;
            transform.localScale = scale;
        }

        // Updates the position of the Note Object along the Lane based on current audio position.
        void UpdatePosition()
        {
            // Get the number of samples we traverse given the current speed in Units-Per-Second.
            float samplesPerUnit = paceController1P.SampleRate / paceController1P.noteSpeed;

            // Our position is offset by the distance from the target in world coordinates.  This depends on
            //  the distance from "perfect time" in samples (the time of the Koreography Event!).
            Vector3 pos = paceLaneController1P.TargetPosition;
            pos.x += (paceController1P.DelayedSampleTime - trackedEvent.StartSample) / samplesPerUnit;
            transform.position = pos;
        }

        // Checks to see if the Note Object is currently hittable or not based on current audio sample
        //  position and the configured hit window width in samples (this window used during checks for both
        //  before/after the specific sample time of the Note Object).
        public bool IsNotePerfect()
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = paceController1P.DelayedSampleTime;
            //int hitWindow = paceController1P.HitWindowSampleWidth;
            int hitWindow = (int)(0.0005f * 60 * 44100);

            return (Mathf.Abs(noteTime - curTime) <= hitWindow);
        }

        public bool IsNoteGood()
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = paceController1P.DelayedSampleTime;
            int hitWindow = (int)(0.0015f * 60 * 44100);  //change the judgment area

            return (Mathf.Abs(noteTime - curTime) <= hitWindow);
        }

        // Checks to see if the note is no longer hittable based on the configured hit window width in
        //  samples.
        public bool IsNoteMissed()
        {
            bool bMissed = true;

            if (enabled)
            {
                int noteTime = trackedEvent.StartSample;
                int curTime = paceController1P.DelayedSampleTime;
                int hitWindow = (int)(0.0015f * 60 * 44100);

                bMissed = (curTime - noteTime > hitWindow);
            }

            return bMissed;
        }

        // Returns this Note Object to the pool which is controlled by the Rhythm Game Controller1P.  This
        //  helps reduce runtime allocations.
        void ReturnToPool()
        {
            paceController1P.ReturnNoteObjectToPool(this);
            Reset();
        }

        // Performs actions when the Note Object is hit.
        public void OnHit()
        {
            ReturnToPool();
        }

        // Performs actions when the Note Object is cleared.
        public void OnClear()
        {
            ReturnToPool();
        }

        #endregion
    }
}
