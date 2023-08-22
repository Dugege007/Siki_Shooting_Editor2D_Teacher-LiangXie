using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class NextLevel : MonoBehaviour
    {
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
