using UnityEngine;
using UnityEngine.Analytics;

namespace a
{
    public class UIcontrollerScript:MonoBehaviour
    {
        /// <summary>
        /// Панель конца игры
        /// </summary>
        public GameObject GameoverPanel;
        /// <summary>
        /// Нижняя панель
        /// </summary>
        public GameObject BottomPanel;


        /// <summary>
        /// Панель конца игры
        /// </summary>
        public static GameObject EndGamePanel;
        /// <summary>
        /// Нижняя панель
        /// </summary>
        public static GameObject DownPanel;
        
        private void Start()
        {
            HidePanel(GameoverPanel);


            DownPanel = BottomPanel;
            EndGamePanel = GameoverPanel;
        }

        /// <summary>
        /// Скрывает элемент меню
        /// </summary>
        /// <param name="panel"></param>
        public static void HidePanel(GameObject panel)
        {
            panel.SetActive(false);
        }
        
        
        /// <summary>
        /// Возвращает элемент на исходное место
        /// </summary>
        /// <param name="panel"></param>
        public static void ShowPanel(GameObject panel)
        {
            panel.SetActive(true);
        }
        
    }
}