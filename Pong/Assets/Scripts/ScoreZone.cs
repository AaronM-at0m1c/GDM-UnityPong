using UnityEngine;
using Unity.Netcode;

public class ScoreZone : NetworkBehaviour
{
    public enum Side { Left, Right }

    [SerializeField] private Side side;

    // Use OnTrigger to call Score functions in game manager when hitting a ScoreZone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.gameObject.name == "Ball")
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm == null) return;

            if (side == Side.Left)
            {
                gm.LeftPlayerScored();
            }
            else
            {
                gm.RightPlayerScored();
            }
        }
    }
}
