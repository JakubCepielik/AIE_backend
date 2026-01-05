using AIO_API.Entities.Campaigns;
using AIO_API.Entities.Users;
using AIO_API.Interfaces;

namespace AIO_API.Entities.Characters
{
    public abstract class Character : ICharacter
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Career { get; set; }
        public short Age { get; set; }

        //public int BallisticSkill { get; set; }
        //public int Strength { get; set; }
        //public int Toughness { get; set; }
        //public int Agility { get; set; }
        //public int Intelligence { get; set; }
        //public int WillPower { get; set; }
        //public int Fellowship { get; set; }

        //public int Attacks { get; set; }
        //public int Wounds { get; set; }
        //public int Movement { get; set; }
        //public int Magic { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Statistic> Statistics { get; set; }
         = new List<Statistic>();
        public ICollection<CharacterItem> CharacterItems { get; set; }
            = new List<CharacterItem>();
        public ICollection<CharacterSkill> CharacterSkills { get; set; }
            = new List<CharacterSkill>();
        public ICollection<CharacterAbility> CharacterAbilities { get; set; }
            = new List<CharacterAbility>();
    }
}
