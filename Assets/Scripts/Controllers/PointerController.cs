using GameStaticValues;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameControllers
{
    public class PointerController : MonoBehaviour
    {
        private enum PointerClickType
        {
            None,
            Click,
            Drag,
            Zoom
        }

        public static int lockFrameCount = 0;

        private static Vector2 touchFirstPos = Vector2.negativeInfinity;
        private static int touchFirstFrames = 0;

        private static float dragCamMass = 0;
        private static Vector3 dragCamDirection = Vector3.negativeInfinity;

        private static PointerClickType type = PointerClickType.None;
        void Update()
        {
            if(lockFrameCount == 0)
            {
                if (Input.touchCount == 0)
                {
                    touchFirstFrames = 0;
                    touchFirstPos = Vector2.negativeInfinity;

                    if (dragCamMass > .5f && type == PointerClickType.Drag)
                    {
                        dragCamMass -= Time.deltaTime;
                        Camera.main.transform.position += dragCamDirection * Time.deltaTime * dragCamMass;
                        return;
                    }

                    type = PointerClickType.None;
                    return;
                }
                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    if (type == PointerClickType.Click || type == PointerClickType.None)
                        return;
                }

                if (Input.touchCount == 1)
                {
                    if (touchFirstPos == Vector2.negativeInfinity)
                        touchFirstPos = Input.GetTouch(0).position;

                    if (Vector2.Distance(Input.GetTouch(0).position, touchFirstPos) < CameraConfig.DistanceForDrag)
                        touchFirstFrames++;
                    else
                    {
                        touchFirstFrames = 0;
                        touchFirstPos = Vector2.negativeInfinity;
                        type = PointerClickType.Drag; //ONDRAG
                        Drag();
                    }

                    if (touchFirstFrames >= CameraConfig.FramesForClick)
                    {
                        touchFirstFrames = 0;
                        touchFirstPos = Vector2.negativeInfinity;
                        type = PointerClickType.Click; //ONCLICK
                        Click();
                    }
                }

                if(Input.touchCount > 2)
                {
                    type = PointerClickType.Zoom; //ONZOOM
                    Zoom();
                }
            }
            else
            {
                touchFirstFrames = 0;
                touchFirstPos = Vector2.negativeInfinity;
                lockFrameCount--;
            }
        }

        private static void Click()
        {
            MainController.Click(Input.GetTouch(0).position);
        }
        private static void Drag()
        {
            Vector3 newPos = Camera.main.transform.position - (new Vector3(Input.touches[0].deltaPosition.x / Screen.width, Input.touches[0].deltaPosition.y / Screen.height * (Screen.height / Screen.width), 0) * 10 * (Camera.main.orthographicSize / CameraConfig.ZoomMinBound));
            
            if (newPos.x > TileMapController.MapBounds.xMax || newPos.x < TileMapController.MapBounds.xMin || newPos.y > TileMapController.MapBounds.zMax || newPos.y < TileMapController.MapBounds.zMin)
                return;
            dragCamDirection = (newPos - Camera.main.transform.position).normalized;
            dragCamMass = Vector3.Distance(Camera.main.transform.position, newPos);
            Camera.main.transform.position = newPos;
        }

        private static void Zoom()
        {

        }
    }
}
