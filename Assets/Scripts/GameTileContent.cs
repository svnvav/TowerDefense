using UnityEngine;

namespace Catlike.TowerDefense
{
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField] private GameTileContentType type = default;
        
        private GameTileContentFactory originFactory;

        public GameTileContentType Type => type;
        
        public GameTileContentFactory OriginFactory {
            get => originFactory;
            set {
                Debug.Assert(originFactory == null, "Redefined origin factory!");
                originFactory = value;
            }
        }
        
        public void Recycle () {
            originFactory.Reclaim(this);
        }
    }
}