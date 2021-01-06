using skilltree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.skill
{
    public class PlayerSkillCollection
    {
        private List<PlayerSkill> _skills = new List<PlayerSkill>();

        public PlayerSkill CurrentSkill
        {
            get; set;
        }

        public void SelectSkill(string skillUuid)
        {
            if (ContainSkill(skillUuid) && IsSkillUnlocked(skillUuid))
            {
                Debug.Log("Skill " + skillUuid + " selected !");
                CurrentSkill = GetSkill(skillUuid);
            }
        }

        public bool ContainSkill(string skillUuid)
        {
            foreach (PlayerSkill skill in _skills)
            {
                if (skill.Uuid.Equals(skillUuid))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSkillUnlocked(string skillUuid)
        {
            return GetSkill(skillUuid).Unlocked;
        }

        public bool IsSkillUsable(string skillUuid)
        {
            return GetSkill(skillUuid).Usable;
        }

        public PlayerSkill GetSkill(string skillUuid)
        {
            foreach (PlayerSkill skill in _skills)
            {
                if (skill.Uuid.Equals(skillUuid))
                {
                    return skill;
                }
            }
            return null;
        }

        public void Load(List<SaveSkill> skillsSnapshot)
        {
            if (skillsSnapshot != null)
            {
                _skills = new List<PlayerSkill>(skillsSnapshot.Count);

                foreach (SaveSkill skill in skillsSnapshot)
                {
                    _skills.Add(new PlayerSkill(skill._uuid, skill._unlocked, skill._usable, skill._icon));
                }
            }
        }
    }
}
