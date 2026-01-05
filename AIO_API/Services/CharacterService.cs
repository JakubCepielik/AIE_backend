using AIO_API.Entities;
using AIO_API.Entities.Characters;
using AIO_API.Exceptions;
using AIO_API.Interfaces;
using AIO_API.Models.CharacterDto;
using AIO_API.Models.CharacterDto.Ability;
using AIO_API.Models.CharacterDto.Skill;
using AIO_API.Models.CharacterDto.Statistic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIO_API.Services
{
    public class CharacterService : ICharacterService
    {

        private readonly AieDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(AieDbContext dbContext, IMapper mapper, ILogger<CharacterService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public void Update(int id, int userId, UpdateCharacterDto dto)
        {
            var CharacterById = _dbContext
                            .Characters
                            .Where(pc => pc.UserId == userId)
                            .FirstOrDefault(p => p.id == id);

            if (CharacterById == null)
                throw new NotFoundException("Character not found");

            _dbContext.Entry(CharacterById).CurrentValues.SetValues(dto);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogError($"Character with id: {id} DELETE action invoked");

            var playableCharacterById = _dbContext
                            .PlayableCharacter
                            .FirstOrDefault(p => p.id == id);
            if (playableCharacterById == null)
                throw new NotFoundException("Character not found");

            _dbContext.Remove(playableCharacterById);
            _dbContext.SaveChanges();
        }

        public CharacterDto GetById(int id)
        {
            var CharacterById = _dbContext
                            .Characters
                            .Include(c => c.CharacterSkills)
                            .ThenInclude(cs => cs.Skill)
                            .Include(c => c.CharacterAbilities)
                            .ThenInclude(cs => cs.Ability)
                            .Include(i => i.CharacterItems)
                            .ThenInclude(ci => ci.Item)
                            .Include(s => s.Statistics)
                            .FirstOrDefault(p => p.id == id);

            if (CharacterById == null)
            {
                throw new NotFoundException("Character not found");
            }

            var result = _mapper.Map<CharacterDto>(CharacterById);

            return result;
        }

        public IEnumerable<CharacterDto> GetAll(int userId)
        {
            var Characters = _dbContext.
                                   Characters.
                                   Where(pc => pc.UserId == userId).
                                   ToList();

            var CharactersDto = _mapper.Map<List<CharacterDto>>(Characters);

            return CharactersDto;
        }

        public int Create(int userId, CreateCharacterDto dto)
        {
            Character character;
            ValidateStatistics(dto.Statistics);

            switch (dto.CharacterType)
            {
                case CharacterType.Playable:
                    var playableDto = dto;
                    if (playableDto == null)
                        throw new BadRequestException("Invalid PlayableCharacter data");

                    var playable = _mapper.Map<PlayableCharacter>(playableDto);
                    playable.UserId = userId;
                    character = playable;
                    break;

                case CharacterType.Npc:
                    var npcDto = dto;
                    if (npcDto == null)
                        throw new BadRequestException("Invalid NpcCharacter data");

                    var npc = _mapper.Map<NpcCharacter>(npcDto);
                    npc.UserId = userId;
                    character = npc;
                    break;

                default:
                    throw new BadRequestException("Unknown character type");
            }

            _dbContext.Characters.Add(character);
            _dbContext.SaveChanges();

            var statistics = dto.Statistics
               .Select(s =>
               {
                   var stat = _mapper.Map<Statistic>(s);
                   stat.CharacterId = character.id;
                   return stat;
               })
               .ToList();

            _dbContext.Statistics.AddRange(statistics);
            _dbContext.SaveChanges();

            return character.id;
        }

        public void AddSkill(int id, AddCharacterSkillDto dto)
        {
            var character = _dbContext.Characters
                .Include(c => c.CharacterSkills)
                .FirstOrDefault(c => c.id == id);
            if (character == null)
                throw new NotFoundException("Character not found");
            foreach (var skillDto in dto.Skills)
            {
                var skill = _dbContext.Skills.FirstOrDefault(s => s.Id == skillDto.SkillId);
                if (skill == null)
                    throw new NotFoundException("Skill not found");
                if (character.CharacterSkills.Any(s => s.SkillId == skillDto.SkillId))
                    throw new BadRequestException("Character already has this skill");
                skillDto.CharacterId = id;
                var addSkillDto = _mapper.Map<CharacterSkill>(skillDto);
                character.CharacterSkills.Add(addSkillDto);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<SkillDto> GetSkills()
        {
            var skills = _dbContext.
                                   Skills.
                                   ToList();

            var skillsDto = _mapper.Map<List<SkillDto>>(skills);

            return skillsDto;
        }

        public void DeleteSkill(int id, DeleteCharacterSkillDto dto)
        {
            var character = _dbContext.Characters
                .Include(c => c.CharacterSkills)
                .FirstOrDefault(c => c.id == id);
            if (character == null)
                throw new NotFoundException("Character not found");
            foreach (var skillDto in dto.Skills)
            {
                var characterSkill = character.CharacterSkills
                    .FirstOrDefault(s => s.SkillId == skillDto.SkillId);
                if (characterSkill == null)
                    throw new NotFoundException("Character does not have this skill");
                _dbContext.Remove(characterSkill);
                _dbContext.SaveChanges();
            }
        }

        public void AddAbility(int id, AddCharacterAbilityDto dto)
        {
            var character = _dbContext.Characters
               .Include(c => c.CharacterAbilities)
               .FirstOrDefault(c => c.id == id);
            if (character == null)
                throw new NotFoundException("Character not found");
            foreach (var abilityDto in dto.Abilities)
            {
                var ability = _dbContext.Abilities.FirstOrDefault(s => s.Id == abilityDto.AbilityId);
                if (ability == null)
                    throw new NotFoundException("Ability not found");
                if (character.CharacterAbilities.Any(s => s.AbilityId == abilityDto.AbilityId))
                    throw new BadRequestException("Character already has this ability");
                abilityDto.CharacterId = id;
                var addAbilityDto = _mapper.Map<CharacterAbility>(abilityDto);
                character.CharacterAbilities.Add(addAbilityDto);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<AbilityDto> GetAbilities()
        {
            var abilities = _dbContext.
                                   Abilities.
                                   ToList();

            var abilitiesDto = _mapper.Map<List<AbilityDto>>(abilities);

            return abilitiesDto;
        }
        public void DeleteAbility(int id, DeleteCharacterAbilityDto dto)
        {
            var character = _dbContext.Characters
               .Include(c => c.CharacterAbilities)
               .FirstOrDefault(c => c.id == id);
            if (character == null)
                throw new NotFoundException("Character not found");
            foreach (var abilityDto in dto.Abilities)
            {
                var characterAbility = character.CharacterAbilities
                    .FirstOrDefault(s => s.AbilityId == abilityDto.AbilityId);
                if (characterAbility == null)
                    throw new NotFoundException("Character does not have this ability");
                _dbContext.Remove(characterAbility);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateStatistic(int id, int userId, UpdateStatisticDto dto)
        {
            var character = _dbContext.Characters
                .Include(c => c.Statistics)
                .FirstOrDefault(c => c.id == id && c.UserId == userId);

            if (character is null)
                throw new NotFoundException("Character not found");

            var statistic = character.Statistics
                .FirstOrDefault(s => s.StatisticType == dto.StatisticType);

            if (statistic is null)
                throw new NotFoundException(
                    $"Statistic type '{dto.StatisticType}' not found for this character");

            // Aktualizacja pól
            _mapper.Map(dto, statistic);
            _dbContext.SaveChanges();
        }

        //Validation helper
        private void ValidateStatistics(List<CreateCharacterStatisticDto> stats)
        {
            if (stats.Count != 2)
                throw new BadRequestException("Character must have exactly 2 statistics (Base & Current)");

            if (!stats.Any(s => s.StatisticType == StatisticType.Base))
                throw new BadRequestException("Missing Basic statistics");

            if (!stats.Any(s => s.StatisticType == StatisticType.Current))
                throw new BadRequestException("Missing Current statistics");

            var basic = stats.First(s => s.StatisticType == StatisticType.Base);
            var current = stats.First(s => s.StatisticType == StatisticType.Current);

            // Soft rule: Current >= Basic
            if (current.Strength < basic.Strength ||
                current.Agility < basic.Agility ||
                current.Toughness < basic.Toughness)
            {
                throw new BadRequestException("Current statistics cannot be lower than Basic");
            }
        }
    }
}
