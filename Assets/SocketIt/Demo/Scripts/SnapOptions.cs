using UnityEngine;
using System.Collections;
using SocketIt;
using UnityEngine.UI;

namespace SocketIt.Demo { 

public class SnapOptions : MonoBehaviour {

        public InstantSnapper snapper;

        public Button ButtonPosition;
        public Button ButtonRotationUp;
        public Button ButtonRotationForward;
        public Button ButtonSetParent;

        private Color ActiveButtonColor = new Color(0,200,255, 1);
        private Color InActiveButtonColor = new Color(0, 200, 255, .5f);

        void Start()
        {
            UpdateButtons();
        }

        public void OnToggleSnapPosition()
        {
            snapper.SnapPosition = !snapper.SnapPosition;
            UpdateButtons();

        }

        public void OnToggleRotationUp()
        {
            snapper.SnapRotationUp = !snapper.SnapRotationUp;
            UpdateButtons();

        }

        public void OnToggleRotationForward()
        {
            snapper.SnapRotationForward = !snapper.SnapRotationForward;
            UpdateButtons();

        }

        public void OnToggleSetParent() 
        {
            snapper.SetParentOnSnap = !snapper.SetParentOnSnap;
            UpdateButtons();

        }

        private void UpdateButtons()
        {

            SetButtonColor(ButtonPosition, snapper.SnapPosition ? ActiveButtonColor : InActiveButtonColor);
            SetButtonColor(ButtonRotationForward, snapper.SnapRotationForward ? ActiveButtonColor : InActiveButtonColor);
            SetButtonColor(ButtonRotationUp, snapper.SnapRotationUp ? ActiveButtonColor : InActiveButtonColor);
            SetButtonColor(ButtonSetParent, snapper.SetParentOnSnap ? ActiveButtonColor : InActiveButtonColor);
        }

        private void SetButtonColor(Button button, Color color)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = color;
            button.colors = colors;
        }
    }
}