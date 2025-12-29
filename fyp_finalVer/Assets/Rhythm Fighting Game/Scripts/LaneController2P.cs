using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace SonicBloom.Koreo.Demos
{
    [AddComponentMenu("Koreographer/Demos/Rhythm Game/Lane Controller 2P")]
    public class LaneController2P : MonoBehaviour
    {
        #region Fields

        [Tooltip("The Color of Note Objects and Buttons in this Lane.")]
        public Color color = Color.blue;

        [Tooltip("A reference to the visuals for the \"target\" location.")]
        public SpriteRenderer targetVisuals;

        public SpriteRenderer[] targetType;

        [Tooltip("The Keyboard Button used by this lane.")]
        public KeyCode keyboardButton;

        [Tooltip("A list of Payload strings that Koreography Events will contain for this Lane.")]
        public List<string> matchedPayloads = new List<string>();

        private Coroutine judgementCoroutine;

        public double deltahit2P = 0f;

        // The list that will contain all events in this lane.  These are added by the Rhythm Game Controller.
        List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();

        // A Queue that contains all of the Note Objects currently active (on-screen) within this lane.  Input and
        //  lifetime validity checks are tracked with operations on this Queue.
        Queue<NoteObject2P> trackedNotes = new Queue<NoteObject2P>();

        // A reference to the Rythm Game Controller.  Provides access to the NoteObject pool and other parameters.
        RhythmGameController2P gameController2P;

        // Lifetime boundaries.  This game goes from the top of the screen to the bottom.
        float spawnX = 0f;
        float despawnX = 0f;

        // Index of the next event to check for spawn timing in this lane.
        int pendingEventIdx = 0;

        // Feedback Scales used for resizing the buttons on press.
        Vector3 defaultScale;
        float scaleNormal = 1f;
        float scalePress = 1.2f;
        float scaleHold = 1.1f;

        public event Action<String, String> NoteJudged;

        public static LaneController2P Instance { get; private set; }

        #endregion
        #region Properties

        // The position at which new Note Objects should spawn.
        public Vector3 SpawnPosition
        {
            get
            {
                return new Vector3(spawnX, transform.position.y);
            }
        }

        // The position at which the timing target exists.
        public Vector3 TargetPosition
        {
            get
            {
                return new Vector3(transform.position.x, transform.position.y);
            }
        }

        // The position at which Note Objects should despawn and return to the pool.
        public float DespawnX
        {
            get
            {
                return despawnX;
            }
        }

        #endregion
        #region Methods

        public void Initialize(RhythmGameController2P controller)
        {
            gameController2P = controller;
        }

        // This method controls cleanup, resetting the internals to a fresh state.
        public void Restart()
        {
            pendingEventIdx = 0;

            // Clear out the tracked notes.
            int numToClear = trackedNotes.Count;
            for (int i = 0; i < numToClear; ++i)
            {
                trackedNotes.Dequeue().OnClear();
            }
            if (judgementCoroutine != null)
            {
                StopCoroutine(judgementCoroutine);
                judgementCoroutine = null;
            }
        }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject); // destroy duplicate instance
            }
        }

        void Start()
        {
            // Get the vertical bounds of the camera.  Offset by a bit to allow for offscreen spawning/removal.
            float cameraOffsetZ = -Camera.main.transform.position.z;
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, cameraOffsetZ)).x + 1f;
            despawnX = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).x + 13.3f; // original + 12.5f  // modified +14f

            // Update our visual color.
            targetVisuals.color = color;

            // Capture the default scale set in the Inspector.
            defaultScale = targetVisuals.transform.localScale;
        }

        void Update()
        {
            // Clear out invalid entries.
            while (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteMissed())
            {
                trackedNotes.Dequeue();
                judgementCoroutine = StartCoroutine(ShowJudgmentResult(2, 0.1f));

                JudgeNote("Miss");
                //print("2P_Miss");
            }

            // Check for new spawns.
            CheckSpawnNext();

            // Check for input.  Note that touch controls are handled by the Event System, which is all
            //  configured within the Inspector on the buttons themselves, using the same functions as
            //  what is found here.  Touch input does not have a built-in concept of "Held", so it is not
            //  currently supported.
            if (Input.GetKeyDown(keyboardButton))
            {
                CheckNoteHit();
                SetScalePress();
            }
            else if (Input.GetKey(keyboardButton))
            {
                SetScaleHold();
            }
            else if (Input.GetKeyUp(keyboardButton))
            {
                SetScaleDefault();
            }
        }

        public IEnumerator ShowJudgmentResult(int ind, float displayTime)
        {
            targetVisuals.sprite = targetType[ind].sprite;
            targetVisuals.color = targetType[ind].color;
            targetVisuals.enabled = true;

            yield return new WaitForSeconds(displayTime);

            targetVisuals.enabled = false;
            targetVisuals.sprite = targetType[3].sprite;
            targetVisuals.color = color;
            targetVisuals.enabled = true;
        }


        // Adjusts the scale with a multiplier against the default scale.
        void AdjustScale(float multiplier)
        {
            targetVisuals.transform.localScale = defaultScale * multiplier;
        }

        // Uses the Target position and the current Note Object speed to determine the audio sample
        //  "position" of the spawn location.  This value is relative to the audio sample position at
        //  the Target position (the "now" time).
        int GetSpawnSampleOffset()
        {
            // Given the current speed, what is the sample offset of our current.
            float spawnDistToTarget = spawnX - transform.position.x;

            // At the current speed, what is the time to the location?
            double spawnSecsToTarget = (double)spawnDistToTarget / (double)gameController2P.noteSpeed;

            // Figure out the samples to the target.
            return (int)(spawnSecsToTarget * gameController2P.SampleRate);
        }

        // Checks if a Note Object is hit.  If one is, it will perform the Hit and remove the object
        //  from the trackedNotes Queue.
        public void CheckNoteHit()
        {
            // Always check only the first event as we clear out missed entries before.
            if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNotePerfect())
            {
                NoteObject2P hitNote = trackedNotes.Dequeue();

                hitNote.OnHit();
                judgementCoroutine = StartCoroutine(ShowJudgmentResult(0, 0.1f));
                //P2Movement.Instance.Anime2P.SetTrigger("ATTACK");
                //print("2P:" + Time.deltaTime);

                JudgeNote("Perfect");
                //print("2P_Perfect");
            }

            else if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteGood())
            {
                NoteObject2P hitNote = trackedNotes.Dequeue();

                hitNote.OnHit();
                judgementCoroutine = StartCoroutine(ShowJudgmentResult(1, 0.1f));
                //P2Movement.Instance.Anime2P.SetTrigger("ATTACK");
                //deltahit2P = Time.deltaTime;
                deltahit2P = Koreographer.Instance.GetMusicSecondsTime(RhythmGameController1P.Instance.audioCom.clip.name);

                JudgeNote("Good");
                //print("2P_Good");
            }
            
            
        }

        // Checks if the next Note Object should be spawned.  If so, it will spawn the Note Object and
        //  add it to the trackedNotes Queue.
        void CheckSpawnNext()
        {
            int samplesToTarget = GetSpawnSampleOffset();

            int currentTime = gameController2P.DelayedSampleTime;

            // Spawn for all events within range.
            while (pendingEventIdx < laneEvents.Count &&
                   laneEvents[pendingEventIdx].StartSample < currentTime + samplesToTarget)
            {
                KoreographyEvent evt = laneEvents[pendingEventIdx];

                NoteObject2P newObj = gameController2P.GetFreshNoteObject();
                newObj.Initialize(evt, color, this, gameController2P);

                trackedNotes.Enqueue(newObj);

                pendingEventIdx++;
            }
        }

        // Adds a KoreographyEvent to the Lane.  The KoreographyEvent contains the timing information
        //  that defines when a Note Object should appear on screen.
        public void AddEventToLane(KoreographyEvent evt)
        {
            laneEvents.Add(evt);
        }

        // Checks to see if the string value passed in matches any of the configured values specified
        //  in the matchedPayloads List.
        public bool DoesMatchPayload(string payload)
        {
            bool bMatched = false;

            for (int i = 0; i < matchedPayloads.Count; ++i)
            {
                if (payload == matchedPayloads[i])
                {
                    bMatched = true;
                    break;
                }
            }

            return bMatched;
        }

        // Sets the Target scale to the original default scale.
        public void SetScaleDefault()
        {
            AdjustScale(scaleNormal);
        }

        // Sets the Target scale to the specified "initially pressed" scale.
        public void SetScalePress()
        {
            AdjustScale(scalePress);
        }

        // Sets the Target scale to the specified "continuously held" scale.
        public void SetScaleHold()
        {
            AdjustScale(scaleHold);
        }

        public void JudgeNote(String judgement)
        {
            NoteJudged?.Invoke("Player2", judgement);
        }

        #endregion
    }
}

