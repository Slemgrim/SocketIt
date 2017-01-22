using UnityEngine;
using System.Collections;
using SocketIt;
using UnityEngine.UI;

namespace SocketIt.Example02 { 

public class SnapOptions : MonoBehaviour {

        public InstantSnapper snapper;

        public Button ButtonPosition;
        public Button ButtonRotationUp;
        public Button ButtonRotationForward;

        private Color ActiveButtonColor;
        private Color InActiveButtonColor;

        void Start()
        {
            ActiveButtonColor = ButtonPosition.colors.pressedColor;
            InActiveButtonColor = ButtonPosition.colors.normalColor;

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
            UpdateButtons();

        }

        private void UpdateButtons()
        {
            SetButtonColor(ButtonPosition, snapper.SnapPosition ? ActiveButtonColor : InActiveButtonColor);
            SetButtonColor(ButtonRotationForward, snapper.SnapRotationForward ? ActiveButtonColor : InActiveButtonColor);
            SetButtonColor(ButtonRotationUp, snapper.SnapRotationUp ? ActiveButtonColor : InActiveButtonColor);
        }

        private void SetButtonColor(Button button, Color color)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = color;
            button.colors = colors;
        }
    }
}