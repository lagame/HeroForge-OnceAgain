using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroForge_OnceAgain.Models
{
    public class CharacterClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }        
        public string Type { get; set; }
        public string Alignment { get; set; }
        public string HitDie { get; set; }
        public int Level { get; set; }
        public string ClassSkills { get; set; }
        public string SkillPoints { get; set; }
        public string SkillPointsAbility { get; set; }
        public string SpellStat { get; set; }
        public string Proficiencies { get; set; }
        public string SpellType { get; set; }
        public string EpicFeatBaseLevel { get; set; }
        public string EpicFeatInterval { get; set; }
        public string EpicFeatList { get; set; }
        public string EpicFullText { get; set; }
        public string ReqRace { get; set; }
        public string ReqWeaponProficiency { get; set; }
        public string ReqBaseAttackBonus { get; set; }
        public string ReqSkill { get; set; }
        public string ReqFeat { get; set; }
        public string ReqSpells { get; set; }
        public string ReqLanguages { get; set; }
        public string ReqPsionics { get; set; }
        public string ReqEpicFeat { get; set; }
        public string ReqSpecial { get; set; }
        public string SpellList1 { get; set; }
        public string SpellList2 { get; set; }
        public string SpellList3 { get; set; }
        public string SpellList4 { get; set; }
        public string SpellList5 { get; set; }
        public string FullText { get; set; }
        public string Reference { get; set; }

    }
}
