using UnityEngine;
using Dungeonborn.Characters;

namespace Dungeonborn.Camera
{
    public sealed class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 12.5f, -10.5f);
        [SerializeField] private float followSpeed = 8f;
        [SerializeField] private float lookAheadDistance = 1.8f;
        [SerializeField] private Vector2 arenaXBounds = new Vector2(-5.5f, 5.5f);
        [SerializeField] private Vector2 arenaZBounds = new Vector2(-2.5f, 8.5f);

        private PlayerMotor playerMotor;

        private void LateUpdate()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                target = player != null ? player.transform : null;
                playerMotor = target != null ? target.GetComponent<PlayerMotor>() : null;
            }

            if (target == null)
            {
                return;
            }

            if (playerMotor == null)
            {
                playerMotor = target.GetComponent<PlayerMotor>();
            }

            var focus = target.position;
            if (playerMotor != null)
            {
                focus += playerMotor.FacingDirection * lookAheadDistance;
            }

            focus.x = Mathf.Clamp(focus.x, arenaXBounds.x, arenaXBounds.y);
            focus.z = Mathf.Clamp(focus.z, arenaZBounds.x, arenaZBounds.y);

            transform.position = Vector3.Lerp(transform.position, focus + offset, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }
    }
}
