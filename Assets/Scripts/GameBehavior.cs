using UnityEngine;

namespace Catlike.TowerDefense
{
    public abstract class GameBehavior : MonoBehaviour
    {
        public virtual bool GameUpdate () => true;
    }
}