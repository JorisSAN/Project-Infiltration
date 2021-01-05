using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.skill
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private Animation _animationEcranDeFumee = default;
        [SerializeField] private Animation _animationTraverseMur = default;

        public static SkillManager Instance { get; private set; } // Singleton for the moment : to change

        private void Awake()
        {
            Instance = this;
        }

        public void UseSkill(string skillUuid)
        {
            switch (skillUuid)
            {
                case "ecran_de_fumee":
                    // Animation du pouvoir écran de fumée
                    //_animationEcranDeFumee.Play();
                    Debug.Log("Ecran de fumée !");
                    break;

                case "traverse_mur":
                    // Animation du pouvoir traverse mur
                    Debug.Log("Traverse mur !");
                    //_animationTraverseMur.Play();
                    break;
            }
        }
    }
}
