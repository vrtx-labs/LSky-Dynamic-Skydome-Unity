//////////////////////////////////
/// LSky
/// =============
///
/// Description:
/// =============
/// Empty Object.
//////////////////////////////////


using System;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Rallec.LSky
{

	[Serializable]
	public class LSky_EmptyObject
	{

		public GameObject gameObject;
		public Transform transform;


		public virtual bool CheckComponents
		{
			get
			{

				if(gameObject == null) return false;
				if(transform == null) return false;


				return true;

			}
		}


		public void InitTransform(Transform parent, Vector3 posOffset)
        {

            if (transform == null) return;

            if (PrefabStageUtility.GetCurrentPrefabStage() != null && transform.parent != parent)
            {
                Debug.LogWarning($"Transform cannot be correctly initialised within a prefab. Please set the parent of {gameObject.name} to {parent.name} manually");
            }

            //skip setting parent when in prefab edit mode to avoid errors
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
                transform.parent = parent;
            transform.position = Vector3.zero + posOffset;
            transform.localPosition = Vector3.zero + posOffset;
            transform.rotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }


        public virtual void Instantiate(string parentName, string name)
        {

            if (gameObject == null)
            {
                // Check if exist gameobject with this name.
                var childObj = GameObject.Find(parentName).transform.Find(name).gameObject;

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);
            }

            if (transform == null)
                transform = gameObject.transform; // Get transform.

        }

        public virtual void Instantiate(Transform parentTransform, string name)
        {

            if (gameObject == null)
            {
                // Check if exist gameobject with this name.
                var childObj = parentTransform.Find(name).gameObject;

                if (childObj != null)
                    gameObject = childObj;
                else
                    gameObject = new GameObject(name);
            }

            if (transform == null)
                transform = gameObject.transform; // Get transform.

        }

    }

}