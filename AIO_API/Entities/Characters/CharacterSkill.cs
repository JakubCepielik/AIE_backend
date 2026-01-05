namespace AIO_API.Entities.Characters
{
    public class CharacterSkill
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }

    }
}
