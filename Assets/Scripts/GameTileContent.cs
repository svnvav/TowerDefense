using UnityEngine;

namespace Catlike.TowerDefense
{
    [SelectionBase]
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField] private GameTileContentType type = default;
        
        private GameTileContentFactory originFactory;

        public GameTileContentType Type => type;
        
        public bool BlocksPath =>
            Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;
        
        public GameTileContentFactory OriginFactory {
            get => originFactory;
            set {
                Debug.Assert(originFactory == null, "Redefined origin factory!");
                originFactory = value;
            }
        }
        
        public virtual void GameUpdate () {}
        
        public void Recycle () {
            originFactory.Reclaim(this);
        }
    }
}