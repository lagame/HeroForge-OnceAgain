using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroForge_OnceAgain.Models;

namespace HeroForge_OnceAgain.Models
{
    public class CharacterCreationInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Race { get; set; }
        public int Gender { get; set; }
        public int Alignment { get; set; }
        public int Deity { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }

        public List<CharacterClassCreationInfo> Classes { get; set; }

        public CharacterCreationInfo() {
            Classes = new List<CharacterClassCreationInfo>();
        }
    }
}