using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform, grassBlockParent;
		public GameObject GrassBlock, DiamondOre, Tree, MainBlock, dirtBlock, Torch;

		public enum Selected {Weapon, GrassBlock, DiamondOre, Tree, Move, Scale, Torch};

		public static Selected currentSelected; 

		public static Vector3 currentScale;

		private Vector2 firstTouchPosition;

		private int horizontalScale = 0;

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes, Transform desiredTransform)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
					desiredTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					desiredTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }

		void PlaceNewObject(GameObject newBlock, ARPoint point, ARHitTestResultType[] resultTypes){

			newBlock.transform.localPosition = Vector3.zero;
			newBlock.transform.localScale = currentScale;

			GetComponent<AudioSource> ().Play ();

			foreach (ARHitTestResultType resultType in resultTypes)
			{
				if (HitTestWithResultType (point, resultType,newBlock.transform))
				{
					return;
				}
			}
		}

		void ScaleVerticalUp(){
			
			foreach (Transform block in grassBlockParent) {
				
				if (block.gameObject.activeSelf && block.localPosition.y == 0) {
					GameObject currentDirtBlock = Instantiate (dirtBlock, this.transform);
					//bug in unity causing crash when I set the parent from the instantiate function above? Setting it in DirtBlockBehavior as a work around.
					currentDirtBlock.GetComponent<DirtBlockBehavior>().UpdateBlock(block.localPosition);
				} 
				block.localPosition += new Vector3 (0, 1, 0);
			}
		}

		void ScaleVerticalDown(){

			foreach (Transform block in grassBlockParent) {
				if (block.gameObject.activeSelf && block.tag != "grass" && block.localPosition.y == 0) {
					Destroy (block.gameObject);
				} else if (block.localPosition.y > 0){
					block.localPosition -= new Vector3 (0, 1, 0);
				}
			}
		}

		void ScaleHorizontalUp(){
			if (horizontalScale < 3) {
				
				horizontalScale++;

				foreach (Transform block in grassBlockParent) {

					//had to round here and use ints because problems were occuring with floats
					int thisX = Mathf.RoundToInt(Mathf.Abs (block.localPosition.x));
					int thisZ = Mathf.RoundToInt(Mathf.Abs (block.localPosition.z));

					if (thisX == horizontalScale || thisZ == horizontalScale){

						if (thisX <= horizontalScale && thisZ <= horizontalScale) {

							block.gameObject.SetActive (true);
						}
					}
				}
			}
		}

		void ScaleHorizontalDown(){

			if (horizontalScale != 0) {
				
				foreach (Transform block in grassBlockParent) {

					//had to round here and use ints because problems were occuring with floats
					int thisX = Mathf.RoundToInt(Mathf.Abs (block.localPosition.x));
					int thisZ = Mathf.RoundToInt(Mathf.Abs (block.localPosition.z));

					if (thisX == horizontalScale || thisZ == horizontalScale) {
						
						block.gameObject.SetActive (false);
					}

				}
				horizontalScale--;
			}
		}
		
		// Update is called once per frame
		void Update () {

			if (Input.touchCount == 1 && currentSelected == Selected.GrassBlock) {

				var grassTouch = Input.GetTouch(0);
				if (grassTouch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject (0)) {
					
					firstTouchPosition = grassTouch.position;
				} else if (grassTouch.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject (0)) {

					Vector2 currentTouchPosition = grassTouch.position;
					//first find which way touch is moving
					float deltaXMag = Mathf.Abs(firstTouchPosition.x - currentTouchPosition.x);
					float deltaYMag = Mathf.Abs(firstTouchPosition.y - currentTouchPosition.y);

					if (deltaXMag > deltaYMag) {

						if (firstTouchPosition.x - currentTouchPosition.x < 0) {
							//scale up sideways
							ScaleHorizontalUp();
						} else {
							//scale down sideways
							ScaleHorizontalDown();
						}

					} else {
						
						if (firstTouchPosition.y - currentTouchPosition.y < 0) {
							//scale up vertically
							ScaleVerticalUp ();
						} else {
							//scale down vertically
							ScaleVerticalDown();
						}
					}
				}

			} else if (Input.touchCount == 1 && m_HitTransform != null && currentSelected != Selected.GrassBlock){

				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(0))
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					// prioritize reults types
					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					//move entire creation
					if (currentSelected == Selected.Move) {
						PlaceNewObject (m_HitTransform.gameObject, point, resultTypes);
					}

					//create and place diamondBlock
					if (currentSelected == Selected.DiamondOre) {
						GameObject newBlock = Instantiate (DiamondOre, this.transform);
						PlaceNewObject (newBlock, point, resultTypes);
					}

					//create and place Torch
					if (currentSelected == Selected.Torch) {
						GameObject newBlock = Instantiate (Torch, this.transform);
						PlaceNewObject (newBlock, point, resultTypes);
					}

					//create and place tree
					if (currentSelected == Selected.Tree) {
						GameObject newBlock = Instantiate (Tree, this.transform);
						PlaceNewObject (newBlock, point, resultTypes);
					}
						
				}
			}
		}

	}
}

