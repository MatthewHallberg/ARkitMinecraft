using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class BottomBarBehavior : MonoBehaviour {

		public Transform Weapon, Green, Diamond, Tree, Move, Up, Down, Torch;

		public GameObject PickAxeParent;

		public AxeControl axeControl;

		public GameObject areaParent;

		private Vector3 moveUp = new Vector3 (0, 8f, 0);
		private Vector3 scaleUp = new Vector3 (.1f, .1f, 0);

		void Start(){

			ResetButtons ();
			ButtonPressed (Weapon);
			PickAxeParent.SetActive (true);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Weapon;
			UnityARHitTestExample.currentScale = new Vector3 (1, 1, 1);
		}

		private void ResetButtons(){
			//reset all other buttons
			foreach (Transform child in this.transform) {
				if (child.localScale.x > 1.01f) {
					child.localPosition -= moveUp;
					child.localScale -= scaleUp;
					PickAxeParent.SetActive (false);
				}
			}
		}

		public void WeaponButtonDown(){

			if (PickAxeParent.activeSelf) {
				
				axeControl.UseWeapon ();
			} else {

				ResetButtons ();
				ButtonPressed (Weapon);
				PickAxeParent.SetActive (true);
				UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Weapon;
			}
		}

		public void GreenButtonDown(){

			ResetButtons ();
			ButtonPressed (Green);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.GrassBlock;

		}

		public void DiamondButtonDown(){
			
			ResetButtons ();
			ButtonPressed (Diamond);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.DiamondOre;

		}

		public void TorchButtonDown(){

			ResetButtons ();
			ButtonPressed (Torch);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Torch;

		}

		public void TreeButtonDown(){

			ResetButtons ();
			ButtonPressed (Tree);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Tree;

		}

		public void MoveButtonDown(){

			ResetButtons ();
			ButtonPressed (Move);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Move;

		}

		public void UpButtonDown(){

			ResetButtons ();
			ButtonPressed (Up);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Scale;
			UnityARHitTestExample.currentScale += new Vector3 (1, 1, 1);

			foreach (Transform child in areaParent.transform) {
				child.localScale += new Vector3 (1, 1, 1);
				//move each block further out according to scale
				child.localPosition = Reposition(child, 1);
			}
		}

		public void DownButtonDown(){

			ResetButtons ();
			ButtonPressed (Down);
			UnityARHitTestExample.currentSelected = UnityARHitTestExample.Selected.Scale;

			//dont let scale go below 1
			if (areaParent.transform.GetChild(0).transform.localScale.x > 1) {
				UnityARHitTestExample.currentScale -= new Vector3 (1, 1, 1);
				//loop through each child block
				foreach (Transform child in areaParent.transform) {
					//increase scale by 1 
					child.localScale -= new Vector3 (1, 1, 1);
					//move each block further out according to scale
					child.localPosition = Reposition(child, -1);
				}
			}
		}

		void ButtonPressed(Transform desiredTransform){
			//scale buttons to show when they are selected
			desiredTransform.localPosition += moveUp;
			desiredTransform.localScale += scaleUp;
			desiredTransform.SetAsLastSibling ();
		}

		Vector3 Reposition(Transform child, int amount){

			float childX = 0;
			float childY = 0;
			float childZ = 0;

			if (child.localPosition.x != 0) {
				if (child.localPosition.x < 0) {
					childX = child.localPosition.x - amount;
				} else {
					childX = child.localPosition.x + amount;
				}
			}
			if (child.localPosition.y != 0) {
				if (child.localPosition.y < 0) {
					childY = child.localPosition.y - amount;
				} else {
					childY = child.localPosition.y + amount;
				}
			}
			if (child.localPosition.z != 0) {
				if (child.localPosition.z < 0) {
					childZ = child.localPosition.z - amount;
				} else {
					childZ = child.localPosition.z + amount;
				}
			}
				
			return new Vector3 (childX, childY, childZ);
		}

	}
}
