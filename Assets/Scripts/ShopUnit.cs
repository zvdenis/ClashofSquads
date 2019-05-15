using UnityEngine;

namespace a
{
    public class ShopUnit : MonoBehaviour
    {
        /// <summary>
        /// Коэффициент атаки синего лучника
        /// </summary>
        public static float BlueArcherAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент здоровья синего лучника
        /// </summary>
        public static float BlueArcherHealthCoefficient = 1f;


        
        /// <summary>
        /// Коэффициент атаки красного лучника
        /// </summary>
        public static float RedArcherAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент здоровья красного лучника
        /// </summary>
        public static float RedArcherHealthCoefficient = 1f;

        /// <summary>
        /// Коэффициент атаки синего копейщика
        /// </summary>
        public static float BlueSpearmanAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент атаки синего копейщика
        /// </summary>
        public static float BlueSpearmanHealthCoefficient = 1f;

        /// <summary>
        /// Коэффициент атаки красного копейщика
        /// </summary>
        public static float RedSpearmanAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент здоровья красного  копейщика
        /// </summary>
        public static float RedSpearmanHealthCoefficient = 1f;


        /// <summary>
        /// Коэффициент атаки синей катапульты
        /// </summary>
        public static float BlueCatapultAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент здоровья синей катапульты
        /// </summary>
        public static float BlueCatapultHealthCoefficient = 1f;

        /// <summary>
        /// Коэффициент атаки красной катапульты
        /// </summary>
        public static float RedCatapultAttackCoefficient = 1f;
        
        /// <summary>
        /// Коэффициент здоровья красной катапульты
        /// </summary>
        public static float RedCatapultHealthCoefficient = 1f;

        /// <summary>
        /// Тип юнита
        /// </summary>
        public string UnitName;
        /// <summary>
        /// цвет юнита
        /// </summary>
        public string UnitColor;
        /// <summary>
        /// Текст о стоимости улучшения здоровья
        /// </summary>
        public TextMesh HealthCost;
        /// <summary>
        /// текст стоимости улучшения атаки
        /// </summary>
        public TextMesh AttackCost;
        /// <summary>
        /// кнопка улучшения здоровья
        /// </summary>
        public GameObject HealthButton;
        /// <summary>
        /// кнопка улучшения атаки
        /// </summary>
        public GameObject AttackButton;
       
        
        /// <summary>
        /// стоимость улучшения атаки
        /// </summary>
        private float CurrentAttackCost;
        /// <summary>
        /// стоимость улучшения здоровья
        /// </summary>
        private float CurrentHealthCost;

        /// <summary>
        /// коэффициент увеличения стоимости улучшений и их качества
        /// </summary>
        public const float IncreaseCoefficient = 1.2f;
        /// <summary>
        /// Стартовая стоимость улучшения
        /// </summary>
        public  float StartCost = 30f;

        
        /// <summary>
        /// Обновляют информацию и задает начальные значения
        /// </summary>
        private void Start()
        {
            CurrentAttackCost = StartCost;
            CurrentHealthCost = StartCost;
            
            UpdateUI();
        }

        /// <summary>
        /// Покупает улучшение здоровья и рассчитывает дальнейшие коэффициенты
        /// </summary>
        public void BuyHealth()
        {
            if (UnitColor == "Blue" && GameInfo.BluePlayer.Resources.AmountOfGold >= CurrentHealthCost)
            {
                GameInfo.BluePlayer.Resources.AmountOfGold -= CurrentHealthCost;

                switch (UnitName)
                {
                    case "Archer":
                        BlueArcherHealthCoefficient *= IncreaseCoefficient;

                        break;
                    case "Spearman":
                        BlueSpearmanHealthCoefficient *= IncreaseCoefficient;
                        break;
                    case "Catapult":
                        BlueCatapultHealthCoefficient *= IncreaseCoefficient;
                        break;
                }

                CurrentHealthCost *= (IncreaseCoefficient);
            }
            if (UnitColor == "Red" && GameInfo.RedPlayer.Resources.AmountOfGold >= CurrentHealthCost)
            {
                GameInfo.RedPlayer.Resources.AmountOfGold -= CurrentHealthCost;

                switch (UnitName)
                {
                    case "Archer":
                        RedArcherHealthCoefficient *= IncreaseCoefficient;

                        break;
                    case "Spearman":
                        RedSpearmanHealthCoefficient *= IncreaseCoefficient;
                        break;
                    case "Catapult":
                        RedCatapultHealthCoefficient *= IncreaseCoefficient;
                        break;
                }

                CurrentHealthCost *= (IncreaseCoefficient);
            }
            
            UpdateUI();
        }
        
        
        
        
        /// <summary>
        /// Покупает улучшение атаки и рассчитывает дальнейшие коэффициенты
        /// </summary>
        public void BuyAttack()
        {
            if (UnitColor == "Blue" && GameInfo.BluePlayer.Resources.AmountOfGold >= CurrentAttackCost)
            {
                GameInfo.BluePlayer.Resources.AmountOfGold -= CurrentAttackCost;

                switch (UnitName)
                {
                    case "Archer":
                        BlueArcherAttackCoefficient *= IncreaseCoefficient;

                        break;
                    case "Spearman":
                        BlueSpearmanAttackCoefficient *= IncreaseCoefficient;
                        break;
                    case "Catapult":
                        BlueCatapultAttackCoefficient *= IncreaseCoefficient;
                        break;
                }

                CurrentAttackCost *= (IncreaseCoefficient);
            }
            if (UnitColor == "Red" && GameInfo.RedPlayer.Resources.AmountOfGold >= CurrentAttackCost)
            {
                GameInfo.RedPlayer.Resources.AmountOfGold -= CurrentAttackCost;

                switch (UnitName)
                {
                    case "Archer":
                        RedArcherAttackCoefficient *= IncreaseCoefficient;

                        break;
                    case "Spearman":
                        RedSpearmanAttackCoefficient *= IncreaseCoefficient;
                        break;
                    case "Catapult":
                        RedCatapultAttackCoefficient *= IncreaseCoefficient;
                        break;
                }

                CurrentAttackCost *= (IncreaseCoefficient);
            }
            
            UpdateUI();
        }

        /// <summary>
        /// выводит информацию на экран
        /// </summary>
        public void UpdateUI()
        {
            HealthCost.text = "" + (int)CurrentHealthCost;
            AttackCost.text = "" + (int)CurrentAttackCost;
        }

        /// <summary>
        /// устанавливает авто-сворачивание кнопок
        /// </summary>
        public void SetAutoClose()
        {
            if (GameInfo.CurrentGameType == GameInfo.GameType.OnePlayerOneDevice && UnitColor == "Red")
            {
                HideButtons();
                return;
            }
            
            CancelInvoke();
            Invoke("HideButtons", 5);
        }

        /// <summary>
        /// скрывает кнопки
        /// </summary>
        private void HideButtons()
        {
            HealthButton.SetActive(false);
            AttackButton.SetActive(false);
        }
    }
}