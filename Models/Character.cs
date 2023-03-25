using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroForge_OnceAgain.Models
{
    public class Character
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

        public List<CharacterClass> CharacterClasses { get; set; }

        public Character(CharacterCreationInfo creationInfo)
        {

            if (creationInfo != null)
            {
                Name = creationInfo.Name;
                Age = creationInfo.Age;
                Race = creationInfo.Race;
                Gender = creationInfo.Gender;
                Alignment = creationInfo.Alignment;
                Deity = creationInfo.Deity;
                Height = creationInfo.Height;
                Weight = creationInfo.Weight;
                Eyes = creationInfo.Eyes;
                Hair = creationInfo.Hair;
                CharacterClasses = new List<CharacterClass>();

                foreach (var classInfo in creationInfo.Classes)
                {
                    var characterClass = new CharacterClass
                    {
                        Name = classInfo.ClassName,
                        Level = classInfo.Level,

                    };
                    CharacterClasses.Add(characterClass);
                }
            }
            else
            {
                // lidar com o caso em que creationInfo é nulo
                Console.WriteLine("");
                CharacterCreationInfo creationInfo2 = new CharacterCreationInfo();
                Name = creationInfo2.Name;
                Age = creationInfo2.Age;
                Race = creationInfo2.Race;
                Gender = creationInfo2.Gender;
                CharacterClasses = new List<CharacterClass>();

                foreach (var classInfo in creationInfo2.Classes)
                {
                    var characterClass = new CharacterClass
                    {
                        Name = classInfo.ClassName,
                        Level = classInfo.Level,

                    };
                    CharacterClasses.Add(characterClass);
                }
            }


            
        }



    }
}