using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    public class GestureManager : Singleton<GestureManager>
    {
        // Tap and Navigation gesture recognizer.
        //public GestureRecognizer NavigationRecognizer { get; private set; }

        // Manipulation gesture recognizer.
        public GestureRecognizer ManipulationRecognizer { get; private set; }

        // Currently active gesture recognizer.
        //public GestureRecognizer ActiveRecognizer { get; private set; }

        //public bool IsNavigating { get; private set; }

        //public Vector3 NavigationPosition { get; private set; }

        public bool IsManipulating { get; private set; }

        public Vector3 ManipulationPosition { get; private set; }

        void Awake()
        {
            /* TODO: DEVELOPER CODING EXERCISE 2.b */

            // Instantiate the ManipulationRecognizer.
            ManipulationRecognizer = new GestureRecognizer();

            // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
            ManipulationRecognizer.SetRecognizableGestures(
                GestureSettings.ManipulationTranslate);
            
            // Register for the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

            //ManipulationRecognizer.StartCapturingGestures();
            ManipulationRecognizer.StopCapturingGestures();
        }

        void OnDestroy()
        {
            // Unregister the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
        }

        /// <summary>
        /// Revert back to the default GestureRecognizer.
        /// </summary>
        /*public void ResetGestureRecognizers()
        {
            // Default to the navigation gestures.
            Transition(ManipulationRecognizer);
        }

        /// <summary>
        /// Transition to a new GestureRecognizer.
        /// </summary>
        /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (ActiveRecognizer != null)
            {
                if (ActiveRecognizer == newRecognizer)
                {
                    return;
                }

                ActiveRecognizer.CancelGestures();
                ActiveRecognizer.StopCapturingGestures();
            }

            //if (IndicatorControl.inMoveMode)
                newRecognizer.StartCapturingGestures();
            //else
                //newRecognizer.StopCapturingGestures();
            ActiveRecognizer = newRecognizer;
        }*/

       

        private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            if (InteractibleManager.Instance.FocusedGameObject != null)
            {
                IndicatorControl.removeWorldAnchor(InteractibleManager.Instance.FocusedGameObject, InteractibleManager.Instance.FocusedGameObject.name);
                //for better tracking?
                InteractibleManager.Instance.tempFocusedGameObject = InteractibleManager.Instance.FocusedGameObject;

                IndicatorControl.removeWorldAnchor(InteractibleManager.Instance.tempFocusedGameObject, InteractibleManager.Instance.tempFocusedGameObject.name);


                //can probably mix these into same variable
                IndicatorControl.inMoveMode = true;
                IsManipulating = true;

                ManipulationPosition = position;

                InteractibleManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);
            }
        }

        private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            if (InteractibleManager.Instance.FocusedGameObject != null || InteractibleManager.Instance.tempFocusedGameObject != null)
            {
                //can probably mix these into same variable
                IndicatorControl.inMoveMode = true;
                IsManipulating = true;

                ManipulationPosition = position;

                InteractibleManager.Instance.tempFocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);
            }
        }

        private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = false;
            IndicatorControl.createWorldAnchor(InteractibleManager.Instance.tempFocusedGameObject, InteractibleManager.Instance.tempFocusedGameObject.name);
            InteractibleManager.Instance.tempFocusedGameObject = null;
        }

        private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = true;
        }
    }
}