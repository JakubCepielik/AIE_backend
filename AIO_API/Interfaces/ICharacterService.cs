using AIO_API.Entities.Characters;
using AIO_API.Models.CharacterDto;
using AIO_API.Models.CharacterDto.Ability;
using AIO_API.Models.CharacterDto.Skill;
using AIO_API.Models.CharacterDto.Statistic;

namespace AIO_API.Interfaces
{
    public interface ICharacterService
    {

        public CharacterDto GetById(int id);
        public int Create(int userId,CreateCharacterDto dto);
        public IEnumerable<CharacterDto> GetAll(int userId);
        public void Delete(int id);
        public void Update(int id,int userId, UpdateCharacterDto dto);
        public void AddSkill(int id, AddCharacterSkillDto dto);
        public void DeleteSkill(int id, DeleteCharacterSkillDto dto);
        public void AddAbility(int id, AddCharacterAbilityDto dto);
        public void DeleteAbility(int id, DeleteCharacterAbilityDto dto);
        public void UpdateStatistic(int id, int userId, UpdateStatisticDto dto);
        public IEnumerable<SkillDto> GetSkills();
        public IEnumerable<AbilityDto> GetAbilities();

        //Task<ICharacter> CreateCharacterAsync(ICharacter character);
        //Task<bool> UpdateCharacterAsync(int id, ICharacter updatedCharacter);
        //Task<bool> DeleteCharacterAsync(int id);
    }
}
