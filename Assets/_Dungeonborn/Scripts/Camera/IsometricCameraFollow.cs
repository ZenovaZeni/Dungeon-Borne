using UnityEngine;

namespace Dungeonborn.Camera
{
    public sealed class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 11f, -9f);
        [SerializeField] private float followSpeed = 10f;

        private void LateUpdate()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                target = player != null ? player.transform : null;
            }

            if (target == null)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }
    }
}
