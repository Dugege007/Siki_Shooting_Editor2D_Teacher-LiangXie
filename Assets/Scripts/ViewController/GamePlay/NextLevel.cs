using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class NextLevel : MonoBehaviour
    {
        /// <summary>
        /// 下关名称（场景名称）
        /// </summary>
        public string LevelName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SceneManager.LoadScene(LevelName);
            }
        }
    }
}
